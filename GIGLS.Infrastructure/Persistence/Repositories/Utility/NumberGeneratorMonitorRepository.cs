using GIGLS.Core.Domain;
using GIGLS.Core.IRepositories.Utility;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.Utility
{
    public class NumberGeneratorMonitorRepository : Repository<NumberGeneratorMonitor, GIGLSContext>, INumberGeneratorMonitorRepository
    {
        public NumberGeneratorMonitorRepository(GIGLSContext context) : base(context)
        {
        }
    }
}
