using AutoMapper;
using GIGLS.Core;
using GIGLS.Core.Domain.Wallet;
using GIGLS.Core.DTO.OnlinePayment;
using GIGLS.Core.DTO.Wallet;
using GIGLS.Core.IServices.User;
using GIGLS.Core.IServices.Utility;
using GIGLS.Core.IServices.Wallet;
using GIGLS.CORE.DTO.Shipments;
using GIGLS.Services.Implementation.Utility.FlutterWaveEncryptionService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Services.Implementation.Wallet
{
    public class WaybillPaymentLogService : IWaybillPaymentLogService
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserService _userService;
        private readonly IPasswordGenerator _passwordGenerator;
        private readonly IPaystackPaymentService _paystackService;

        public WaybillPaymentLogService(IUnitOfWork uow, IUserService userService, IPasswordGenerator passwordGenerator,
            IPaystackPaymentService paystackService)
        {
            _uow = uow;
            _userService = userService;
            _passwordGenerator = passwordGenerator;
            _paystackService = paystackService;
            MapperConfig.Initialize();
        }

        public async Task<PaystackWebhookDTO> AddWaybillPaymentLog(WaybillPaymentLogDTO waybillPaymentLog)
        {
            var response = new PaystackWebhookDTO();

            if (waybillPaymentLog.UserId == null)
            {
                waybillPaymentLog.UserId = await _userService.GetCurrentUserId();
            }

            //check if any transaction on the waybill is successful
            var paymentLog = _uow.WaybillPaymentLog.GetAllAsQueryable()
                .Where(x => x.Waybill == waybillPaymentLog.Waybill && x.IsPaymentSuccessful == true).OrderByDescending(x => x.DateCreated).FirstOrDefault();

            if (paymentLog != null)
            {
                response.Status = false;
                response.Message = $"There is successful transaction for the waybill {waybillPaymentLog.Waybill}";
                response.data.Message = $"There is successful transaction for the waybill {waybillPaymentLog.Waybill}";
                response.data.Status = "failed";
                
                return response;
            }
            else
            {
                if (waybillPaymentLog.OnlinePaymentType == Core.Enums.OnlinePaymentType.Paystack)
                {
                    response = await AddWaybillPaymentLogForPaystack(waybillPaymentLog);
                }

                if(waybillPaymentLog.OnlinePaymentType == Core.Enums.OnlinePaymentType.Flutterwave)
                {
                    //Process Payment for FlutterWave;
                    response = await AddWaybillPaymentLogForFlutterWave(waybillPaymentLog);
                }
            }
                            
            return response;
        }

        public async Task<PaymentInitiate> AddWaybillPaymentLogForTheTellerNet(WaybillPaymentLogDTO waybillPaymentLog)
        {
            var MerchantId = ConfigurationManager.AppSettings["MerchantId"];
            var MerchantUsername = ConfigurationManager.AppSettings["MerchantUsername"];
            var MerchantKey = ConfigurationManager.AppSettings["MerchantKey"];

            //check the current transaction on the waybill
            var paymentLog = _uow.WaybillPaymentLog.GetAllAsQueryable()
                .Where(x => x.Waybill == waybillPaymentLog.Waybill).OrderByDescending(x => x.DateCreated).FirstOrDefault();

            if (paymentLog != null)
            {
                //think on how to solve this later but for now, return this below
                return new PaymentInitiate
                {
                    IsPaymentSuccessful = false,
                    MerchantId = MerchantId,
                    MerchantUsername = MerchantUsername,
                    MerchantKey = MerchantKey,
                    TransactionId = waybillPaymentLog.Reference
                };
            }

            if (waybillPaymentLog.UserId == null)
            {
                waybillPaymentLog.UserId = await _userService.GetCurrentUserId();
            }

            var newPaymentLog = Mapper.Map<WaybillPaymentLog>(waybillPaymentLog);

            _uow.WaybillPaymentLog.Add(newPaymentLog);
            await _uow.CompleteAsync();
                       
            return new PaymentInitiate
            {
                IsPaymentSuccessful = false,
                MerchantId = MerchantId,
                MerchantUsername = MerchantUsername,
                MerchantKey = MerchantKey,
                TransactionId = waybillPaymentLog.Reference
            };
        }

        private async Task<string> SaveWaybillPaymentLog(WaybillPaymentLogDTO waybillPaymentLog)
        {
            waybillPaymentLog.Reference = await GenerateWaybillReferenceCode(waybillPaymentLog.Waybill);
            var newPaymentLog = Mapper.Map<WaybillPaymentLog>(waybillPaymentLog);
            _uow.WaybillPaymentLog.Add(newPaymentLog);
            await _uow.CompleteAsync();

            return waybillPaymentLog.Reference;
        }

        private async Task<PaystackWebhookDTO> AddWaybillPaymentLogForPaystack(WaybillPaymentLogDTO waybillPaymentLog)
        {    
            waybillPaymentLog.Reference = await SaveWaybillPaymentLog(waybillPaymentLog);

            //3. send the request to paystack gateway
            var paystackResponse =  await ProcessMobilePaymentForPaystack(waybillPaymentLog);
            return paystackResponse;
        }

        private async Task<string> GenerateWaybillReferenceCode(string waybill)
        {
            string code = await _passwordGenerator.Generate();
            string reference = "wb-" + waybill + "-" + code;
            return reference;
        }

        private async Task<PaystackWebhookDTO> ProcessMobilePaymentForPaystack(WaybillPaymentLogDTO waybillPaymentLog)
        {
            try
            {
                string payStackSecretGhana = ConfigurationManager.AppSettings["PayStackSecretGhana"];
                string payStackChargeAPI = ConfigurationManager.AppSettings["PayStackChargeAPI"];
                
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                using (var client =  new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", payStackSecretGhana);

                    MobileMoneyDTO mobileMoney = new MobileMoneyDTO
                    {
                        amount = waybillPaymentLog.Amount * 100,
                        currency = waybillPaymentLog.Currency,
                        email = waybillPaymentLog.Email,
                        reference = waybillPaymentLog.Reference,                        
                        mobile_money = new Mobile_Money
                        {
                            phone = waybillPaymentLog.PhoneNumber,
                            provider = waybillPaymentLog.NetworkProvider
                        }
                    };

                    var json = JsonConvert.SerializeObject(mobileMoney);
                    var data = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(payStackChargeAPI, data);
                    string result = await response.Content.ReadAsStringAsync();
                    
                    var paystackResponse = JsonConvert.DeserializeObject<PaystackWebhookDTO>(result);
                    
                    //update the response from paystack
                    var updateWaybillPaymentLog = await _uow.WaybillPaymentLog.GetAsync(x => x.Reference == waybillPaymentLog.Reference);
                    
                    if (paystackResponse.data != null)
                    {
                        updateWaybillPaymentLog.TransactionStatus = paystackResponse.data.Status;
                        updateWaybillPaymentLog.TransactionResponse = paystackResponse.data.Display_Text + " " + paystackResponse.data.Message + " " + paystackResponse.data.Gateway_Response;
                    }
                    else
                    {
                        updateWaybillPaymentLog.TransactionResponse = paystackResponse.Message;
                    }

                    await _uow.CompleteAsync();

                    return paystackResponse;
                }                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<WaybillPaymentLogDTO> GetWaybillPaymentLogByReference(string reference)
        {
            var paymentlog =  await _uow.WaybillPaymentLog.GetAsync(x => x.Reference == reference);
            return Mapper.Map<WaybillPaymentLogDTO>(paymentlog);
        }

        public async Task<WaybillPaymentLogDTO> GetWaybillPaymentLogByWaybill(string waybill)
        {
            var paymentLog = _uow.WaybillPaymentLog.GetAllAsQueryable().Where(x => x.Waybill == waybill).LastOrDefault();
            var paymentDto =  Mapper.Map<WaybillPaymentLogDTO>(paymentLog);
            return await Task.FromResult(paymentDto);
        }

        public async Task<List<WaybillPaymentLogDTO>> GetWaybillPaymentLogListByWaybill(string waybill)
        {
            var paymentLog = await _uow.WaybillPaymentLog.FindAsync(x => x.Waybill == waybill || x.Reference == waybill);
            return Mapper.Map<List<WaybillPaymentLogDTO>>(paymentLog);
        }

        public async Task<IEnumerable<WaybillPaymentLogDTO>> GetWaybillPaymentLogs()
        {
            var paymentlogs = _uow.WaybillPaymentLog.GetAll().ToList();
            var paymentlogDto = Mapper.Map<IEnumerable<WaybillPaymentLogDTO>>(paymentlogs);
            return await Task.FromResult(paymentlogDto);
        }

        public Tuple<Task<List<WaybillPaymentLogDTO>>> GetWaybillPaymentLogs(FilterOptionsDto filterOptionsDto)
        {
            throw new NotImplementedException();
        }

        public Task UpdateWaybillPaymentLog(WaybillPaymentLogDTO waybillPaymentLog)
        {
            throw new NotImplementedException();
        }

        public async Task<PaystackWebhookDTO> VerifyAndValidateWaybill(string waybill)
        {
            //check the current transaction on the waybill
            var paymentLog = _uow.WaybillPaymentLog.GetAllAsQueryable()
                .Where(x => x.Waybill == waybill || x.Reference == waybill).OrderByDescending(x => x.DateCreated).FirstOrDefault();
            
            if (paymentLog != null)
            {
                var response = await _paystackService.VerifyAndValidateMobilePayment(paymentLog.Reference);
                return response;
            }
                        
            return new PaystackWebhookDTO { 
                Status = false,
                Message = $"No online payment process occurred for the waybill/reference {waybill}",
                data = new Core.DTO.OnlinePayment.Data
                {
                    Message = $"No online payment process occurred for the waybill/reference {waybill}",
                    Status = "failed"
                }
            };
        }

        public async Task<PaystackWebhookDTO> VerifyAndValidateWaybillForVodafoneMobilePayment(string waybill, string pin)
        {
            //check the current transaction on the waybill
            var paymentLog = _uow.WaybillPaymentLog.GetAllAsQueryable()
                .Where(x => x.Waybill == waybill).OrderByDescending(x => x.DateCreated).FirstOrDefault();

            if (paymentLog != null)
            {
                var response = await _paystackService.ProcessPaymentForWaybillUsingPin(paymentLog, pin);
                return response;
            }

            return new PaystackWebhookDTO
            {
                Status = false,
                Message = $"No online payment process occurred for the waybill {waybill}",
                data = new Core.DTO.OnlinePayment.Data
                {
                    Message = $"No online payment process occurred for the waybill {waybill}",
                    Status = "failed"
                }
            };
        }

        private async Task<PaystackWebhookDTO> AddWaybillPaymentLogForFlutterWave(WaybillPaymentLogDTO waybillPaymentLog)
        {
            waybillPaymentLog.Reference = await SaveWaybillPaymentLog(waybillPaymentLog);

            //3. send the request to paystack gateway
            var paystackResponse = await ProcessMobilePaymentForFlutterWave(waybillPaymentLog);
            return paystackResponse;
        }

        private async Task<PaystackWebhookDTO> ProcessMobilePaymentForFlutterWave(WaybillPaymentLogDTO waybillPaymentLog)
        {
            try
            {
                var responseResult = new PaystackWebhookDTO();

                string flutterSandBox = ConfigurationManager.AppSettings["FlutterSandBox"];
                string flutterDirectAccountDebit = flutterSandBox + ConfigurationManager.AppSettings["FlutterDirectAccountDebit"];
                string PBFPubKey = ConfigurationManager.AppSettings["FlutterwavePubKey"];
                string SecretKey = ConfigurationManager.AppSettings["FlutterwaveSecretKey"];

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                FlutterWaveDTO mobileMoney = new FlutterWaveDTO
                {
                    amount = waybillPaymentLog.Amount,
                    currency = waybillPaymentLog.Currency,
                    email = waybillPaymentLog.Email,
                    txRef = waybillPaymentLog.Reference,
                    phonenumber = waybillPaymentLog.PhoneNumber,
                    accountbank = waybillPaymentLog.FlutterWaveData.accountbank,
                    accountnumber = waybillPaymentLog.FlutterWaveData.accountnumber,
                    bvn = waybillPaymentLog.FlutterWaveData.bvn,
                    country = waybillPaymentLog.FlutterWaveData.country,
                    payment_type = waybillPaymentLog.FlutterWaveData.payment_type,
                    firstname = waybillPaymentLog.FlutterWaveData.firstname,
                    lastname = waybillPaymentLog.FlutterWaveData.lastname,
                    passcode = waybillPaymentLog.FlutterWaveData.passcode,
                    PBFPubKey = PBFPubKey
                };

                //encrypt data
                string mobileMoneySerialize = JsonConvert.SerializeObject(mobileMoney);
                IPaymentDataEncryption en = new RavePaymentDataEncryption();
                string key = en.GetEncryptionKey(SecretKey);
                string cipher = en.EncryptData(key, mobileMoneySerialize);

                var flutterObject = new FlutterWaveObject
                {
                    PBFPubKey = PBFPubKey,
                    client = cipher,
                    alg = "3DES-24"
                };

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", flutterSandBox);

                    var json = JsonConvert.SerializeObject(flutterObject);
                    StringContent data = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(flutterDirectAccountDebit, data);
                    string result = await response.Content.ReadAsStringAsync();

                    var flutterResponse = JsonConvert.DeserializeObject<FlutterWebhookDTO>(result);

                    //update the response from flutterwave
                    var updateWaybillPaymentLog = await _uow.WaybillPaymentLog.GetAsync(x => x.Reference == waybillPaymentLog.Reference);

                    responseResult.Message = flutterResponse.Message;
                    if(flutterResponse.Status == "success")
                    {
                        responseResult.Status = true;
                    }

                    if (flutterResponse.data != null)
                    {
                        responseResult.data.Status = flutterResponse.data.Status;
                        responseResult.data.Message = flutterResponse.data.ChargeResponseCode + " | " + flutterResponse.data.ChargeResponseMessage;

                        if (flutterResponse.data.validateInstructions != null)
                        {
                            responseResult.data.Message = responseResult.data.Message + " | " + flutterResponse.data.validateInstructions.Instruction;
                        }
                    }
                    else
                    {
                        responseResult.data.Message = flutterResponse.Message;
                        responseResult.data.Status = flutterResponse.Status;
                    }

                    //update waybill payment log
                    updateWaybillPaymentLog.TransactionStatus = responseResult.data.Status;
                    updateWaybillPaymentLog.TransactionResponse = responseResult.data.Gateway_Response;
                    await _uow.CompleteAsync();

                    return responseResult;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
