using GIGLS.Core.Enums;
using GIGLS.CORE.DTO;
using System;
using System.Collections.Generic;

namespace GIGLS.Core.DTO.Partnership
{
    public class PartnerDTO : BaseDomainDTO
    {
        public int PartnerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address;
        public string PartnerName { get; set; }
        public string PartnerCode { get; set; }
        public string OptionalPhoneNumber { get; set; }
        public PartnerType PartnerType { get; set; }
        public int PartnerApplicationId { get; set; }
        public string IdentificationNumber { get; set; }
        public int WalletId { get; set; }
        public string WalletPan { get; set; }
        public string UserId { get; set; }
        public bool IsActivated { get; set; }
        public List<string> VehicleType { get; set; }
        public List<VehicleTypeDTO> VehicleTypeDetails { get; set; }

        public string PictureUrl { get; set; }
        public string BankName { get; set; }
        public long? AccountNumber { get; set; }

        public string AccountName { get; set; }
        public string VehicleLicenseNumber { get; set; }
        public DateTime? VehicleLicenseExpiryDate { get; set; }

        public string VehicleLicenseImageDetails { get; set; }

    }
}
