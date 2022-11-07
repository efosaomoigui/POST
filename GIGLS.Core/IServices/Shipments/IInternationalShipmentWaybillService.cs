using POST.Core.DTO.DHL;
using POST.CORE.DTO.Report;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Core.IServices.Shipments
{
    public interface IInternationalShipmentWaybillService : IServiceDependencyMarker
    {
        Task<List<InternationalShipmentWaybillDTO>> GetInternationalWaybills(DateFilterCriteria dateFilterCriteria);
        Task<List<InternationalShipmentWaybillDTO>> GetInternationalWaybill(string waybill);
        Task<List<InternationalShipmentWaybillDTO>> GetInternationalShipmentOnwardDeliveryWaybills();
        Task<List<InternationalShipmentWaybillDTO>> GetInternationalShipmentArrivedWaybills();
        Task<bool> UpdateToEnrouteDelivery(List<string> waybills);
        Task<bool> UpdateToDelivered(List<string> waybills);
        Task<InternationalShipmentTracking> TrackInternationalShipment(string internationalWaybill);
    }
}
