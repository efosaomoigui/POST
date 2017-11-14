using GIGLS.CORE.Domain;
using GIGLS.CORE.IRepositories.Shipments;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.Shipments
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
