using POST.Core.Domain.SLA;
using POST.Core.IRepositories.Sla;
using POST.Infrastructure.Persistence.Repository;

namespace POST.Infrastructure.Persistence.Repositories.Sla
{
    public class SLARepository : Repository<SLA, GIGLSContext>, ISLARepository
    {
        public SLARepository(GIGLSContext context) : base(context)
        {
        }
    }
}
