using GIGL.GIGLS.Core.Domain;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.DTO.ShipmentScan;
using GIGLS.Core.IRepositories.Shipments;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.Shipments
{
    public class ShipmentTrackingRepository : Repository<ShipmentTracking, GIGLSContext>, IShipmentTrackingRepository
    {
        private GIGLSContext _context;
        public ShipmentTrackingRepository(GIGLSContext context)
            : base(context)
        {
            _context = context;
        }

        public Task<List<ShipmentTrackingDTO>> GetShipmentTrackingsAsync()
        {
            try
            {
                var shipmentTrackings = Context.ShipmentTracking;

                var shipmentTrackingDto = from shipmentTracking in shipmentTrackings
                                          select new ShipmentTrackingDTO
                                          {
                                              DateTime = shipmentTracking.DateTime,
                                              Location = shipmentTracking.Location,
                                              Waybill = shipmentTracking.Waybill,
                                              ShipmentTrackingId = shipmentTracking.ShipmentTrackingId,
                                              TrackingType = shipmentTracking.TrackingType.ToString(),
                                              User = shipmentTracking.User.FirstName + " " + shipmentTracking.User.LastName,
                                              Status = shipmentTracking.Status
                                          };
                return Task.FromResult(shipmentTrackingDto.ToList());
            }
            catch (Exception)
            {

                throw;
            }

        }

        public Task<List<ShipmentTrackingDTO>> GetShipmentTrackingsAsync(string waybill)
        {
            try
            {
                var shipmentTrackings = Context.ShipmentTracking.Where(x => x.Waybill == waybill);

                var shipmentTrackingDto = from shipmentTracking in shipmentTrackings
                                          select new ShipmentTrackingDTO
                                          {
                                              DateTime = shipmentTracking.DateTime,
                                              Location = shipmentTracking.Location,
                                              Waybill = shipmentTracking.Waybill,
                                              ShipmentTrackingId = shipmentTracking.ShipmentTrackingId,
                                              TrackingType = shipmentTracking.TrackingType.ToString(),
                                              User = shipmentTracking.User.FirstName + " " + shipmentTracking.User.LastName,
                                              Status = shipmentTracking.Status,
                                              ScanStatus = Context.ScanStatus.Where(c => c.Code == shipmentTracking.Status).Select(x => new ScanStatusDTO
                                              {
                                                Code = x.Code,
                                                Incident = x.Incident,
                                                Reason = x.Reason,
                                                Comment = x.Comment
                                              }).FirstOrDefault(),
                                          };
                return Task.FromResult(shipmentTrackingDto.ToList().OrderByDescending(x => x.DateTime).ToList());
            }
            catch (Exception)
            {
                throw;
            }

        }

    }
}
