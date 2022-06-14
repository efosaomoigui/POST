using GIGLS.Core.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace GIGLS.Core.Domain.Wallet
{
    public class CODTransferRegister : BaseDomain, IAuditable
    {
        public int CODTransferRegisterId { get; set; }
        [MaxLength(100)]
        public string Waybill { get; set; }
        [MaxLength(100)]
        public string AccountNo { get; set; }
        public decimal Amount { get; set; }  
        [MaxLength(300)]
        public string RefNo { get; set; }
        [MaxLength(300)]
        public string ClientRefNo { get; set; }
        [MaxLength(100)]
        public string CustomerCode { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        [MaxLength(10)]
        public string StatusCode { get; set; }
        [MaxLength(500)]
        public string StatusDescription { get; set; }
        public string ReceiverNarration { get; set; }
        public DateTime? TransferDate { get; set; }
    }
}