using GIGLS.Core.Enums;
using GIGLS.CORE.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.DTO
{
    public class AzapayTransferDetailsDTO : BaseDomainDTO
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("createdAt")]
        public string CreatedAt { get; set; }

        [JsonProperty("modifiedAt")]
        public string ModifiedAt { get; set; }

        [JsonProperty("fromKey")]
        public string FromKey { get; set; }

        [JsonProperty("toKey")]
        public string ToKey { get; set; }

        [JsonProperty("senderName")]
        public string SenderName { get; set; }

        [JsonProperty("senderBank")]
        public string SenderBank { get; set; }

        [JsonProperty("amount")]
        public double Amount { get; set; }

        [JsonProperty("charge")]
        public double Charge { get; set; }

        [JsonProperty("note")]
        public string Note { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("refId")]
        public string RefId { get; set; }

        [JsonProperty("customerRef")]
        public string CustomerRef { get; set; }

        [JsonProperty("setRefId")]
        public string SetRefId { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("settled")]
        public bool Settled { get; set; }

        [JsonProperty("deviceId")]
        public string DeviceId { get; set; }

        [JsonProperty("timedAccNo")]
        public string TimedAccNo { get; set; }

        [JsonProperty("managerName")]
        public string ManagerName { get; set; }

        [JsonProperty("isPaymentGateway")]
        public bool IsPaymentGateway { get; set; }
        public string TransactionStatus { get; set; }
        public bool IsVerified { get; set; }
        public ProcessingPartnerType ProcessingPartner { get; set; }
    }
}
