using GIGLS.Core.Enums;
using GIGLS.CORE.DTO;

namespace GIGLS.Core.DTO.PaymentTransactions
{
    public class PaymentPartialTransactionDTO : BaseDomainDTO
    {
        public int PaymentPartialTransactionId { get; set; }
        public string Waybill { get; set; }
        public string TransactionCode { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public PaymentType PaymentType { get; set; }
        public decimal Amount { get; set; }
        public string UserId { get; set; }
    }
}
