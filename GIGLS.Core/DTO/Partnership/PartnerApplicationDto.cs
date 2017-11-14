using GIGLS.Core.Enums;
using GIGLS.CORE.DTO;

namespace GIGLS.Core.DTO.Partnership
{
    public class PartnerApplicationDTO : BaseDomainDTO
    {
        public int PartnerApplicationId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string PartnerName { get; set; }
        public string CompanyRcNumber { get; set; }
        public string IdentificationNumber { get; set; }
        public string TellAboutYou { get; set; }
        public string IdentificationType { get; set; }
        public bool IsRegistered { get; set; }
        public PartnerType PartnerType { get; set; }

        public string Approver { get; set; }
        public PartnerApplicationStatus PartnerApplicationStatus { get; set; }

    }
}
