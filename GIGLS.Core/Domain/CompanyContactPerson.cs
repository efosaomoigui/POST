using GIGLS.Core;
using GIGLS.Core.Domain;

namespace GIGL.GIGLS.Core.Domain
{
    public class CompanyContactPerson : BaseDomain, IAuditable
    {
        public int CompanyContactPersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Designation { get; set; }
        public string PhoneNumber { get; set; }

        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }

    }
}