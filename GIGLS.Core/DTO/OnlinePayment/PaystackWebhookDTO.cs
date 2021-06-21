using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace GIGLS.Core.DTO.OnlinePayment
{
    public class PaystackWebhookDTO
    {
        public PaystackWebhookDTO()
        {
            data = new Data();
        }
        public string Event { get; set; }
        public string Message { get; set; }
        public bool Status { get; set; }
        public Data data { get; set; }

        //waybill Grand Total
        public decimal Amount { get; set; }
    }

    public class Data
    {
        public Data()
        {
            Authorization = new Authorization();
        }
        public string Status { get; set; }
        public string Reference { get; set; }
        public string Authorization_url { get; set; }
        public decimal Amount { get; set; }
        public string Gateway_Response { get; set; }
        public string Display_Text { get; set; }
        public string Message { get; set; }
        public Authorization Authorization { get; set; }
    }

    public class Authorization
    {
        [JsonProperty("authorization_code")]
        public string AuthorizationCode { get; set; }

        [JsonProperty("card_type")]
        public string CardType { get; set; }

        [JsonProperty("last4")]
        public string Last4 { get; set; }

        [JsonProperty("exp_month")]
        public string ExpMonth { get; set; }

        [JsonProperty("exp_year")]
        public string ExpYear { get; set; }

        [JsonProperty("bin")]
        public string Bin { get; set; }

        [JsonProperty("bank")]
        public string Bank { get; set; }

        [JsonProperty("channel")]
        public string Channel { get; set; }

        [JsonProperty("reusable")]
        public bool? Reusable { get; set; }

        [JsonProperty("country_code")]
        public string CountryCode { get; set; }
    }

    public class PaymentResponse
    {
        public bool Result { get; set; }
        public string Message { get; set; }
        public string GatewayResponse { get; set; }
        public string Status { get; set; }
        public bool ResponseStatus { get; set; }

    }

    public enum WaybillWalletPaymentType
    {
        Waybill,
        Wallet
    }

    public class FlutterWebhookDTO
    {
        public FlutterWebhookDTO()
        {
            data = new FlutterResponseData();
        }
        public string Message { get; set; }
        public string Status { get; set; }
        public FlutterResponseData data { get; set; }
    }

    public class FlutterTransactionWebhookDTO
    {
        public FlutterTransactionWebhookDTO()
        {
            data = new List<FlutterResponseData>();
        }
        public string Status { get; set; }
        public List<FlutterResponseData> data { get; set; }
    }

    public class FlutterResponseData
    {
        public FlutterResponseData()
        {
            validateInstructions = new ValidateInstructions();
            Card = new Card();
        }
        public string Status { get; set; }
        public int Id { get; set; }
        public int TxId { get; set; }
        public string TX_Ref { get; set; }
        public decimal Amount { get; set; }
        public string ChargeResponseMessage { get; set; }
        public string ChargeResponseCode { get; set; }
        public string ChargeMessage { get; set; }
        public string PaymentType { get; set; }
        public string FlwRef { get; set; }
        public string Acctvalrespcode { get; set; }
        public string Acctvalrespmsg { get; set; }
        public string ChargeCode { get; set; }
        public string Processor_Response { get; set; }
        public ValidateInstructions validateInstructions { get; set; }
        public Card Card { get; set; }
    }

    public class Card
    {
        [JsonProperty("expirymonth")]
        public string ExpiryMonth { get; set; }

        [JsonProperty("expiryyear")]
        public string ExpiryYear { get; set; }

        [JsonProperty("cardBIN")]
        public string CardBIN { get; set; }

        [JsonProperty("last4digits")]
        public string Last4Digits { get; set; }

        [JsonProperty("brand")]
        public string Brand { get; set; }

        [JsonProperty("type")]
        public string CardType { get; set; }

    }

    public class ValidateInstructions
    {
        public string Instruction { get; set; }
    }

    public class USSDWebhook
    {
        public string Status { get; set; }
        public decimal Amount { get; set; }
        public string MSISDN { get; set; }
        public string Order_Reference { get; set; }
        public string Transaction_Ref { get; set; }
    }
    
    public class BonusAddOn
    {
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public bool BonusAdded { get; set; }
    }


    public class CreateNubanAccountDTO
    {
        public int customer { get; set; }
        public string preferred_bank { get; set; }
    }



    public class CreateNubanAccountResponseDTO
    {
        public CreateNubanAccountResponseDTO()
        {
            data = new NubanDataResponse();
            assignment = new NubanAssignment();
            customer = new NubanCustomer();
        }
        public string status { get; set; }
        public string message { get; set; }
        public NubanDataResponse data { get; set; }
        public NubanAssignment assignment { get; set; }
        public NubanCustomer customer { get; set; }
        public string account_name { get; set; }
        public string assigned { get; set; }
        public string currency { get; set; }
        public string active { get; set; }
        public bool succeeded { get; set; }
    }

    public class NubanDataResponse
    {
        public NubanDataResponse()
        {
            NubanBank = new NubanBank();
        }
        public NubanBank NubanBank { get; set; }
    }

    public class NubanBank
    {
        public int id { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
    }

    public class NubanCustomer
    {
        public int id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string customer_code { get; set; }
        public string phone { get; set; }
        public string risk_action { get; set; }

    }


    public class NubanAssignment
    {
        public int integration { get; set; }
        public int assignee_id { get; set; }
        public string assignee_type { get; set; }
        public bool expired { get; set; }
        public string account_type { get; set; }
        public DateTime assigned_at { get; set; }
        public string phone { get; set; }
        public string risk_action { get; set; }

    }


}
