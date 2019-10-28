using GIGLS.CORE.DTO;

namespace GIGLS.Core.DTO.ServiceCentres
{
    public class RegionDTO : BaseDomainDTO
    {
        public int RegionId { get; set; }
        public string RegionName { get; set; }

        //public ICollection<ServiceCentre> ServiceCentres { get; set; }
    }
}
