using GIGLS.Core.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace GIGLS.Core.Domain.Wallet
{
    public class CODTransferLog : BaseDomain, IAuditable
    {
        public int CODTransferLogId { get; set; }
        public decimal Amount { get; set; }  
        [MaxLength(300)]
        public string OriginatingBankName { get; set; }
        [MaxLength(300)]
        public string OriginatingBankAccount { get; set; }
        [MaxLength(300)]
        public string DestinationBankName { get; set; }
        [MaxLength(300)]
        public string DestinationBankAccount { get; set; }
        [MaxLength(100)]
        public string CustomerCode { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        [MaxLength(10)]
        public string StatusCode { get; set; }
        public string StatusDescription { get; set; }
        [MaxLength(300)]
        public string ReferenceNo { get; set; }
    }
}