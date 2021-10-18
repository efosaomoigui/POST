using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.DTO
{
    public class TicketMannResponseDTO
    {
            [JsonProperty("code")]
            public int Code { get; set; }

            [JsonProperty("shortDescription")]
            public string ShortDescription { get; set; }

            [JsonProperty("payload")]
            public Payload Payload { get; set; }
    }
    public class Payload
    {
        [JsonProperty("amount")]
        public string Amount { get; set; }

        [JsonProperty("currencyCode")]
        public string CurrencyCode { get; set; }

        [JsonProperty("customer")]
        public string Customer { get; set; }

        [JsonProperty("customerEmail")]
        public string CustomerEmail { get; set; }

        [JsonProperty("customerMobile")]
        public string CustomerMobile { get; set; }

        [JsonProperty("paymentDate")]
        public string PaymentDate { get; set; }

        [JsonProperty("requestReference")]
        public string RequestReference { get; set; }

        [JsonProperty("serviceCode")]
        public string ServiceCode { get; set; }

        [JsonProperty("serviceName")]
        public string ServiceName { get; set; }

        [JsonProperty("serviceProviderId")]
        public string ServiceProviderId { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("surcharge")]
        public string Surcharge { get; set; }

        [JsonProperty("transactionRef")]
        public string TransactionRef { get; set; }

        [JsonProperty("transactionResponseCode")]
        public string TransactionResponseCode { get; set; }

        [JsonProperty("transactionSet")]
        public string TransactionSet { get; set; }

        [JsonProperty("responseCode")]
        public string ResponseCode { get; set; }
    }

    public class MerchantSalesPayload
    {
        [JsonProperty("merchantName")]
        public string MerchantName { get; set; }

        [JsonProperty("airtime")]
        public double Airtime { get; set; }

        [JsonProperty("dataSub")]
        public double DataSub { get; set; }

        [JsonProperty("tvSub")]
        public double TvSub { get; set; }

        [JsonProperty("electricity")]
        public double Electricity { get; set; }

        [JsonProperty("totaldue")]
        public double Totaldue { get; set; }
    }

    public class MerchantSalesDTO
    {
        public MerchantSalesDTO()
        {
            Payload = new List<MerchantSalesPayload>();
        }
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("shortDescription")]
        public string ShortDescription { get; set; }

        [JsonProperty("payload")]
        public List<MerchantSalesPayload> Payload { get; set; }
    }
}
