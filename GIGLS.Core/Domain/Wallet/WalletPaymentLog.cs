using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GIGLS.Core.Domain.Wallet
{
    public class WalletPaymentLog : BaseDomain
    {
        [Key]
        public int WalletPaymentLogId { get; set; }

        public int WalletId { get; set; }
        public virtual Wallet Wallet { get; set; }

        [MaxLength(100), MinLength(5)]
        [Index(IsUnique = true)]
        public string Reference { get; set; }

        public decimal Amount { get; set; }
        public string TransactionStatus { get; set; }
        public string UserId { get; set; }

        public bool IsWalletCredited { get; set; }
    }
}
