using GIGLS.Core.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace GIGLS.Core.Domain.Wallet
{
    public class CODWallet : BaseDomain, IAuditable
    {
        public int CODWalletId { get; set; }
        [MaxLength(100)]
        public string AccountNo { get; set; }
        [MaxLength(100)]
        public string AvailableBalance { get; set; }
        [MaxLength(100)]
        public string CustomerId { get; set; }
        public CustomerType CustomerType { get; set; }
        [MaxLength(100)]
        public string CustomerCode { get; set; }
        [MaxLength(100)]
        public string CompanyType { get; set; }
        [MaxLength(100)]
        public string AccountType { get; set; }
        [MaxLength(100)]
        public string WithdrawableBalance { get; set; }
        [MaxLength(100)]
        public string CustomerAccountId { get; set; }
        [MaxLength(100)]
        public string UserId { get; set; }
        [MaxLength(100)]
        public string DateOfBirth { get; set; }
        [MaxLength(300)]

        public string PlaceOfBirth { get; set; }
        [MaxLength(300)]
        public string Address { get; set; }
        [MaxLength(128)]
        public string NationalIdentityNo { get; set; }
    }
}