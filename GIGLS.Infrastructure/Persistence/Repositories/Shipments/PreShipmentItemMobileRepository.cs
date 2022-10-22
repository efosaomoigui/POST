using POST.Core.Domain;
using POST.Core.IRepositories.Shipments;
using POST.Infrastructure.Persistence.Repository;

namespace POST.Infrastructure.Persistence.Repositories.Shipments
{
    public class PreShipmentItemMobileRepository : Repository<PreShipmentItemMobile, GIGLSContext>, IPreShipmentItemMobileRepository
    {
        //private GIGLSContext _context;
        public PreShipmentItemMobileRepository(GIGLSContext context) : base(context)
        {
            //_context = context;
        }
    }
}
