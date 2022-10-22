using POST.Core.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace POST.Core.Domain.Wallet
{
    public class CODGeneratedAccountNo : BaseDomain, IAuditable
    {
        public int CODGeneratedAccountNoId { get; set; }
        [MaxLength(100)]
        public string Waybill { get; set; }
        [MaxLength(128)]
        public string AccountNo { get; set; }
        [MaxLength(100)]
        public string AccountName { get; set; }
        public string Amount { get; set; }  
        [MaxLength(300)]
        public string BankName { get; set; }
    }
}