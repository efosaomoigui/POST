using GIGLS.Core.DTO.PaymentTransactions;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Business
{
    public interface IPaymentService : IServiceDependencyMarker
    {
        Task<bool> ProcessPayment(PaymentTransactionDTO paymentDto);
    }
}
