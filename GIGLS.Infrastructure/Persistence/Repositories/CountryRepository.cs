using GIGLS.Core.Domain;
using GIGLS.Core.IRepositories;
using GIGLS.Infrastructure.Persistence.Repository;

namespace GIGLS.Infrastructure.Persistence.Repositories
{
    public class CountryRepository : Repository<Country, GIGLSContext>, ICountryRepository
    {
        public CountryRepository(GIGLSContext context) : base(context)
        {
        }
    }
}
