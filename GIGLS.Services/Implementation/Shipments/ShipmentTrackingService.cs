using System;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.IServices.Shipments;
using GIGLS.Core;
using GIGLS.Core.Enums;
using GIGLS.Infrastructure;
using System.Collections.Generic;

namespace GIGLS.Services.Implementation.Shipments
{
    public class ShipmentTrackingService : IShipmentTrackingService
    {
        private readonly IUnitOfWork _uow;
        public ShipmentTrackingService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<object> AddShipmentTracking(ShipmentTrackingDTO tracking)
        {
            try
            {
                var newShipmentTracking = new GIGL.GIGLS.Core.Domain.ShipmentTracking
                {
                    Waybill = tracking.Waybill,
                    //TrackingType = (TrackingType)Enum.Parse(typeof(TrackingType), tracking.TrackingType),
                    Location = tracking.Location,
                    Status = tracking.Status,
                    DateTime = DateTime.Now
                };
                _uow.ShipmentTracking.Add(newShipmentTracking);
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
                    User = shipmentTracking.User.UserName
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<ShipmentTrackingDTO>> GetShipmentTrackings()
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
    }
}
