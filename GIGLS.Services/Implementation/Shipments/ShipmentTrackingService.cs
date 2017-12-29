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

namespace GIGLS.Services.Implementation.Shipments
{
    public class ShipmentTrackingService : IShipmentTrackingService
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserService _userService;

        public ShipmentTrackingService(IUnitOfWork uow, IUserService userService)
        {
            _uow = uow;
            _userService = userService;
        }

        public async Task<object> AddShipmentTracking(ShipmentTrackingDTO tracking, ShipmentScanStatus scanStatus)
        {
            try
            {
                if (tracking.User == null)
                {
                    tracking.User = await _userService.GetCurrentUserId();
                }

                if(tracking.Location == null)
                {
                    var UserServiceCenters = await _userService.GetPriviledgeServiceCenters();
                    var serviceCenter = await _uow.ServiceCentre.GetAsync(UserServiceCenters[0]);
                    tracking.Location = serviceCenter.Name;
                }

                var newShipmentTracking = new GIGL.GIGLS.Core.Domain.ShipmentTracking
                {
                    Waybill = tracking.Waybill,
                    //TrackingType = (TrackingType)Enum.Parse(typeof(TrackingType), tracking.TrackingType),
                    Location = tracking.Location,
                    Status = tracking.Status,
                    DateTime = DateTime.Now,
                    UserId = tracking.User
                };
                _uow.ShipmentTracking.Add(newShipmentTracking);

                if (scanStatus.Equals(ShipmentScanStatus.DASD) || scanStatus.Equals(ShipmentScanStatus.DASP))
                {
                    var newShipmentCollection = new ShipmentCollection
                    {
                        Waybill = tracking.Waybill,
                        ShipmentScanStatus = scanStatus
                    };

                    _uow.ShipmentCollection.Add(newShipmentCollection);
                }

                await _uow.CompleteAsync();
                return new { Id = newShipmentTracking.ShipmentTrackingId };
            }
            catch (Exception)
            {
                throw;
            }
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
                    User = shipmentTracking.User.FirstName + " " + shipmentTracking.User.LastName,
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
                return await _uow.ShipmentTracking.GetShipmentTrackingsAsync(waybill);
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
