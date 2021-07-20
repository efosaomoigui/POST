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
using GIGLS.Core.DTO;
using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.Core.DTO.Customers;
using GIGLS.Core.IServices.DHL;
using System.Globalization;
using GIGLS.Core.DTO.ShipmentScan;
using System.Configuration;

namespace GIGLS.Services.Implementation.Shipments
{
    public class ShipmentTrackingService : IShipmentTrackingService
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserService _userService;
        private readonly IMessageSenderService _messageSenderService;
        private readonly IDHLService _DhlService;

        public ShipmentTrackingService(IUnitOfWork uow, IUserService userService, IMessageSenderService messageSenderService,
            IDHLService dHLService)
        {
            _uow = uow;
            _userService = userService;
            _messageSenderService = messageSenderService;
            _DhlService = dHLService;
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

                var UserServiceCenters = await _userService.GetPriviledgeServiceCenters();

                //default sc
                if (UserServiceCenters.Length <= 0)
                {
                    UserServiceCenters = new int[] { 0 };
                    var defaultServiceCenter = await _userService.GetDefaultServiceCenter();
                    UserServiceCenters[0] = defaultServiceCenter.ServiceCentreId;
                }
                var serviceCenter = await _uow.ServiceCentre.GetAsync(UserServiceCenters[0]);
                tracking.ServiceCentreId = serviceCenter.ServiceCentreId;

                if (string.IsNullOrWhiteSpace(tracking.Location))
                {
                    tracking.Location = serviceCenter.Name;
                }

                if (scanStatus.Equals(ShipmentScanStatus.ARF))
                {
                    //Get shipment Details
                    var shipment = await _uow.Shipment.GetAsync(x => x.Waybill == tracking.Waybill);
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

                    //set international shipmet to arrived
                    if (shipment.InternationalShipmentType == InternationalShipmentType.DHL)
                    {
                        var internationalShipment = await _uow.InternationalShipmentWaybill.GetAsync(x => x.Waybill == tracking.Waybill);
                        if (internationalShipment != null)
                        {
                            internationalShipment.InternationalShipmentStatus = InternationalShipmentStatus.Arrived;
                        }
                    }
                }

                //check if the waybill has not been scan for the status before
                bool shipmentTracking = await _uow.ShipmentTracking.ExistAsync(x => x.Waybill.Equals(tracking.Waybill) && x.Status.Equals(tracking.Status));

                if (!shipmentTracking || scanStatus.Equals(ShipmentScanStatus.AD) || scanStatus.Equals(ShipmentScanStatus.AST)
                    || scanStatus.Equals(ShipmentScanStatus.DST) || scanStatus.Equals(ShipmentScanStatus.ARP) || scanStatus.Equals(ShipmentScanStatus.APT))
                {
                    var newShipmentTracking = new ShipmentTracking
                    {
                        Waybill = tracking.Waybill,
                        Location = tracking.Location,
                        Status = tracking.Status,
                        DateTime = DateTime.Now,
                        UserId = tracking.User,
                        ServiceCentreId = tracking.ServiceCentreId
                    };
                    _uow.ShipmentTracking.Add(newShipmentTracking);

                    Id = newShipmentTracking.ShipmentTrackingId;

                    var shipment = await _uow.Shipment.GetAsync(x => x.Waybill == tracking.Waybill);

                    //send sms and email
                    if (!(scanStatus.Equals(ShipmentScanStatus.CRT) || scanStatus.Equals(ShipmentScanStatus.SSC)))
                    {
                        if (tracking.TrackingType == TrackingType.InBound)
                        {
                            if (tracking.isInternalShipment)
                            {
                                await SendEmailToStoreKeeper(tracking, scanStatus);
                            }
                            else if (scanStatus.Equals(ShipmentScanStatus.ARF) && shipment.IsInternational == true)
                            {
                                await SendEmailToCustomerForIntlShipment(shipment);
                            }
                            else
                            {

                                await sendSMSEmail(tracking, scanStatus);

                            }
                        }
                    }
                }

                //use to optimise shipment progress for shipment that has depart service centre
                //update shipment table if the scan status contain any of the following : TRO, DSC, DTR
                if (scanStatus.Equals(ShipmentScanStatus.DSC) || scanStatus.Equals(ShipmentScanStatus.TRO) || scanStatus.Equals(ShipmentScanStatus.DTR))
                {
                    //Get shipment Details
                    var shipment = await _uow.Shipment.GetAsync(x => x.Waybill.Equals(tracking.Waybill));

                    //dont allow shipmet scan status to be update once ARF has been done on the shipment
                    if (shipment.ShipmentScanStatus != ShipmentScanStatus.ARF)
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

        public async Task<bool> AddShipmentTrackingForReceivedItems(ShipmentTrackingDTO tracking, ShipmentScanStatus scanStatus, string reqNo)
        {
            try
            {
                if (tracking.User == null)
                {
                    tracking.User = await _userService.GetCurrentUserId();
                }
                var request = _uow.IntlShipmentRequest.GetAll("ShipmentRequestItems").Where(x => x.RequestNumber == reqNo).FirstOrDefault();
                if (request != null)
                {
                    if (String.IsNullOrEmpty(tracking.Location))
                    {
                        var storeNames = request.ShipmentRequestItems.Select(x => x.storeName).ToList();
                        tracking.Location = string.Join(",", storeNames);
                    }

                    //check if the waybill has not been scan for the status before
                    bool shipmentTracking = await _uow.ShipmentTracking.ExistAsync(x => x.Waybill.Equals(tracking.Waybill) && x.Status.Equals(tracking.Status));
                    if (!shipmentTracking)
                    {
                        var newShipmentTracking = new ShipmentTracking
                        {
                            Waybill = tracking.Waybill,
                            Location = tracking.Location,
                            Status = tracking.Status,
                            DateTime = DateTime.Now,
                            UserId = tracking.User,
                            ServiceCentreId = tracking.ServiceCentreId
                        };
                        _uow.ShipmentTracking.Add(newShipmentTracking);
                    }
                }

                await _uow.CompleteAsync();
                return true;
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
            if (messageType != MessageType.ARF)
            {
                await _messageSenderService.SendMessage(messageType, EmailSmsType.All, tracking);
            }
            else
            {
                await _messageSenderService.SendMessage(messageType, EmailSmsType.SMS, tracking);
                var shipment = await _uow.Shipment.GetAsync(s => s.Waybill.Equals(tracking.Waybill));
                var shipmentDTO = Mapper.Map<ShipmentDTO>(shipment);

                //Send Whatsapp message
                await SendWhatsappMessage(shipmentDTO);

                if (shipment != null && shipment.PickupOptions == PickupOptions.HOMEDELIVERY && scanStatus == ShipmentScanStatus.ARF && shipment.IsInternational == false)
                {
                    await SendEmailShipmentARFHomeDelivery(shipmentDTO);
                }
                //Send Email on Shipment Arrive final Destination for Terminal pickup option
                if (shipment != null && shipment.PickupOptions == PickupOptions.SERVICECENTER && scanStatus == ShipmentScanStatus.ARF && shipment.IsInternational == false)
                {
                    await SendEmailShipmentARFTerminalPickup(shipmentDTO);
                }
            }

            return true;
        }

        //Send email to store keeper when the shipment has arrived final destination
        private async Task<bool> SendEmailToStoreKeeper(ShipmentTrackingDTO tracking, ShipmentScanStatus scanStatus)
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
            await _messageSenderService.SendMessage(messageType, EmailSmsType.Email, tracking);

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

                    //3. Check for international shipments
                    {
                        var shipment = _uow.ShipmentTracking.GetShipmentByWayBill(waybill);
                        if (!string.IsNullOrWhiteSpace(shipment.InternationalWayBill))
                        {
                            var dhlTracking = new List<ShipmentTrackingDTO>();
                            var intlTracking = await _DhlService.TrackInternationalShipment(shipment.InternationalWayBill);
                            if (intlTracking.Shipments.Count > 0 && intlTracking.Shipments != null)
                            {
                                if (intlTracking.Shipments[0].Events.Count > 0)
                                {
                                    foreach (var item in intlTracking.Shipments[0].Events)
                                    {
                                        var data = new ShipmentTrackingDTO
                                        {
                                            Waybill = waybill,
                                            DateTime = Convert.ToDateTime(item.Date + " " + item.Time),
                                            Location = item.ServiceArea[0].Description,
                                            TrackingType = TrackingType.OutBound,
                                            User = "International Shipping",
                                            Status = item.Description,
                                            ScanStatus = new ScanStatusDTO
                                            {
                                                Code = item.ServiceArea[0].Code,
                                                Incident = item.Description,
                                                Reason = item.ServiceArea[0].Description,
                                                Comment = item.Description,
                                                DateCreated = Convert.ToDateTime(item.Date + " " + item.Time),
                                                DateModified = Convert.ToDateTime(item.Date + " " + item.Time)
                                            }
                                        };
                                        dhlTracking.Add(data);
                                    }
                                    shipmentTracking.AddRange(dhlTracking);
                                }
                            }
                        }
                    }
                }

                return shipmentTracking.ToList().OrderByDescending(x => x.DateTime).ToList();
            }
            catch (Exception ex)
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

                    //3. Check for international shipments
                    {
                        var shipment = _uow.ShipmentTracking.GetShipmentByWayBill(waybill);
                        if (!string.IsNullOrWhiteSpace(shipment.InternationalWayBill))
                        {
                            var dhlTracking = new List<ShipmentTrackingDTO>();
                            var intlTracking = await _DhlService.TrackInternationalShipment(shipment.InternationalWayBill);
                            if (intlTracking.Shipments.Count > 0 && intlTracking.Shipments != null)
                            {
                                if (intlTracking.Shipments[0].Events.Count > 0)
                                {
                                    foreach (var item in intlTracking.Shipments[0].Events)
                                    {
                                        var data = new ShipmentTrackingDTO
                                        {
                                            Waybill = waybill,
                                            DateTime = Convert.ToDateTime(item.Date + " " + item.Time),
                                            Location = item.ServiceArea[0].Description,
                                            TrackingType = TrackingType.OutBound,
                                            User = "International Shipping",
                                            Status = item.Description,
                                            ScanStatus = new ScanStatusDTO
                                            {
                                                Code = item.ServiceArea[0].Code,
                                                Incident = item.Description,
                                                Reason = item.ServiceArea[0].Description,
                                                Comment = item.Description,
                                                DateCreated = Convert.ToDateTime(item.Date + " " + item.Time),
                                                DateModified = Convert.ToDateTime(item.Date + " " + item.Time)
                                            }
                                        };
                                        dhlTracking.Add(data);
                                    }
                                    shipmentTracking.AddRange(dhlTracking);
                                }
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
                        ServiceCenterAgentName = userDTO.FirstName + " " + userDTO.LastName,
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

        public async Task<List<UserDTO>> GetAllRegionalManagersForServiceCentre(int currentServiceCenterId)
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

        //Send email to sender when international shipment has arrived Nigeria
        public async Task<bool> SendEmailToCustomerForIntlShipment(Shipment shipment)
        {
            //SEND EMAIL
            var invoice = await _uow.Invoice.GetAsync(x => x.Waybill == shipment.Waybill);

            //Mails to Receiver
            var messageDTO = new MessageDTO()
            {
                CustomerName = shipment.ReceiverName,
                Waybill = shipment.Waybill,
                IntlMessage = new IntlMessageDTO()
                {
                    DeliveryAddressOrCenterName = shipment.ReceiverAddress,
                },
                To = shipment.ReceiverEmail,
                ToEmail = shipment.ReceiverEmail,
                Subject = "International Shipments Arrive Final Destination",
            };

            if (invoice.PaymentStatus == PaymentStatus.Paid)
            {
                var deliveryCode = await _uow.DeliveryNumber.GetAsync(x => x.Waybill == shipment.Waybill);
                messageDTO.IntlMessage.DeliveryCode = deliveryCode.SenderCode;

                if (shipment.PickupOptions == PickupOptions.HOMEDELIVERY)
                {
                    messageDTO.MessageTemplate = "OverseasHomeDelivery";
                    await _messageSenderService.SendOverseasMails(messageDTO);
                }
                else
                {
                    var destination = await _uow.ServiceCentre.GetAsync(x => x.ServiceCentreId == shipment.DestinationServiceCentreId);
                    messageDTO.IntlMessage.DeliveryAddressOrCenterName = destination.FormattedServiceCentreName;
                    messageDTO.MessageTemplate = "OverseasPickup";
                    await _messageSenderService.SendOverseasMails(messageDTO);
                }
            }
            return true;
        }

        //Send email to sender when international shipment is cargoed,(both US and UK)
        public async Task<bool> SendEmailToCustomerWhenIntlShipmentIsCargoed(ShipmentDTO shipmentDTO)
        {
            //SEND Email

            if (shipmentDTO.CustomerType.Contains("Individual"))
            {
                shipmentDTO.CustomerType = CustomerType.IndividualCustomer.ToString();
            }
            CustomerType customerType = (CustomerType)Enum.Parse(typeof(CustomerType), shipmentDTO.CustomerType);

            var customerObj = await _messageSenderService.GetCustomer(shipmentDTO.CustomerId, customerType);

            var country = await _uow.Country.GetAsync(x => x.CountryId == shipmentDTO.DepartureCountryId);

            var messageDTO = new MessageDTO()
            {
                CustomerName = customerObj.FirstName,
                Waybill = shipmentDTO.Waybill,
                Currency = country.CurrencySymbol,
                IntlMessage = new IntlMessageDTO()
                {
                    ShippingCost = $"{country.CurrencySymbol}{shipmentDTO.GrandTotal.ToString()}",
                    DepartureCenter = _uow.ServiceCentre.SingleOrDefault(x => x.ServiceCentreId == shipmentDTO.DepartureServiceCentreId).Name,
                },
                To = customerObj.Email,
                ToEmail = customerObj.Email,
                Body = shipmentDTO.DepartureCountryId == 207 ? DateTime.Now.AddDays(14).ToString("dd/MM/yyyy") : DateTime.Now.AddDays(5).ToString("dd/MM/yyyy"),
                Subject = $"International Shipment Cargoed ({country.CountryName})",
                MessageTemplate = "OverseasDepartsHub"
            };

            await _messageSenderService.SendOverseasMails(messageDTO);
            return true;
        }

        //Send email and SMS when Scan of "Intl Shipment Arrive Nigeria" and payment has not been made
        public async Task<bool> SendEmailToCustomerForIntlShipmentArriveNigeria(ShipmentDTO shipmentDTO, List<string> paymentLinks)
        {
            //Send SMS
            shipmentDTO.CustomerDetails = new CustomerDTO();
            shipmentDTO.DepartureServiceCentre = new ServiceCentreDTO();
            shipmentDTO.DestinationServiceCentre = new ServiceCentreDTO();
            shipmentDTO.CustomerDetails.PhoneNumber = shipmentDTO.ReceiverPhoneNumber;
            shipmentDTO.URL = paymentLinks[0];

            await _messageSenderService.SendMessage(MessageType.AISNU, EmailSmsType.SMS, shipmentDTO);

            //Send Email
            await _messageSenderService.SendOverseasShipmentReceivedMails(shipmentDTO, paymentLinks, 1);

            return true;
        }

        //Send email and SMS when Scan of "Intl Shipment Arrive Nigeria" and payment has not been made
        public async Task<bool> SendEmailShipmentArriveFinalDestination(ShipmentDTO shipmentDTO)
        {
            if (shipmentDTO != null)
            {
                if (shipmentDTO.CustomerType.Contains("Individual"))
                {
                    shipmentDTO.CustomerType = CustomerType.IndividualCustomer.ToString();
                }
            }
            CustomerType customerType = (CustomerType)Enum.Parse(typeof(CustomerType), shipmentDTO.CustomerType);

            var customerObj = await _messageSenderService.GetCustomer(shipmentDTO.CustomerId, customerType);

            var country = await _uow.Country.GetAsync(x => x.CountryId == shipmentDTO.DepartureCountryId);

            var messageDTO = new MessageDTO()
            {
                CustomerName = customerObj.FirstName,
                Waybill = shipmentDTO.Waybill,
                Currency = country.CurrencySymbol,
                To = customerObj.Email,
                ToEmail = customerObj.Email,
                Body = shipmentDTO.DepartureCountryId == 207 ? DateTime.Now.AddDays(14).ToString("dd/MM/yyyy") : DateTime.Now.AddDays(5).ToString("dd/MM/yyyy"),
                Subject = $"Shipment arrive final destination",
                MessageTemplate = "OverseasDepartsHub"
            };

            //Send Email
            await _messageSenderService.SendMailsShipmentARF(messageDTO);

            return true;
        }

        //Send email when Shipment Arrive final destination and its home delivery
        public async Task<bool> SendEmailShipmentARFHomeDelivery(ShipmentDTO shipmentDTO)
        {
            if (shipmentDTO != null)
            {
                if (shipmentDTO.CustomerType.Contains("Individual"))
                {
                    shipmentDTO.CustomerType = CustomerType.IndividualCustomer.ToString();
                }
            }
            CustomerType customerType = (CustomerType)Enum.Parse(typeof(CustomerType), shipmentDTO.CustomerType);

            shipmentDTO.ReceiverEmail = shipmentDTO.ReceiverEmail == null ? shipmentDTO.ReceiverEmail : shipmentDTO.ReceiverEmail.Trim();



            var deliveryNumber = _uow.DeliveryNumber.GetAll()
                                           .Where(s => s.Waybill == shipmentDTO.Waybill)
                                           .Select(s => new { s.SenderCode }).FirstOrDefault().SenderCode;

            var messageDTO = new MessageDTO()
            {
                CustomerName = string.IsNullOrEmpty(shipmentDTO?.ReceiverName) ? "Customer" : shipmentDTO?.ReceiverName,
                Waybill = shipmentDTO.Waybill,
                To = shipmentDTO?.ReceiverEmail,
                ToEmail = shipmentDTO?.ReceiverEmail,
                ShipmentCreationMessage = new ShipmentCreationMessageDTO
                {
                    DeliveryNumber = deliveryNumber,
                },
                Subject = $"Shipment Arrival Notification",
                MessageTemplate = "ArrivedFinalDestinationForHomeDelivery"
            };

            //Send Email
            if (!String.IsNullOrEmpty(shipmentDTO.ReceiverEmail))
            {
                await _messageSenderService.SendMailsShipmentARFHomeDelivery(messageDTO);
            }

            return true;
        }

        //Send email when Shipment Arrive final destination and its terminal pickup
        public async Task<bool> SendEmailShipmentARFTerminalPickup(ShipmentDTO shipmentDTO)
        {
            if (shipmentDTO != null)
            {
                if (shipmentDTO.CustomerType.Contains("Individual"))
                {
                    shipmentDTO.CustomerType = CustomerType.IndividualCustomer.ToString();
                }
            }
            CustomerType customerType = (CustomerType)Enum.Parse(typeof(CustomerType), shipmentDTO.CustomerType);

            shipmentDTO.ReceiverEmail = shipmentDTO.ReceiverEmail == null ? shipmentDTO.ReceiverEmail : shipmentDTO.ReceiverEmail.Trim();

            var deliveryNumber = _uow.DeliveryNumber.GetAll()
                                            .Where(s => s.Waybill == shipmentDTO.Waybill)
                                            .Select(s => new { s.SenderCode }).FirstOrDefault().SenderCode;

            var messageDTO = new MessageDTO()
            {
                CustomerName = string.IsNullOrEmpty(shipmentDTO?.ReceiverName) ? "Customer" : shipmentDTO?.ReceiverName,
                Waybill = shipmentDTO.Waybill,
                To = shipmentDTO?.ReceiverEmail,
                ToEmail = shipmentDTO?.ReceiverEmail,
                ShipmentCreationMessage = new ShipmentCreationMessageDTO
                {
                    DeliveryNumber = deliveryNumber,
                },
                Subject = $"Shipment Arrival Notification",
                MessageTemplate = "ArrivedFinalDestinationForTerminalPickup"
            };

            //Send Email
            if (!String.IsNullOrEmpty(shipmentDTO.ReceiverEmail))
            {
                await _messageSenderService.SendMailsShipmentARFTerminalPickup(messageDTO);
            }

            return true;
        }

        //Send whatsapp message when Shipment Arrive final destination
        public async Task<bool> SendWhatsappMessage(ShipmentDTO shipmentDTO)
        {
            if (shipmentDTO != null)
            {

                var invoice = _uow.Invoice.GetAllInvoiceShipments().Where(s => s.Waybill == shipmentDTO.Waybill).FirstOrDefault();

                var deliveryNumber = _uow.DeliveryNumber.GetAll()
                                            .Where(s => s.Waybill == invoice.Waybill)
                                            .Select(s => new { s.SenderCode }).FirstOrDefault().SenderCode;

                var sourceId = ConfigurationManager.AppSettings["WhatsAppSourceID"];

                var whatsappMessage = new WhatsAppMessageDTO
                {
                    RecipientWhatsapp = invoice.ReceiverPhoneNumber,
                    MessageType = "template",
                    Source = sourceId,
                    RecipientType = "individual",
                    TypeTemplate = new List<TypeTemplateDTO>
                        {
                            new TypeTemplateDTO
                            {
                                Name = "shipment_notification_terminal_pickup",
                                Attributes = new List<string>
                                {
                                    invoice.ReceiverName, 
                                    invoice.Waybill,
                                    invoice.DestinationServiceCentreName,
                                    deliveryNumber
                                },
                                Language = new LanguageDTO
                                {
                                    Locale = "en",
                                    Policy = "deterministic"
                                }
                            }
                        }
                };

                if(shipmentDTO.PickupOptions == PickupOptions.HOMEDELIVERY)
                {
                    foreach (var template in whatsappMessage.TypeTemplate)
                    {
                        template.Name = "shipment_notification_home_delivery";
                    }
                }
                //Send Whatsapp message
                if (!String.IsNullOrEmpty(invoice.ReceiverPhoneNumber))
                {
                    await _messageSenderService.SendWhatsappMessage(whatsappMessage);
                }
            }

            return true;
        }

        //Send whatsapp message when Shipment Arrive final destination Temporal
        public async Task<bool> SendWhatsappMessageTemporal( MessageType messageType, object tracking)
        {
            if(tracking != null)
            {
                await _messageSenderService.SendWhatsappMessageTemporal(messageType, tracking);
            }
            return true;
        }
    }
}
