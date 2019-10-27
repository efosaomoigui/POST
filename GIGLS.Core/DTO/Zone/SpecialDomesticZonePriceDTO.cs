using GIGLS.Core.DTO.User;
using GIGLS.CORE.DTO;
using System.Collections.Generic;

namespace GIGLS.Core.DTO.Zone
{
    public class SpecialDomesticZonePriceDTO : BaseDomainDTO
    {
        public SpecialDomesticZonePriceDTO()
        {
            UserDetail = new List<UserDTO>();
        }

        public int SpecialDomesticZonePriceId { get; set; }

        public decimal Weight { get; set; }
        public decimal Price { get; set; }

        public string Description { get; set; }

        public int ZoneId { get; set; }
        public string ZoneName { get; set; }

        public int SpecialDomesticPackageId { get; set; }
        public string SpecialDomesticPackageName { get; set; }
        
        public string UserId { get; set; }
        public string UserName { get; set; }

        public List<UserDTO> UserDetail { get; set; }

        public int CountryId { get; set; }
        public CountryDTO Country { get; set; }
    }

}
