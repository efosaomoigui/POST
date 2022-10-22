using GIGL.POST.Core.Domain;
using POST.Core.IRepositories.Shipments;
using POST.Infrastructure.Persistence;
using POST.Infrastructure.Persistence.Repository;

namespace POST.INFRASTRUCTURE.Persistence.Repositories.Shipments
{
    public class ShipmentItemRepository :  Repository<ShipmentItem, GIGLSContext>, IShipmentItemRepository
    {
        private GIGLSContext _context;
        public ShipmentItemRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }
    }

    public class IntlShipmentRequestItemRepository : Repository<IntlShipmentRequestItem, GIGLSContext>, IIntlShipmentRequestItemRepository 
    {
        private GIGLSContext _context;
        public IntlShipmentRequestItemRepository(GIGLSContext context) : base(context) 
        {
            _context = context;
        }
    }
}
