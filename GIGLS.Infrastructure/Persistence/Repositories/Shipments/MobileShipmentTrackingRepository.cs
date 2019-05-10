using GIGLS.Core.Domain;
using GIGLS.Core.DTO;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.DTO.ShipmentScan;
using GIGLS.Core.IRepositories.Shipments;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GIGLS.Infrastructure.Persistence.Repositories.Shipments
{
    public class MobileShipmentTrackingRepository : Repository<MobileShipmentTracking, GIGLSContext>, IMobileShipmentTrackingRepository
    {
        private GIGLSContext _context;
       
        public MobileShipmentTrackingRepository(GIGLSContext context)
            : base(context)
        {
            _context = context;
            
        }

        public Task<List<MobileShipmentTrackingDTO>> GetMobileShipmentTrackingsAsync()
        {
            try
            {
                var MobileshipmentTrackings = _context.MobileShipmentTracking;

                var mobileshipmentTrackingDto = from shipmentTracking in MobileshipmentTrackings
                                          select new MobileShipmentTrackingDTO
                                          {
                                              DateTime = shipmentTracking.DateTime,
                                              Location = shipmentTracking.Location,
                                              Waybill = shipmentTracking.Waybill,
                                              MobileShipmentTrackingId = shipmentTracking.MobileShipmentTrackingId,
                                              TrackingType = shipmentTracking.TrackingType.ToString(),
                                              User = shipmentTracking.User.FirstName + " " + shipmentTracking.User.LastName,
                                              Status = shipmentTracking.Status,
                                              
                                          };
                
                var shipmentTrackings = Context.ShipmentTracking;

                var shipmentTrackingDto = from shipmentTracking in shipmentTrackings
                                          select new MobileShipmentTrackingDTO
                                          {
                                              DateTime = shipmentTracking.DateTime,
                                              Location = shipmentTracking.Location,
                                              Waybill = shipmentTracking.Waybill,
                                              MobileShipmentTrackingId = shipmentTracking.ShipmentTrackingId,
                                              TrackingType = shipmentTracking.TrackingType.ToString(),
                                              User = shipmentTracking.User.FirstName + " " + shipmentTracking.User.LastName,
                                              Status = shipmentTracking.Status,

                                          };
                var AllshipmentTrackings = shipmentTrackingDto.ToList();
                AllshipmentTrackings.AddRange(mobileshipmentTrackingDto);
                return Task.FromResult(AllshipmentTrackings.ToList());
            }
            catch (Exception)
            {

                throw;
            }

        }

        public Task<List<MobileShipmentTrackingDTO>> GetMobileShipmentTrackingsAsync(string waybill)
        {
            try
            {
                var shipmentTrackings = Context.ShipmentTracking.Where(x => x.Waybill == waybill);

                var shipmentTrackingDto = from shipmentTracking in shipmentTrackings
                                          select new MobileShipmentTrackingDTO
                                          {
                                              DateTime = shipmentTracking.DateTime,
                                              Location = shipmentTracking.Location,
                                              Waybill = shipmentTracking.Waybill,
                                              MobileShipmentTrackingId = shipmentTracking.ShipmentTrackingId,
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
                var MobileshipmentTrackings = Context.MobileShipmentTracking.Where(x => x.Waybill == waybill);

                var MobileshipmentTrackingDto = from shipmentTracking in MobileshipmentTrackings
                                          select new MobileShipmentTrackingDTO
                                          {
                                              DateTime = shipmentTracking.DateTime,
                                              Location = shipmentTracking.Location,
                                              Waybill = shipmentTracking.Waybill,
                                              MobileShipmentTrackingId = shipmentTracking.MobileShipmentTrackingId,
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
                var AllshipmentTrackings = MobileshipmentTrackingDto.ToList();
                AllshipmentTrackings.AddRange(shipmentTrackingDto);

                return Task.FromResult(AllshipmentTrackings.ToList().OrderByDescending(x => x.DateTime).ToList());
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
