using GIGLS.Core.Domain;
using GIGLS.Core.IRepositories;
using GIGLS.Infrastructure.Persistence.Repository;

namespace GIGLS.Infrastructure.Persistence.Repositories
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
