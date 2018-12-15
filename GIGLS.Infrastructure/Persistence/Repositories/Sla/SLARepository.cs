using GIGLS.Core.Domain.SLA;
using GIGLS.Core.IRepositories.Sla;
using GIGLS.Infrastructure.Persistence.Repository;

namespace GIGLS.Infrastructure.Persistence.Repositories.Sla
{
    public class SLARepository : Repository<SLA, GIGLSContext>, ISLARepository
    {
        public SLARepository(GIGLSContext context) : base(context)
        {
        }
    }
}
