using POST.CORE.Domain;
using POST.CORE.IRepositories.Shipments;
using POST.Infrastructure.Persistence;
using POST.Infrastructure.Persistence.Repository;

namespace POST.INFRASTRUCTURE.Persistence.Repositories.Shipments
{
    public class ShipmentReturnRepository : Repository<ShipmentReturn, GIGLSContext>, IShipmentReturnRepository
    {
        private GIGLSContext _context;
        public ShipmentReturnRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }
    }
}
