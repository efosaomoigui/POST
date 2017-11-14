using GIGLS.Core.Domain;
using GIGLS.Core.IRepositories.Shipments;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.Shipments
{
    public class GroupWaybillNumberMonitorRepository : Repository<GroupWaybillNumberMonitor, GIGLSContext>, IGroupWaybillNumberMonitorRepository
    {
        public GroupWaybillNumberMonitorRepository(GIGLSContext context) : base(context)
        {
        }
    }
}
