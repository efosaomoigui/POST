using System.Threading.Tasks;
using GIGLS.Core.DTO.PaymentTransactions;
using GIGLS.Core.IServices.Business;
using GIGLS.Core.IServices.PaymentTransactions;

namespace GIGLS.Services.Business.Payment
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentTransactionService _paymentTransactionService;
        private readonly IPaymentPartialTransactionService _paymentPartialTransactionService;

        public PaymentService(IPaymentTransactionService paymentService,
            IPaymentPartialTransactionService paymentPartialService)
        {
            _paymentTransactionService = paymentService;
            _paymentPartialTransactionService = paymentPartialService;
        }

        public async Task<bool> ProcessPayment(PaymentTransactionDTO paymentDto)
        {
            return await _paymentTransactionService.ProcessPaymentTransaction(paymentDto);
        }

        public async Task<bool> ProcessPaymentPartial(PaymentPartialTransactionProcessDTO paymentPartialTransactionProcessDTO)
        {
            return await _paymentPartialTransactionService.ProcessPaymentPartialTransaction(paymentPartialTransactionProcessDTO);
        }
    }
}
