using GIGLS.Core.Enums;
using GIGLS.CORE.DTO;
using System.Collections.Generic;

namespace GIGLS.Core.DTO.Zone
{
    public class ZonePercentDTO : BaseDomainDTO
    {
        public string Category { get; set; }
        public PricingType PriceType { get; set; }
        public PartnerType CustomerType { get; set; }
        public ModificationType ModificationType { get; set; }
        public RateType RateType { get; set; }
        public List<ZoneDTO> Zones { get; set; }
    }
}
