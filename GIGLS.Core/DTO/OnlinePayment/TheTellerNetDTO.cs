namespace GIGLS.Core.DTO.OnlinePayment
{
    public class TheTellerNetDTO
    {
        public string Status { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Reason { get; set; }
        public string Transaction_Id { get; set; }
        public string Amount { get; set; }
        public string Currency { get; set; }
    }

    public class PaymentInitiate
    {
        public string MerchantId { get; set; }
        public string MerchantUsername { get; set; }
        public string MerchantKey { get; set; }
        public string TransactionId { get; set; }
        public bool IsPaymentSuccessful { get; set; }
    }
}
