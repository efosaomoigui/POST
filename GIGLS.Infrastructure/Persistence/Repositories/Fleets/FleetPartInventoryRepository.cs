using GIGL.GIGLS.Core.Domain;
using GIGLS.Core.IRepositories.Fleets;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.Fleets
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
