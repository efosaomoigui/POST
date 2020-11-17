using GIGLS.Core.Domain.DHL;
using GIGLS.Core.IRepositories.Shipments;
using GIGLS.Infrastructure.Persistence.Repository;

namespace GIGLS.Infrastructure.Persistence.Repositories.Shipments
{
    public class InternationalShipmentWaybillRepository : Repository<InternationalShipmentWaybill, GIGLSContext>, IInternationalShipmentWaybillRepository
    {
        public InternationalShipmentWaybillRepository(GIGLSContext context) : base(context)
        {

        }
    }
}
