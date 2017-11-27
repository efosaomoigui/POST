using System.Threading.Tasks;
using GIGLS.Core.DTO.PaymentTransactions;
using GIGLS.Core.IServices.Business;
using GIGLS.Core.IServices.PaymentTransactions;
using GIGLS.Core.Enums;

namespace GIGLS.Services.Business.Payment
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentTransactionService _paymentTransactionService;

        public PaymentService(IPaymentTransactionService paymentService)
        {
            _paymentTransactionService = paymentService;
        }

        public async Task<bool> ProcessPayment(PaymentTransactionDTO paymentDto)
        {
            return await _paymentTransactionService.ProcessPaymentTransaction(paymentDto);
        }

    }
}
