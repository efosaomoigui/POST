using GIGLS.Core.Enums;
using GIGLS.CORE.DTO;

namespace GIGLS.Core.DTO.Wallet
{
    public class CashOnDeliveryAccountDTO : BaseDomainDTO
    {
        public int CashOnDeliveryAccountId { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public CreditDebitType CreditDebitType { get; set; }
        public int WalletId { get; set; }
        public WalletDTO Wallet { get; set; }
        public string UserId { get; set; }
    }
}
