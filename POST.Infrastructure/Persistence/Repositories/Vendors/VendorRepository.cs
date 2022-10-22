using GIGL.POST.Core.Domain;
using POST.Core.IRepositories.Vendors;
using POST.Infrastructure.Persistence;
using POST.Infrastructure.Persistence.Repository;

namespace POST.INFRASTRUCTURE.Persistence.Repositories.Vendors
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
