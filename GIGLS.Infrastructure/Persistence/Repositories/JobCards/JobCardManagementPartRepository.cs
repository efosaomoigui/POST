using GIGL.POST.Core.Domain;
using POST.Core.IRepositories.JobCards;
using POST.Infrastructure.Persistence;
using POST.Infrastructure.Persistence.Repository;

namespace POST.INFRASTRUCTURE.Persistence.Repositories.JobCards
{
    public class JobCardManagementPartRepository : Repository<JobCardManagementPart, GIGLSContext>, IJobCardManagementPartRepository
    {
        private GIGLSContext _context;
        public JobCardManagementPartRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }
    }
}
