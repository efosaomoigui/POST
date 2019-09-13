using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Shipments
{
    public interface IShipmentTrackingService : IServiceDependencyMarker
    {
        Task<List<ShipmentTrackingDTO>> GetShipmentTrackings();
        //Task<IEnumerable<ShipmentTrackingDTO>> GetShipmentWaitingForCollection();
        Task<IEnumerable<ShipmentTrackingDTO>> GetShipmentTrackings(string waybill);
        Task<ShipmentTrackingDTO> GetShipmentTrackingById(int trackingId);
        Task<object> AddShipmentTracking(ShipmentTrackingDTO tracking, ShipmentScanStatus scanStatus);
        Task UpdateShipmentTracking(int trackingId, ShipmentTrackingDTO tracking);
        Task DeleteShipmentTracking(int trackingId);
        Task<bool> CheckShipmentTracking(string waybill, string status);
        Task<bool> SendEmailForAttemptedScanOfCancelledShipments(ScanDTO scan);
        Task<bool> AddTrackingAndSendEmailForRemovingMissingShipmentsInManifest(ShipmentTrackingDTO tracking, ShipmentScanStatus scanStatus, MessageType messageType);
    }
}
