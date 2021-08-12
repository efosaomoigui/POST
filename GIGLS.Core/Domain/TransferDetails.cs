using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.Domain
{
    public class TransferDetails : BaseDomain, IAuditable
    {
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
    }
}
