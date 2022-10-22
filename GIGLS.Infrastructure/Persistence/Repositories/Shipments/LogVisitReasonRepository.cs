using POST.Core.Domain;
using POST.Core.IRepositories.Shipments;
using POST.Infrastructure.Persistence.Repository;

namespace POST.Infrastructure.Persistence.Repositories.Shipments
{
    public class LogVisitReasonRepository : Repository<LogVisitReason, GIGLSContext>, ILogVisitReasonRepository
    {
        public LogVisitReasonRepository(GIGLSContext context) : base(context)
        {
        }
    }
}
