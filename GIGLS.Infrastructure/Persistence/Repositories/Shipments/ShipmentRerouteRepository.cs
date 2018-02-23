using GIGLS.Core.Domain;
using GIGLS.Core.IRepositories.Shipments;
using GIGLS.Infrastructure.Persistence.Repository;

namespace GIGLS.Infrastructure.Persistence.Repositories.Shipments
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
