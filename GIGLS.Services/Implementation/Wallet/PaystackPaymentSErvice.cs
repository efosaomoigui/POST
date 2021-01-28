using GIGLS.Core;
using GIGLS.Core.DTO.Wallet;
using GIGLS.Core.IServices.User;
using GIGLS.Core.IServices.Wallet;
using System.Threading.Tasks;
using PayStack.Net;
using GIGLS.Core.DTO.OnlinePayment;
using System.Configuration;
using GIGLS.Core.Enums;
using System.Net;
using System;
using System.IO;
using Newtonsoft.Json;
using AutoMapper;
using GIGLS.Core.Domain.Wallet;
using GIGLS.Core.IServices.PaymentTransactions;
using GIGLS.Core.DTO.PaymentTransactions;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Collections.Generic;
using GIGLS.Core.IMessageService;
using GIGLS.Core.DTO;
using System.Linq;
using GIGLS.Core.IServices.Node;
using GIGLS.Core.DTO.Node;

namespace GIGLS.Services.Implementation.Wallet
{
    public class PaystackPaymentService : IPaystackPaymentService
    {
        private readonly IUserService _userService;
        private readonly IWalletService _walletService;
        private readonly IUnitOfWork _uow;
        private readonly IPaymentTransactionService _paymentTransactionService;
        private readonly IMessageSenderService _messageSenderService;
        private readonly INodeService _nodeService;


        private readonly string secretKey = ConfigurationManager.AppSettings["PayStackSecret"];

        public PaystackPaymentService(IUserService userService, IWalletService walletService, IUnitOfWork uow, IPaymentTransactionService paymentTransactionService,
            IMessageSenderService messageSenderService, INodeService nodeService)
        {
            _userService = userService;
            _walletService = walletService;
            _paymentTransactionService = paymentTransactionService;
            _messageSenderService = messageSenderService;
            _nodeService = nodeService;
            _uow = uow;
            MapperConfig.Initialize();
        }

        //not used for now
        public async Task<bool> MakePayment(string LiveSecret, WalletPaymentLogDTO wpd)
        {
            var api = new PayStackApi(LiveSecret);

            // Initializing a transaction
            var response = api.Transactions.Initialize(wpd.Email, wpd.PaystackAmount);

            return response.Status;
        }

        //not used for now
        public async Task<bool> VerifyPayment(string reference, string livesecret)
        {
            var api = new PayStackApi(livesecret);

            // Verifying a transaction
            var verifyResponse = api.Transactions.Verify(reference);

            return verifyResponse.Status;
        }

        public async Task<PaystackWebhookDTO> VerifyPayment(string reference)
        {
            PaystackWebhookDTO result = new PaystackWebhookDTO();

            await Task.Run(() =>
            {
                var api = new PayStackApi(secretKey);

                // Verifying a transaction
                var verifyResponse = api.Transactions.Verify(reference);

                result.Status = verifyResponse.Status;
                result.Message = verifyResponse.Message;
                result.data.Reference = reference;
                result.data.Authorization.AuthorizationCode = verifyResponse.Data.Authorization.AuthorizationCode;
                result.data.Authorization.Bin = verifyResponse.Data.Authorization.Bin;
                result.data.Authorization.Last4 = verifyResponse.Data.Authorization.Last4;
                result.data.Authorization.ExpMonth = verifyResponse.Data.Authorization.ExpMonth;
                result.data.Authorization.ExpYear = verifyResponse.Data.Authorization.ExpYear;
                result.data.Authorization.Channel = verifyResponse.Data.Authorization.Channel;
                result.data.Authorization.CardType = verifyResponse.Data.Authorization.CardType;
                result.data.Authorization.Bank = verifyResponse.Data.Authorization.Bank;
                result.data.Authorization.CountryCode = verifyResponse.Data.Authorization.CountryCode;
                result.data.Authorization.Reusable = verifyResponse.Data.Authorization.Reusable;

                if (verifyResponse.Status)
                {
                    result.data.Gateway_Response = verifyResponse.Data.GatewayResponse;
                    result.data.Status = verifyResponse.Data.Status;
                    result.data.Amount = verifyResponse.Data.Amount / 100;
                }
            });

            return result;
        }

