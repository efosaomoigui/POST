using GIGLS.Core.Domain.Wallet;
using GIGLS.Core.IRepositories.Wallet;
using GIGLS.Infrastructure.Persistence.Repository;

namespace GIGLS.Infrastructure.Persistence.Repositories.Wallet
{
    public class CountryRateConversionRepository : Repository<CountryRateConversion, GIGLSContext>, ICountryRateConversionRepository
    {
        private GIGLSContext _context;

        public CountryRateConversionRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }
    }
}
