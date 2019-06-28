using GIGLS.Core.IRepositories.Shipments;
using GIGLS.Core.Domain;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;
using GIGLS.Core.DTO.Shipments;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.CORE.DTO.Report;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.Shipments
{
    public class HUBManifestWaybillMappingRepository : Repository<HUBManifestWaybillMapping, GIGLSContext>, IHUBManifestWaybillMappingRepository
    {
        private GIGLSContext _context;
        public HUBManifestWaybillMappingRepository(GIGLSContext context)
            : base(context)
        {
            _context = context;
        }

        public async Task<List<HUBManifestWaybillMappingDTO>> GetHUBManifestWaybillMappings(int[] serviceCentreIds)
        {
            var manifestWaybillMapping = Context.HUBManifestWaybillMapping.AsQueryable();

            var serviceCentreGroupWaybills = new List<string>();
            if (serviceCentreIds.Length > 0)
            {
                manifestWaybillMapping = manifestWaybillMapping.Where(s => serviceCentreIds.Contains(s.ServiceCentreId));
            }

            var manifestWaybillMappingDTO = from mgw in manifestWaybillMapping
                                            select new HUBManifestWaybillMappingDTO
                                            {
                                                HUBManifestWaybillMappingId = mgw.HUBManifestWaybillMappingId,
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
                                                    ReceiverBy = Context.Users.Where(r => r.Id == p.ReceiverById).Select(x => x.LastName + " " + x.FirstName).FirstOrDefault(),
                                                    DepartureServiceCentreId = p.DepartureServiceCentreId,
                                                    DestinationServiceCentreId = p.DestinationServiceCentreId
                                                }).FirstOrDefault()
                                            };

            return await Task.FromResult(manifestWaybillMappingDTO.OrderByDescending(x => x.DateCreated).ToList());
        }


        public async Task<List<HUBManifestWaybillMappingDTO>> GetHUBManifestWaybillMappings(int[] serviceCentreIds, DateFilterCriteria dateFilterCriteria)
        {
            //get startDate and endDate
            var queryDate = dateFilterCriteria.getStartDateAndEndDate();
            var startDate = queryDate.Item1;
            var endDate = queryDate.Item2;

            var manifestWaybillMapping = Context.HUBManifestWaybillMapping.Where(s => s.DateCreated >= startDate && s.DateCreated < endDate).AsQueryable();

            var serviceCentreGroupWaybills = new List<string>();
            if (serviceCentreIds.Length > 0)
            {
                manifestWaybillMapping = manifestWaybillMapping.Where(s => serviceCentreIds.Contains(s.ServiceCentreId));
            }

            var manifestWaybillMappingDTO = from mgw in manifestWaybillMapping
                                            select new HUBManifestWaybillMappingDTO
                                            {
                                                HUBManifestWaybillMappingId = mgw.HUBManifestWaybillMappingId,
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
                                                    ReceiverBy = Context.Users.Where(r => r.Id == p.ReceiverById).Select(x => x.LastName + " " + x.FirstName).FirstOrDefault(),
                                                    DepartureServiceCentreId = p.DepartureServiceCentreId,
                                                    DestinationServiceCentreId = p.DestinationServiceCentreId
                                                }).FirstOrDefault()
                                            };

            return await Task.FromResult(manifestWaybillMappingDTO.OrderByDescending(x => x.DateCreated).ToList());
        }

        public async Task<List<HUBManifestWaybillMappingDTO>> GetHUBManifestWaybillWaitingForSignOff(int[] serviceCentreIds, List<string> manifests)
        {
            var manifestWaybillMapping = Context.HUBManifestWaybillMapping.AsQueryable();

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
                                            select new HUBManifestWaybillMappingDTO
                                            {
                                                HUBManifestWaybillMappingId = mgw.HUBManifestWaybillMappingId,
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
                                                    ReceiverBy = Context.Users.Where(r => r.Id == p.ReceiverById).Select(x => x.LastName + " " + x.FirstName).FirstOrDefault(),
                                                    DepartureServiceCentreId = p.DepartureServiceCentreId,
                                                    DestinationServiceCentreId = p.DestinationServiceCentreId
                                                }).FirstOrDefault(),
                                                DispatchRider = Context.Users.Where(u => u.Id == Context.Dispatch.Where(m => m.ManifestNumber == mgw.ManifestCode).Select(c => c.DriverDetail).FirstOrDefault()).Select(x => x.LastName + " " + x.FirstName).FirstOrDefault()
                                            };

            return await Task.FromResult(manifestWaybillMappingDTO.OrderByDescending(x => x.DateCreated).ToList());
        }
    }
}
