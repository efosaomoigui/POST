using GIGLS.CORE.Domain;
using GIGLS.CORE.IRepositories.Shipments;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.Shipments
{
    public class ShipmentCollectionRepository : Repository<ShipmentCollection, GIGLSContext>, IShipmentCollectionRepository
    {
        private GIGLSContext _context;
        public ShipmentCollectionRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }
    }
}
