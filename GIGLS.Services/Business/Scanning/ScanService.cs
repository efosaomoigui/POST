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
using System.Collections.Generic;
using GIGL.GIGLS.Core.Domain;
using GIGLS.Core;
using GIGLS.Core.Domain;

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
        private readonly ITransitWaybillNumberService _transitWaybillNumberService;
        private readonly IManifestWaybillMappingService _manifestWaybillService;
        private readonly IUnitOfWork _uow;


        public ScanService(IShipmentService shipmentService, IShipmentTrackingService shipmentTrackingService,
            IGroupWaybillNumberMappingService groupService, IGroupWaybillNumberService groupWaybill,
            IManifestService manifestService, IManifestGroupWaybillNumberMappingService groupManifest,
            IUserService userService, IScanStatusService scanService, IDispatchService dispatchService,
            ITransitWaybillNumberService transitWaybillNumberService, IManifestWaybillMappingService manifestWaybillService, IUnitOfWork uow)
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
            _transitWaybillNumberService = transitWaybillNumberService;
            _manifestWaybillService = manifestWaybillService;
            _uow = uow;
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

        public async Task<bool> ScanMultipleShipment(List<ScanDTO> scanList)
        {
            bool result = false;

            //loop through and call scan for each item
            foreach (var item in scanList)
            {
                result = await ScanShipment(item);
            }

            return result;
        }

        public async Task<bool> ScanShipment(ScanDTO scan)
        {
            // check if the waybill number exists in the system
            if(scan.WaybillNumber != null)
            {
                scan.WaybillNumber = scan.WaybillNumber.Trim();
            }
            var shipment = await _shipmentService.GetShipmentForScan(scan.WaybillNumber);

            string scanStatus = scan.ShipmentScanStatus.ToString();

            /////////////////////////1. Shipment
            if (shipment != null)
            {
                ////// ShipmentCheck  - CheckIfUserIsAtShipmentFinalDestination
                await CheckIfUserIsAtShipmentFinalDestination(scan, shipment.DestinationServiceCentreId);

                //If the scan status is SRC - Shipment received from Dispatch
                if (scan.ShipmentScanStatus == ShipmentScanStatus.SRC)
                {
                    //Process Shipment Return to Service centre for repackaging
                    await ProcessReturnWaybillFromDispatch(shipment.Waybill);
                    return true;
                }
                else
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
            }
            
            /////////////////////////2. GroupShipment
            // check if the group waybill number exists in the system
            var groupWaybill = await _groupWaybill.GetGroupWayBillNumberForScan(scan.WaybillNumber);

            if (groupWaybill != null)
            {
                var groupMappingShipmentList = await _groupService.GetWaybillNumbersInGroup(scan.WaybillNumber);

                var groupShipmentList = groupMappingShipmentList.Shipments;

                //In case no shipment attached to the group waybill  
                if (groupShipmentList.Count > 0)
                {
                    ////// GroupShipmentCheck  - CheckIfUserIsAtShipmentFinalDestination
                    foreach (var item in groupShipmentList)
                    {
                        await CheckIfUserIsAtShipmentFinalDestination(scan, item.DestinationServiceCentreId);
                    }

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
            var waybillsInManifest = new HashSet<string>();

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
                                //check already scanned manifest
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

                                //add to waybillsInManifest
                                waybillsInManifest.Add(waybill);
                            }
                        }
                    }
                }


                //Delivery Manifest
                if(manifest.ManifestType == ManifestType.Delivery)
                {
                    var waybillInManifestList = await _manifestWaybillService.GetWaybillsInManifest(manifest.ManifestCode);

                    if (waybillInManifestList.Count > 0)
                    {

                        //update dispatch to scan for Shipment recieved by Courier for delivery manifest
                        if (scan.ShipmentScanStatus == ShipmentScanStatus.SRC ) // ||scan.ShipmentScanStatus == ShipmentScanStatus.WC )
                        {
                            var dispatch = await _dispatchService.GetDispatchManifestCode(manifest.ManifestCode);
                            if (dispatch != null && dispatch.ReceivedBy == null)
                            {
                                //get the user that login
                                var userId = await _userService.GetCurrentUserId();
                                var user = await _userService.GetUserById(userId);

                                string reciever = user.FirstName + " " + user.LastName;
                                dispatch.ReceivedBy = reciever;

                                //update manifest also
                                var manifestObj = await _manifestService.GetManifestByCode(manifest.ManifestCode);
                                if (manifestObj != null && manifestObj.ReceiverBy == null)
                                {
                                    manifestObj.IsReceived = true;
                                    manifestObj.ReceiverBy = userId;
                                    await _manifestService.UpdateManifest(manifestObj.ManifestId, manifestObj);
                                }
                                await _dispatchService.UpdateDispatch(dispatch.DispatchId, dispatch);
                            }

                            //If the scan status is SRC - Shipment received from Dispatch
                            foreach (var itemWaybillDTO in waybillInManifestList)
                            {
                                if (scan.ShipmentScanStatus == ShipmentScanStatus.SRC)
                                {
                                    //Process Shipment Return to Service centre for repackaging
                                    await ProcessReturnWaybillFromDispatch(itemWaybillDTO.Waybill);
                                }
                            }

                        }
                        else
                        {
                            throw new GenericException($"Wrong Waybill Number {scan.WaybillNumber} ");
                        }
                    }
                    else
                    {
                        throw new GenericException($"No Shipment attached to this Manifest: {scan.WaybillNumber} ");
                    }
                    return true;
                }
               
            }

            if (shipment == null && groupWaybill == null && manifest == null)
            {
                throw new GenericException($"Shipment with waybill: {scan.WaybillNumber} does not exist");
            }
            
            //////////////////////4. Check and Create Entries for Transit Manifest
            await CheckAndCreateEntriesForTransitManifest(scan, manifest, waybillsInManifest);
            
            return true;
        }


        private async Task ProcessReturnWaybillFromDispatch(string waybill)
        {
            var getManifest = await _manifestWaybillService.GetActiveManifestForWaybill(waybill);

            List<string> waybills = new List<string>();
            waybills.Add(waybill);

            //call ReturnWaybillsInManifest in ManifestWaybillMappingService
            await _manifestWaybillService.ReturnWaybillsInManifest(getManifest.ManifestCode, waybills);
        }

        private async Task<bool> CheckIfUserIsAtShipmentFinalDestination(ScanDTO scan, int destinationServiceCentreId)
        {
            //1. For Shipment Check if user has rights to this action
            {
                if (scan.ShipmentScanStatus == ShipmentScanStatus.ARF  || scan.ShipmentScanStatus == ShipmentScanStatus.SRC)
                {
                    //Check if the user is a staff at final destination
                    var serviceCenters = await _userService.GetPriviledgeServiceCenters();
                    if (serviceCenters.Length == 1 && serviceCenters[0] == destinationServiceCentreId)
                    {
                        //do nothing
                    }
                    else
                    {
                        //added for GWA and GWARIMPA service centres
                        {
                            if (serviceCenters.Length == 1)
                            {
                                if (serviceCenters[0] == 4 || serviceCenters[0] == 294)
                                {
                                    if(serviceCenters[0] == destinationServiceCentreId)
                                    {
                                        serviceCenters = new int[] { 4, 294 };
                                        return true;
                                    }
                                }
                            }
                        }

                        throw new GenericException("Error processing request. The login user is not at the final Destination nor has the right privilege");
                    }
                }
            }
            return true;
        }

        private async Task<bool> CheckAndCreateEntriesForTransitManifest(ScanDTO scan, Manifest manifest, HashSet<string> waybillsInManifest)
        {
            var serviceCenters = await _userService.GetPriviledgeServiceCenters();
            var currentUserSercentreId = serviceCenters.Length > 0 ? serviceCenters[0] : 0;
            var currentUserId = await _userService.GetCurrentUserId();
            var groupWaybillsInManifest = new HashSet<string>();

            //1. Only scan for manifest
            if (manifest != null)
            {
                //2. Check for Scan related to Transit Manifest
                if (scan.ShipmentScanStatus == ShipmentScanStatus.AST || scan.ShipmentScanStatus == ShipmentScanStatus.APT)
                {
                    //3. Create new entries in TransitWaybills or update existing entries
                    foreach (var waybill in waybillsInManifest)
                    {
                        //3a. check if entry exist
                        var transitWaybillNumber = await _uow.TransitWaybillNumber.GetAsync(s => s.WaybillNumber == waybill);

                        if (transitWaybillNumber == null)
                        {
                            //3b. create new entry
                            await _transitWaybillNumberService.AddTransitWaybillNumber(
                                new TransitWaybillNumberDTO
                                {
                                    WaybillNumber = waybill,
                                    IsGrouped = true,
                                    ServiceCentreId = currentUserSercentreId,
                                    UserId = currentUserId
                                }
                            );
                        }
                        else
                        {
                            //3c. update existing entry
                            transitWaybillNumber.ServiceCentreId = currentUserSercentreId;
                            transitWaybillNumber.UserId = currentUserId;
                            _uow.Complete();
                        }

                        //4. Update entry in GroupWaybillMapping
                        var groupWaybillNumberMapping = await _uow.GroupWaybillNumberMapping.GetAsync(s => s.WaybillNumber == waybill && s.IsDeleted == false);
                        groupWaybillNumberMapping.DepartureServiceCentreId = currentUserSercentreId;
                        _uow.Complete();

                        //5. Get the GroupWaybill numbers in the manifest
                        groupWaybillsInManifest.Add(groupWaybillNumberMapping.GroupWaybillNumber);
                    }

                    //6. Remove entry from ManifestGroupWaybillNumberMappingService
                    //6.1 Find the groupWaybill attached to the Manifest
                    foreach(var groupWaybill in groupWaybillsInManifest)
                    {
                        await _groupManifest.RemoveGroupWaybillNumberFromManifest(manifest.ManifestCode, groupWaybill);
                    }

                }
            }

            return true;
        }

        /// <summary>
        /// ///////////SignOffDeliveryManifest
        /// </summary>
        /// <param name="manifest"></param>
        /// <returns></returns>
        public async Task<bool> ScanSignOffDeliveryManifest(string manifest)
        {
            // get waybills in manifest that have not been collected by customer
            var waybills = await _manifestWaybillService.GetWaybillsInManifest(manifest);

            // scan the Delivery Manifest with the scan code of 'ShipmentScanStatus.SRC'
            // (Mark the manifest as Received at the final service centre)  - scanning does this
            var result = await ScanShipment(new ScanDTO()
            {
                WaybillNumber = manifest,
                ShipmentScanStatus = ShipmentScanStatus.SRC
            });

            return true;
        }


    }
}
