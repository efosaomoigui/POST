using POST.Core.Domain.Wallet;
using POST.Core.DTO.OnlinePayment;
using System.Threading.Tasks;

namespace POST.Core.IServices.Wallet
{
    public interface ISterlingPaymentService : IServiceDependencyMarker
    {
        Task VerifyAndValidatePayment(FlutterWebhookDTO webhook);
        Task<string> GetSecurityKey();
        Task<PaystackWebhookDTO> VerifyAndValidateMobilePayment(string reference);
        Task<PaystackWebhookDTO> ProcessPaymentForWaybillUsingOTP(WaybillPaymentLog paymentLog, string otp);
    }
}
