using POST.Core.Domain;
using POST.Core.IRepositories.ServiceCentres;
using POST.Infrastructure.Persistence;
using POST.Infrastructure.Persistence.Repository;

namespace POST.INFRASTRUCTURE.Persistence.Repositories.ServiceCentres
{
    public class RegionRepository : Repository<Region, GIGLSContext>, IRegionRepository
    {
        public RegionRepository(GIGLSContext context) : base(context)
        {
        }

    }
}
