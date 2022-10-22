using POST.Core.Enums;
using POST.CORE.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace POST.Core.DTO.Wallet
{
    public class CODTransferLogDTO : BaseDomainDTO
    {
        public int CODTransferLogId { get; set; }
        public decimal Amount { get; set; }
        public string OriginatingBankName { get; set; }
        public string OriginatingBankAccount { get; set; }
        public string DestinationBankName { get; set; }
        public string DestinationBankAccount { get; set; }
        public string CustomerCode { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public string StatusCode { get; set; }
        public string StatusDescription { get; set; }
    }

}
