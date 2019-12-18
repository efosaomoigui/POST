using GIGL.GIGLS.Core.Domain;
using GIGLS.Core.Domain;
using GIGLS.Core.IRepositories.Shipments;
using GIGLS.Infrastructure.Persistence.Repository;

namespace GIGLS.Infrastructure.Persistence.Repositories.Shipments
{
    public class ShipmentHashRepository : Repository<ShipmentHash, GIGLSContext>, IShipmentHashRepository
    {
        private GIGLSContext _context;
        public ShipmentHashRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }
    }
}

