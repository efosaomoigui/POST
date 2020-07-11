using GIGLS.CORE.DTO;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GIGLS.Core.DTO.Customers
{
    public class EcommerceAgreementDTO : BaseDomainDTO
    {
        public int EcommerceAgreementId { get; set; }
        [Required]
        public string BusinessEmail { get; set; }
        [Required]
        public string BusinessOwnerName { get; set; }
        [Required]
        public string OfficeAddress { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string ContactName { get; set; }
        [Required]
        public string ContactEmail { get; set; }
        [Required]
        public string ReturnAddress { get; set; }
        [Required]
        public EcommerceSignatureDTO EcommerceSignature { get; set; }
        [Required]
        public List<string> NatureOfBusiness { get; set; }
    }

    public class EcommerceSignatureDTO 
    {
        [Required]
        public string EcommerceSignatureName { get; set; }
        [Required]
        public string EcommerceSignatureAddress { get; set; }
    }

}
