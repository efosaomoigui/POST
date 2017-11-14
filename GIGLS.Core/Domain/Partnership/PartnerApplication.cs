using GIGL.GIGLS.Core.Domain;
using GIGLS.Core.Enums;

namespace GIGLS.Core.Domain.Partnership
{
    public class PartnerApplication : BaseDomain, IAuditable
    {
        public int PartnerApplicationId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public string Email { get; set; }
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
        public virtual IdentificationType IdentificationType { get; set; }

        public int? ApproverId { get; set; }
        public virtual User Approver { get; set; }
    }
}
