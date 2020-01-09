using AutoMapper;
using GIGLS.Core;
using GIGLS.Core.Domain.Wallet;
using GIGLS.Core.DTO.OnlinePayment;
using GIGLS.Core.DTO.Wallet;
using GIGLS.Core.IServices.User;
using GIGLS.Core.IServices.Utility;
using GIGLS.Core.IServices.Wallet;
using GIGLS.CORE.DTO.Shipments;
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
            //check the current transaction on the waybill
            var paymentLog = _uow.WaybillPaymentLog.GetAllAsQueryable()
                .Where(x => x.Waybill == waybillPaymentLog.Waybill).OrderByDescending(x => x.DateCreated).FirstOrDefault();

            var response = new PaystackWebhookDTO();

            if (paymentLog != null)
            {
                //think on how to solve this later but for now, return this below
                return response;
            }
            
            if (waybillPaymentLog.UserId == null)
            {
                waybillPaymentLog.UserId = await _userService.GetCurrentUserId();
            }

            if(waybillPaymentLog.OnlinePaymentType == Core.Enums.OnlinePaymentType.Paystack)
            {
                response = await AddWaybillPaymentLogForPaystack(waybillPaymentLog);
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

        private async Task<PaystackWebhookDTO> AddWaybillPaymentLogForPaystack(WaybillPaymentLogDTO waybillPaymentLog)
        {    
            waybillPaymentLog.Reference = await GenerateWaybillReferenceCode(waybillPaymentLog.Waybill);
            var newPaymentLog = Mapper.Map<WaybillPaymentLog>(waybillPaymentLog);
            _uow.WaybillPaymentLog.Add(newPaymentLog);
            await _uow.CompleteAsync();

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
                        amount = waybillPaymentLog.Amount,
                        currency = waybillPaymentLog.Currency,
                        email = waybillPaymentLog.Email,
                        reference = waybillPaymentLog.Reference,                        
                        mobile_money = new Mobile_Money
                        {
                            phone = waybillPaymentLog.PhoneNumber,
                            provider = waybillPaymentLog.Provider
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
            var paymentLog = await _uow.WaybillPaymentLog.FindAsync(x => x.Waybill == waybill);
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
                .Where(x => x.Waybill == waybill).OrderByDescending(x => x.DateCreated).FirstOrDefault();
            
            if (paymentLog != null)
            {
                var response = await _paystackService.VerifyAndValidateMobilePayment(paymentLog.Reference);
                return response;
            }
                        
            return new PaystackWebhookDTO { 
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
                var response = await _paystackService.VerifyAndValidateMobilePayment(paymentLog.Reference);
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
    }
}
