using GIGL.GIGLS.Core.Domain;
using GIGLS.Core.IRepositories.JobCards;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.JobCards
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
