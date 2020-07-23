using GIGLS.Core;
using GIGLS.Core.DTO.OnlinePayment;
using GIGLS.Core.DTO.PaymentTransactions;
using GIGLS.Core.DTO.Wallet;
using GIGLS.Core.Enums;
using GIGLS.Core.IServices.PaymentTransactions;
using GIGLS.Core.IServices.User;
using GIGLS.Core.IServices.Wallet;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Services.Implementation.Wallet
{
    public class UssdService : IUssdService
    {
        private readonly IUserService _userService;
        private readonly IWalletService _walletService;
        private readonly IUnitOfWork _uow;
        private readonly IPaymentTransactionService _paymentTransactionService;

        public UssdService(IUserService userService, IWalletService walletService, IUnitOfWork uow, IPaymentTransactionService paymentTransactionService)
        {
            _userService = userService;
            _walletService = walletService;
            _paymentTransactionService = paymentTransactionService;
            _uow = uow;
            MapperConfig.Initialize();
        }

        public async Task<USSDResponse> ProcessPaymentForUSSD(USSDDTO ussdDto)
        {
            try
            {
                var responseResult = new USSDResponse();

                //1. Get Token  
                string token = ConfigurationManager.AppSettings["UssdToken"];

                //2. Encrypt token and private_key to generate public key 
                string publicKey = GetPublicKey(token);

                ussdDto.country_code = ussdDto.country_code.Length <= 2 ? ussdDto.country_code : ussdDto.country_code.Substring(0, 2);
                string pay01Url = GetBaseUrl() + "/pay/" + ussdDto.country_code;

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("token", token);
                    client.DefaultRequestHeaders.Add("publickey", publicKey);

                    var ussdDataInJson = JsonConvert.SerializeObject(ussdDto);
                    var data = new StringContent(ussdDataInJson, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(pay01Url, data);
                    string result = await response.Content.ReadAsStringAsync();

                    responseResult = JsonConvert.DeserializeObject<USSDResponse>(result);
                }

                //4. Send SMS to the customer phone number

                return responseResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<USSDResponse> GetPaymentStatus(USSDDTO ussdDto)
        {
            try
            {
                var responseResult = new USSDResponse();

                //1. Get Token  
                string token = ConfigurationManager.AppSettings["UssdToken"];                

                //2. Encrypt token and private_key to generate public key 
                string publicKey = GetPublicKey(token);

                ussdDto.country_code = ussdDto.country_code.Length <= 2 ? ussdDto.country_code : ussdDto.country_code.Substring(0, 2);

                //reference represent order_reference
                string pay01Url = GetBaseUrl() + "/payment/" + ussdDto.reference + "/status/" + ussdDto.country_code;

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("token", token);
                    client.DefaultRequestHeaders.Add("publickey", publicKey);

                    var ussdDataInJson = JsonConvert.SerializeObject(ussdDto);
                    var data = new StringContent(ussdDataInJson, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(pay01Url, data);
                    string result = await response.Content.ReadAsStringAsync();

                    responseResult = JsonConvert.DeserializeObject<USSDResponse>(result);
                }
                return responseResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string GetPublicKey(string token)
        {           
            string privateKey = ConfigurationManager.AppSettings["UssdPrivateKey"];

            string publicKey = string.Empty;

            var bytes = Encoding.UTF8.GetBytes(token + privateKey);

            using (var hash = new SHA512Managed())
            {
                var hashedData = hash.ComputeHash(bytes);
                publicKey = BitConverter.ToString(hashedData).Replace("-", "").ToLower();
            }

            return publicKey;
        }

        private string GetBaseUrl()
        {
            string merchantId = ConfigurationManager.AppSettings["UssdMerchantID"];
            string baseUrl = ConfigurationManager.AppSettings["UssdOgaranyaAPI"];
            return baseUrl + merchantId;
        }

        public async Task<PaystackWebhookDTO> VerifyAndValidatePayment(string reference)
        {
            PaystackWebhookDTO response = new PaystackWebhookDTO();

            WaybillWalletPaymentType waybillWalletPaymentType = GetPackagePaymentType(reference);

            if (waybillWalletPaymentType == WaybillWalletPaymentType.Waybill)
            {
                response = await ProcessPaymentForWaybill(reference);
            }
            else
            {
               response = await ProcessPaymentForWallet(reference);
            }

            return response;
        }

        private async Task<PaystackWebhookDTO> ProcessPaymentForWallet(string reference)
        {
            //1. get wallet payment log by reference code
            var paymentLog = await _uow.WalletPaymentLog.GetAsync(x => x.Reference == reference);

            if (paymentLog != null)
            {
                //Get the country currency code
                var countryCurrency = await _uow.Country.GetAsync(x => x.CountryId == paymentLog.PaymentCountryId);

                var ussdData = new USSDDTO
                {
                    reference = paymentLog.ExternalReference,
                    country_code = countryCurrency.CurrencyCode
                };

                //1. verify the payment 
                var verifyResult = await GetPaymentStatus(ussdData);

                if (verifyResult.Status == "success")
                {
                    if (verifyResult.data != null)
                    {
                        //2. if the payment successful
                        if (verifyResult.data.Status.ToLower() == "successful" && !paymentLog.IsWalletCredited)
                        {
                            //a. update the wallet for the customer
                            string customerId = null;  //set customer id to null

                            //get wallet detail to get customer code
                            var walletDto = await _walletService.GetWalletById(paymentLog.WalletId);

                            if (walletDto != null)
                            {
                                //use customer code to get customer id
                                var user = await _userService.GetUserByChannelCode(walletDto.CustomerCode);

                                if (user != null)
                                {
                                    customerId = user.Id;
                                }
                            }

                            //update the wallet
                            await _walletService.UpdateWallet(paymentLog.WalletId, new WalletTransactionDTO()
                            {
                                WalletId = paymentLog.WalletId,
                                Amount = paymentLog.Amount,
                                CreditDebitType = CreditDebitType.Credit,
                                Description = "Funding made through online payment",
                                PaymentType = PaymentType.Online,
                                PaymentTypeReference = paymentLog.Reference,
                                UserId = customerId
                            }, false);
                        }

                        paymentLog.TransactionStatus = verifyResult.data.Status;
                        paymentLog.TransactionResponse = verifyResult.data.Message;
                    }
                    await _uow.CompleteAsync();
                }

                return await Task.FromResult(ManageReturnResponse(verifyResult));
            }

            return new PaystackWebhookDTO
            {
                Status = false,
                Message = $"No online payment process occurred for the reference {reference}",
                data = new Core.DTO.OnlinePayment.Data
                {
                    Message = $"No online payment process occurred for the reference {reference}",
                    Status = "failed"
                }
            };
        }

        private async Task<PaystackWebhookDTO> ProcessPaymentForWaybill(string reference)
        {
            //1. get wallet payment log by reference code
             var paymentLog = await _uow.WaybillPaymentLog.GetAsync(x => x.Reference == reference); 

            if(paymentLog != null)
            {
                var ussdData = new USSDDTO
                {
                    reference = paymentLog.NetworkProvider,
                    country_code = paymentLog.Currency
                };

                //1. verify the payment 
                var verifyResult = await GetPaymentStatus(ussdData);

                if (verifyResult.Status == "success")
                {
                    if(verifyResult.data != null)
                    {
                        //2. if the payment successful
                        if (verifyResult.data.Status.ToLower() == "successful" && !paymentLog.IsWaybillSettled)
                        {
                            //3. Process payment for the waybill if successful
                            PaymentTransactionDTO paymentTransaction = new PaymentTransactionDTO
                            {
                                Waybill = paymentLog.Waybill,
                                PaymentType = PaymentType.Online,
                                TransactionCode = paymentLog.Reference,
                                UserId = paymentLog.UserId
                            };

                            var processWaybillPayment = await _paymentTransactionService.ProcessPaymentTransaction(paymentTransaction);

                            if (processWaybillPayment)
                            {
                                //2. Update waybill Payment log
                                paymentLog.IsPaymentSuccessful = true;
                                paymentLog.IsWaybillSettled = true;
                            }
                        }

                        paymentLog.TransactionStatus = verifyResult.data.Status;
                        paymentLog.TransactionResponse = verifyResult.data.Message;
                    }
                    await _uow.CompleteAsync();
                }

                return await Task.FromResult(ManageReturnResponse(verifyResult));
            }

            return new PaystackWebhookDTO
            {
                Status = false,
                Message = $"No online payment process occurred for the waybill/reference {reference}",
                data = new Core.DTO.OnlinePayment.Data
                {
                    Message = $"No online payment process occurred for the waybill/reference {reference}",
                    Status = "failed"
                }
            };
        }

        private WaybillWalletPaymentType GetPackagePaymentType(string refCode)
        {
            if (refCode.StartsWith("wb"))
            {
                return WaybillWalletPaymentType.Waybill;
            }

            return WaybillWalletPaymentType.Wallet;
        }

        private PaystackWebhookDTO ManageReturnResponse(USSDResponse ussdResponse)
        {
            var response = new PaystackWebhookDTO();

            if (ussdResponse.Status.Equals("success"))
            {
                response.Status = true;
            }

            if (ussdResponse.data != null)
            {
                response.Message = ussdResponse.data.Message;
                response.data.Gateway_Response = ussdResponse.data.Message;
                response.data.Status = ussdResponse.data.Status;
                response.data.Message = ussdResponse.data.Message;
                response.data.Display_Text = ussdResponse.Message;
            }
            else
            {
                response.data.Message = ussdResponse.Message;
                response.data.Gateway_Response = ussdResponse.Message;
                response.data.Status = ussdResponse.Status;
            }

            return response;
        }

    }
}
