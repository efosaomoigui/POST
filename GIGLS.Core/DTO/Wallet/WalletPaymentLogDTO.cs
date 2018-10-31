using GIGLS.CORE.DTO;

namespace GIGLS.Core.DTO.Wallet
{
    public class WalletPaymentLogDTO : BaseDomainDTO
    {
        public int WalletPaymentLogId { get; set; }

        public int WalletId { get; set; }
        public  WalletDTO Wallet { get; set; }

        public decimal Amount { get; set; }
        public string TransactionStatus { get; set; }
        public string UserId { get; set; }
    }
}