        public async Task<bool> VerifyAndValidateWallet(PaystackWebhookDTO webhook)
        {
            bool result = false;

            //1. verify the payment 
            var verifyResult = await VerifyPayment(webhook.data.Reference);

            if (verifyResult.Status)
            {
                //get wallet payment log by reference code
                var paymentLog = _uow.WalletPaymentLog.SingleOrDefault(x => x.Reference == webhook.data.Reference);

                if (paymentLog == null)
                    return result;

                bool sendPaymentNotification = false;
                var walletDto = new WalletDTO();
                var userPayload = new UserPayload();
                var bonusAddon = new BonusAddOn();

                //2. if the payment successful
                if (verifyResult.data.Status.Equals("success") && !paymentLog.IsWalletCredited && verifyResult.data.Amount == paymentLog.Amount)
                {
                    //a. update the wallet for the customer
                    string customerId = null;  //set customer id to null

                    //get wallet detail to get customer code
                    walletDto = await _walletService.GetWalletById(paymentLog.WalletId);

                    if (walletDto != null)
                    {
                        //use customer code to get customer id
                      var  user = await _userService.GetUserByChannelCode(walletDto.CustomerCode);

                        if (user != null)
                        {
                            customerId = user.Id;
                            userPayload.Email = user.Email;
                            userPayload.UserId = user.Id;
                            userPayload.Reference = webhook.data.Reference;
                            userPayload.Authorization = verifyResult.data.Authorization;
                        }
                    }

                    //if pay was done using Master VIsa card, give some discount
                    bonusAddon = await ProcessBonusAddOnForCardType(verifyResult, paymentLog.PaymentCountryId);

                    //update the wallet
                    await _walletService.UpdateWallet(paymentLog.WalletId, new WalletTransactionDTO() {
                        WalletId = paymentLog.WalletId,
                        Amount = bonusAddon.Amount,
                        CreditDebitType = CreditDebitType.Credit,
                        Description = bonusAddon.Description,
                        PaymentType = PaymentType.Online,
                        PaymentTypeReference = verifyResult.data.Reference,
                        UserId = customerId
                    }, false);

                    //await SendPaymentNotificationAsync(walletDto, paymentLog);
                    sendPaymentNotification = true;
                }

                //3. update the wallet payment log
                if (verifyResult.data.Status != null)
                {
                    paymentLog.IsWalletCredited = true;
                }
                paymentLog.TransactionStatus = verifyResult.data.Status;
                paymentLog.TransactionResponse = verifyResult.data.Gateway_Response;
                await _uow.CompleteAsync();

                if (sendPaymentNotification)
                {
                    await SendPaymentNotificationAsync(walletDto, paymentLog);
                }

                if (bonusAddon.BonusAdded)
                {
                    await SendVisaBonusNotificationAsync(bonusAddon, verifyResult, walletDto);
                }

                //Call Node API for subscription process
                if (paymentLog.TransactionType == WalletTransactionType.ClassSubscription && verifyResult.data.Status.Equals("success") && verifyResult.data.Amount == paymentLog.Amount)
                {
                    if (userPayload != null)
                    {
                        await _nodeService.WalletNotification(userPayload);
                    }
                }

                result = true;
            }

            return await Task.FromResult(result);
        }

        public async Task<PaymentResponse> VerifyAndProcessPayment(string referenceCode)
        {
            PaymentResponse result = new PaymentResponse();            

            //1. Get PaymentLog
            var paymentLog = await _uow.WalletPaymentLog.GetAsync(x => x.Reference == referenceCode);

            if(paymentLog != null)
            {
                if(paymentLog.PaymentCountryId == 76)
                {
                    //process for Ghana
                    result = await ProcessPaymentForWallet(referenceCode);
                }
                else
                {
                    //Process for Nigeria
                    result = await VerifyAndValidateWallet(referenceCode);
                }
            }
            else
            {
                result.Result = false;
                result.Message = "";
                result.GatewayResponse = "Wallet Payment Log Information does not exist";
            }

            return await Task.FromResult(result);
        }

