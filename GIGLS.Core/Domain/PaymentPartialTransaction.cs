using GIGLS.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace GIGLS.Core.Domain
{
    public class PaymentPartialTransaction : BaseDomain, IAuditable
    {
        public int PaymentPartialTransactionId { get; set; }

        [MaxLength(100)]
        public string Waybill { get; set; }

        [MaxLength(100)]
        public string TransactionCode { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public PaymentType PaymentType { get; set; }
        public decimal Amount { get; set; }

        [MaxLength(128)]
        public string UserId { get; set; }
    }
}