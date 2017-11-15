using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Account
{
    public interface IPaymentService : IServiceDependencyMarker
    {
        Task<object> ConfirmPayment(string waybill);
    }
}