        public async Task<PaymentResponse> VerifyAndValidateWallet(string referenceCode)
        {
            //1. verify the payment 
            var verifyResult = await VerifyPayment(referenceCode);

            PaymentResponse result = new PaymentResponse
            {
                Result = verifyResult.Status,
                Message = verifyResult.Message
            };

            if (verifyResult.Status)
            {
                //get wallet payment log by reference code
                var paymentLog = _uow.WalletPaymentLog.SingleOrDefault(x => x.Reference == referenceCode);

                if (paymentLog == null)
                {
                    result.GatewayResponse = "Wallet Payment Log Information does not exist";
                    return result;
                }

                bool sendPaymentNotification = false;
                var walletDto = new WalletDTO();
                var userPayload = new UserPayload();
                var bonusAddon = new BonusAddOn();

                //2. if the payment successful
                if (verifyResult.data.Status.Equals("success") && !paymentLog.IsWalletCredited && verifyResult.data.Amount == paymentLog.Amount)
                {
                    //a. update the wallet for the customer
                    string customerId = null;  //set customer id to null

                    //get wallet detail to get customer code
                    walletDto = await _walletService.GetWalletById(paymentLog.WalletId);

                    if (walletDto != null)
                    {
                        //use customer code to get customer id
                        var user = await _userService.GetUserByChannelCode(walletDto.CustomerCode);

                        if (user != null)
                        {
                            customerId = user.Id;
                            userPayload.Email = user.Email;
                            userPayload.UserId = user.Id;
                            userPayload.Reference = referenceCode;
                            userPayload.Authorization = verifyResult.data.Authorization;
                        }
                    }

                    //if pay was done using Master VIsa card, give some discount
                    bonusAddon = await ProcessBonusAddOnForCardType(verifyResult, paymentLog.PaymentCountryId);

                    //update the wallet
                    await _walletService.UpdateWallet(paymentLog.WalletId, new WalletTransactionDTO()
                    {
                        WalletId = paymentLog.WalletId,
                        Amount = bonusAddon.Amount,
                        CreditDebitType = CreditDebitType.Credit,
                        Description = bonusAddon.Description,
                        PaymentType = PaymentType.Online,
                        PaymentTypeReference = verifyResult.data.Reference,
                        UserId = customerId
                    }, false);

                    //await SendPaymentNotificationAsync(walletDto, paymentLog);
                    sendPaymentNotification = true;
                }

                //3. update the wallet payment log
                if (verifyResult.data.Status != null)
                {
                    paymentLog.IsWalletCredited = true;
                }
                paymentLog.TransactionStatus = verifyResult.data.Status;
                paymentLog.TransactionResponse = verifyResult.data.Gateway_Response;
                await _uow.CompleteAsync();

                result.GatewayResponse = verifyResult.data.Gateway_Response;
                result.Status = verifyResult.data.Status;

                if (sendPaymentNotification)
                {
                    await SendPaymentNotificationAsync(walletDto, paymentLog);
                }

                if (bonusAddon.BonusAdded)
                {
                    await SendVisaBonusNotificationAsync(bonusAddon, verifyResult, walletDto);
                }

                //Call Node API for subscription process
                if (paymentLog.TransactionType == WalletTransactionType.ClassSubscription && verifyResult.data.Status.Equals("success") && verifyResult.data.Amount == paymentLog.Amount)
                {
                    if (userPayload != null)
                    {
                        await _nodeService.WalletNotification(userPayload);
                    }
                }
            }

            return await Task.FromResult(result);
        }

