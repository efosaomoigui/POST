using System.ComponentModel.DataAnnotations;

namespace GIGLS.Core.DTO.UPS
{
    public class UPSAddress
    {
        [MaxLength(50, ErrorMessage = "Address cannot be more than 50 characters")]
        public string AddressLine { get; set; }
        public string City { get; set; }
        public string StateProvinceCode { get; set; }
        public string PostalCode { get; set; }
        public string CountryCode { get; set; }
    }
}
