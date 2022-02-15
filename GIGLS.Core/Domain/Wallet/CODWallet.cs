using GIGLS.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace GIGLS.Core.Domain.Wallet
{
    public class CODWallet : BaseDomain, IAuditable
    {
        public int CODWalletId { get; set; }
        [MaxLength(100)]
        public string AccountNo { get; set; }
        public string AvailableBalance { get; set; }
        public string CustomerId { get; set; }
        public CustomerType CustomerType { get; set; }
        [MaxLength(100)]
        public string CustomerCode { get; set; }
        [MaxLength(100)]
        public string CompanyType { get; set; }
        [MaxLength(100)]
        public string AccountType { get; set; }
        public string WithdrawableBalance { get; set; }
        public string CustomerAccountId { get; set; }
        public string UserId { get; set; }
    }
}