        public async Task<PaystackWebhookDTO> VerifyPaymentMobile(string reference, string UserId)
        {
            PaystackWebhookDTO result = new PaystackWebhookDTO();
            WalletPaymentLogDTO paymentLog = new WalletPaymentLogDTO();
            WalletTransactionDTO transaction = new WalletTransactionDTO();

            var baseAddress = "https://api.paystack.co/transaction/verify/" + reference;

            var SecKey = string.Format("Bearer {0}", "sk_test_75eb63768f05426fa4f4c2ae68cd451dc10b4ac4");
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            var http = (HttpWebRequest)WebRequest.Create(new Uri(baseAddress));
            http.Headers.Add("Authorization", SecKey);
            http.Accept = "application/json";
            http.ContentType = "application/json";
            http.Method = "GET";

            var response = http.GetResponse();
            var stream = response.GetResponseStream();
            var sr = new StreamReader(stream);
            var content = sr.ReadToEnd();
            var ResponseModel = JsonConvert.DeserializeObject<PaystackWebhookDTO>(content);

            result.Status = ResponseModel.Status;
            result.Message = ResponseModel.Message;
            result.data.Reference = reference;

            if (ResponseModel.data.Status == "success")
            {
                var user = await _userService.GetUserById(UserId);
                var wallet = await _uow.Wallet.GetAsync(x => x.CustomerCode.Equals(user.UserChannelCode));

                result.data.Gateway_Response = ResponseModel.data.Gateway_Response;
                result.data.Status = ResponseModel.data.Status;
                result.data.Amount = ResponseModel.data.Amount / 100;
                paymentLog.Amount = result.data.Amount;
                paymentLog.TransactionResponse = result.data.Gateway_Response;
                paymentLog.TransactionStatus = result.data.Status;
                paymentLog.WalletId = wallet.WalletId;
                paymentLog.Description = "Wallet Fund";
                paymentLog.IsWalletCredited = true;
                paymentLog.PaystackAmount = Convert.ToInt32(paymentLog.Amount);
                paymentLog.Email = user.Email;
                paymentLog.Reference = ResponseModel.data.Reference;
                paymentLog.UserId = user.Id;
                transaction.WalletId = paymentLog.WalletId;
                transaction.Amount = paymentLog.Amount;
                transaction.CreditDebitType = CreditDebitType.Credit;
                transaction.Description = "Funding made through debit card";
                transaction.PaymentType = PaymentType.Online;
                transaction.PaymentTypeReference = paymentLog.Reference;
                transaction.UserId = user.Id;
                transaction.ServiceCentreId = 296;
                transaction.DateOfEntry = DateTime.Now;
                transaction.IsDeferred = false;
                var newWalletTransaction = Mapper.Map<WalletTransaction>(transaction);
                var newPaymentLog = Mapper.Map<WalletPaymentLog>(paymentLog);
                _uow.WalletTransaction.Add(newWalletTransaction);
                _uow.WalletPaymentLog.Add(newPaymentLog);
                wallet.Balance = (wallet.Balance + result.data.Amount);
                await _uow.CompleteAsync();
            }


            return result;
        }

        //Ghana Paystack 
        public async Task VerifyAndValidateMobilePayment(PaystackWebhookDTO webhook)
        {
            WaybillWalletPaymentType waybillWalletPaymentType = GetPackagePaymentType(webhook.data.Reference);

            if(waybillWalletPaymentType == WaybillWalletPaymentType.Waybill)
            {
                await ProcessPaymentForWaybill(webhook);
            }
            else
            {
                await ProcessPaymentForWallet(webhook);
            }
        }

        public async Task<PaystackWebhookDTO> VerifyAndValidateMobilePayment(string reference)
        {
            var webhook = await VerifyGhanaPayment(reference);

            WaybillWalletPaymentType waybillWalletPaymentType = GetPackagePaymentType(reference);

            if (waybillWalletPaymentType == WaybillWalletPaymentType.Waybill)
            {
               await ProcessPaymentForWaybill(webhook);
            }
            else
            {
               await ProcessPaymentForWallet(webhook);
            }

            return webhook;
        }
        
        private async Task<PaystackWebhookDTO> VerifyGhanaPayment(string reference)
        {
            string payStackSecretGhana = ConfigurationManager.AppSettings["PayStackSecretGhana"];

            PaystackWebhookDTO result = new PaystackWebhookDTO();

            await Task.Run(() =>
            {
                var api = new PayStackApi(payStackSecretGhana);

                // Verifying a transaction
                var verifyResponse = api.Transactions.Verify(reference);

                result.Status = verifyResponse.Status;
                result.Message = verifyResponse.Message;
                result.data.Reference = reference;

                if (verifyResponse.Status)
                {
                    result.data.Gateway_Response = verifyResponse.Data.GatewayResponse;
                    result.data.Status = verifyResponse.Data.Status;
                    result.data.Amount = verifyResponse.Data.Amount / 100;
                }
            });

            return result;
        }

