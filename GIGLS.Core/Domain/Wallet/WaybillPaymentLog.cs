using GIGLS.Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GIGLS.Core.Domain.Wallet
{
    public class WaybillPaymentLog : BaseDomain
    {
        [Key]
        public int WaybillPaymentLogId { get; set; }

        public string Waybill { get; set; }

        [MaxLength(100), MinLength(5)]
        [Index(IsUnique = true)]
        public string Reference { get; set; }

        public decimal Amount { get; set; }
        public string TransactionStatus { get; set; }
        public string TransactionResponse { get; set; }
        public string Currency { get; set; }

        public OnlinePaymentType OnlinePaymentType { get; set; }

        [MaxLength(128)]
        public string UserId { get; set; }

        public bool IsWaybillSettled { get; set; }
    }
}
