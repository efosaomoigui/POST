using GIGLS.Core.DTO;
using GIGLS.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Shipments
{
    public interface IMobileShipmentTrackingService : IServiceDependencyMarker
    {
        Task<List<MobileShipmentTrackingDTO>> GetMobileShipmentTrackings();
        //Task<IEnumerable<ShipmentTrackingDTO>> GetShipmentWaitingForCollection();
        Task<IEnumerable<MobileShipmentTrackingDTO>> GetMobileShipmentTrackings(string waybill);
        Task<MobileShipmentTrackingDTO> GetMobileShipmentTrackingById(int trackingId);
        Task<int> AddMobileShipmentTracking(MobileShipmentTrackingDTO tracking, ShipmentScanStatus scanStatus);
        Task UpdateShipmentTracking(int trackingId, MobileShipmentTrackingDTO tracking);
        Task<bool> CheckMobileShipmentTracking(string waybill, string status);
    }
}
