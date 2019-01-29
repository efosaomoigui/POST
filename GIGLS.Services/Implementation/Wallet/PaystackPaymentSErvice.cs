using GIGLS.Core;
using GIGLS.Core.DTO.Wallet;
using GIGLS.Core.IServices.User;
using GIGLS.Core.IServices.Wallet;
using System.Threading.Tasks;
using PayStack.Net;
using GIGLS.Core.DTO.OnlinePayment;
using System.Configuration;
using GIGLS.Core.Enums;

namespace GIGLS.Services.Implementation.Wallet
{
    public class PaystackPaymentService : IPaystackPaymentService
    {
        private readonly IUserService _userService;
        private readonly IWalletService _walletService;
        private readonly IUnitOfWork _uow;

        private string secretKey = ConfigurationManager.AppSettings["PayStackSecret"];

        public PaystackPaymentService(IUserService userService,IWalletService walletService, IUnitOfWork uow)
        {
            _userService = userService;
            _walletService = walletService;
            _uow = uow;
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
                result.data.Amount = verifyResponse.Data.Amount;
                result.data.Gateway_Response = verifyResponse.Data.GatewayResponse;
                result.data.Reference = verifyResponse.Data.Reference;
                result.data.Status = verifyResponse.Data.Status;
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

                    if(walletDto != null)
                    {
                        //use customer code to get customer id
                        var user = await _userService.GetUserByChannelCode(walletDto.CustomerCode);

                        if (user != null)
                            customerId = user.Id;
                    }

                    //update the wallet
                    await _walletService.UpdateWallet(paymentLog.WalletId, new WalletTransactionDTO() {
                        WalletId = paymentLog.WalletId,
                        Amount = verifyResult.data.Amount / 100,
                        CreditDebitType = CreditDebitType.Credit,
                        Description = "Funding made through debit card",
                        PaymentType = Core.Enums.PaymentType.Online,
                        PaymentTypeReference = verifyResult.data.Reference,
                        UserId = customerId
                    }, false);

                    //set IsWalletCredited = true if nothing fail above
                    paymentLog.IsWalletCredited = true;
                }

                //3. update the wallet payment log
                paymentLog.TransactionStatus = verifyResult.data.Status;
                paymentLog.TransactionResponse = verifyResult.data.Gateway_Response;                
                await _uow.CompleteAsync();

                result = true;
            }

            return await Task.FromResult(result);
        }

        public async Task<bool> VerifyAndValidateWallet(string referenceCode)
        {
            bool result = false;

            //1. verify the payment 
            var verifyResult = await VerifyPayment(referenceCode);

            if (verifyResult.Status)
            {
                //get wallet payment log by reference code
                var paymentLog = _uow.WalletPaymentLog.SingleOrDefault(x => x.Reference == referenceCode);

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
                    await _walletService.UpdateWallet(paymentLog.WalletId, new WalletTransactionDTO()
                    {
                        WalletId = paymentLog.WalletId,
                        Amount = verifyResult.data.Amount / 100,
                        CreditDebitType = CreditDebitType.Credit,
                        Description = "Funding made through debit card",
                        PaymentType = PaymentType.Online,
                        PaymentTypeReference = verifyResult.data.Reference,
                        UserId = customerId
                    }, false);

                    //set IsWalletCredited = true if nothing fail above
                    paymentLog.IsWalletCredited = true;
                }

                //3. update the wallet payment log
                paymentLog.TransactionStatus = verifyResult.data.Status;
                paymentLog.TransactionResponse = verifyResult.data.Gateway_Response;
                await _uow.CompleteAsync();

                result = true;
            }

            return await Task.FromResult(result);
        }
    }
}