using System;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.IServices.Shipments;
using GIGLS.Core;
using GIGLS.Core.Enums;
using GIGLS.Infrastructure;
using System.Collections.Generic;
using GIGLS.CORE.Domain;
using GIGLS.Core.IServices.User;
using System.Linq;
using GIGL.GIGLS.Core.Domain;
using GIGLS.Core.IMessageService;
using GIGLS.Core.DTO.User;
using AutoMapper;
using GIGLS.Core.Domain;

namespace GIGLS.Services.Implementation.Shipments
{
    public class ShipmentTrackingService : IShipmentTrackingService
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserService _userService;
        private readonly IMessageSenderService _messageSenderService;

        public ShipmentTrackingService(IUnitOfWork uow, IUserService userService, IMessageSenderService messageSenderService)
        {
            _uow = uow;
            _userService = userService;
            _messageSenderService = messageSenderService;
        }

        public async Task<object> AddShipmentTracking(ShipmentTrackingDTO tracking, ShipmentScanStatus scanStatus)
        {
            try
            {
                object Id = null;

                if (tracking.User == null)
                {
                    tracking.User = await _userService.GetCurrentUserId();
                }

                if (tracking.Location == null)
                {
                    var UserServiceCenters = await _userService.GetPriviledgeServiceCenters();

                    //default sc
                    if (UserServiceCenters.Length <= 0)
                    {
                        UserServiceCenters = new int[] { 0 };
                        var defaultServiceCenter = await _userService.GetDefaultServiceCenter();
                        UserServiceCenters[0] = defaultServiceCenter.ServiceCentreId;
                    }

                    var serviceCenter = await _uow.ServiceCentre.GetAsync(UserServiceCenters[0]);
                    tracking.Location = serviceCenter.Name;
                    tracking.ServiceCentreId = serviceCenter.ServiceCentreId;
                }

                if (scanStatus.Equals(ShipmentScanStatus.ARF))
                {
                    //Get shipment Details
                    var shipment = await _uow.Shipment.GetAsync(x => x.Waybill.Equals(tracking.Waybill));
                    shipment.ShipmentScanStatus = ShipmentScanStatus.ARF;

                    //add service centre
                    var newShipmentCollection = new ShipmentCollection
                    {
                        Waybill = tracking.Waybill,
                        ShipmentScanStatus = scanStatus,
                        DepartureServiceCentreId = shipment.DepartureServiceCentreId,
                        DestinationServiceCentreId = shipment.DestinationServiceCentreId,
                        IsCashOnDelivery = shipment.IsCashOnDelivery
                    };

                    _uow.ShipmentCollection.Add(newShipmentCollection);
                }

                //check if the waybill has not been scan for the status before
                bool shipmentTracking = await _uow.ShipmentTracking.ExistAsync(x => x.Waybill.Equals(tracking.Waybill) && x.Status.Equals(tracking.Status));

                if (!shipmentTracking || scanStatus.Equals(ShipmentScanStatus.AD) || scanStatus.Equals(ShipmentScanStatus.AST)
                    || scanStatus.Equals(ShipmentScanStatus.DST) || scanStatus.Equals(ShipmentScanStatus.ARP) || scanStatus.Equals(ShipmentScanStatus.APT))
                {
                    var newShipmentTracking = new ShipmentTracking
                    {
                        Waybill = tracking.Waybill,
                        //TrackingType = (TrackingType)Enum.Parse(typeof(TrackingType), tracking.TrackingType),
                        Location = tracking.Location,
                        Status = tracking.Status,
                        DateTime = DateTime.Now,
                        UserId = tracking.User,
                        ServiceCentreId = tracking.ServiceCentreId
                    };
                    _uow.ShipmentTracking.Add(newShipmentTracking);

                    Id = newShipmentTracking.ShipmentTrackingId;
                    
                    //send sms and email
                    if (!scanStatus.Equals(ShipmentScanStatus.CRT))
                    {
                        await sendSMSEmail(tracking, scanStatus);
                    }
                }

                //use to optimise shipment progress for shipment that has depart service centre
                //update shipment table if the scan status contain any of the following : TRO, DSC, DTR
                if (scanStatus.Equals(ShipmentScanStatus.DSC) || scanStatus.Equals(ShipmentScanStatus.TRO) || scanStatus.Equals(ShipmentScanStatus.DTR))
                {
                    //Get shipment Details
                    var shipment = await _uow.Shipment.GetAsync(x => x.Waybill.Equals(tracking.Waybill));

                    //dont allow shipmet scan status to be update once ARF has been done on the shipment
                    if(shipment.ShipmentScanStatus != ShipmentScanStatus.ARF)
                    {
                        //update shipment if the user belong to original departure service centre
                        if (shipment.DepartureServiceCentreId == tracking.ServiceCentreId && shipment.ShipmentScanStatus != scanStatus)
                        {
                            shipment.ShipmentScanStatus = scanStatus;
                        }
                    }                    
                }
                
                await _uow.CompleteAsync();
                return new { Id };
                //return new { Id = newShipmentTracking.ShipmentTrackingId };
            }
            catch (Exception)
            {
                throw;
            }
        }


