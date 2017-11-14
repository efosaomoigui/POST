using GIGL.GIGLS.Core.Domain;
using GIGLS.Core.IRepositories.Workshops;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.Workshops
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
