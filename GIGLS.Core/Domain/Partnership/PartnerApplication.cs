using GIGLS.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace GIGLS.Core.Domain.Partnership
{
    public class PartnerApplication : BaseDomain, IAuditable
    {
        public int PartnerApplicationId { get; set; }

        [MaxLength(500)]
        public string FirstName { get; set; }

        [MaxLength(500)]
        public string LastName { get; set; }

        [MaxLength(500)]
        public string CompanyName { get; set; }
        public string Email { get; set; }

        [MaxLength(100)]
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string CompanyRcNumber { get; set; }
        public string IdentificationNumber { get; set; }
        public PartnerType PartnerType { get; set; }
        public string TellAboutYou { get; set; }
        public bool IsRegistered { get; set; }

        public PartnerApplicationStatus PartnerApplicationStatus { get; set; }

        // FK
        public int? IdentificationTypeId { get; set; }

        [MaxLength(128)]
        public string ApproverId { get; set; }
    }
}
