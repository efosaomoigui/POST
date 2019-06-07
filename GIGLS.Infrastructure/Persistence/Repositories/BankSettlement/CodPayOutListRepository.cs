
using GIGL.GIGLS.Core.Domain;
using GIGLS.Core.Domain.BankSettlement;
using GIGLS.Core.DTO.BankSettlement;
using GIGLS.Core.Enums;
using GIGLS.Core.IRepositories.BankSettlement;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.BankSettlement
{
    public class CodPayOutListRepository : Repository<CodPayOutList, GIGLSContext>, ICodPayOutListRepository
    {
        public CodPayOutListRepository(GIGLSContext context) : base(context)
        {
        }

        public Task<Shipment> GetShipmentByWaybill(string waybill)
        {
            var shipment = Context.Shipment.Where(x => x.Waybill == waybill).FirstOrDefault();
            return Task.FromResult(shipment);
        }
    }
}
