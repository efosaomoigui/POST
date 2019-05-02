using GIGLS.CORE.DTO;

namespace GIGLS.Core.DTO.ServiceCentres
{
    public class RegionServiceCentreMappingDTO : BaseDomainDTO
    {
        public int RegionServiceCentreMappingId { get; set; }

        public int RegionId { get; set; }
        public int ServiceCentreId { get; set; }
    }
}
