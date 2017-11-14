using GIGLS.Core.Domain;
using GIGLS.Core.IRepositories.Shipments;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.Shipments
{
    public class ManifestMonitorRepository : Repository<ManifestMonitor, GIGLSContext>, IManifestMonitorRepository
    {
        public ManifestMonitorRepository(GIGLSContext context) : base(context)
        {
        }
    }
}
