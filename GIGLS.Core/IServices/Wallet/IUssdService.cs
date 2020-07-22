using GIGLS.Core.DTO.OnlinePayment;
using GIGLS.Core.DTO.Wallet;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Wallet
{
    public interface IUssdService : IServiceDependencyMarker
    {
        Task<USSDResponse> ProcessPaymentForUSSD(USSDDTO ussdDto);
        Task<USSDResponse> GetPaymentStatus(USSDDTO ussdDto);
        Task<PaystackWebhookDTO> VerifyAndValidatePayment(string reference);


        //Task VerifyAndValidatePayment(FlutterWebhookDTO webhook);
        //Task<string> GetSecurityKey();
        //Task<PaystackWebhookDTO> ProcessPaymentForWaybillUsingOTP(WaybillPaymentLog paymentLog, string otp);
    }
}
