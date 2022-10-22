using POST.Core.DTO.DHL;
using POST.Core.DTO.Shipments;
using System.Threading.Tasks;

namespace POST.Core.IServices.DHL
{
    public interface IDHLService : IServiceDependencyMarker
    {
        Task<TotalNetResult> GetInternationalShipmentPrice(InternationalShipmentDTO shipmentDTO);
        Task<InternationalShipmentWaybillDTO> CreateInternationalShipment(InternationalShipmentDTO shipmentDTO);
        Task<InternationalShipmentTracking> TrackInternationalShipment(string internationalWaybill);
    }
}
