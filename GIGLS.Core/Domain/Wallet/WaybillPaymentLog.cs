using GIGLS.Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GIGLS.Core.Domain.Wallet
{
    public class WaybillPaymentLog : BaseDomain
    {
        [Key]
        public int WaybillPaymentLogId { get; set; }

        public string Waybill { get; set; }

        [MaxLength(100), MinLength(5)]
        [Index(IsUnique = true)]
        public string Reference { get; set; }

        public decimal Amount { get; set; }
        public string TransactionStatus { get; set; }
        public string TransactionResponse { get; set; }

        [MaxLength(10)]
        public string Currency { get; set; }

        public OnlinePaymentType OnlinePaymentType { get; set; }

        [MaxLength(128)]
        public string UserId { get; set; }

        public bool IsWaybillSettled { get; set; }
        public bool IsPaymentSuccessful { get; set; }

        [MaxLength(100)]
        public string PhoneNumber { get; set; }

        [MaxLength(100)]
        public string Email { get; set; }

        //NetworkProvider for Ghana Payment
        // //use NetworkProvide to represent flutter flwRef -- security code for otp confirmation
        //Network Provider represent Order_Reference for USSD 
        [MaxLength(50)]
        public string NetworkProvider { get; set; }
        public int PaymentCountryId { get; set; }
    }
}
