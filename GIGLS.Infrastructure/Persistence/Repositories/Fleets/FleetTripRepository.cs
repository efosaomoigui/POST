using GIGL.GIGLS.Core.Domain;
using GIGLS.Core.IRepositories.Fleets;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.Fleets
{
    public class FleetTripRepository : Repository<FleetTrip, GIGLSContext>, IFleetTripRepository
    {
        private GIGLSContext _context;

        public FleetTripRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }
    }
}
