using GIGL.GIGLS.Core.Domain;
using GIGLS.Core.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace GIGLS.Core.Domain
{
    public class GeneralLedger : BaseDomain, IAuditable
    {
        [Key]
        public int GeneralLedgerId { get; set; }

        public DateTime DateOfEntry { get; set; }

        public int ServiceCentreId { get; set; }
        public virtual ServiceCentre ServiceCentre { get; set; }

        [MaxLength(128)]
        public string UserId { get; set; }
        public decimal Amount { get; set; }
        public CreditDebitType CreditDebitType { get; set; }
        public string Description { get; set; }
        public bool IsDeferred { get; set; }

        [MaxLength(100)]
        public string Waybill { get; set; }
        public string ClientNodeId { get; set; }
        public PaymentType PaymentType { get; set; }
        public string PaymentTypeReference { get; set; }
        public PaymentServiceType PaymentServiceType { get; set; }
        public bool IsInternational { get; set; }

        public int CountryId { get; set; }
    }
}