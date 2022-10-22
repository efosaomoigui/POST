using GIGL.POST.Core.Domain;
using POST.Core.IRepositories.Zone;
using POST.Infrastructure.Persistence;
using POST.Infrastructure.Persistence.Repository;


namespace POST.INFRASTRUCTURE.Persistence.Repositories.Zone
{
    public class DomesticRouteZoneMapRepository : Repository<DomesticRouteZoneMap, GIGLSContext>, IDomesticRouteZoneMapRepository
    {
        public DomesticRouteZoneMapRepository(GIGLSContext context) : base(context)
        {
        }
    }
}
