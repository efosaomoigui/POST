using GIGLS.Core;
using GIGLS.Core.Domain;
using System.ComponentModel.DataAnnotations;

namespace GIGL.GIGLS.Core.Domain
{
    public class CompanyContactPerson : BaseDomain, IAuditable
    {
        public int CompanyContactPersonId { get; set; }

        [MaxLength(100)]
        public string FirstName { get; set; }

        [MaxLength(100)]
        public string LastName { get; set; }

        [MaxLength(100)]
        public string Email { get; set; }

        [MaxLength(100)]
        public string Designation { get; set; }

        [MaxLength(100)]
        public string PhoneNumber { get; set; }

        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }

    }
}