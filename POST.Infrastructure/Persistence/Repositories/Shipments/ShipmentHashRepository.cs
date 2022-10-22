using GIGL.POST.Core.Domain;
using POST.Core.Domain;
using POST.Core.IRepositories.Shipments;
using POST.Infrastructure.Persistence.Repository;

namespace POST.Infrastructure.Persistence.Repositories.Shipments
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

