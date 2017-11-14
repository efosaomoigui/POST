using GIGLS.Core.DTO.Shipments;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Shipments
{
    public interface IShipmentTrackingService : IServiceDependencyMarker
    {
        Task<IEnumerable<ShipmentTrackingDTO>> GetShipmentTrackings();
        Task<IEnumerable<ShipmentTrackingDTO>> GetShipmentWaitingForCollection();
        Task<IEnumerable<ShipmentTrackingDTO>> GetShipmentTrackings(string waybill);
        Task<ShipmentTrackingDTO> GetShipmentTrackingById(int trackingId);
        Task<object> AddShipmentTracking(ShipmentTrackingDTO tracking);
        Task UpdateShipmentTracking(int trackingId, ShipmentTrackingDTO tracking);
        Task DeleteShipmentTracking(int trackingId);
    }
}
