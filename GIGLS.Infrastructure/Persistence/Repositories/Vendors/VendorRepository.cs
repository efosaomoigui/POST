using GIGL.GIGLS.Core.Domain;
using GIGLS.Core.IRepositories.Vendors;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.Vendors
{
    public class VendorRepository : Repository<Vendor, GIGLSContext>, IVendorRepository
    {
        private GIGLSContext _context;

        public VendorRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }
    }
}
