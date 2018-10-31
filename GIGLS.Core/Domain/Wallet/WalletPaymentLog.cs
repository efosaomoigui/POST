using System.ComponentModel.DataAnnotations;

namespace GIGLS.Core.Domain.Wallet
{
    public class WalletPaymentLog : BaseDomain
    {
        [Key]
        public int WalletPaymentLogId { get; set; }

        public int WalletId { get; set; }
        public virtual Wallet Wallet { get; set; }

        public decimal Amount { get; set; }
        public string TransactionStatus { get; set; }
        public string UserId { get; set; }
    }
}
