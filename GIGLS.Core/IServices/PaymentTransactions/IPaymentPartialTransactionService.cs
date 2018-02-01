using GIGLS.Core.DTO.PaymentTransactions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.PaymentTransactions
{
    public interface IPaymentPartialTransactionService : IServiceDependencyMarker
    {
        Task<IEnumerable<PaymentPartialTransactionDTO>> GetPaymentPartialTransactions();
        Task<IEnumerable<PaymentPartialTransactionDTO>> GetPaymentPartialTransactionById(string waybill);
        //Task<object> AddPaymentPartialTransaction(PaymentPartialTransactionDTO paymentPartialTransaction);
        Task UpdatePaymentPartialTransaction(string waybill, PaymentPartialTransactionDTO paymentPartialTransaction);
        Task<bool> ProcessPaymentPartialTransaction(PaymentPartialTransactionProcessDTO paymentPartialTransactionProcessDTO);
        Task RemovePaymentPartialTransaction(string waybill);
    }
}
