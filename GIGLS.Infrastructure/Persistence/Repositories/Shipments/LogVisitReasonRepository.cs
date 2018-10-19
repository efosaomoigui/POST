using GIGLS.Core.Domain;
using GIGLS.Core.IRepositories.Shipments;
using GIGLS.Infrastructure.Persistence.Repository;

namespace GIGLS.Infrastructure.Persistence.Repositories.Shipments
{
    public class LogVisitReasonRepository : Repository<LogVisitReason, GIGLSContext>, ILogVisitReasonRepository
    {
        public LogVisitReasonRepository(GIGLSContext context) : base(context)
        {
        }
    }
}
