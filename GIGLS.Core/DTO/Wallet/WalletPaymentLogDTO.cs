using GIGLS.Core.Enums;
using GIGLS.CORE.DTO;

namespace GIGLS.Core.DTO.Wallet
{
    public class WalletPaymentLogDTO : BaseDomainDTO
    {
        public int WalletPaymentLogId { get; set; }

        public int WalletId { get; set; }
        public  WalletDTO Wallet { get; set; }

        public decimal Amount { get; set; }
        public int PaystackAmount { get; set; } 
        public string TransactionStatus { get; set; }
        public string TransactionResponse { get; set; }
        public string Description { get; set; }
        public string Email { get; set; } 
        public string UserId { get; set; }

        public bool IsWalletCredited { get; set; }
        public string Reference { get; set; }

        public OnlinePaymentType OnlinePaymentType { get; set; }
        public string Currency { get; set; }
        public int PaymentCountryId { get; set; }
    }

}