        public async Task<ResponseDTO> VerifyBVN(string bvnNo)
        {

            var response = new HttpResponseMessage();
            var result = new ResponseDTO();
            var url = ConfigurationManager.AppSettings["VerifyBVNURL"];
            url = $"{url}{bvnNo}";
            var liveSecretKey = ConfigurationManager.AppSettings["PayStackLiveSecret"];
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            try
            {
                await Task.Run(async () =>
                   {
                       HttpClient client = new HttpClient();
                       client.BaseAddress = new Uri(url);
                       client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                       client.DefaultRequestHeaders.Add("Authorization", $"Bearer {liveSecretKey}");
                       response = await client.GetAsync(url);
                   });

                string jObject = await response.Content.ReadAsStringAsync();
                var content = JsonConvert.DeserializeObject<BVNVerificationDTO>(jObject);
               
                result.Succeeded = content.Status;
                result.Exist = content.Status;
                result.Message = content.Message;
                result.Entity = content.Data;
             
            }
            catch (Exception ex)
            {

                throw;
            }
            return result;
        }

        //Ghana Wallet Payment
        private async Task<bool> ProcessPaymentForWallet(PaystackWebhookDTO webhook)
        {
            bool result = false;

            //1. verify the payment 
            var verifyResult = await VerifyGhanaPayment(webhook.data.Reference);

            var walletDto = new WalletDTO();
            var userPayload = new UserPayload();
            var bonusAddon = new BonusAddOn();

            if (verifyResult.Status)
            {
                //get wallet payment log by reference code
                var paymentLog = _uow.WalletPaymentLog.SingleOrDefault(x => x.Reference == webhook.data.Reference);

                if (paymentLog == null)
                    return result;

                //2. if the payment successful
                if (verifyResult.data.Status.Equals("success") && !paymentLog.IsWalletCredited)
                {
                    //a. update the wallet for the customer
                    string customerId = null;  //set customer id to null

                    //get wallet detail to get customer code
                    walletDto = await _walletService.GetWalletById(paymentLog.WalletId);

                    if (walletDto != null)
                    {
                        //use customer code to get customer id
                        var user = await _userService.GetUserByChannelCode(walletDto.CustomerCode);

                        if (user != null)
                        {
                            customerId = user.Id;
                            userPayload.Email = user.Email;
                            userPayload.UserId = user.Id;
                            userPayload.Reference = webhook.data.Reference;
                            userPayload.Authorization = verifyResult.data.Authorization;
                        }
                    }

                    //if pay was done using Master VIsa card, give some discount
                    bonusAddon = await ProcessBonusAddOnForCardType(verifyResult, paymentLog.PaymentCountryId);

                    //update the wallet
                    await _walletService.UpdateWallet(paymentLog.WalletId, new WalletTransactionDTO()
                    {
                        WalletId = paymentLog.WalletId,
                        Amount = bonusAddon.Amount,
                        CreditDebitType = CreditDebitType.Credit,
                        Description = bonusAddon.Description,
                        PaymentType = PaymentType.Online,
                        PaymentTypeReference = verifyResult.data.Reference,
                        UserId = customerId
                    }, false);
                }

                //3. update the wallet payment log
                if (verifyResult.data.Status != null)
                {
                    paymentLog.IsWalletCredited = true;
                }
                paymentLog.TransactionStatus = verifyResult.data.Status;
                paymentLog.TransactionResponse = verifyResult.data.Gateway_Response;
                await _uow.CompleteAsync();

                if (bonusAddon.BonusAdded)
                {
                    await SendVisaBonusNotificationAsync(bonusAddon, verifyResult, walletDto);
                }

                //Call Node API for subscription process
                if (paymentLog.TransactionType == WalletTransactionType.ClassSubscription && verifyResult.data.Status.Equals("success") && verifyResult.data.Amount == paymentLog.Amount)
                {
                    if (userPayload != null)
                    {
                        await _nodeService.WalletNotification(userPayload);
                    }
                }

                result = true;
            }

            return await Task.FromResult(result);
        }

