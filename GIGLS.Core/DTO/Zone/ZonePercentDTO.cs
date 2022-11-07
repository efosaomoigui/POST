using POST.Core.Enums;
using POST.CORE.DTO;
using System.Collections.Generic;

namespace POST.Core.DTO.Zone
{
    public class ZonePercentDTO : BaseDomainDTO
    {
        public string Category { get; set; }
        public PricingType PriceType { get; set; }
        public PartnerType CustomerType { get; set; }
        public ModificationType ModificationType { get; set; }
        public RateType RateType { get; set; }
        public List<ZoneDTO> Zones { get; set; }
        public int CountryId { get; set; }
    }
}
