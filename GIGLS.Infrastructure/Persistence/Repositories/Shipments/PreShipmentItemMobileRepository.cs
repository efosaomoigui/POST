using GIGLS.Core.Domain;
using GIGLS.Core.IRepositories.Shipments;
using GIGLS.Infrastructure.Persistence.Repository;

namespace GIGLS.Infrastructure.Persistence.Repositories.Shipments
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
