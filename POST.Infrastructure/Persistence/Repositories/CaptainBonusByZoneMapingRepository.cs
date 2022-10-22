using POST.Core.Domain;
using POST.Core.IRepositories;
using POST.Infrastructure.Persistence.Repository;

namespace POST.Infrastructure.Persistence.Repositories
{
    public class CaptainBonusByZoneMapingRepository : Repository<CaptainBonusByZoneMaping, GIGLSContext>, ICaptainBonusByZoneMapingRepository
    {
        private GIGLSContext _context;
        public CaptainBonusByZoneMapingRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }
    }
}
