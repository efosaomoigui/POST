using GIGLS.Core.Enums;
using System;

namespace GIGLS.Core.Domain
{
    public class PaymentTransaction : BaseDomain, IAuditable
    {
        public int PaymentTransactionId { get; set; }
        public string Waybill { get; set; }
        public string TransactionCode { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public PaymentType PaymentTypes { get; set; }
    }
}
