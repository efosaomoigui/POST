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
        public string email { get; set; }
        public string phone { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
    }



    public class CreateNubanAccountResponseDTO
    {
        public CreateNubanAccountResponseDTO()
        {
            data = new NubanDataResponse();
        }
        public string status { get; set; }
        public string message { get; set; }
        public NubanDataResponse data { get; set; }
        public bool succeeded { get; set; }
    }

    public class NubanDataResponse
    {
        public NubanDataResponse()
        {
            bank = new NubanBank();
            assignment = new NubanAssignment();
            customer = new NubanCustomer();
        }
        public NubanAssignment assignment { get; set; }
        public NubanCustomer customer { get; set; }
        public string account_name { get; set; }
        public string account_number { get; set; }
        public string assigned { get; set; }
        public string currency { get; set; }
        public string active { get; set; }
        public NubanBank bank { get; set; }
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


    public class NubanCustomerDataResponse
    {
      public string email { get; set; }
      public int integration { get; set; }
      public int id { get; set; }
      public string domain { get; set; }
      public string customer_code { get; set; }
      public bool identified { get; set; }
      public string identifications { get; set; }
      public string createdAt { get; set; }
      public string updatedAt { get; set; }
    }

    public class NubanCreateCustomerDTO
    {
        public NubanCreateCustomerDTO()
        {
            data = new NubanCustomerDataResponse();
        }
        public string status { get; set; }
        public string message { get; set; }
        public NubanCustomerDataResponse data { get; set; }
        public bool succeeded { get; set; }
    }

    public class NubanCustomerResponse
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string CustomerCode { get; set; }
        public string Reference { get; set; }
        public decimal Amount { get; set; }
    }

    public class Payment
    {
        [JsonProperty("MSISDN")]
        public long MSISDN { get; set; }

        [JsonProperty("payerClientName")]
        public string PayerClientName { get; set; }

        [JsonProperty("currencyCode")]
        public string CurrencyCode { get; set; }

        [JsonProperty("amountPaid")]
        public int AmountPaid { get; set; }

        [JsonProperty("cpgTransactionID")]
        public string CpgTransactionID { get; set; }

        [JsonProperty("serviceCode")]
        public string ServiceCode { get; set; }

        [JsonProperty("payerTransactionID")]
        public string PayerTransactionID { get; set; }

        [JsonProperty("hubOverallStatus")]
        public int HubOverallStatus { get; set; }

        [JsonProperty("accountNumber")]
        public string AccountNumber { get; set; }

        [JsonProperty("customerName")]
        public string CustomerName { get; set; }

        [JsonProperty("payerClientCode")]
        public string PayerClientCode { get; set; }

        [JsonProperty("datePaymentReceived")]
        public string DatePaymentReceived { get; set; }
    }

    public class CellulantWebhookDTO
    {
        [JsonProperty("serviceCode")]
        public string ServiceCode { get; set; }

        [JsonProperty("MSISDN")]
        public string MSISDN { get; set; }

        [JsonProperty("originalRequestCurrencyCode")]
        public string OriginalRequestCurrencyCode { get; set; }

        [JsonProperty("originalRequestAmount")]
        public int OriginalRequestAmount { get; set; }

        [JsonProperty("checkoutRequestID")]
        public int CheckoutRequestID { get; set; }

        [JsonProperty("requestCurrencyCode")]
        public string RequestCurrencyCode { get; set; }

        [JsonProperty("requestAmount")]
        public string RequestAmount { get; set; }

        [JsonProperty("accountNumber")]
        public string AccountNumber { get; set; }

        [JsonProperty("requestStatusCode")]
        public int RequestStatusCode { get; set; }

        [JsonProperty("requestStatusDescription")]
        public string RequestStatusDescription { get; set; }

        [JsonProperty("merchantTransactionID")]
        public string MerchantTransactionID { get; set; }

        [JsonProperty("requestDate")]
        public string RequestDate { get; set; }

        [JsonProperty("currencyCode")]
        public string CurrencyCode { get; set; }

        [JsonProperty("amountPaid")]
        public int AmountPaid { get; set; }

        [JsonProperty("serviceChargeAmount")]
        public int ServiceChargeAmount { get; set; }

        [JsonProperty("payments")]
        public List<Payment> Payments { get; set; }

        [JsonProperty("failedPayments")]
        public List<object> FailedPayments { get; set; }
    }

    public class CellulantPaymentResponse
    {
        [JsonProperty("checkoutRequestID")]
        public int CheckoutRequestID { get; set; }

        [JsonProperty("merchantTransactionID")]
        public string MerchantTransactionID { get; set; }

        [JsonProperty("statusCode")]
        public string StatusCode { get; set; }

        [JsonProperty("statusDescription")]
        public string StatusDescription { get; set; }

        [JsonProperty("receiptNumber")]
        public string ReceiptNumber { get; set; }
    }

    #region Korapay
    public class KorapayCustomer
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }
    }

    public class KoarapayInitializeCharge
    {
        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("redirect_url")]
        public string RedirectUrl { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("reference")]
        public string Reference { get; set; }

        [JsonProperty("narration")]
        public string Narration { get; set; }

        [JsonProperty("customer")]
        public KorapayCustomer Customer { get; set; }
    }

    public class KorapayInitializeChargeData
    {
        [JsonProperty("reference")]
        public string Reference { get; set; }

        [JsonProperty("checkout_url")]
        public string CheckoutUrl { get; set; }
    }

    public class KorapayInitializeChargeResponse
    {
        [JsonProperty("status")]
        public bool Status { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("data")]
        public KorapayInitializeChargeData Data { get; set; }
    }

    public class KorapayWebhookData
    {
        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("amount_expected")]
        public decimal AmountExpected { get; set; }

        [JsonProperty("fee")]
        public decimal Fee { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("reference")]
        public string Reference { get; set; }

        [JsonProperty("payment_reference")]
        public string PaymentReference { get; set; }

        [JsonProperty("transaction_status")]
        public string TransactionStatus { get; set; }
    }

    public class KorapayWebhookDTO
    {
        [JsonProperty("event")]
        public string Event { get; set; }

        [JsonProperty("data")]
        public KorapayWebhookData Data { get; set; }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class KoraPayerBankAccount
    {
        [JsonProperty("account_number")]
        public string AccountNumber { get; set; }

        [JsonProperty("account_name")]
        public string AccountName { get; set; }

        [JsonProperty("bank_name")]
        public string BankName { get; set; }
    }

    public class KorapayQueryData
    {
        [JsonProperty("reference")]
        public string Reference { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("amount")]
        public string Amount { get; set; }

        [JsonProperty("fee")]
        public string Fee { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("payer_bank_account")]
        public KoraPayerBankAccount PayerBankAccount { get; set; }
    }

    public class KorapayQueryChargeResponse
    {
        [JsonProperty("status")]
        public bool Status { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("data")]
        public KorapayQueryData Data { get; set; }
    }

    #endregion
}
