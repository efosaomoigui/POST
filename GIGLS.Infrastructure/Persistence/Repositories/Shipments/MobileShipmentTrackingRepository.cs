using GIGLS.Core.Domain;
using GIGLS.Core.DTO;
using GIGLS.Core.DTO.ShipmentScan;
using GIGLS.Core.IRepositories.Shipments;
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
       
        public MobileShipmentTrackingRepository(GIGLSContext context) : base(context)
        {
            _context = context;            
        }

       public Task<List<MobileShipmentTrackingDTO>> GetMobileShipmentTrackingsAsync(string waybill)
        {
            try
            {
                var MobileshipmentTrackings = Context.MobileShipmentTracking.Where(x => x.Waybill == waybill);

                var MobileshipmentTrackingDto = from shipmentTracking in MobileshipmentTrackings
                                          select new MobileShipmentTrackingDTO
                                          {
                                              DateTime = shipmentTracking.DateTime,
                                              Location = shipmentTracking.Location,
                                              Waybill = shipmentTracking.Waybill,
                                              MobileShipmentTrackingId = shipmentTracking.MobileShipmentTrackingId,
                                              TrackingType = shipmentTracking.TrackingType,
                                              User = shipmentTracking.User.FirstName + " " + shipmentTracking.User.LastName,
                                              Status = shipmentTracking.Status,
                                              ScanStatus = Context.MobileScanStatus.Where(c => c.Code == shipmentTracking.Status).Select(x => new MobileScanStatusDTO
                                              {
                                                  Code = x.Code,
                                                  Incident = x.Incident,
                                                  Reason = x.Reason,
                                                  Comment = x.Comment
                                              }).FirstOrDefault(),
                                          };
                return Task.FromResult(MobileshipmentTrackingDto.ToList());
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
