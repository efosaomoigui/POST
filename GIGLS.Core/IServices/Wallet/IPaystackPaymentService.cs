using GIGLS.Core.DTO.Wallet;
using System.Threading.Tasks;
using GIGLS.Core.DTO.OnlinePayment;

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
    }
}
