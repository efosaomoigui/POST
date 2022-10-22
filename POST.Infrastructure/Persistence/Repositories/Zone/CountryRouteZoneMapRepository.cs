using POST.Core.Domain;
using POST.Core.IRepositories.Zone;
using POST.Infrastructure.Persistence;
using POST.Infrastructure.Persistence.Repository;


namespace POST.INFRASTRUCTURE.Persistence.Repositories.Zone
{
    public class CountryRouteZoneMapRepository : Repository<CountryRouteZoneMap, GIGLSContext>, ICountryRouteZoneMapRepository
    {
        public CountryRouteZoneMapRepository(GIGLSContext context) : base(context)
        {
        }
    }
}
