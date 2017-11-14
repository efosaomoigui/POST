using GIGL.GIGLS.Core.Domain;
using GIGLS.Core.IRepositories.JobCards;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.JobCards
{
    public class JobCardManagementRepository : Repository<JobCardManagement, GIGLSContext> , IJobCardManagementRepository
    {
        private GIGLSContext _context;
        public JobCardManagementRepository(GIGLSContext context)
            : base(context)
        {
            _context = context;
        }
    }
}
