using GIGLS.Core;
using GIGLS.Core.IServices.User;
using GIGLS.Core.IMessageService;
using GIGLS.Core.IServices.Shipments;
using GIGLS.Core.DTO;
using GIGLS.Core.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using GIGLS.Infrastructure;
using GIGLS.CORE.Domain;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.DTO.ShipmentScan;
using GIGLS.Core.IServices.Business;

namespace GIGLS.Services.Implementation.Shipments
{
    public class MobileShipmentTrackingService : IMobileShipmentTrackingService
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserService _userService;
        private readonly IMessageSenderService _messageSenderService;
        private readonly IShipmentTrackService _shipmentTrackService;


        public MobileShipmentTrackingService(IUnitOfWork uow, IUserService userService, IMessageSenderService messageSenderService,
            IShipmentTrackService shipmentTrackService)
        {
            _uow = uow;
            _userService = userService;
            _messageSenderService = messageSenderService;
            _shipmentTrackService = shipmentTrackService;

        }

        

        public async Task<MobileShipmentTrackingHistoryDTO> GetMobileShipmentTrackings(string waybill)
        {
            try
            {
                var trackings = new MobileShipmentTrackingHistoryDTO();

                //1. call agility core tracking
                var shipmentTracking = await _shipmentTrackService.TrackShipment(waybill);

                //2. call mobile tracking
                var MobileshipmentTracking = await _uow.MobileShipmentTracking.GetMobileShipmentTrackingsAsync(waybill);

                //3. convert agility tracking object to mobile tracking object
                var shipmentTrackingMobileVersion = shipmentTracking.Select(s => new MobileShipmentTrackingDTO
                {
                    Waybill = s.Waybill,
                    Location = s.Location,
                    Status = s.Status,
                    DateTime = s.DateTime,
                    TrackingType = s.TrackingType,
                    User = s.User,
                    MobileShipmentTrackingId = s.ShipmentTrackingId,
                    ScanStatus = new MobileScanStatusDTO
                    {
                        Code = s.ScanStatus.Code,
                        Incident = s.ScanStatus.Incident,
                        Reason = s.ScanStatus.Reason,
                        Comment = s.ScanStatus.Comment
                    }
                }
                );
                //4. append the two lists together
                MobileshipmentTracking.AddRange(shipmentTrackingMobileVersion);
                var orderedtrackings = MobileshipmentTracking.OrderByDescending(x => x.DateTime).ToList();
                var addresses = await _uow.PreShipmentMobile.GetAsync(s=>s.Waybill == waybill);
                if(addresses !=null)
                {
                   trackings = new MobileShipmentTrackingHistoryDTO
                    {
                        Origin = addresses.SenderAddress,
                        Destination = addresses.ReceiverAddress,
                        MobileShipmentTrackings = orderedtrackings
                   };
                }
                else
                {
                    var shipmentaddress = await _uow.Shipment.GetAsync(s => s.Waybill == waybill);
                    if (shipmentaddress != null)
                    {
                        trackings = new MobileShipmentTrackingHistoryDTO
                        {
                            Origin = shipmentaddress.SenderAddress,
                            Destination = shipmentaddress.ReceiverAddress,
                            MobileShipmentTrackings = orderedtrackings
                        };
                    }
                    else
                    {
                        throw new GenericException("Invalid waybill number!!");
                    }
                }

                return trackings;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<MobileShipmentTrackingDTO> GetMobileShipmentTrackingById(int trackingId)
        {
            try
            {
                var shipmentTracking = await _uow.MobileShipmentTracking.GetAsync(trackingId);
                if (shipmentTracking == null)
                {
                    throw new GenericException($"MobileShipmentTrackingId: {trackingId} does Not Exist");
                }
                return new MobileShipmentTrackingDTO
                {
                    Waybill = shipmentTracking.Waybill,
                    DateTime = shipmentTracking.DateTime,
                    Location = shipmentTracking.Location,
                    MobileShipmentTrackingId = shipmentTracking.MobileShipmentTrackingId,
                    TrackingType = shipmentTracking.TrackingType,
                    Status = shipmentTracking.Status,
                    User = shipmentTracking.User.FirstName + " " + shipmentTracking.User.LastName,
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task AddMobileShipmentTracking(MobileShipmentTrackingDTO tracking, ShipmentScanStatus scanStatus)
        {
            
            try
            {
                
                if (tracking.User == null)
                {
                    tracking.User = await _userService.GetCurrentUserId();
                }
                //check if the waybill has not been scan for the status before
                bool shipmentTracking = await _uow.MobileShipmentTracking.ExistAsync(x => x.Waybill.Equals(tracking.Waybill) && x.Status.Equals(tracking.Status));

                if (!shipmentTracking || scanStatus.Equals(ShipmentScanStatus.AD))
                {
                    var newShipmentTracking = new MobileShipmentTracking
                    {
                        Waybill = tracking.Waybill,
                        Status = tracking.Status,
                        DateTime = DateTime.Now,
                        UserId = tracking.User,
                        ServiceCentreId = tracking.ServiceCentreId
                    };
                    _uow.MobileShipmentTracking.Add(newShipmentTracking);
                    await _uow.CompleteAsync();
                    
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateShipmentTracking(int trackingId, MobileShipmentTrackingDTO tracking)
        {
            try
            {
                var shipmentTracking = await _uow.MobileShipmentTracking.GetAsync(trackingId);
                if (shipmentTracking == null || tracking.MobileShipmentTrackingId != trackingId)
                {
                    throw new GenericException("MobileShipmentTracking Not Exist");
                }
                shipmentTracking.Location = shipmentTracking.Location;
                shipmentTracking.TrackingType = shipmentTracking.TrackingType;
                shipmentTracking.User = shipmentTracking.User;
                shipmentTracking.Status = tracking.Status;
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<bool> CheckMobileShipmentTracking(string waybill, string status)
        {
            bool shipmentTracking = await _uow.MobileShipmentTracking.ExistAsync(x => x.Waybill.Equals(waybill) && x.Status.Equals(status));
            return shipmentTracking;
        }
    }
}
