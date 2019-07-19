using GIGLS.CORE.DTO;
using System.Collections.Generic;

namespace GIGLS.Core.DTO.Zone
{
    public class ZonePercentDTO : BaseDomainDTO
    {
        public string Category { get; set; }
        public string PriceType { get; set; }
        public string CustomerType { get; set; }
        public List<ZoneDTO> Zones { get; set; }
    }
}
