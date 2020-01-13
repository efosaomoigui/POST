using GIGLS.Core.DTO.Wallet;
using System.Threading.Tasks;
using GIGLS.Core.DTO.OnlinePayment;
using GIGLS.Core.Domain.Wallet;

namespace GIGLS.Core.IServices.Wallet
{
    public interface IPaystackPaymentService : IServiceDependencyMarker 
    {
        Task<bool> MakePayment(string LiveSecret, WalletPaymentLogDTO wpd);
        Task<bool> VerifyPayment(string reference, string livesecret);  
        Task<PaystackWebhookDTO> VerifyPayment(string reference);
        Task<bool> VerifyAndValidateWallet(PaystackWebhookDTO webhook);
        Task<PaymentResponse> VerifyAndValidateWallet(string referenceCode);
        Task<PaystackWebhookDTO> VerifyPaymentMobile(string reference, string UserId);

        Task VerifyAndValidateMobilePayment(PaystackWebhookDTO webhook);
        Task<PaystackWebhookDTO> VerifyAndValidateMobilePayment(string reference);
        Task<PaystackWebhookDTO> ProcessPaymentForWaybillUsingPin(WaybillPaymentLog waybillPaymentLog, string pin);
    }
}
