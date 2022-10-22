using POST.Core.Domain;
using POST.Core.IRepositories.Utility;
using POST.Infrastructure.Persistence;
using POST.Infrastructure.Persistence.Repository;

namespace POST.INFRASTRUCTURE.Persistence.Repositories.Utility
{
    public class NumberGeneratorMonitorRepository : Repository<NumberGeneratorMonitor, GIGLSContext>, INumberGeneratorMonitorRepository
    {
        public NumberGeneratorMonitorRepository(GIGLSContext context) : base(context)
        {
        }
    }
}
