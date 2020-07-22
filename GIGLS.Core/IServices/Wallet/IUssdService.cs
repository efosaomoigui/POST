using GIGLS.Core.DTO.Wallet;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Wallet
{
    public interface IUssdService : IServiceDependencyMarker
    {
        Task<USSDResponse> ProcessPaymentForUSSD(USSDDTO ussdDto);
        Task<USSDResponse> VerifyAndValidateUSSDPayment(USSDDTO ussdDto);
        

        //Task VerifyAndValidatePayment(FlutterWebhookDTO webhook);
        //Task<string> GetSecurityKey();
        //Task<PaystackWebhookDTO> VerifyAndValidateMobilePayment(string reference);
        //Task<PaystackWebhookDTO> ProcessPaymentForWaybillUsingOTP(WaybillPaymentLog paymentLog, string otp);
    }
}
