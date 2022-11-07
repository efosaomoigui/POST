
using GIGL.POST.Core.Domain;
using POST.Core.Domain.BankSettlement;
using POST.Core.DTO.BankSettlement;
using POST.Core.Enums;
using POST.Core.IRepositories.BankSettlement;
using POST.Infrastructure.Persistence;
using POST.Infrastructure.Persistence.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POST.INFRASTRUCTURE.Persistence.Repositories.BankSettlement
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
