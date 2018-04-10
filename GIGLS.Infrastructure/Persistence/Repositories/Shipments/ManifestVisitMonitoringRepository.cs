using GIGLS.Core.Domain;
using GIGLS.Core.IRepositories.Shipments;
using GIGLS.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Shipments;

namespace GIGLS.Infrastructure.Persistence.Repositories.Shipments
{
    public class ManifestVisitMonitoringRepository : Repository<ManifestVisitMonitoring, GIGLSContext>, IManifestVisitMonitoringRepository
    {
        private GIGLSContext _context;
        public ManifestVisitMonitoringRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }
        
        public Task<List<ManifestVisitMonitoringDTO>> GetManifestVisitMonitorings()
        {
            try
            {
                var manifestVisitMonitorings = _context.ManifestVisitMonitoring;

                var manifestVisitMonitoringDto = from r in manifestVisitMonitorings
                                                 select new ManifestVisitMonitoringDTO()
                                                 {
                                                     ManifestVisitMonitoringId = r.ManifestVisitMonitoringId,
                                                     Waybill = r.Waybill,
                                                     ReceiverName = r.ReceiverName,
                                                     ReceiverPhoneNumber = r.ReceiverPhoneNumber,
                                                     Address = r.Address,
                                                     Signature = r.Signature,
                                                     Status = r.Status,
                                                     UserId = r.User.FirstName + " " + r.User.LastName,
                                                     DateCreated = r.DateCreated,
                                                     DateModified = r.DateModified
                                                 };

                return Task.FromResult(manifestVisitMonitoringDto.OrderByDescending(x => x.DateCreated).ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<List<ManifestVisitMonitoringDTO>> GetManifestVisitMonitoringByWaybill(string waybill)
        {
            try
            {
                var manifestVisitMonitorings = _context.ManifestVisitMonitoring.Where(x => x.Waybill == waybill);

                var manifestVisitMonitoringDto = from r in manifestVisitMonitorings
                                                 select new ManifestVisitMonitoringDTO()
                                                 {
                                                     ManifestVisitMonitoringId = r.ManifestVisitMonitoringId,
                                                     Waybill = r.Waybill,
                                                     ReceiverName = r.ReceiverName,
                                                     ReceiverPhoneNumber = r.ReceiverPhoneNumber,
                                                     Address = r.Address,
                                                     Signature = r.Signature,
                                                     Status = r.Status,
                                                     UserId = r.User.FirstName + " " + r.User.LastName,
                                                     DateCreated = r.DateCreated,
                                                     DateModified = r.DateModified
                                                 };

                return Task.FromResult(manifestVisitMonitoringDto.OrderByDescending(x => x.DateCreated).ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
