using GIGL.GIGLS.Core.Domain;
using GIGLS.Core.IRepositories.Zone;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;


namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.Zone
{
    public class DomesticRouteZoneMapRepository : Repository<DomesticRouteZoneMap, GIGLSContext>, IDomesticRouteZoneMapRepository
    {
        public DomesticRouteZoneMapRepository(GIGLSContext context) : base(context)
        {
        }
    }
}
