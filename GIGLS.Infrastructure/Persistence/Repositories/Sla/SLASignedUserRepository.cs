using GIGLS.Core.Domain.SLA;
using GIGLS.Core.IRepositories.Sla;
using GIGLS.Infrastructure.Persistence.Repository;

namespace GIGLS.Infrastructure.Persistence.Repositories.Sla
{
    public class SLASignedUserRepository : Repository<SLASignedUser, GIGLSContext>, ISLASignedUserRepository
    {
        public SLASignedUserRepository(GIGLSContext context) : base(context)
        {
        }
    }
}