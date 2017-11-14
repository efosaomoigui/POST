using GIGL.GIGLS.Core.Domain;
using GIGLS.Core.IRepositories.Shipments;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.Shipments
{
    public class ShipmentItemRepository :  Repository<ShipmentItem, GIGLSContext>, IShipmentItemRepository
    {
        private GIGLSContext _context;
        public ShipmentItemRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }
    }
}
