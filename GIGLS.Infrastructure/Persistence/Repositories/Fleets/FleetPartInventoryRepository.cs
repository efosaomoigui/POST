using GIGL.POST.Core.Domain;
using POST.Core.IRepositories.Fleets;
using POST.Infrastructure.Persistence;
using POST.Infrastructure.Persistence.Repository;

namespace POST.INFRASTRUCTURE.Persistence.Repositories.Fleets
{
    public class FleetPartInventoryRepository : Repository<FleetPartInventory, GIGLSContext>, IFleetPartInventoryRepository
    {
        private GIGLSContext _context;

        public FleetPartInventoryRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }
    }
}
