using GIGLS.CORE.Domain;
using GIGLS.CORE.IRepositories.Shipments;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;
using System.Linq;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.Shipments
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
