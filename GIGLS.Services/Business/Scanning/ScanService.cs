using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.IServices.Shipments;
using GIGLS.Core.IServices.Business;
using System;
using System.Threading.Tasks;
using GIGLS.Infrastructure;
using GIGLS.Services.Implementation.Utility;

namespace GIGLS.Services.Business.Scanning
{
    public class ScanService : IScanService
    {
        private readonly IShipmentTrackingService _shipmentTrackingService;
        private readonly IShipmentService _shipmentService;
        private readonly IGroupWaybillNumberMappingService _groupService;
        private readonly IGroupWaybillNumberService _groupWaybill;
        private readonly IManifestService _manifestService;
        private readonly IManifestGroupWaybillNumberMappingService _groupManifest;


        public ScanService(IShipmentService shipmentService, IShipmentTrackingService shipmentTrackingService, 
            IGroupWaybillNumberMappingService groupService, IGroupWaybillNumberService groupWaybill, 
            IManifestService manifestService, IManifestGroupWaybillNumberMappingService groupManifest)
        {
            _shipmentService = shipmentService;
            _shipmentTrackingService = shipmentTrackingService;
            _groupService = groupService;
            _groupWaybill = groupWaybill;
            _manifestService = manifestService;
            _groupManifest = groupManifest;
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
            // check if the waybill number exists in the system
            var shipment = await _shipmentService.GetShipmentForScan(scan.WaybillNumber);

            string scanStatus = scan.ShipmentScanStatus.ToString();

            if (shipment != null)
            {
                //check if the waybill has not been scan for the same status before
                var checkTrack = await _shipmentTrackingService.CheckShipmentTracking(scan.WaybillNumber, scanStatus);

                if (!checkTrack)
                {
                    var newShipmentTracking = await _shipmentTrackingService.AddShipmentTracking(new ShipmentTrackingDTO
                    {
                        DateTime = DateTime.Now,
                        Status = scanStatus, 
                        Waybill = scan.WaybillNumber,
                    }, scan.ShipmentScanStatus);
                    return true;
                }
                else
                {
                    throw new GenericException($"Shipment with waybill: {scan.WaybillNumber} already scan for { scanStatus }");
                }                
            }

            // check if the group waybill number exists in the system
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
                        var checkTrack = await _shipmentTrackingService.CheckShipmentTracking(groupShipment.Waybill, scanStatus);
                        if (!checkTrack)
                        {
                            await _shipmentTrackingService.AddShipmentTracking(new ShipmentTrackingDTO
                            {
                                DateTime = DateTime.Now,
                                Status = scanStatus,
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

            // check if the manifest number exists in the system
            var manifest = await _manifestService.GetManifestCodeForScan(scan.WaybillNumber);

            if (manifest != null)
            {
                var groupWaybillInManifestList = await _groupManifest.GetGroupWaybillNumbersInManifest(manifest.ManifestId);
               
                //In case no shipment attached to the manifest  
                if (groupWaybillInManifestList.Count > 0)
                {
                    foreach (var groupShipment in groupWaybillInManifestList)
                    {
                        if(groupShipment.WaybillNumbers.Count > 0)
                        {
                            foreach (var waybill in groupShipment.WaybillNumbers)
                            {
                                var checkTrack = await _shipmentTrackingService.CheckShipmentTracking(waybill, scanStatus);
                                if (!checkTrack)
                                {
                                    await _shipmentTrackingService.AddShipmentTracking(new ShipmentTrackingDTO
                                    {
                                        DateTime = DateTime.Now,
                                        Status = scanStatus,
                                        Waybill = waybill,
                                    }, scan.ShipmentScanStatus);
                                }
                            }
                        }
                    }
                }
                else
                {
                    throw new GenericException($"No Shipment attached to this Manifest: {scan.WaybillNumber} ");
                }
            }

            if (shipment == null && groupWaybill == null && manifest == null)
            {
                throw new GenericException($"Shipment with waybill: {scan.WaybillNumber} does not exist");
            }

            return true;
        }
    }
}
