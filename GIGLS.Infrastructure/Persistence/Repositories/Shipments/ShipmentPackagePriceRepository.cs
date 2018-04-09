using GIGLS.Core.Domain;
using GIGLS.Core.IRepositories.Shipments;
using GIGLS.Infrastructure.Persistence.Repository;

namespace GIGLS.Infrastructure.Persistence.Repositories.Shipments
{
    public class ShipmentPackagePriceRepository : Repository<ShipmentPackagePrice, GIGLSContext>, IShipmentPackagePriceRepository
    {
        public ShipmentPackagePriceRepository(GIGLSContext context) : base(context)
        {
        }
    }
}
