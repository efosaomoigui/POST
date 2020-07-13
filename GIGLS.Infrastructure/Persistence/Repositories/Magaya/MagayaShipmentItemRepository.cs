using GIGL.GIGLS.Core.Domain;
using GIGLS.Core.IRepositories.Magaya;
using GIGLS.Core.IRepositories.Shipments;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;
 
namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.Magaya
{
    public class MagayaShipmentItemRepository :  Repository<MagayaShipmentItem, GIGLSContext>, IMagayaShipmentItemRepository
    {
        private GIGLSContext _context;
        public MagayaShipmentItemRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }
    }
}