        private async Task<bool> sendSMSEmail(ShipmentTrackingDTO tracking, ShipmentScanStatus scanStatus)
        {
            var messageType = MessageType.ShipmentCreation;
            foreach (var item in Enum.GetValues(typeof(MessageType)).Cast<MessageType>())
            {
                if (item.ToString() == scanStatus.ToString())
                {
                    messageType = (MessageType)Enum.Parse(typeof(MessageType), scanStatus.ToString());
                    break;
                }
            }

            //send message
            await _messageSenderService.SendMessage(messageType, EmailSmsType.All, tracking);

            return true;
        }

        public Task DeleteShipmentTracking(int trackingId)
        {
            throw new NotImplementedException();
        }

        public async Task<ShipmentTrackingDTO> GetShipmentTrackingById(int trackingId)
        {
            try
            {
                var shipmentTracking = await _uow.ShipmentTracking.GetAsync(trackingId);
                if (shipmentTracking == null)
                {
                    throw new GenericException($"ShipmentTrackingId: {trackingId} does Not Exist");
                }
                return new ShipmentTrackingDTO
                {
                    Waybill = shipmentTracking.Waybill,
                    DateTime = shipmentTracking.DateTime,
                    Location = shipmentTracking.Location,
                    ShipmentTrackingId = shipmentTracking.ShipmentTrackingId,
                    TrackingType = shipmentTracking.TrackingType,
                    Status = shipmentTracking.Status,
                    User = shipmentTracking.User.FirstName + " " + shipmentTracking.User.LastName
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ShipmentTrackingDTO>> GetShipmentTrackings()
        {
            try
            {
                return await _uow.ShipmentTracking.GetShipmentTrackingsAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        public async Task<IEnumerable<ShipmentTrackingDTO>> GetShipmentTrackings(string waybill)
        {
            try
            {
                var shipmentTracking = await _uow.ShipmentTracking.GetShipmentTrackingsAsync(waybill);

                if (shipmentTracking.Any())
                {
                    //1. check if waybill is a returned waybill
                    {
                        var shipmentReturn = await _uow.ShipmentReturn.GetAsync(s => s.WaybillNew == waybill || s.WaybillOld == waybill);
                        if (shipmentReturn != null)
                        {
                            if (shipmentReturn.WaybillNew == waybill)
                            {
                                //get shipmentTracking for old waybill
                                var shipmentTrackingOldWaybill = await _uow.ShipmentTracking.GetShipmentTrackingsAsync(shipmentReturn.WaybillOld);

                                //add to original list
                                shipmentTracking.AddRange(shipmentTrackingOldWaybill);
                            }

                            if (shipmentReturn.WaybillOld == waybill)
                            {
                                //get shipmentTracking for new waybill
                                var shipmentTrackingNewWaybill = await _uow.ShipmentTracking.GetShipmentTrackingsAsync(shipmentReturn.WaybillNew);

                                //add to original list
                                shipmentTracking.AddRange(shipmentTrackingNewWaybill);
                            }
                        }
                    }

                    //2. check if waybill is a rerouted waybill
                    {
                        var shipmentReroute = await _uow.ShipmentReroute.GetAsync(s => s.WaybillNew == waybill || s.WaybillOld == waybill);
                        if (shipmentReroute != null)
                        {
                            if (shipmentReroute.WaybillNew == waybill)
                            {
                                //get shipmentTracking for old waybill
                                var shipmentTrackingOldWaybill = await _uow.ShipmentTracking.GetShipmentTrackingsAsync(shipmentReroute.WaybillOld);

                                //add to original list
                                shipmentTracking.AddRange(shipmentTrackingOldWaybill);
                            }

                            if (shipmentReroute.WaybillOld == waybill)
                            {
                                //get shipmentTracking for new waybill
                                var shipmentTrackingNewWaybill = await _uow.ShipmentTracking.GetShipmentTrackingsAsync(shipmentReroute.WaybillNew);

                                //add to original list
                                shipmentTracking.AddRange(shipmentTrackingNewWaybill);
                            }
                        }
                    }
                }

                return shipmentTracking.ToList().OrderByDescending(x => x.DateTime).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<ShipmentTrackingDTO>> GetShipmentTrackingsForMobile(string waybill)
        {
            try
            {
                var shipmentTracking = await _uow.ShipmentTracking.GetShipmentTrackingsForMobileAsync(waybill);

                if (shipmentTracking.Any())
                {
                    //1. check if waybill is a returned waybill
                    {
                        var shipmentReturn = await _uow.ShipmentReturn.GetAsync(s => s.WaybillNew == waybill || s.WaybillOld == waybill);
                        if (shipmentReturn != null)
                        {
                            if (shipmentReturn.WaybillNew == waybill)
                            {
                                //get shipmentTracking for old waybill
                                var shipmentTrackingOldWaybill = await _uow.ShipmentTracking.GetShipmentTrackingsForMobileAsync(shipmentReturn.WaybillOld);

                                //add to original list
                                shipmentTracking.AddRange(shipmentTrackingOldWaybill);
                            }

                            if (shipmentReturn.WaybillOld == waybill)
                            {
                                //get shipmentTracking for new waybill
                                var shipmentTrackingNewWaybill = await _uow.ShipmentTracking.GetShipmentTrackingsForMobileAsync(shipmentReturn.WaybillNew);

                                //add to original list
                                shipmentTracking.AddRange(shipmentTrackingNewWaybill);
                            }
                        }
                    }

                    //2. check if waybill is a rerouted waybill
                    {
                        var shipmentReroute = await _uow.ShipmentReroute.GetAsync(s => s.WaybillNew == waybill || s.WaybillOld == waybill);
                        if (shipmentReroute != null)
                        {
                            if (shipmentReroute.WaybillNew == waybill)
                            {
                                //get shipmentTracking for old waybill
                                var shipmentTrackingOldWaybill = await _uow.ShipmentTracking.GetShipmentTrackingsForMobileAsync(shipmentReroute.WaybillOld);

                                //add to original list
                                shipmentTracking.AddRange(shipmentTrackingOldWaybill);
                            }

                            if (shipmentReroute.WaybillOld == waybill)
                            {
                                //get shipmentTracking for new waybill
                                var shipmentTrackingNewWaybill = await _uow.ShipmentTracking.GetShipmentTrackingsForMobileAsync(shipmentReroute.WaybillNew);

                                //add to original list
                                shipmentTracking.AddRange(shipmentTrackingNewWaybill);
                            }
                        }
                    }
                }

                return shipmentTracking;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateShipmentTracking(int trackingId, ShipmentTrackingDTO trackingDto)
        {
            try
            {
                var shipmentTracking = await _uow.ShipmentTracking.GetAsync(trackingId);
                if (shipmentTracking == null || trackingDto.ShipmentTrackingId != trackingId)
                {
                    throw new GenericException("ShipmentTracking Not Exist");
                }

                shipmentTracking.Location = shipmentTracking.Location;
                shipmentTracking.TrackingType = shipmentTracking.TrackingType;
                shipmentTracking.User = shipmentTracking.User;
                shipmentTracking.Status = trackingDto.Status;
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> CheckShipmentTracking(string waybill, string status)
        {
            bool shipmentTracking = await _uow.ShipmentTracking.ExistAsync(x => x.Waybill.Equals(waybill) && x.Status.Equals(status));
            return shipmentTracking;
        }

        public async Task<bool> SendEmailForAttemptedScanOfCancelledShipments(ScanDTO scan)
        {
            //1a. Get the ServiceCenter Agent
            var currentUserId = await _userService.GetCurrentUserId();
            var serviceCenterIds = await _userService.GetPriviledgeServiceCenters();
            var currentServiceCenterId = serviceCenterIds[0];

            var userDTO = await _userService.GetUserById(currentUserId);
            var serviceCentreDTO = _uow.ServiceCentre.Get(currentServiceCenterId);

            //get all scan status
            var allScanStatus = _uow.ScanStatus.GetAll().ToList();

            //1b. Get all the Regional Managers assigned to the ServiceCentre where Scan took place
            List<UserDTO> allRegionalManagers = await GetAllRegionalManagersForServiceCentre(currentServiceCenterId);

            //2. Use a loop to send to all Regional Managers
            foreach (var regionalManager in allRegionalManagers)
            {
                var scanStatusReason = allScanStatus.Where(s => s.Code == scan.ShipmentScanStatus.ToString()).
                    Select(s => s.Reason).FirstOrDefault();

                //2a. Create MessageExtensionDTO to hold custom message info
                var messageExtensionDTO = new MessageExtensionDTO()
                {
                    ShipmentScanStatus = scan.ShipmentScanStatus,
                    RegionalManagerName = regionalManager.FirstName + " " + regionalManager.LastName,
                    RegionalManagerEmail = regionalManager.Email,
                    ServiceCenterAgentName = userDTO.Email,
                    ServiceCenterName = serviceCentreDTO.Name,
                    ScanStatus = scanStatusReason,
                    WaybillNumber = scan.WaybillNumber,
                    CancelledOrCollected = scan.CancelledOrCollected
                };

                //2b. send message
                await _messageSenderService.SendGenericEmailMessage(MessageType.SSC_Email, messageExtensionDTO);
            }

            return true;
        }
                
        public async Task<bool> AddTrackingAndSendEmailForRemovingMissingShipmentsInManifest(ShipmentTrackingDTO tracking, ShipmentScanStatus scanStatus, MessageType messageType)
        {
            try
            {
                if (tracking.User == null)
                {
                    tracking.User = await _userService.GetCurrentUserId();
                }

                var scanMessage = await _uow.ScanStatus.GetAsync(x => x.Code == scanStatus.ToString());

                //if missing, create incident report 
                if (scanStatus.Equals(ShipmentScanStatus.SMIM))
                {
                    var incidentReport = new MissingShipment
                    {
                        Waybill = tracking.Waybill,
                        Reason = "Missing",
                        Comment = scanMessage.Incident,
                        Status = "Pending",
                        CreatedBy = tracking.User,
                        ServiceCentreId = tracking.ServiceCentreId
                    };
                    _uow.MissingShipment.Add(incidentReport);
                }

                //if found, update incident report
                if (scanStatus.Equals(ShipmentScanStatus.FMS))
                {
                    var incidentReport = await _uow.MissingShipment.GetAsync(x => x.Waybill == tracking.Waybill);
                    if (incidentReport != null)
                    {
                        incidentReport.Status = "Resolved";
                        incidentReport.Feedback = scanMessage.Incident;
                        incidentReport.ResolvedBy = tracking.User;
                    }
                }

                var newTracking = new ShipmentTracking
                {
                    Waybill = tracking.Waybill,
                    Location = tracking.Location,
                    UserId = tracking.User,
                    DateTime = DateTime.Now,
                    Status = tracking.Status,
                    ServiceCentreId = tracking.ServiceCentreId
                };
                
                _uow.ShipmentTracking.Add(newTracking);
                await _uow.CompleteAsync();

                //send sms and email Departure Regional Manager, Destination Regional Manager and Current Service Centre Regional Manager
                List<UserDTO> allRegionalManagers = new List<UserDTO>();
                
                //1a. Get all the Regional Managers assigned to the ServiceCentre where Scan took place including the departure and destination
                var departureRegionalManagers = await GetAllRegionalManagersForServiceCentre(tracking.DepartureServiceCentreId);
                //var destinationRegionalManagers = await GetAllRegionalManagersForServiceCentre(tracking.DestinationServiceCentreId);
                var currentRegionalManagers = await GetAllRegionalManagersForServiceCentre(tracking.ServiceCentreId);

                allRegionalManagers.AddRange(departureRegionalManagers);
               // allRegionalManagers.AddRange(destinationRegionalManagers);
                allRegionalManagers.AddRange(currentRegionalManagers);
                
                var userDTO = await _userService.GetUserById(tracking.User);

                //2. Use a loop to send to all Regional Managers
                foreach (var regionalManager in allRegionalManagers)
                {
                    //2a. Create MessageExtensionDTO to hold custom message info
                    var messageExtensionDTO = new MessageExtensionDTO()
                    {
                        ShipmentScanStatus = scanStatus,
                        RegionalManagerName = regionalManager.FirstName + " " + regionalManager.LastName,
                        RegionalManagerEmail = regionalManager.Email,
                        ServiceCenterAgentName = userDTO.FirstName + " "+ userDTO.LastName,
                        ServiceCenterName = tracking.Location,
                        ScanStatus = scanMessage.Incident,
                        WaybillNumber = tracking.Waybill,
                        CancelledOrCollected = scanMessage.Reason,
                        GroupWaybill = tracking.GroupWaybill,
                        Manifest = tracking.Manifest
                    };

                    //2b. send message
                    await _messageSenderService.SendGenericEmailMessage(messageType, messageExtensionDTO);
                }
            }
            catch (Exception)
            {
                throw;
            }         
            return true;
        }

        private async Task<List<UserDTO>> GetAllRegionalManagersForServiceCentre(int currentServiceCenterId)
        {
            List<UserDTO> filteredRegionalManagers = new List<UserDTO>();

            //1. get the unique regions for that service centre
            var regionSCMappingIds = _uow.RegionServiceCentreMapping.GetAllAsQueryable().Where(s =>
                        s.ServiceCentreId == currentServiceCenterId).Select(s => s.RegionId).Distinct().ToList();

            if (regionSCMappingIds.Count <= 0)
            {
                return filteredRegionalManagers;
            }

            //2. get all users that are regional managers
            var allEmployees = await _uow.User.GetUsers();
            var regionalManagers = allEmployees.Where(s => s.SystemUserRole == "Regional Manager");

            //3. Loop thru each Regional Manager
            foreach (var regionalManager in regionalManagers)
            {
                //3a. Get user claims
                var userClaims = await _uow.User.GetClaimsAsync(regionalManager.Id);
                string[] claimValue = null;
                foreach (var claim in userClaims)
                {
                    if (claim.Type == "Privilege")
                    {
                        claimValue = claim.Value.Split(':');   // format stringName:stringValue
                    }
                }

                //
                if (claimValue != null && claimValue[0] == "Region")
                {
                    var regionId = int.Parse(claimValue[1]);

                    if (regionSCMappingIds.Contains(regionId))
                    {
                        //Add to filteredRegionalManagers
                        var regionalManagerDTO = Mapper.Map<UserDTO>(regionalManager);
                        filteredRegionalManagers.Add(regionalManagerDTO);
                    }
                }
            }

            return filteredRegionalManagers;
        }
    }
}
