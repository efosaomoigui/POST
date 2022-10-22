using GIGL.POST.Core.Domain;
using POST.Core.IRepositories.Shipments;
using POST.Infrastructure.Persistence;
using POST.Infrastructure.Persistence.Repository;

namespace POST.INFRASTRUCTURE.Persistence.Repositories.Shipments
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
