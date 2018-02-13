using GIGLS.Core.Domain;
using GIGLS.Core.IRepositories.Zone;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;


namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.Zone
{
    public class CountryRouteZoneMapRepository : Repository<CountryRouteZoneMap, GIGLSContext>, ICountryRouteZoneMapRepository
    {
        public CountryRouteZoneMapRepository(GIGLSContext context) : base(context)
        {
        }
    }
}
