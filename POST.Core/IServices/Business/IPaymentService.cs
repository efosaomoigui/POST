using POST.Core.DTO.PaymentTransactions;
using System.Threading.Tasks;

namespace POST.Core.IServices.Business
{
    public interface IPaymentService : IServiceDependencyMarker
    {
        Task<bool> ProcessPayment(PaymentTransactionDTO paymentDto);
        Task<bool> ProcessPaymentPartial(PaymentPartialTransactionProcessDTO paymentPartialTransactionProcessDTO);
    }
}
