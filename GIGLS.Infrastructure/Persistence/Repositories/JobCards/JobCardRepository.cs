using GIGL.POST.Core.Domain;
using POST.Core.IRepositories.JobCards;
using POST.Infrastructure.Persistence;
using POST.Infrastructure.Persistence.Repository;

namespace POST.INFRASTRUCTURE.Persistence.Repositories.JobCards
{
    public class JobCardRepository : Repository<JobCard, GIGLSContext>, IJobCardRepository
    {
        private GIGLSContext _context;
        public JobCardRepository(GIGLSContext context)
            : base(context)
        {
            _context = context;
        }
    }
}
