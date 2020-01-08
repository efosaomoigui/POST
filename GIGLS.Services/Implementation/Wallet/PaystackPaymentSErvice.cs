using GIGLS.Core;
using GIGLS.Core.DTO.Wallet;
using GIGLS.Core.IServices.User;
using GIGLS.Core.IServices.Wallet;
using System.Threading.Tasks;
using PayStack.Net;
using GIGLS.Core.DTO.OnlinePayment;
using System.Configuration;
using GIGLS.Core.Enums;
using GIGLS.Core.Domain;
using System.Net;
using System;
using System.IO;
using Newtonsoft.Json;
using AutoMapper;
using GIGLS.Core.Domain.Wallet;

namespace GIGLS.Services.Implementation.Wallet
{
    public class PaystackPaymentService : IPaystackPaymentService
    {
        private readonly IUserService _userService;
        private readonly IWalletService _walletService;
        private readonly IUnitOfWork _uow;
        private readonly IWalletPaymentLogService _walletPaymentLogService;

        private string secretKey = ConfigurationManager.AppSettings["PayStackSecret"];

        public PaystackPaymentService(IUserService userService, IWalletService walletService, IUnitOfWork uow, IWalletPaymentLogService walletPaymentLogService,
            IWalletTransactionService transactionService)
        {
            _userService = userService;
            _walletService = walletService;
            _uow = uow;
            _walletPaymentLogService = walletPaymentLogService;
            MapperConfig.Initialize();
        }

        //not used for now
        public async Task<bool> MakePayment(string LiveSecret, WalletPaymentLogDTO wpd)
        {
            var api = new PayStackApi(LiveSecret);

            // Initializing a transaction
            var response = api.Transactions.Initialize(wpd.Email, wpd.PaystackAmount);

            //return response.Status;
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

                //2. if the payment successful
                if (verifyResult.data.Status.Equals("success") && !paymentLog.IsWalletCredited)
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
                            customerId = user.Id;
                    }

                    //update the wallet
                    await _walletService.UpdateWallet(paymentLog.WalletId, new WalletTransactionDTO() {
                        WalletId = paymentLog.WalletId,
                        Amount = verifyResult.data.Amount,
                        CreditDebitType = CreditDebitType.Credit,
                        Description = "Funding made through debit card",
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

                result = true;
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

                //2. if the payment successful
                if (verifyResult.data.Status.Equals("success") && !paymentLog.IsWalletCredited)
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
                            customerId = user.Id;
                    }

                    //update the wallet
                    await _walletService.UpdateWallet(paymentLog.WalletId, new WalletTransactionDTO()
                    {
                        WalletId = paymentLog.WalletId,
                        Amount = verifyResult.data.Amount,
                        CreditDebitType = CreditDebitType.Credit,
                        Description = "Funding made through debit card",
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

                result.GatewayResponse = verifyResult.data.Gateway_Response;
                result.Status = verifyResult.data.Status;
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

        private Task ProcessPaymentForWallet(PaystackWebhookDTO webhook)
        {
            throw new NotImplementedException();
        }

        private Task ProcessPaymentForWaybill(PaystackWebhookDTO webhook)
        {
            //1. Verify payment
            //2. Update waybill Payment log
            //3. Process payment for the waybill if successful

            throw new NotImplementedException();
        }

        private WaybillWalletPaymentType GetPackagePaymentType(string refCode){

            if(refCode.StartsWith("wa"))
            {
                return WaybillWalletPaymentType.Wallet;
            }

            return WaybillWalletPaymentType.Waybill;
        }
            
    }
}