        private async Task<PaymentResponse> ProcessPaymentForWallet(string referenceCode)
        {
            //1. verify the payment 
            var verifyResult = await VerifyGhanaPayment(referenceCode);

            PaymentResponse result = new PaymentResponse()
            {
                Result = verifyResult.Status,
                Message = verifyResult.Message
            };

            bool sendPaymentNotification = false;
            var walletDto = new WalletDTO();
            var userPayload = new UserPayload();
            var bonusAddon = new BonusAddOn();

            if (verifyResult.Status)
            {
                //get wallet payment log by reference code
                var paymentLog = _uow.WalletPaymentLog.SingleOrDefault(x => x.Reference == referenceCode);

                if (paymentLog == null)
                {
                    result.GatewayResponse = "Wallet Payment Log Information does not exist";
                    return result;
                }

                //2. if the payment successful
                if (verifyResult.data.Status.Equals("success") && !paymentLog.IsWalletCredited)
                {
                    //a. update the wallet for the customer
                    string customerId = null;  //set customer id to null

                    //get wallet detail to get customer code
                    walletDto = await _walletService.GetWalletById(paymentLog.WalletId);

                    if (walletDto != null)
                    {
                        //use customer code to get customer id
                        var user = await _userService.GetUserByChannelCode(walletDto.CustomerCode);

                        if (user != null)
                        {
                            customerId = user.Id;
                            userPayload.Email = user.Email;
                            userPayload.UserId = user.Id;
                            userPayload.Reference = referenceCode;
                            userPayload.Authorization = verifyResult.data.Authorization;
                        }
                    }

                    //if pay was done using Master VIsa card, give some discount
                    bonusAddon = await ProcessBonusAddOnForCardType(verifyResult, paymentLog.PaymentCountryId);

                    //update the wallet
                    await _walletService.UpdateWallet(paymentLog.WalletId, new WalletTransactionDTO()
                    {
                        WalletId = paymentLog.WalletId,
                        Amount = bonusAddon.Amount,
                        CreditDebitType = CreditDebitType.Credit,
                        Description = bonusAddon.Description,
                        PaymentType = PaymentType.Online,
                        PaymentTypeReference = verifyResult.data.Reference,
                        UserId = customerId
                    }, false);

                    //await SendPaymentNotificationAsync(walletDto, paymentLog);
                    sendPaymentNotification = true;
                }

                //3. update the wallet payment log
                if (verifyResult.data.Status != null)
                {
                    paymentLog.IsWalletCredited = true;
                }
                paymentLog.TransactionStatus = verifyResult.data.Status;
                paymentLog.TransactionResponse = verifyResult.data.Gateway_Response;
                await _uow.CompleteAsync();

                //update return response
                result.GatewayResponse = verifyResult.data.Gateway_Response;
                result.Status = verifyResult.data.Status;

                if (sendPaymentNotification)
                {
                    await SendPaymentNotificationAsync(walletDto, paymentLog);
                }

                if (bonusAddon.BonusAdded)
                {
                    await SendVisaBonusNotificationAsync(bonusAddon, verifyResult, walletDto);
                }

                //Call Node API for subscription process
                if (paymentLog.TransactionType == WalletTransactionType.ClassSubscription && verifyResult.data.Status.Equals("success") && verifyResult.data.Amount == paymentLog.Amount)
                {
                    if (userPayload != null)
                    {
                        await _nodeService.WalletNotification(userPayload);
                    }
                }
            }
            {
                if (verifyResult.data.Status != null)
                {
                    result.Status = verifyResult.data.Status;
                    result.GatewayResponse = verifyResult.data.Gateway_Response;
                }
            }

            return await Task.FromResult(result);
        }

