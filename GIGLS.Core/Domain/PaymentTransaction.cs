using GIGLS.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace GIGLS.Core.Domain
{
    public class PaymentTransaction : BaseDomain, IAuditable
    {
        public int PaymentTransactionId { get; set; }

        [MaxLength(100)]
        public string Waybill { get; set; }

        [MaxLength(100)]
        public string TransactionCode { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public PaymentType PaymentTypes { get; set; }

        [MaxLength(128)]
        public string UserId { get; set; }
    }
}