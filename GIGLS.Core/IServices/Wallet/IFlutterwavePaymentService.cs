using GIGLS.Core.DTO.OnlinePayment;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Wallet
{
    public interface IFlutterwavePaymentService : IServiceDependencyMarker
    {
        Task VerifyAndValidatePayment(FlutterWebhookDTO webhook);
        Task<string> GetSecurityKey();
    }
}
