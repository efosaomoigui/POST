using GIGL.POST.Core.Repositories;
using POST.Core.Domain.DHL;
using POST.Core.DTO.DHL;
using POST.CORE.DTO.Report;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Core.IRepositories.Shipments
{
    public interface IInternationalShipmentWaybillRepository : IRepository<InternationalShipmentWaybill>
    {
        Task<List<InternationalShipmentWaybillDTO>> GetInternationalWaybills(DateFilterCriteria dateFilterCriteria);
    }
}
