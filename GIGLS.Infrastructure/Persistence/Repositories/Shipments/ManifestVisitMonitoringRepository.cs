using GIGLS.Core.Domain;
using GIGLS.Core.IRepositories.Shipments;
using GIGLS.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.DTO.ServiceCentres;

namespace GIGLS.Infrastructure.Persistence.Repositories.Shipments
{
    public class ManifestVisitMonitoringRepository : Repository<ManifestVisitMonitoring, GIGLSContext>, IManifestVisitMonitoringRepository
    {
        private GIGLSContext _context;
        public ManifestVisitMonitoringRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }
        
        public Task<List<ManifestVisitMonitoringDTO>> GetManifestVisitMonitorings(int[] serviceCentreIds)
        {
            try
            {
                var manifestVisitMonitorings = _context.ManifestVisitMonitoring.AsQueryable();

                if(serviceCentreIds.Length > 0)
                {
                    manifestVisitMonitorings = manifestVisitMonitorings.Where(s => serviceCentreIds.Contains(s.ServiceCentreId));
                }

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
                                                     DateModified = r.DateModified,
                                                     ServiceCentreId = r.ServiceCentreId,
                                                     ServiceCentre = Context.ServiceCentre.Where(c => c.ServiceCentreId == r.ServiceCentreId).Select(x => new ServiceCentreDTO
                                                     {
                                                         Code = x.Code,
                                                         Name = x.Name
                                                     }).FirstOrDefault(),
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
                                                     DateModified = r.DateModified,
                                                     ServiceCentreId = r.ServiceCentreId,
                                                     ServiceCentre = Context.ServiceCentre.Where(c => c.ServiceCentreId == r.ServiceCentreId).Select(x => new ServiceCentreDTO
                                                     {
                                                         Code = x.Code,
                                                         Name = x.Name
                                                     }).FirstOrDefault(),
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
