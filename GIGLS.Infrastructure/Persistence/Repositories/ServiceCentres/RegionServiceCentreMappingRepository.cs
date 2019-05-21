using GIGL.GIGLS.Core.Domain;
using GIGLS.Core.IRepositories.ServiceCentres;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.ServiceCentres
{
    public class RegionServiceCentreMappingRepository : Repository<RegionServiceCentreMapping, GIGLSContext>, IRegionServiceCentreMappingRepository
    {
        public RegionServiceCentreMappingRepository(GIGLSContext context) : base(context)
        {
        }

    }
}
