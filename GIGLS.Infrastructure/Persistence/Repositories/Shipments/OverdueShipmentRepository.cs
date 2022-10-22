using POST.CORE.Domain;
using POST.CORE.IRepositories.Shipments;
using POST.Infrastructure.Persistence;
using POST.Infrastructure.Persistence.Repository;
using System.Linq;

namespace POST.INFRASTRUCTURE.Persistence.Repositories.Shipments
{
    public class OverdueShipmentRepository : Repository<OverdueShipment, GIGLSContext>, IOverdueShipmentRepository
    {
        private GIGLSContext _context;
        public OverdueShipmentRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<OverdueShipment> GetAllFromOverdueShipmentAsQueryable()
        {
            var result = _context.OverdueShipment.AsQueryable();
            return result;
        }

    }
}
