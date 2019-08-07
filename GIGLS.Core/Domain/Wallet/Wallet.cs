using GIGLS.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace GIGLS.Core.Domain.Wallet
{
    public class Wallet : BaseDomain, IAuditable
    {
        public int WalletId { get; set; }

        [MaxLength(100)]
        public string WalletNumber { get; set; }
        public decimal Balance { get; set; }
        public int CustomerId { get; set; }
        public CustomerType CustomerType { get; set; }
        public bool IsSystem { get; set; }

        [MaxLength(100)]
        public string CustomerCode { get; set; }

        [MaxLength(100)]
        public string CompanyType { get; set; }
    }
}