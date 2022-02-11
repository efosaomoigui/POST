using GIGLS.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace GIGLS.Core.Domain.Wallet
{
    public class CODWallet : BaseDomain, IAuditable
    {
        public int CODWalletId { get; set; }
        [MaxLength(100)]
        public string AccountNo { get; set; }
        public decimal AvailableBalance { get; set; }
        public int CustomerId { get; set; }
        public CustomerType CustomerType { get; set; }
        [MaxLength(100)]
        public string CustomerCode { get; set; }
        [MaxLength(100)]
        public string CompanyType { get; set; }
        [MaxLength(100)]
        public string AccountType { get; set; }
        public decimal WithdrawableBalance { get; set; }
        public string CustomerAccountId { get; set; }
    }
}