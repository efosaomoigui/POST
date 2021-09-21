using GIGLS.Core.Domain;
using GIGLS.Core.IRepositories;
using GIGLS.Infrastructure.Persistence.Repository;

namespace GIGLS.Infrastructure.Persistence.Repositories
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
