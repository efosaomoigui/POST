using GIGL.POST.Core.Domain;
using POST.Core.IRepositories.JobCards;
using POST.Infrastructure.Persistence;
using POST.Infrastructure.Persistence.Repository;

namespace POST.INFRASTRUCTURE.Persistence.Repositories.JobCards
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
