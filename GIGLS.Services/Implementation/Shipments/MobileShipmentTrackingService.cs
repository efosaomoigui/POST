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

namespace GIGLS.Services.Implementation.Shipments
{
    public class MobileShipmentTrackingService : IMobileShipmentTrackingService
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserService _userService;
        private readonly IMessageSenderService _messageSenderService;
       

        public MobileShipmentTrackingService(IUnitOfWork uow, IUserService userService, IMessageSenderService messageSenderService
            )
        {
            _uow = uow;
            _userService = userService;
            _messageSenderService = messageSenderService;
           
        }

        public async Task<List<MobileShipmentTrackingDTO>> GetMobileShipmentTrackings()
        {
            try
            {
                return await _uow.MobileShipmentTracking.GetMobileShipmentTrackingsAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<MobileShipmentTrackingHistoryDTO> GetMobileShipmentTrackings(string waybill)
        {
            try
            {
                var shipmentTracking = await _uow.MobileShipmentTracking.GetMobileShipmentTrackingsAsync(waybill);
                var addresses = await _uow.PreShipmentMobile.GetAsync(s=>s.Waybill == waybill);
                var trackings = new MobileShipmentTrackingHistoryDTO
                {
                    Origin = addresses.SenderAddress,
                    Destination = addresses.ReceiverAddress,
                    MobileShipmentTrackings = shipmentTracking
                };

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
                    TrackingType = shipmentTracking.TrackingType.ToString(),
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
