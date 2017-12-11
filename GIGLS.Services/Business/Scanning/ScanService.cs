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
                //check if the waybill has not been scan for the same status before
                var checkTrack = await _shipmentTrackingService.CheckShipmentTracking(scan.WaybillNumber, scan.ShipmentScanStatus.ToString());

                if (!checkTrack)
                {
                    var newShipmentTracking = await _shipmentTrackingService.AddShipmentTracking(new ShipmentTrackingDTO
                    {
                        DateTime = DateTime.Now,
                        Status = scan.ShipmentScanStatus.ToString(),
                        Waybill = scan.WaybillNumber,
                    }, scan.ShipmentScanStatus);
                    return true;
                }
                else
                {
                    throw new GenericException($"Shipment with waybill: {scan.WaybillNumber} already scan for { scan.ShipmentScanStatus.ToString() } status");
                }                
            }

            // verify the group waybill number exists in the system
            var groupWaybill = await _groupWaybill.GetGroupWayBillNumberForScan(scan.WaybillNumber);

            if (groupWaybill != null)
            {
                var groupMappingShipmentList = await _groupService.GetWaybillNumbersInGroup(scan.WaybillNumber);

                var groupShipmentList = groupMappingShipmentList.Shipments;
                //In case no shipment attached to the group waybill  
                if (groupShipmentList.Count > 0 )
                {
                    foreach (var groupShipment in groupShipmentList)
                    {
                        var checkTrack = await _shipmentTrackingService.CheckShipmentTracking(groupShipment.Waybill, scan.ShipmentScanStatus.ToString());
                        if (!checkTrack)
                        {
                            await _shipmentTrackingService.AddShipmentTracking(new ShipmentTrackingDTO
                            {
                                DateTime = DateTime.Now,
                                Status = scan.ShipmentScanStatus.ToString(),
                                Waybill = groupShipment.Waybill,
                            }, scan.ShipmentScanStatus);
                        }
                    }
                }
                else
                {
                    throw new GenericException($"No Shipment for Group waybill: {scan.WaybillNumber} ");
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
