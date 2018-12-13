using GIGLS.Core.IRepositories.Shipments;
using GIGLS.Core.Domain;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;
using GIGLS.Core.DTO.Shipments;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.Core.DTO.ServiceCentres;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.Shipments
{
    public class ManifestWaybillMappingRepository : Repository<ManifestWaybillMapping, GIGLSContext>, IManifestWaybillMappingRepository
    {
        private GIGLSContext _context;
        public ManifestWaybillMappingRepository(GIGLSContext context)
            : base(context)
        {
            _context = context;
        }
        
        public async Task<List<ManifestWaybillMappingDTO>> GetManifestWaybillMappings(int[] serviceCentreIds)
        {
            var manifestWaybillMapping = Context.ManifestWaybillMapping.AsQueryable();

            var serviceCentreGroupWaybills = new List<string>();
            if (serviceCentreIds.Length > 0)
            {
                manifestWaybillMapping = manifestWaybillMapping.Where(s => serviceCentreIds.Contains(s.ServiceCentreId));
            }

            var manifestWaybillMappingDTO = from mgw in manifestWaybillMapping
                                                 select new ManifestWaybillMappingDTO
                                                 {
                                                     ManifestWaybillMappingId = mgw.ManifestWaybillMappingId,
                                                     ManifestCode = mgw.ManifestCode,
                                                     IsActive = mgw.IsActive,
                                                     DateCreated = mgw.DateCreated,
                                                     DateModified = mgw.DateModified,
                                                     Waybill = mgw.Waybill,
                                                     ServiceCentreId = mgw.ServiceCentreId,
                                                     ServiceCentre = Context.ServiceCentre.Where(c => c.ServiceCentreId == mgw.ServiceCentreId).Select(x => new ServiceCentreDTO
                                                     {
                                                         Code = x.Code,
                                                         Name = x.Name
                                                     }).FirstOrDefault(),
                                                     ManifestDetails = Context.Manifest.Where(x => x.ManifestCode == mgw.ManifestCode).
                                                     Select(p => new ManifestDTO
                                                     {
                                                         DateCreated = p.DateCreated,
                                                         DateModified = p.DateModified,
                                                         ManifestCode = p.ManifestCode,
                                                         ManifestType = p.ManifestType,
                                                         DateTime = p.DateTime,
                                                         IsDispatched = p.IsDispatched,
                                                         IsReceived = p.IsReceived,
                                                         DispatchedBy = Context.Users.Where(d => d.Id == p.DispatchedById).Select(x => x.LastName + " " + x.FirstName).FirstOrDefault(),
                                                         ReceiverBy = Context.Users.Where(r => r.Id == p.ReceiverById).Select(x => x.LastName + " " + x.FirstName).FirstOrDefault()
                                                     }).FirstOrDefault()
                                                 };

            return await Task.FromResult(manifestWaybillMappingDTO.OrderByDescending(x => x.DateCreated).ToList());
        }


        public async Task<List<ManifestWaybillMappingDTO>> GetManifestWaybillWaitingForSignOff(int[] serviceCentreIds, List<string> manifests)
        {
            var manifestWaybillMapping = Context.ManifestWaybillMapping.AsQueryable();

            var serviceCentreGroupWaybills = new List<string>();
            if (serviceCentreIds.Length > 0)
            {
                manifestWaybillMapping = manifestWaybillMapping.Where(s => serviceCentreIds.Contains(s.ServiceCentreId));
            }

            if (manifests.Count() > 0)
            {
                manifestWaybillMapping = manifestWaybillMapping.Where(s => manifests.Contains(s.ManifestCode));
            }

            var manifestWaybillMappingDTO = from mgw in manifestWaybillMapping
                                            select new ManifestWaybillMappingDTO
                                            {
                                                ManifestWaybillMappingId = mgw.ManifestWaybillMappingId,
                                                ManifestCode = mgw.ManifestCode,
                                                IsActive = mgw.IsActive,
                                                DateCreated = mgw.DateCreated,
                                                DateModified = mgw.DateModified,
                                                Waybill = mgw.Waybill,
                                                ServiceCentreId = mgw.ServiceCentreId,
                                                ServiceCentre = Context.ServiceCentre.Where(c => c.ServiceCentreId == mgw.ServiceCentreId).Select(x => new ServiceCentreDTO
                                                {
                                                    Code = x.Code,
                                                    Name = x.Name
                                                }).FirstOrDefault(),
                                                ManifestDetails = Context.Manifest.Where(x => x.ManifestCode == mgw.ManifestCode).
                                                Select(p => new ManifestDTO
                                                {
                                                    DateCreated = p.DateCreated,
                                                    DateModified = p.DateModified,
                                                    ManifestCode = p.ManifestCode,
                                                    ManifestType = p.ManifestType,
                                                    DateTime = p.DateTime,
                                                    IsDispatched = p.IsDispatched,
                                                    IsReceived = p.IsReceived,
                                                    DispatchedBy = Context.Users.Where(d => d.Id == p.DispatchedById).Select(x => x.LastName + " " + x.FirstName).FirstOrDefault(),
                                                    ReceiverBy = Context.Users.Where(r => r.Id == p.ReceiverById).Select(x => x.LastName + " " + x.FirstName).FirstOrDefault()
                                                }).FirstOrDefault()
                                            };

            return await Task.FromResult(manifestWaybillMappingDTO.OrderByDescending(x => x.DateCreated).ToList());
        }
    }
}
