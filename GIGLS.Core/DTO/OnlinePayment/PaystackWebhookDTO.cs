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
    }

    public class Data
    {
        public string Status { get; set; }
        public string Reference { get; set; }
        public decimal Amount { get; set; }
        public string Gateway_Response { get; set; }
        public string Display_Text { get; set; }
        public string Message { get; set; }
    }

    public class PaymentResponse
    {
        public bool Result { get; set; }
        public string Message { get; set; }
        public string GatewayResponse { get; set; }
        public string Status { get; set; }
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
        }
        public string Status { get; set; }
        public int Id { get; set; }
        public string TXRef { get; set; }
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
        public ValidateInstructions validateInstructions { get; set; }
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
    

}
