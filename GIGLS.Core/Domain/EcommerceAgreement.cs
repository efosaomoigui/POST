using GIGLS.Core.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace GIGLS.Core.Domain
{
    public class EcommerceAgreement : BaseDomain, IAuditable
    {
        public int EcommerceAgreementId { get; set; }

        [MaxLength(100)]
        public string BusinessEmail { get; set; }

        [MaxLength(200)]
        public string BusinessOwnerName { get; set; }

        [MaxLength(500)]
        public string OfficeAddress { get; set; }

        [MaxLength(50)]
        public string City { get; set; }

        [MaxLength(50)]
        public string State { get; set; }

        public int CountryId { get; set; }

        [MaxLength(200)]
        public string ContactName { get; set; }

        [MaxLength(500)]
        public string ReturnAddress { get; set; }

        [MaxLength(100)]
        public string ContactEmail { get; set; }

        [MaxLength(100)]
        public string ContactPhoneNumber { get; set; }

        public DateTime? AgreementDate { get; set; }

        [MaxLength(500)]
        public string EcommerceSignature { get; set; }

        [MaxLength(500)]
        public string NatureOfBusiness { get; set; }

        public EcommerceAgreementStatus Status { get; set; }


    }
}
