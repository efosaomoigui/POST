using POST.Core.Domain;

namespace GIGL.POST.Core.Domain
{
    public class RegionServiceCentreMapping : BaseDomain
    {
        public int RegionServiceCentreMappingId { get; set; }

        public int RegionId { get; set; }
        public int ServiceCentreId { get; set; }
    }
}