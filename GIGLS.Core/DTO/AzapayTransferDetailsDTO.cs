using POST.Core.Enums;
using POST.CORE.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POST.Core.DTO
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

    public class InitiateTimedAccountRequestDTO
    {
        [JsonProperty("customerEmail")]
        public string CustomerEmail { get; set; }

        [JsonProperty("customerName")]
        public string CustomerName { get; set; }

        [JsonProperty("customerPhone")]
        public string CustomerPhone { get; set; }

        [JsonProperty("ref")]
        public string Ref { get; set; }

        [JsonProperty("amount")]
        public int Amount { get; set; }
    }

    public class AzapayTransferRequestDTO
    {
        [JsonProperty("clientTag")]
        public string ClientTag { get; set; }

        [JsonProperty("pin")]
        public string Pin { get; set; }

        [JsonProperty("amount")]
        public string Amount { get; set; }

        [JsonProperty("ref")]
        public string Ref { get; set; }
    }

    // ValidateTimedAccountResponseDTO
    public class ValidateTimedAccountResponseDTO
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("senderDetails")]
        public SenderDetails SenderDetails { get; set; }

        [JsonProperty("data")]
        public GeneralResponseData Data { get; set; }

        [JsonProperty("callbackUrl")]
        public string CallbackUrl { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("customerDetails")]
        public CustomerDetails CustomerDetails { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }
    }

    public class SenderDetails
    {
        [JsonProperty("senderAccNo")]
        public string SenderAccNo { get; set; }

        [JsonProperty("senderAccName")]
        public string SenderAccName { get; set; }
    }

    public class CustomerDetails
    {
        [JsonProperty("customerPhone")]
        public string CustomerPhone { get; set; }

        [JsonProperty("customerEmail")]
        public string CustomerEmail { get; set; }

        [JsonProperty("customerName")]
        public string CustomerName { get; set; }
    }

    public class GeneralResponseData
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("modifiedAt")]
        public DateTime ModifiedAt { get; set; }

        [JsonProperty("fromKey")]
        public string FromKey { get; set; }

        [JsonProperty("toKey")]
        public string ToKey { get; set; }

        [JsonProperty("senderName")]
        public string SenderName { get; set; }

        [JsonProperty("senderBank")]
        public string SenderBank { get; set; }

        [JsonProperty("amount")]
        public string Amount { get; set; }

        [JsonProperty("charge")]
        public string Charge { get; set; }

        [JsonProperty("note")]
        public string Note { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("refId")]
        public string RefId { get; set; }

        [JsonProperty("setRefId")]
        public string SetRefId { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("settled")]
        public bool Settled { get; set; }

        [JsonProperty("deviceId")]
        public string DeviceId { get; set; }

        [JsonProperty("isPaymentGateway")]
        public bool IsPaymentGateway { get; set; }

        //Extra for Azapay transfer response dto
        [JsonProperty("customerRef")]
        public string CustomerRef { get; set; }
        [JsonProperty("timedAccNo")]
        public string TimedAccNo { get; set; }
        [JsonProperty("managerName")]
        public string ManagerName { get; set; }
    }

    public class GetTransactionHistoryResponseDTO
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("data")]
        public List<GeneralResponseData> Data { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }
    }

    // InitiateTimedAccountResponseDTO
    public class InitiateTimedAccountResponseDTO
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("data")]
        public InitiateTimedAccountResponse Data { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }
    }

    public class InitiateTimedAccountResponse
    {
        [JsonProperty("amount")]
        public string Amount { get; set; }

        [JsonProperty("accountName")]
        public string AccountName { get; set; }

        [JsonProperty("accountNo")]
        public string AccountNo { get; set; }

        [JsonProperty("refId")]
        public string RefId { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }

    //AzapayTransferResponseDTO
    public class AzapayTransferResponseDTO
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("data")]
        public GeneralResponseData Data { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }
    }
}
