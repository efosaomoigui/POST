using GIGL.GIGLS.Core.Domain;
using GIGLS.Core.IRepositories.JobCards;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.JobCards
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
