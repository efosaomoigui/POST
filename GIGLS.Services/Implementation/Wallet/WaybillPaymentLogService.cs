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
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Services.Implementation.Wallet
{
    public class WaybillPaymentLogService : IWaybillPaymentLogService
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserService _userService;
        private readonly IPasswordGenerator _passwordGenerator;

        public WaybillPaymentLogService(IUnitOfWork uow, IUserService userService, IPasswordGenerator passwordGenerator)
        {
            _uow = uow;
            _userService = userService;
            _passwordGenerator = passwordGenerator;
            MapperConfig.Initialize();
        }

        public async Task<PaymentInitiate> AddWaybillPaymentLog(WaybillPaymentLogDTO waybillPaymentLog)
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

        public async Task<PaymentInitiate> AddWaybillPaymentLogForPaystack(WaybillPaymentLogDTO waybillPaymentLog)
        {
            //1. Generate reference code
            string reference = "wb-" + waybillPaymentLog.Waybill + "-" + await _passwordGenerator.Generate();
            
            //2. Log the detail
            
            if (waybillPaymentLog.UserId == null)
            {
                waybillPaymentLog.UserId = await _userService.GetCurrentUserId();
            }

            var newPaymentLog = Mapper.Map<WaybillPaymentLog>(waybillPaymentLog);

            _uow.WaybillPaymentLog.Add(newPaymentLog);
            await _uow.CompleteAsync();

            //3. send the request to paystack gateway
            await ProcessMobilePaymentForPaystack(waybillPaymentLog);
            
            return new PaymentInitiate
            {
                IsPaymentSuccessful = false,
                TransactionId = reference
            };
        }

        public async Task ProcessMobilePaymentForPaystack(WaybillPaymentLogDTO waybillPaymentLog)
        {
            try
            {
                string payStackSecretGhana = ConfigurationManager.AppSettings["PayStackSecretGhana"];
                string payStackChargeAPI = ConfigurationManager.AppSettings["PayStackChargeAPI"];


                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                using (var client =  new HttpClient())
                {
                    //client.BaseAddress = new Uri(payStackChargeAPI);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(payStackChargeAPI);

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
                }
                

                //var SecKey = string.Format("Bearer {0}", payStackSecretGhana);
                //var http = (HttpWebRequest)WebRequest.Create(new Uri(payStackChargeAPI));
                //http.Headers.Add("Authorization", SecKey);
                //http.Accept = "application/json";
                //http.ContentType = "application/json";
                //http.Method = "POST";

                //var smsURL = await ReturnValidUrl(message);
                //var smsApiKey = ConfigurationManager.AppSettings["smsApiKey"];

                ////ogosms url format
                //var finalURL = $"{smsURL}&password={smsApiKey}&sender={message.From}&numbers={message.To}&message={message.FinalBody}&response=json&unicode=0";
                //var httpWebRequest = (HttpWebRequest)WebRequest.Create(finalURL);

                //using (var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                //{
                //    using (var sr = new StreamReader(httpResponse.GetResponseStream()))
                //    {
                //        result = sr.ReadToEnd();
                //    }
                //}

                //result = GetOGOSMSResponseMessage(result);


                //var baseAddress = "https://api.paystack.co/transaction/verify/" + reference;

                //var SecKey = string.Format("Bearer {0}", "sk_test_75eb63768f05426fa4f4c2ae68cd451dc10b4ac4");
                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                //var http = (HttpWebRequest)WebRequest.Create(new Uri(baseAddress));
                //http.Headers.Add("Authorization", SecKey);
                //http.Accept = "application/json";
                //http.ContentType = "application/json";
                //http.Method = "GET";


            }
            catch (Exception ex)
            {
                throw ex;
            }


            //1. Post the request to paystack

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
    }
}
