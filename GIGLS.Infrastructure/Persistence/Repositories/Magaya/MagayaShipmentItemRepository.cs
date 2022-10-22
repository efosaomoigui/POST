using GIGL.POST.Core.Domain;
using POST.Core.IRepositories.Magaya;
using POST.Core.IRepositories.Shipments;
using POST.Infrastructure.Persistence;
using POST.Infrastructure.Persistence.Repository;
 
namespace POST.INFRASTRUCTURE.Persistence.Repositories.Magaya
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
