using GIGLS.Core.DTO.PaymentTransactions;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Business
{
    public interface IPaymentService : IServiceDependencyMarker
    {
        Task<bool> ProcessCashPayment(string waybill, PaymentTransactionDTO paymentDto);
        Task<bool> ProcessPosPayment(string waybill, PaymentTransactionDTO paymentDto);
        Task<bool> ProcessOnlinePayment(string waybill, PaymentTransactionDTO paymentDto);
    }
}
