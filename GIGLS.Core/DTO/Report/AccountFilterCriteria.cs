using GIGLS.Core.Enums;
using System;

namespace GIGLS.CORE.DTO.Report
{
    public class AccountFilterCriteria : BaseFilterCriteria
    {
        public CreditDebitType? creditDebitType { get; set; }
        public PaymentStatus? PaymentStatus { get; set; }
        public PaymentType? PaymentType { get; set; }
        public DateTime? PaymentDate { get; set; }
        public bool? IsDeferred { get; set; }
        public string CompanyType { get; set; }
        public PaymentServiceType? PaymentServiceType { get; set; }
        public bool? IsCashOnDelivery { get; set; }
    }
}
