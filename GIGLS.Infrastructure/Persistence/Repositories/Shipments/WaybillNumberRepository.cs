using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.Core.IRepositories.Shipments;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;
using System.Linq;
using GIGLS.Core.DTO.Shipments;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.Shipments
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
