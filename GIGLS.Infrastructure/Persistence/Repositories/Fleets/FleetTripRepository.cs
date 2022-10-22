using GIGL.POST.Core.Domain;
using POST.Core.IRepositories.Fleets;
using POST.Infrastructure.Persistence;
using POST.Infrastructure.Persistence.Repository;

namespace POST.INFRASTRUCTURE.Persistence.Repositories.Fleets
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
