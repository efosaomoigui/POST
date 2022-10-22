using GIGL.POST.Core.Domain;
using POST.Core.IRepositories.ServiceCentres;
using POST.Infrastructure.Persistence;
using POST.Infrastructure.Persistence.Repository;

namespace POST.INFRASTRUCTURE.Persistence.Repositories.ServiceCentres
{
    public class RegionServiceCentreMappingRepository : Repository<RegionServiceCentreMapping, GIGLSContext>, IRegionServiceCentreMappingRepository
    {
        public RegionServiceCentreMappingRepository(GIGLSContext context) : base(context)
        {
        }

    }
}
