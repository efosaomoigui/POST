using GIGLS.Core.DTO;
using GIGLS.Core.DTO.Shipments;
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
       
        Task<MobileShipmentTrackingHistoryDTO> GetMobileShipmentTrackings(string waybill);
        Task<MobileShipmentTrackingDTO> GetMobileShipmentTrackingById(int trackingId);
        Task AddMobileShipmentTracking(MobileShipmentTrackingDTO tracking, ShipmentScanStatus scanStatus);
        Task UpdateShipmentTracking(int trackingId, MobileShipmentTrackingDTO tracking);
        Task<bool> CheckMobileShipmentTracking(string waybill, string status);
    }
}
