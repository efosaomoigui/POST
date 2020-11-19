using GIGLS.Core.DTO.DHL;
using GIGLS.CORE.DTO.Report;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Shipments
{
    public interface IInternationalShipmentWaybillService : IServiceDependencyMarker
    {
        Task<List<InternationalShipmentWaybillDTO>> GetInternationalWaybills(DateFilterCriteria dateFilterCriteria);
        Task<InternationalShipmentWaybillDTO> GetInternationalWaybill(string waybill);
    }
}