        private async Task<bool> ProcessPaymentForWaybill(PaystackWebhookDTO webhook)
        {
            bool result = false;

            //1. verify the payment 
            var verifyResult = await VerifyGhanaPayment(webhook.data.Reference);
            
            if (verifyResult.Status)
            {
                //get wallet payment log by reference code
                var paymentLog = await _uow.WaybillPaymentLog.GetAsync(x => x.Reference == webhook.data.Reference);

                if (paymentLog == null)
                    return result;

                //2. if the payment successful
                if (verifyResult.data.Status.Equals("success") && !paymentLog.IsWaybillSettled && verifyResult.data.Amount == paymentLog.Amount)
                {
                    //3. Process payment for the waybill if successful
                    PaymentTransactionDTO paymentTransaction = new PaymentTransactionDTO
                    {
                        Waybill = paymentLog.Waybill,
                        PaymentType  = PaymentType.Online,
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
                paymentLog.TransactionResponse = verifyResult.data.Gateway_Response;
                await _uow.CompleteAsync();
                result = true;
            }

            return await Task.FromResult(result);
        }
        
        private WaybillWalletPaymentType GetPackagePaymentType(string refCode){

            if(refCode.StartsWith("wb"))
            {
                return WaybillWalletPaymentType.Waybill;
            }

            return WaybillWalletPaymentType.Wallet;
        }

        public async Task<PaystackWebhookDTO> ProcessPaymentForWaybillUsingPin(WaybillPaymentLog waybillPaymentLog, string pin)
        {
            //1. verify the payment 
            var verifyResult = await SubmitPinForPayment(waybillPaymentLog.Reference, pin);

            if (verifyResult.Status)
            {
                //get wallet payment log by reference code
                var paymentLog = await _uow.WaybillPaymentLog.GetAsync(x => x.Reference == waybillPaymentLog.Reference);

                if (paymentLog == null)
                {
                    verifyResult.Message = "No payment log registered!!!";
                    return verifyResult;
                }

                //2. if the payment successful
                if (verifyResult.data.Status.Equals("success") && !paymentLog.IsWaybillSettled)
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
                paymentLog.TransactionResponse = verifyResult.data.Gateway_Response;
                await _uow.CompleteAsync();
            }

            return await Task.FromResult(verifyResult);
        }
        
        private async Task<PaystackWebhookDTO> SubmitPinForPayment(string reference, string pin)
        {
            try
            {
                string payStackSecretGhana = ConfigurationManager.AppSettings["PayStackSecretGhana"];

                string pinUrl = "/submit_otp";
                string payStackChargeAPI = ConfigurationManager.AppSettings["PayStackChargeAPI"] + pinUrl;

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", payStackSecretGhana);
                    
                    var dic = new Dictionary<string, string>
                    {
                        { "otp",  pin},
                        { "reference", reference }
                    };
                    
                    var json = JsonConvert.SerializeObject(dic);
                    var data = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(payStackChargeAPI, data);
                    string result = await response.Content.ReadAsStringAsync();
                    var paystackResponse = JsonConvert.DeserializeObject<PaystackWebhookDTO>(result);
                    return paystackResponse;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task SendPaymentNotificationAsync(WalletDTO walletDto, WalletPaymentLog paymentLog)
        {
            if (walletDto != null)
            {
                walletDto.Balance = walletDto.Balance + paymentLog.Amount;

                var message = new MessageDTO()
                {
                    CustomerCode = walletDto.CustomerCode,
                    CustomerName = walletDto.CustomerName,
                    ToEmail = walletDto.CustomerEmail,
                    To = walletDto.CustomerEmail,
                    Currency = walletDto.Country.CurrencySymbol,
                    Body = walletDto.Balance.ToString("N"),
                    Amount = paymentLog.Amount.ToString("N"),
                    Date = paymentLog.DateCreated.ToString("dd-MM-yyyy")
                };

                //send mail to customer
                await _messageSenderService.SendPaymentNotificationAsync(message);

                //send a copy to chairman
                var chairmanEmail = await _uow.GlobalProperty.GetAsync(s => s.Key == GlobalPropertyType.ChairmanEmail.ToString() && s.CountryId == 1);

                if (chairmanEmail != null)
                {
                    //seperate email by comma and send message to those email
                    string[] chairmanEmails = chairmanEmail.Value.Split(',').ToArray();

                    foreach (string email in chairmanEmails)
                    {
                        message.ToEmail = email;
                        await _messageSenderService.SendPaymentNotificationAsync(message);
                    }
                }
            }
        }

        private async Task SendPaymentNotificationAsyncOld(WalletDTO walletDto, WalletPaymentLog paymentLog)
        {
            if (walletDto != null)
            {
                var walletBalance = await _uow.Wallet.GetAsync(x => x.WalletId == walletDto.WalletId);

                if (walletBalance != null)
                {
                    var message = new MessageDTO()
                    {
                        CustomerCode = walletDto.CustomerCode,
                        CustomerName = walletDto.CustomerName,
                        ToEmail = walletDto.CustomerEmail,
                        To = walletDto.CustomerEmail,
                        Currency = walletDto.Country.CurrencySymbol,
                        Body = walletBalance.Balance.ToString("N"),
                        Amount = paymentLog.Amount.ToString("N"),
                        Date = paymentLog.DateCreated.ToString("dd-MM-yyyy")
                    };

                    //send mail to customer
                    await _messageSenderService.SendPaymentNotificationAsync(message);

                    //send a copy to chairman
                    var chairmanEmail = await _uow.GlobalProperty.GetAsync(s => s.Key == GlobalPropertyType.ChairmanEmail.ToString() && s.CountryId == 1);

                    if (chairmanEmail != null)
                    {
                        //seperate email by comma and send message to those email
                        string[] chairmanEmails = chairmanEmail.Value.Split(',').ToArray();

                        foreach (string email in chairmanEmails)
                        {
                            message.ToEmail = email;
                            await _messageSenderService.SendPaymentNotificationAsync(message);
                        }
                    }
                }
            }
        }

        private async Task SendVisaBonusNotificationAsync(BonusAddOn bonusAddon, PaystackWebhookDTO verifyResult, WalletDTO walletDto)
        {
            string body = $"{bonusAddon.Description} / Bin {verifyResult.data.Authorization.Bin} / Ref code {verifyResult.data.Reference}  / Bank {verifyResult.data.Authorization.Bank}";

            var message = new MessageDTO()
            {
                Subject = "Visa Commercial Card Bonus",
                CustomerCode = walletDto.CustomerEmail,  
                CustomerName = walletDto.CustomerName,
                Body = body                
            };

            //send a copy to chairman
            var visaBonusEmail = await _uow.GlobalProperty.GetAsync(s => s.Key == GlobalPropertyType.VisaBonusEmail.ToString() && s.CountryId == 1);

            if (visaBonusEmail != null)
            {
                //seperate email by comma and send message to those email
                string[] emails = visaBonusEmail.Value.Split(',').ToArray();

                foreach (string email in emails)
                {
                    message.ToEmail = email;
                    await _messageSenderService.SendEcommerceRegistrationNotificationAsync(message);
                }
            }
        }

        private async Task<BonusAddOn> ProcessBonusAddOnForCardType(PaystackWebhookDTO verifyResult, int countryId)
        {
            BonusAddOn result = new BonusAddOn
            {
                Description = "Funding made through debit card.",
                Amount = verifyResult.data.Amount
            };

            if (verifyResult.data.Authorization.CardType != null)
            {
                if (verifyResult.data.Authorization.CardType.Contains("visa"))
                {
                    bool isPresent = await IsTheCardInTheList(verifyResult.data.Authorization.Bin, countryId);
                    if (isPresent)
                    {
                        result.Amount = await CalculateCardBonus(result.Amount, countryId);
                        result.Description = $"{result.Description}. Bonus Added for using Visa Commercial Card";
                        result.BonusAdded = true;
                    }
                }
            }

            return result;
        }

        private async Task<decimal> CalculateCardBonus(decimal amount, int countryId)
        {
            var global = await _uow.GlobalProperty.GetAsync(s => s.Key == GlobalPropertyType.VisaBusinessCardBonus.ToString() && s.CountryId == countryId);
            if (global != null)
            {
                decimal bonusPercentage = decimal.Parse(global.Value);
                decimal bonusValue = bonusPercentage / 100M;
                decimal price = amount * bonusValue;
                amount = amount + price;
            }
            return amount;
        }

        private async Task<bool> IsTheCardInTheList(string bin, int countryId)
        {
            bool result = false;
            var global = await _uow.GlobalProperty.GetAsync(s => s.Key == GlobalPropertyType.VisaBusinessCardList.ToString() && s.CountryId == countryId);
            if (global != null)
            {
                int.TryParse(bin, out int binInt);

                List<int> visaList = new List<int>(Array.ConvertAll(global.Value.Split(','), int.Parse));
                if (visaList.Contains(binInt))
                {
                    result = true;
                }
            }
            return result;
        }
    }
}