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

        public bool IsCod { get; set; }

        [MaxLength(500)]
        public string AccountName { get; set; }

        [MaxLength(100)]
        public string AccountNumber { get; set; }

        [MaxLength(100)]
        public string BankName { get; set; }

        public bool IsApi { get; set; }

        [MaxLength(100)]
        public string Source { get; set; }


    }
}
