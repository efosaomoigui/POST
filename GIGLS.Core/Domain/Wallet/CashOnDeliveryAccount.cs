using GIGLS.Core.Enums;

namespace GIGLS.Core.Domain.Wallet
{
    public class CashOnDeliveryAccount : BaseDomain, IAuditable
    {
        public int CashOnDeliveryAccountId { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public CreditDebitType CreditDebitType { get; set; }
        public int WalletId { get; set; }
        public virtual Wallet Wallet { get; set; }
        public string UserId { get; set; }
    }
}
