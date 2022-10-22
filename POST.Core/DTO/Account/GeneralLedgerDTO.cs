using GIGL.POST.Core.Domain;
using POST.Core.DTO.User;
using POST.Core.Enums;
using POST.CORE.DTO;
using System;

namespace POST.Core.DTO.Account
{
    public class GeneralLedgerDTO : BaseDomainDTO
    {
        public int GeneralLedgerId { get; set; }

        public DateTime DateOfEntry { get; set; }

        public int ServiceCentreId { get; set; }
        public virtual ServiceCentre ServiceCentre { get; set; }

        public string UserId { get; set; }
        public UserDTO User { get; set; }

        public decimal Amount { get; set; }
        public CreditDebitType CreditDebitType { get; set; }
        public PaymentType PaymentType { get; set; }
        public string Description { get; set; }
        public bool IsDeferred { get; set; }
        public string Waybill { get; set; }
        public string ClientNodeId { get; set; }
        public PaymentServiceType PaymentServiceType { get; set; }
        public string PaymentTypeReference { get; set; }
        public bool IsInternational { get; set; }
        public int CountryId { get; set; }
    }
}
