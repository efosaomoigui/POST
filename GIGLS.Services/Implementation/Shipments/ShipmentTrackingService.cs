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

                if(tracking.Location == null)
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
                    await sendSMSEmail(tracking, scanStatus);
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
            foreach(var item in Enum.GetValues(typeof(MessageType)).Cast<MessageType>())
            {
                if(item.ToString() == scanStatus.ToString())
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
                    TrackingType = shipmentTracking.TrackingType.ToString(),
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

        //public async Task<IEnumerable<ShipmentTrackingDTO>> GetShipmentWaitingForCollection()
        //{
        //    return await _uow.ShipmentTracking.GetShipmentWaitingForCollection();
        //}

        public async Task<IEnumerable<ShipmentTrackingDTO>> GetShipmentTrackings(string waybill)
        {
            try
            {
                var shipmentTracking = await _uow.ShipmentTracking.GetShipmentTrackingsAsync(waybill);

                if (shipmentTracking.Count > 0)
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
    }
}
