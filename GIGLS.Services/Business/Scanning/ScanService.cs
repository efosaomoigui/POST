using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.IServices.Shipments;
using GIGLS.Core.Enums;
using GIGLS.Core.IServices.Business;
using System;
using System.Threading.Tasks;
using GIGLS.Infrastructure;

namespace GIGLS.Services.Business.Scanning
{
    public class ScanService : IScanService
    {
        private readonly IShipmentTrackingService _shipmentTrackingService;
        private readonly IShipmentService _shipmentService;
        private readonly IGroupWaybillNumberMappingService _groupService;
        private readonly IGroupWaybillNumberService _groupWaybill;

        public ScanService(IShipmentService shipmentService, IShipmentTrackingService shipmentTrackingService, 
            IGroupWaybillNumberMappingService groupService, IGroupWaybillNumberService groupWaybill)
        {
            _shipmentService = shipmentService;
            _shipmentTrackingService = shipmentTrackingService;
            _groupService = groupService;
            _groupWaybill = groupWaybill;
        }

        //public async Task<bool> ScanShipment(string waybillNumber, ShipmentScanStatus scanStatus)
        //{
        //    // verify the waybill number exists in the system
        //    var shipment = await _shipmentService.GetShipment(waybillNumber);
        //    if (shipment == null)
        //    {
        //        throw new Exception($"Shipment with waybill: {waybillNumber} does Not Exist");
        //    }

        //    var newShipmentTracking = await _shipmentTrackingService.AddShipmentTracking(new ShipmentTrackingDTO
        //    {
        //        DateTime = DateTime.Now,
        //        Status = scanStatus.ToString(),
        //        Waybill = waybillNumber
        //    }, scanStatus);

        //    return true;
        //}

        public async Task<bool> ScanShipment(ScanDTO scan)
        {
            // verify the waybill number exists in the system
            var shipment = await _shipmentService.GetShipmentForScan(scan.WaybillNumber);

            if(shipment != null)
            {
                var newShipmentTracking = await _shipmentTrackingService.AddShipmentTracking(new ShipmentTrackingDTO
                {
                    DateTime = DateTime.Now,
                    Status = scan.ShipmentScanStatus.ToString(),
                    Waybill = scan.WaybillNumber,
                }, scan.ShipmentScanStatus);

                return true;
            }

            // verify the group waybill number exists in the system
            var groupWaybill = await _groupWaybill.GetGroupWayBillNumberForScan(scan.WaybillNumber);

            if (groupWaybill != null)
            {
                var groupShipmentList = await _groupService.GetWaybillNumbersInGroup(scan.WaybillNumber);
                
                //In case no shipment attached to the group waybill  
                if(groupShipmentList.Count > 0 )
                {
                    foreach (var groupShipment in groupShipmentList)
                    {
                        await _shipmentTrackingService.AddShipmentTracking(new ShipmentTrackingDTO
                        {
                            DateTime = DateTime.Now,
                            Status = scan.ShipmentScanStatus.ToString(),
                            Waybill = groupShipment.WaybillCode,
                        }, scan.ShipmentScanStatus);
                    }
                }
                else
                {
                    throw new GenericException($"Shipment with waybill: {scan.WaybillNumber} does not exist");
                }
            }
            
            if (shipment == null && groupWaybill == null)
            {
                throw new GenericException($"Shipment with waybill: {scan.WaybillNumber} does not exist");
            }

            return true;
        }
    }
}
