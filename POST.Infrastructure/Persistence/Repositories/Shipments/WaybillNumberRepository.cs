using System.Collections.Generic;
using System.Threading.Tasks;
using POST.Core.Domain;
using POST.Core.DTO.ServiceCentres;
using POST.Core.IRepositories.Shipments;
using POST.Infrastructure.Persistence;
using POST.Infrastructure.Persistence.Repository;
using System.Linq;
using POST.Core.DTO.Shipments;

namespace POST.INFRASTRUCTURE.Persistence.Repositories.Shipments
{
    public class WaybillNumberRepository : Repository<WaybillNumber, GIGLSContext>, IWaybillNumberRepository
    {
        public WaybillNumberRepository(GIGLSContext context) : base(context)
        {
        }

        public Task<List<WaybillNumberDTO>> GetWaybills()
        {
            var waybill = Context.WaybillNumber.Include("ServiceCentre");

            var waybillDto = from w in waybill
                             select new WaybillNumberDTO
                             {
                                 IsActive = w.IsActive,
                                 WaybillCode = w.WaybillCode,
                                 WaybillNumberId = w.WaybillNumberId,
                                 ServiceCentreId = w.ServiceCentreId,
                                 UserId = w.UserId,
                                 ServiceCentre = w.ServiceCentre.Name
                             };
            return Task.FromResult(waybillDto.ToList());
        }
    }
}
