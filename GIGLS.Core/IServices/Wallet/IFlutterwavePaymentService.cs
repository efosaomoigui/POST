using GIGLS.Core.Domain.Wallet;
using GIGLS.Core.DTO.OnlinePayment;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Wallet
{
    public interface IFlutterwavePaymentService : IServiceDependencyMarker
    {
        Task VerifyAndValidatePayment(FlutterWebhookDTO webhook);
        Task<string> GetSecurityKey();
        Task<PaystackWebhookDTO> VerifyAndValidateMobilePayment(string reference);
        Task<PaystackWebhookDTO> ProcessPaymentForWaybillUsingOTP(WaybillPaymentLog paymentLog, string otp);
    }
}
