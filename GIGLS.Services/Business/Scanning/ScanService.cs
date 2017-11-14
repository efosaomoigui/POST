using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.IServices.Shipments;
using GIGLS.Core.Enums;
using GIGLS.Core.IServices.Business;
using System;
using System.Threading.Tasks;

namespace GIGLS.Services.Business.Scanning
{
    public class ScanService : IScanService
    {
        private readonly IShipmentTrackingService _shipmentTrackingService;
        private readonly IShipmentService _shipmentService;

        public ScanService(IShipmentService shipmentService, IShipmentTrackingService shipmentTrackingService)
        {
            _shipmentService = shipmentService;
            _shipmentTrackingService = shipmentTrackingService;
        }

        public async Task<bool> ScanShipment(string waybillNumber, ShipmentScanStatus scanStatus)
        {
            // verify the waybill number exists in the system
            var shipment = await _shipmentService.GetShipment(waybillNumber);
            if (shipment == null)
            {
                throw new Exception($"Shipment with waybill: {waybillNumber} does Not Exist");
            }

            var newShipmentTracking = await _shipmentTrackingService.AddShipmentTracking(new ShipmentTrackingDTO
            {
                DateTime = DateTime.Now,
                Status = scanStatus.ToString(),
                Waybill = waybillNumber
            }, scanStatus);

            return true;
        }
    }
}
