using GIGLS.Core.Enums;
using GIGLS.CORE.DTO;

namespace GIGLS.Core.DTO.PaymentTransactions
{
    public class PaymentTransactionDTO : BaseDomainDTO
    {
        public int PaymentTransactionId { get; set; }
        public string Waybill { get; set; }
        public string TransactionCode { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public PaymentType PaymentType { get; set; }
        public string UserId { get; set; }
    }
}
