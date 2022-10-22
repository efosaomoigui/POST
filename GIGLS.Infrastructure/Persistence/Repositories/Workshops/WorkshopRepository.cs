using GIGL.POST.Core.Domain;
using POST.Core.IRepositories.Workshops;
using POST.Infrastructure.Persistence;
using POST.Infrastructure.Persistence.Repository;

namespace POST.INFRASTRUCTURE.Persistence.Repositories.Workshops
{
    public class WorkshopRepository : Repository<Workshop, GIGLSContext>, IWorkshopRepository
    {
        private GIGLSContext _context;

        public WorkshopRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }
    }
}
