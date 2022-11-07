using POST.Core.Domain;
using POST.Core.IRepositories.Shipments;
using POST.Infrastructure.Persistence.Repository;

namespace POST.Infrastructure.Persistence.Repositories.Shipments
{
    public class DemurrageRepository : Repository<Demurrage, GIGLSContext>, IDemurrageRepository
    {
        private GIGLSContext _context;
        public DemurrageRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }
    }
}
