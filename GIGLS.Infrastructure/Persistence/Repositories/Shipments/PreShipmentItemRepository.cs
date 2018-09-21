using GIGL.GIGLS.Core.Domain;
using GIGLS.Core.IRepositories.Shipments;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.Shipments
{
    public class PreShipmentItemRepository :  Repository<PreShipmentItem, GIGLSContext>, IPreShipmentItemRepository
    {
        private GIGLSContext _context;
        public PreShipmentItemRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }
    }
}
