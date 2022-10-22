using POST.Core.Domain;
using POST.Core.IRepositories.Shipments;
using POST.Infrastructure.Persistence.Repository;

namespace POST.Infrastructure.Persistence.Repositories.Shipments
{
    public class ShipmentRerouteRepository : Repository<ShipmentReroute, GIGLSContext>, IShipmentRerouteRepository
    {
        private GIGLSContext _context;
        public ShipmentRerouteRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }
    }
}
