﻿using GIGLS.Core.Enums;
using GIGLS.CORE.DTO;

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
        public string VehicleType { get; set; }

        public string PictureUrl { get; set; }
    }
}
