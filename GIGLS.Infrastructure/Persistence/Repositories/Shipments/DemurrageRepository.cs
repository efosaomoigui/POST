using GIGLS.Core.Domain;
using GIGLS.Core.IRepositories.Shipments;
using GIGLS.Infrastructure.Persistence.Repository;

namespace GIGLS.Infrastructure.Persistence.Repositories.Shipments
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
