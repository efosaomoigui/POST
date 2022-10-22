using POST.Core.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POST.Core.Domain
{
    public class TransferDetails : BaseDomain, IAuditable
    {
        [Key]
        public int TransferDetailsId { get; set; }
        [MaxLength(50)]
        public string OriginatorAccountNumber { get; set; }
        [MaxLength(50)]
        public string Amount { get; set; }
        [MaxLength(150)]
        public string OriginatorName { get; set; }
        public string Narration { get; set; }
        [MaxLength(100)]
        public string CrAccountName { get; set; }
        public string PaymentReference { get; set; }
        [MaxLength(100)]
        public string BankName { get; set; }
        public string SessionId { get; set; }
        [MaxLength(50)]
        public string CrAccount { get; set; }
        [MaxLength(50)]
        public string BankCode { get; set; }
        [MaxLength(50)]
        public string CreatedAt { get; set; }
        [MaxLength(25)]
        public string ResponseCode { get; set; }
        [MaxLength(25)]
        public string TransactionStatus { get; set; }
        public bool IsVerified { get; set; }

        //Azapay 
        [MaxLength(50)]
        public string Id { get; set; }
        //public DateTime CreatedAt { get; set; }
        [MaxLength(50)]
        public string ModifiedAt { get; set; }
        [MaxLength(50)]
        public string FromKey { get; set; }
        [MaxLength(50)]
        public string ToKey { get; set; }
        [MaxLength(100)]
        public string SenderName { get; set; }
        [MaxLength(100)]
        public string SenderBank { get; set; }
        public decimal Charge { get; set; }
        [MaxLength(150)]
        public string Note { get; set; }
        [MaxLength(50)]
        public string Status { get; set; }
        [MaxLength(100)]
        public string RefId { get; set; }
        [MaxLength(100)]
        public string CustomerRef { get; set; }
        [MaxLength(100)]
        public string SetRefId { get; set; }
        [MaxLength(50)]
        public string Type { get; set; }
        public bool Settled { get; set; }
        [MaxLength(50)]
        public string DeviceId { get; set; }
        [MaxLength(50)]
        public string TimedAccNo { get; set; }
        [MaxLength(50)]
        public string ManagerName { get; set; }
        public bool IsPaymentGateway { get; set; }
        public ProcessingPartnerType ProcessingPartner { get; set; }
    }
}
