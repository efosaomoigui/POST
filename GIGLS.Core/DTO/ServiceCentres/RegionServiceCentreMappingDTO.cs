using GIGLS.CORE.DTO;
using System.Collections.Generic;

namespace GIGLS.Core.DTO.ServiceCentres
{
    public class RegionServiceCentreMappingDTO : BaseDomainDTO
    {
        public int RegionServiceCentreMappingId { get; set; }

        public int RegionId { get; set; }
        public int ServiceCentreId { get; set; }

        public RegionDTO RegionDTO { get; set; }
        public ServiceCentreDTO ServiceCentreDTO { get; set; }

        public List<int> ServiceCentreIds { get; set; }
    }
}
