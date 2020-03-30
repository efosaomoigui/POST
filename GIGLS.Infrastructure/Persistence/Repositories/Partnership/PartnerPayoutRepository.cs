using GIGLS.Core.Domain.Partnership;
using GIGLS.Core.IRepositories.Partnership;
using GIGLS.Infrastructure.Persistence.Repository;

namespace GIGLS.Infrastructure.Persistence.Repositories.Partnership
{
    public class PartnerPayoutRepository : Repository<PartnerPayout, GIGLSContext>, IPartnerPayoutRepository
    {
        private GIGLSContext _context;
        public PartnerPayoutRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }
    }
   
}

