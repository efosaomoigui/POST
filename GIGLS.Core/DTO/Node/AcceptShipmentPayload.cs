using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.DTO.Node
{
    public class AcceptShipmentPayload
    {
        [JsonProperty("waybillNumber")]
        public string WaybillNumber { get; set; }

        [JsonProperty("partnerId")]
        public string PartnerId { get; set; }

        [JsonProperty("partnerInfo")]
        public PartnerPayload PartnerInfo { get; set; }
    }
}
