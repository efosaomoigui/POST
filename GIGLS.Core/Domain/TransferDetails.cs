using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.Domain
{
    public class TransferDetails : BaseDomain, IAuditable
    {
        public int TransferDetailsId { get; set; }
        public string OriginatorAccountNumber { get; set; }
        public string Amount { get; set; }
        public string OriginatorName { get; set; }
        public string Narration { get; set; }
        public string CrAccountName { get; set; }
        public string PaymentReference { get; set; }
        public string BankName { get; set; }
        public string SessionId { get; set; }
        public string CrAccount { get; set; }
        public string BankCode { get; set; }
        public string CreatedAt { get; set; }
        public string ResponseCode { get; set; }
        public string TransactionStatus { get; set; }
    }
}
