using POST.Core.Domain.SLA;
using POST.Core.IRepositories.Sla;
using POST.Infrastructure.Persistence.Repository;

namespace POST.Infrastructure.Persistence.Repositories.Sla
{
    public class SLASignedUserRepository : Repository<SLASignedUser, GIGLSContext>, ISLASignedUserRepository
    {
        public SLASignedUserRepository(GIGLSContext context) : base(context)
        {
        }
    }
}