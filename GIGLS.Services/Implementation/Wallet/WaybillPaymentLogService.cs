using AutoMapper;
using GIGLS.Core;
using GIGLS.Core.Domain.Wallet;
using GIGLS.Core.DTO.OnlinePayment;
using GIGLS.Core.DTO.Wallet;
using GIGLS.Core.Enums;
using GIGLS.Core.IServices.User;
using GIGLS.Core.IServices.Utility;
using GIGLS.Core.IServices.Wallet;
using GIGLS.CORE.DTO.Shipments;
using GIGLS.Services.Implementation.Utility.FlutterWaveEncryptionService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
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
        private readonly IFlutterwavePaymentService _flutterwavePaymentService;
        private readonly IUssdService _ussdService;

        public WaybillPaymentLogService(IUnitOfWork uow, IUserService userService, IPasswordGenerator passwordGenerator,
            IPaystackPaymentService paystackService, IFlutterwavePaymentService flutterwavePaymentService, IUssdService ussdService)
        {
            _uow = uow;
            _userService = userService;
            _passwordGenerator = passwordGenerator;
            _paystackService = paystackService;
            _flutterwavePaymentService = flutterwavePaymentService;
            _ussdService = ussdService;
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
                .Where(x => x.Waybill == waybillPaymentLog.Waybill && x.IsPaymentSuccessful == true).FirstOrDefault();

            if (paymentLog != null)
            {
                response.Status = false;
                response.Message = $"There was successful transaction for the waybill {waybillPaymentLog.Waybill}";
                response.data.Message = $"There was successful transaction for the waybill {waybillPaymentLog.Waybill}";
                response.data.Status = "failed";                
                return response;
            }
            else
            {
                if (waybillPaymentLog.OnlinePaymentType == OnlinePaymentType.Paystack)
                {
                    response = await AddWaybillPaymentLogForPaystack(waybillPaymentLog);
                }

                if(waybillPaymentLog.OnlinePaymentType == OnlinePaymentType.Flutterwave)
                {
                    //Process Payment for FlutterWave;
                    response = await AddWaybillPaymentLogForFlutterWave(waybillPaymentLog);
                }

                if(waybillPaymentLog.OnlinePaymentType == OnlinePaymentType.USSD)
                {
                    response = await AddWaybillPaymentLogForUSSD(waybillPaymentLog);
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
            string refCode = await GenerateWaybillReferenceCode(waybillPaymentLog.Waybill);
            waybillPaymentLog.Reference = refCode;
            var newPaymentLog = Mapper.Map<WaybillPaymentLog>(waybillPaymentLog);
            _uow.WaybillPaymentLog.Add(newPaymentLog);
            await _uow.CompleteAsync();

            return refCode;
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
                PaystackWebhookDTO response = new PaystackWebhookDTO();

                if (paymentLog.OnlinePaymentType == OnlinePaymentType.Paystack)
                {
                    response = await _paystackService.VerifyAndValidateMobilePayment(paymentLog.Reference);
                }

                if (paymentLog.OnlinePaymentType == OnlinePaymentType.Flutterwave)
                {
                    response = await _flutterwavePaymentService.VerifyAndValidateMobilePayment(paymentLog.Reference);
                }

                if (paymentLog.OnlinePaymentType == OnlinePaymentType.USSD)
                {
                    response = await _ussdService.VerifyAndValidatePayment(paymentLog.Reference);
                }

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

        public async Task<PaystackWebhookDTO> VerifyAndValidatePaymentUsingOTP(string waybill, string pin)
        {
            //check the current transaction on the waybill
            var paymentLog = _uow.WaybillPaymentLog.GetAllAsQueryable()
                .Where(x => x.Waybill == waybill).OrderByDescending(x => x.DateCreated).FirstOrDefault();

            if (paymentLog != null)
            {
                if (paymentLog.OnlinePaymentType == OnlinePaymentType.Paystack)
                {
                    //VerifyAndValidateWaybillForVodafoneMobilePayment
                    var response = await _paystackService.ProcessPaymentForWaybillUsingPin(paymentLog, pin);
                    return response;
                }

                if (paymentLog.OnlinePaymentType == OnlinePaymentType.Flutterwave)
                {
                    var response = await _flutterwavePaymentService.ProcessPaymentForWaybillUsingOTP(paymentLog, pin);
                    return response;
                }
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

        public async Task<PaystackWebhookDTO> VerifyAndValidateWaybillForVodafoneMobilePayment(string waybill, string pin)
        {
            //check the current transaction on the waybill
            var paymentLog = _uow.WaybillPaymentLog.GetAllAsQueryable()
                .Where(x => x.Waybill == waybill).OrderByDescending(x => x.DateCreated).FirstOrDefault();

            if (paymentLog != null)
            {
                if(paymentLog.OnlinePaymentType == OnlinePaymentType.Paystack)
                {
                    var response = await _paystackService.ProcessPaymentForWaybillUsingPin(paymentLog, pin);
                    return response;
                }

                if (paymentLog.OnlinePaymentType == OnlinePaymentType.Paystack)
                {
                    var response = await _flutterwavePaymentService.ProcessPaymentForWaybillUsingOTP(paymentLog, pin);
                    return response;
                }
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
                        //use Reference to represent flutter flwRef -- security code for otp confirmation
                        responseResult.data.Reference = flutterResponse.data.FlwRef;
                        responseResult.data.Status = flutterResponse.data.Status;

                        if (flutterResponse.data.validateInstructions.Instruction != null)
                        {
                            responseResult.data.Message =  flutterResponse.data.validateInstructions.Instruction;
                        }
                        else if(flutterResponse.data.ChargeMessage != null)
                        {
                            responseResult.data.Message = flutterResponse.data.ChargeMessage;
                        }
                        else
                        {
                            responseResult.data.Message = flutterResponse.data.ChargeResponseMessage;
                        }                        
                    }
                    else
                    {
                        responseResult.data.Message = flutterResponse.Message;
                        responseResult.data.Status = flutterResponse.Status;
                    }

                    //update waybill payment log
                    updateWaybillPaymentLog.TransactionStatus = responseResult.data.Status;
                    updateWaybillPaymentLog.TransactionResponse = responseResult.data.Message;
                    updateWaybillPaymentLog.NetworkProvider = responseResult.data.Reference;  //use NetworkProvide to represent flutter flwRef -- security code for otp confirmation
                    await _uow.CompleteAsync();

                    return responseResult;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<PaystackWebhookDTO> AddWaybillPaymentLogForUSSD(WaybillPaymentLogDTO waybillPaymentLog)
        {
            var response = new PaystackWebhookDTO();

            //1. Generate Ref Code
            waybillPaymentLog.Reference = await GenerateWaybillReferenceCode(waybillPaymentLog.Waybill);

            //2. Check out the country the phone number belong to so that
            //conversion rate can occur on the waybill amount 

            //3. Send reguest to Oga USSD 
            var ussdResponse = await ProcessPaymentForUSSD(waybillPaymentLog);

            if(ussdResponse.Status == "success")
            {
                //3. Add record to waybill payment log with the order id
                var newPaymentLog = Mapper.Map<WaybillPaymentLog>(waybillPaymentLog);

                //Network Provider represent OrderId
                newPaymentLog.NetworkProvider = ussdResponse.data.Order_Id;
                _uow.WaybillPaymentLog.Add(newPaymentLog);
                await _uow.CompleteAsync();

                //4. Send SMS to the customer phone number


                //return response
                response.Status = true;
                response.Message = ussdResponse.data.Message;
                response.data.Message = ussdResponse.data.Message;
                response.data.Reference = ussdResponse.Order_Reference;
                response.data.Status = ussdResponse.Status;
                response.data.Display_Text = ussdResponse.Dialing_Code;
            }
            else
            {
                response.Status = false;
                response.Message = ussdResponse.Message;
                response.data.Message = ussdResponse.Message;
                response.data.Status = ussdResponse.Status;

                //       "status": "error",
                //      "message": "I lost my glasses so I am finding it hard to find your credentials on the list of authorised users. Help look for my glasses."

            }

            return response;
        }

        private async Task<USSDResponse> ProcessPaymentForUSSD(WaybillPaymentLogDTO waybillPaymentLog)
        {
            try
            {
                string countryCode = waybillPaymentLog.Currency.Length <= 2 ? waybillPaymentLog.Currency : waybillPaymentLog.Currency.Substring(0, 2);

                var ussdData = new USSDDTO
                {
                    amount = (int)waybillPaymentLog.Amount,
                    msisdn = waybillPaymentLog.PhoneNumber,
                    desc = waybillPaymentLog.Waybill,
                    reference = waybillPaymentLog.Reference,
                    country_code = countryCode
                };

                var responseResult = await _ussdService.ProcessPaymentForUSSD(ussdData);
                return responseResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //private async Task<USSDResponse> ProcessPaymentForUSSDOld(WaybillPaymentLogDTO waybillPaymentLog)
        //{
        //    try
        //    {
        //        var responseResult = new USSDResponse();

        //        //1. Get Token  
        //        string token = ConfigurationManager.AppSettings["UssdToken"];
        //        string privateKey = ConfigurationManager.AppSettings["UssdPrivateKey"];

        //        string merchantId = ConfigurationManager.AppSettings["UssdMerchantID"];
        //        string baseUrl = ConfigurationManager.AppSettings["UssdOgaranyaAPI"];

        //        //2. Encrypt token and private_key to generate public key 
        //        string publicKey = GetPublicKey(token, privateKey);

        //        //3.Post the data
        //        var ussdData = new USSDDTO
        //        {
        //            amount = (int)waybillPaymentLog.Amount,
        //            msisdn = waybillPaymentLog.PhoneNumber,
        //            desc = waybillPaymentLog.Waybill,
        //            reference = waybillPaymentLog.Reference
        //        };

        //        string countryCode = waybillPaymentLog.Currency.Length <= 2 ? waybillPaymentLog.Currency : waybillPaymentLog.Currency.Substring(0, 2);
        //        string pay01Url = baseUrl + merchantId + "/pay/" + countryCode;

        //        using (var client = new HttpClient())
        //        {
        //            client.DefaultRequestHeaders.Accept.Clear();
        //            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //            client.DefaultRequestHeaders.Add("token", token);
        //            client.DefaultRequestHeaders.Add("publickey", publicKey);

        //            var ussdDataInJson = JsonConvert.SerializeObject(ussdData);
        //            var data = new StringContent(ussdDataInJson, Encoding.UTF8, "application/json");
        //            var response = await client.PostAsync(pay01Url, data);
        //            string result = await response.Content.ReadAsStringAsync();

        //            responseResult = JsonConvert.DeserializeObject<USSDResponse>(result);
        //        }
        //        return responseResult;
        //    }
        //    catch(Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //private string GetPublicKey(string token, string privateKey)
        //{
        //    string publicKey = string.Empty;

        //    var bytes = Encoding.UTF8.GetBytes(token + privateKey);

        //    using(var hash = new SHA512Managed())
        //    {
        //        var hashedData =  hash.ComputeHash(bytes);
        //        publicKey = BitConverter.ToString(hashedData).Replace("-", "").ToLower();
        //    }

        //    return publicKey;
        //}

    }
}
