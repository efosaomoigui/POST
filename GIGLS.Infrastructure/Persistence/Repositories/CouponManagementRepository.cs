using POST.Core.Domain;
using POST.Core.IRepositories;
using POST.Infrastructure.Persistence.Repository;

namespace POST.Infrastructure.Persistence.Repositories
{
    public class CouponManagementRepository : Repository<CouponCodeManagement, GIGLSContext>, ICouponManagementRepository
    {
        private GIGLSContext _context;
        public CouponManagementRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }
    }
}
