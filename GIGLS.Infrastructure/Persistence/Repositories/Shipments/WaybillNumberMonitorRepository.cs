using GIGLS.Core.Domain;
using GIGLS.Core.IRepositories.Shipments;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.Shipments
{
    public class WaybillNumberMonitorRepository : Repository<WaybillNumberMonitor, GIGLSContext>, IWaybillNumberMonitorRepository
    {
        public WaybillNumberMonitorRepository(GIGLSContext context) : base(context)
        {
        }
    }
}
