using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.Domain.DHL;
using GIGLS.Core.DTO.DHL;
using GIGLS.CORE.DTO.Report;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories.Shipments
{
    public interface IInternationalShipmentWaybillRepository : IRepository<InternationalShipmentWaybill>
    {
        Task<List<InternationalShipmentWaybillDTO>> GetInternationalWaybills(DateFilterCriteria dateFilterCriteria);
    }
}
