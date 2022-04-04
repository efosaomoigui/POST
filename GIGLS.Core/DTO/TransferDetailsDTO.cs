using GIGLS.CORE.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.DTO
{
    public class TransferDetailsDTO : BaseDomainDTO
    {
        [JsonProperty("originatoraccountnumber")]
        public string OriginatorAccountNumber { get; set; }
        [JsonProperty("amount")]
        public string Amount { get; set; }
        [JsonProperty("originatorname")]
        public string OriginatorName { get; set; }
        [JsonProperty("narration")]
        public string Narration { get; set; }
        [JsonProperty("craccountname")]
        public string CrAccountName { get; set; }
        [JsonProperty("paymentreference")]
        public string PaymentReference { get; set; }
        [JsonProperty("bankname")]
        public string BankName { get; set; }
        [JsonProperty("sessionid")]
        public string SessionId { get; set; }
        [JsonProperty("craccount")]
        public string CrAccount { get; set; }
        [JsonProperty("bankcode")]
        public string BankCode { get; set; }
        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }
        [JsonProperty("ResponseCode")]
        public string ResponseCode { get; set; }
        public string TransactionStatus { get; set; }
        public string ServiceCenterName { get; set; }
    }

    public class CODCallBackDTO : BaseDomainDTO
    {
        [JsonProperty("cod_amount")]
        public string CODAmount { get; set; }
        [JsonProperty("waybill")]
        public string Waybill { get; set; }
        [JsonProperty("paymentstatus")]
        public string PaymentStatus { get; set; }
        [JsonProperty("transactionreference")]
        public string TransactionReference { get; set; }
        [JsonProperty("transferaccount")]
        public string TransferAccount { get; set; }
    }


    public class GenerateAccountErrorDTO
    {
        [JsonProperty("status_desc")]
        public string StatusDesc { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }
    }

    public class GenerateAccountResponseDTO
    {
        public int status { get; set; }
        [JsonProperty("accountnumber")]
        public string Accountnumber { get; set; }

        [JsonProperty("accountname")]
        public string Accountname { get; set; }

        [JsonProperty("bankname")]
        public string Bankname { get; set; }
    }

    public class GenerateAccountDTO
    {
        public int status { get; set; }
        public bool Succeeded { get; set; }

        public GenerateAccountErrorDTO Error { get; set; }
        public GenerateAccountResponseDTO ResponsePayload { get; set; }
    }


    public class GenerateAccountPayloadDTO
    {
        [JsonProperty("clientId")]
        public string ClientId { get; set; }

        [JsonProperty("uniqueId")]
        public string UniqueId { get; set; }

        [JsonProperty("amount")]
        public string Amount { get; set; }

        [JsonProperty("serviceCode")]
        public string ServiceCode { get; set; }

        [JsonProperty("accountReference")]
        public string AccountReference { get; set; }

        [JsonProperty("merchantId")]
        public string MerchantId { get; set; }

        [JsonProperty("validity")]
        public string Validity { get; set; }
    }


}
