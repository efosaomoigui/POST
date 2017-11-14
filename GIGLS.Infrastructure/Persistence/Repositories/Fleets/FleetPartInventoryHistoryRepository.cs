using GIGL.GIGLS.Core.Domain;
using GIGLS.Core.IRepositories.Fleets;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.Fleets
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
