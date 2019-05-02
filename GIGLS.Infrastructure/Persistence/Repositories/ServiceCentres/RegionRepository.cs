using GIGLS.Core.Domain;
using GIGLS.Core.IRepositories.ServiceCentres;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.ServiceCentres
{
    public class RegionRepository : Repository<Region, GIGLSContext>, IRegionRepository
    {
        public RegionRepository(GIGLSContext context) : base(context)
        {
        }

    }
}
