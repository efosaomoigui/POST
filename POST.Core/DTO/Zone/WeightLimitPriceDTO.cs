using POST.Core.Enums;
using POST.CORE.DTO;

namespace POST.Core.DTO.Zone
{
    public class WeightLimitPriceDTO : BaseDomainDTO
    {
        public int WeightLimitPriceId { get; set; }
        
        public int ZoneId { get; set; }

        public string ZoneName { get; set; }

        public decimal Price { get; set; }

        public decimal Weight { get; set; }

        public RegularEcommerceType RegularEcommerceType { get; set; }

        public int CountryId { get; set; }
        public CountryDTO Country { get; set; }
    }
}
