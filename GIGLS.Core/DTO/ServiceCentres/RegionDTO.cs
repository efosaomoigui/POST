using POST.CORE.DTO;

namespace POST.Core.DTO.ServiceCentres
{
    public class RegionDTO : BaseDomainDTO
    {
        public int RegionId { get; set; }
        public string RegionName { get; set; }

        //public ICollection<ServiceCentre> ServiceCentres { get; set; }
    }
}
