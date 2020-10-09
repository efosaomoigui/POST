using GIGLS.Core.Enums;
using GIGLS.CORE.DTO;
using System;
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
        public int CountryId { get; set; }
        [Required]
        public string ContactName { get; set; }
        [Required]
        public string ContactEmail { get; set; }
        [Required]
        public string ContactPhoneNumber { get; set; }
        [Required]
        public string ReturnAddress { get; set; }
        [Required]
        public string EcommerceSignatureName { get; set; }
        [Required]
        public string EcommerceSignatureAddress { get; set; }
        public DateTime? AgreementDate { get; set; }
        [Required]
        public List<string> NatureOfBusiness { get; set; }

        public string Industry { get; set; }

        public EcommerceAgreementStatus Status { get; set; }

        [Required]
        public bool IsCod { get; set; }

        [Required]
        public string Source { get; set; }

        public string AccountName { get; set; }
        
        public string AccountNumber { get; set; }
        
        public string BankName { get; set; }

        public bool IsApi { get; set; }
    }

}
