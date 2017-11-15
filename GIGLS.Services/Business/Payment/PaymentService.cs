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

        public async Task<bool> ProcessCashPayment(string waybill, PaymentTransactionDTO paymentDto)
        {
            paymentDto.PaymentTypes = PaymentType.Cash;
            await _paymentTransactionService.UpdatePaymentTransaction(waybill, paymentDto);
            return true;
        }

        public async Task<bool> ProcessPosPayment(string waybill, PaymentTransactionDTO paymentDto)
        {
            paymentDto.PaymentTypes = PaymentType.Pos;
            await _paymentTransactionService.UpdatePaymentTransaction(waybill, paymentDto);
            return true;
        }

        public async Task<bool> ProcessOnlinePayment(string waybill, PaymentTransactionDTO paymentDto)
        {
            paymentDto.PaymentTypes = PaymentType.Online;
            await _paymentTransactionService.UpdatePaymentTransaction(waybill, paymentDto);
            return true;
        }
        
    }
}
