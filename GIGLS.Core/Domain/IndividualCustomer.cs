using GIGLS.Core;
using GIGLS.Core.Domain;
using GIGLS.Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GIGL.GIGLS.Core.Domain
{
    public class IndividualCustomer : BaseDomain, IAuditable
    {
        public IndividualCustomer()
        {
        }
        public int IndividualCustomerId { get; set; }

        [MaxLength(500)]
        public string FirstName { get; set; }

        [MaxLength(500)]
        public string LastName { get; set; }
        public Gender Gender { get; set; }

        [MaxLength(500)]
        public string Email { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Address { get; set; }

        //User Active CountryId
        public int UserActiveCountryId { get; set; }

        [MaxLength(20), MinLength(3)]
        [Index(IsUnique = true)]
        public string PhoneNumber { get; set; }
        public string PictureUrl { get; set; }

        [MaxLength(100)]
        public string CustomerCode { get; set; }
        public string Password { get; set; }

        public bool IsRegisteredFromMobile { get; set; }
    }
}