using GIGL.POST.Core.Domain;
using POST.Core.IRepositories.Fleets;
using POST.Infrastructure.Persistence;
using POST.Infrastructure.Persistence.Repository;

namespace POST.INFRASTRUCTURE.Persistence.Repositories.Fleets
{
    public class FleetPartInventoryHistoryRepository : Repository<FleetPartInventoryHistory, GIGLSContext>, IFleetPartInventoryHistoryRepository
    {
        private GIGLSContext _context;

        public FleetPartInventoryHistoryRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }
    }
}
