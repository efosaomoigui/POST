using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.IServices.Shipments;
using GIGLS.Core.IServices.Business;
using System;
using System.Threading.Tasks;
using GIGLS.Infrastructure;
using GIGLS.Core.IServices.User;
using GIGLS.Core.Enums;
using GIGLS.Core.IServices.ShipmentScan;
using GIGLS.Core.IServices.Fleets;

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
        private IUserService _userService;
        private readonly IScanStatusService _scanService;
        private readonly IDispatchService _dispatchService;


        public ScanService(IShipmentService shipmentService, IShipmentTrackingService shipmentTrackingService,
            IGroupWaybillNumberMappingService groupService, IGroupWaybillNumberService groupWaybill,
            IManifestService manifestService, IManifestGroupWaybillNumberMappingService groupManifest,
            IUserService userService, IScanStatusService scanService, IDispatchService dispatchService)
        {
            _shipmentService = shipmentService;
            _shipmentTrackingService = shipmentTrackingService;
            _groupService = groupService;
            _groupWaybill = groupWaybill;
            _manifestService = manifestService;
            _groupManifest = groupManifest;
            _userService = userService;
            _scanService = scanService;
            _dispatchService = dispatchService;
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

            ////// ShipmentCheck  - CheckIfUserIsAtShipmentFinalDestination
            await CheckIfUserIsAtShipmentFinalDestination(scan, shipment.DestinationServiceCentreId);

            /////////////////////////1. Shipment
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
                    var scanResult = await _scanService.GetScanStatusByCode(scanStatus);
                    throw new GenericException($"Shipment with waybill: {scan.WaybillNumber} already scan for { scanResult.Incident }");
                }
            }


            /////////////////////////2. GroupShipment
            // check if the group waybill number exists in the system
            var groupWaybill = await _groupWaybill.GetGroupWayBillNumberForScan(scan.WaybillNumber);

            if (groupWaybill != null)
            {
                var groupMappingShipmentList = await _groupService.GetWaybillNumbersInGroup(scan.WaybillNumber);

                var groupShipmentList = groupMappingShipmentList.Shipments;

                ////// GroupShipmentCheck  - CheckIfUserIsAtShipmentFinalDestination
                foreach (var item in groupShipmentList)
                {
                    await CheckIfUserIsAtShipmentFinalDestination(scan, item.DestinationServiceCentreId);
                }


                //In case no shipment attached to the group waybill  
                if (groupShipmentList.Count > 0)
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


            /////////////////////////3. Manifest
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
                        if (groupShipment.WaybillNumbers.Count > 0)
                        {
                            ////// ManifestCheck  - CheckIfUserIsAtShipmentFinalDestination
                            if (scan.ShipmentScanStatus == ShipmentScanStatus.ARF)
                            {
                                foreach (var waybill in groupShipment.WaybillNumbers)
                                {
                                    var shipmentItem = await _shipmentService.GetShipmentForScan(waybill);
                                    // For Shipment Check if user has rights to this action
                                    await CheckIfUserIsAtShipmentFinalDestination(scan, shipmentItem.DestinationServiceCentreId);
                                }
                            }

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

                                //if the shipment scan status is shipment arrive final destination then
                                //update Dispatch Receiver and manifest receiver id
                                if (scan.ShipmentScanStatus == ShipmentScanStatus.ARF)
                                {
                                    var dispatch = await _dispatchService.GetDispatchManifestCode(scan.WaybillNumber);
                                    if (dispatch != null)
                                    {
                                        //get the user that login
                                        var userId = await _userService.GetCurrentUserId();
                                        var user = await _userService.GetUserById(userId);

                                        string reciever = user.FirstName + " " + user.LastName;
                                        dispatch.ReceivedBy = reciever;

                                        //update manifest also
                                        var manifestObj = await _manifestService.GetManifestByCode(scan.WaybillNumber);
                                        if (manifestObj != null)
                                        {
                                            manifestObj.IsReceived = true;
                                            manifestObj.ReceiverBy = userId;
                                            await _manifestService.UpdateManifest(manifestObj.ManifestId, manifestObj);
                                        }

                                        await _dispatchService.UpdateDispatch(dispatch.DispatchId, dispatch);
                                    }
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

        private async Task CheckIfUserIsAtShipmentFinalDestination(ScanDTO scan, int destinationServiceCentreId)
        {
            //1. For Shipment Check if user has rights to this action
            {
                if (scan.ShipmentScanStatus == ShipmentScanStatus.ARF)
                {
                    //Check if the user is a staff at final destination
                    var serviceCenters = await _userService.GetPriviledgeServiceCenters();
                    if (serviceCenters.Length == 1 && serviceCenters[0] == destinationServiceCentreId)
                    {
                        //do nothing
                    }
                    else
                    {
                        throw new GenericException("Error processing request. The login user is not at the final Destination nor has the right privilege");
                    }
                }
            }

        }
    }
}
