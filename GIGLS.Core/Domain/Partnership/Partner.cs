using GIGLS.Core.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace GIGLS.Core.Domain.Partnership
{
    public class Partner : BaseDomain, IAuditable
    {
        public int PartnerId { get; set; }

        [MaxLength(100)]
        public string PartnerName { get; set; }

        [MaxLength(100)]
        public string PartnerCode { get; set; }

        [MaxLength(100)]
        public string FirstName { get; set; }

        [MaxLength(100)]
        public string LastName { get; set; }

        [MaxLength(100)]
        public string Email { get; set; }

        [MaxLength(100)]
        public string PhoneNumber { get; set; }

        [MaxLength(100)]
        public string OptionalPhoneNumber { get; set; }
        public string Address { get; set; }

        public virtual PartnerType PartnerType { get; set; }

        [MaxLength(128)]
        public string UserId { get; set; }

        public bool IsActivated { get; set; }

        [MaxLength(100)]
        public string VehicleType { get; set; }

        public string PictureUrl { get; set; }

        [MaxLength(100)]
        public string BankName { get; set; }

        [MaxLength(100)]
        public string AccountNumber { get; set; }

        [MaxLength(100)]
        public string AccountName { get; set; }

        [MaxLength(100)]
        public string VehicleLicenseNumber { get; set; }

        public DateTime? VehicleLicenseExpiryDate { get; set; }

        public string VehicleLicenseImageDetails { get; set; }

        public int UserActiveCountryId { get; set; }

        [MaxLength(100)]
        public string FleetPartnerCode { get; set; }

        public ActivityStatus ActivityStatus { get; set; }

        public DateTime ActivityDate { get; set; }

        public bool Contacted { get; set; }
    }
}
