using GIGL.GIGLS.Core.Domain;
using GIGLS.CORE.DTO;
using System.Collections.Generic;

namespace GIGLS.Core.DTO.ServiceCentres
{
    public class RegionDTO : BaseDomainDTO
    {
        public int RegionId { get; set; }
        public string RegionName { get; set; }

        public ICollection<ServiceCentre> ServiceCentres { get; set; }
    }
}
