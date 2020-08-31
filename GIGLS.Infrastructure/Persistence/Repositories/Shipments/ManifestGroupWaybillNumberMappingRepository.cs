using GIGLS.Core.IRepositories.Shipments;
using GIGLS.Core.Domain;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;
using GIGLS.Core.DTO.Shipments;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.CORE.DTO.Report;
using GIGL.GIGLS.Core.Domain;
using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.Core.DTO.Fleets;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.Shipments
{
    public class ManifestGroupWaybillNumberMappingRepository : Repository<ManifestGroupWaybillNumberMapping, GIGLSContext>, IManifestGroupWaybillNumberMappingRepository
    {
        private GIGLSContext _context;
        public ManifestGroupWaybillNumberMappingRepository(GIGLSContext context)
            : base(context)
        {
            _context = context;
        }

        public async Task<List<ManifestGroupWaybillNumberMappingDTO>> GetManifestGroupWaybillNumberMappings(int[] serviceCentreIds)
        {
            var manifestGroupwaybillMapping = Context.ManifestGroupWaybillNumberMapping.Where(s => s.IsDeleted == false).AsQueryable();

            if (serviceCentreIds.Length > 0)
            {
                var serviceCentreGroupWaybills = _context.GroupWaybillNumberMapping.Where(s => serviceCentreIds.Contains(s.DepartureServiceCentreId)).
                    Select(s => s.GroupWaybillNumber).AsQueryable();

                //manifestGroupwaybillMapping = manifestGroupwaybillMapping.Where(s => serviceCentreGroupWaybills.Contains(s.GroupWaybillNumber));
                manifestGroupwaybillMapping = manifestGroupwaybillMapping.Where(s => serviceCentreGroupWaybills.Any(y => s.GroupWaybillNumber == y));
            }

            var manifestGroupwaybillMappingDTO = from mgw in manifestGroupwaybillMapping
                                                 select new ManifestGroupWaybillNumberMappingDTO
                                                 {
                                                     ManifestGroupWaybillNumberMappingId = mgw.ManifestGroupWaybillNumberMappingId,
                                                     ManifestCode = mgw.ManifestCode,
                                                     GroupWaybillNumber = mgw.GroupWaybillNumber,
                                                     IsActive = mgw.IsActive,
                                                     DateMapped = mgw.DateMapped,
                                                     DateCreated = mgw.DateCreated,
                                                     DateModified = mgw.DateModified,
                                                     IsDeleted = mgw.IsDeleted,
                                                     RowVersion = mgw.RowVersion,
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

            return await Task.FromResult(manifestGroupwaybillMappingDTO.OrderByDescending(x => x.DateCreated).ToList());
        }

        public async Task<ManifestGroupWaybillNumberMappingDTO> GetManifestGroupWaybillNumberMappingsUsingGroupWaybill(string groupWaybill)
        {
            var manifestGroupwaybillMapping = Context.ManifestGroupWaybillNumberMapping.Where(s => s.GroupWaybillNumber == groupWaybill).AsQueryable();

            var manifestGroupwaybillMappingDTO = from mgw in manifestGroupwaybillMapping
                                                 select new ManifestGroupWaybillNumberMappingDTO
                                                 {
                                                     ManifestGroupWaybillNumberMappingId = mgw.ManifestGroupWaybillNumberMappingId,
                                                     ManifestCode = mgw.ManifestCode,
                                                     GroupWaybillNumber = mgw.GroupWaybillNumber,
                                                     IsActive = mgw.IsActive,
                                                     DateMapped = mgw.DateMapped,
                                                     DateCreated = mgw.DateCreated,
                                                     DateModified = mgw.DateModified,
                                                     IsDeleted = mgw.IsDeleted,
                                                     RowVersion = mgw.RowVersion,
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

            return await Task.FromResult(manifestGroupwaybillMappingDTO.FirstOrDefault());
        }

        public async Task<List<ManifestGroupWaybillNumberMappingDTO>> GetManifestGroupWaybillNumberMappings(int[] serviceCentreIds, DateFilterCriteria dateFilterCriteria)
        {
            //get startDate and endDate
            var queryDate = dateFilterCriteria.getStartDateAndEndDate();
            var startDate = queryDate.Item1;
            var endDate = queryDate.Item2;

            var manifestGroupwaybillMapping = Context.ManifestGroupWaybillNumberMapping.Where(s => s.IsDeleted == false && s.DateCreated >= startDate && s.DateCreated < endDate).AsQueryable();

            if (serviceCentreIds.Length > 0)
            {
                var serviceCentreGroupWaybills = _context.GroupWaybillNumberMapping.Where(s => serviceCentreIds.Contains(s.DepartureServiceCentreId)).
                    Select(s => s.GroupWaybillNumber).AsQueryable();

                manifestGroupwaybillMapping = manifestGroupwaybillMapping.Where(s => serviceCentreGroupWaybills.Contains(s.GroupWaybillNumber));
            }

            var manifestGroupwaybillMappingDTO = from mgw in manifestGroupwaybillMapping
                                                 join s in _context.Manifest on mgw.ManifestCode equals s.ManifestCode
                                                 where s.HasSuperManifest == false
                                                 select new ManifestGroupWaybillNumberMappingDTO
                                                 {
                                                     ManifestGroupWaybillNumberMappingId = mgw.ManifestGroupWaybillNumberMappingId,
                                                     ManifestCode = mgw.ManifestCode,
                                                     GroupWaybillNumber = mgw.GroupWaybillNumber,
                                                     IsActive = mgw.IsActive,
                                                     DateMapped = mgw.DateMapped,
                                                     DateCreated = mgw.DateCreated,
                                                     DateModified = mgw.DateModified,
                                                     IsDeleted = mgw.IsDeleted,
                                                     RowVersion = mgw.RowVersion,
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
                                                         HasSuperManifest = p.HasSuperManifest,
                                                         SuperManifestCode = p.SuperManifestCode,
                                                         SuperManifestStatus = p.SuperManifestStatus,
                                                         DispatchedBy = Context.Users.Where(d => d.Id == p.DispatchedById).Select(x => x.LastName + " " + x.FirstName).FirstOrDefault(),
                                                         ReceiverBy = Context.Users.Where(r => r.Id == p.ReceiverById).Select(x => x.LastName + " " + x.FirstName).FirstOrDefault()
                                                     }).FirstOrDefault()
                                                 };

            return await Task.FromResult(manifestGroupwaybillMappingDTO.OrderByDescending(x => x.DateCreated).ToList());
        }

        public async Task<List<ManifestDTO>> GetManifestSuperManifestMappings(int[] serviceCentreIds, DateFilterCriteria dateFilterCriteria)
        {
            //get startDate and endDate
            var queryDate = dateFilterCriteria.getStartDateAndEndDate();
            var startDate = queryDate.Item1;
            var endDate = queryDate.Item2;

            var manifestSuperManifestMapping = Context.Manifest.Where(s => s.IsDeleted == false && s.HasSuperManifest == true && s.DateModified >= startDate && s.DateModified < endDate).AsQueryable();

            if (serviceCentreIds.Length > 0)
            {
                var serviceCentreManifests = _context.Manifest.Where(s => serviceCentreIds.Contains(s.DepartureServiceCentreId)).
                    Select(s => s.ManifestCode).AsQueryable();

                manifestSuperManifestMapping = manifestSuperManifestMapping.Where(s => serviceCentreManifests.Contains(s.ManifestCode));
            }

            var manifestSuperManifestMappingDTO = from mgw in manifestSuperManifestMapping
                                                  select new ManifestDTO
                                                  {
                                                      ManifestCode = mgw.ManifestCode,
                                                      SuperManifestCode = mgw.SuperManifestCode,
                                                      SuperManifestStatus = mgw.SuperManifestStatus,
                                                      ManifestType = mgw.ManifestType,
                                                      DateCreated = mgw.DateCreated,
                                                      DateModified = mgw.DateModified,
                                                      IsDeleted = mgw.IsDeleted,
                                                      RowVersion = mgw.RowVersion,
                                                      IsDispatched = mgw.IsDispatched,
                                                      IsReceived = mgw.IsReceived,
                                                      DispatchedBy = Context.Users.Where(d => d.Id == mgw.DispatchedById).Select(x => x.LastName + " " + x.FirstName).FirstOrDefault(),
                                                      ReceiverBy = Context.Users.Where(r => r.Id == mgw.ReceiverById).Select(x => x.LastName + " " + x.FirstName).FirstOrDefault()
                                                  };

            return await Task.FromResult(manifestSuperManifestMappingDTO.OrderByDescending(x => x.DateModified).ToList());
        }

        public async Task<List<ManifestDTO>> GetManifestGroupWaybillNumberMappingsForSuperManifest(int[] serviceCentreIds)
        {

            var manifestGroupwaybillMapping = Context.ManifestGroupWaybillNumberMapping.Where(s => s.IsDeleted == false).AsQueryable();

            if (serviceCentreIds.Length > 0)
            {
                var serviceCentreGroupWaybills = _context.GroupWaybillNumberMapping.Where(s => serviceCentreIds.Contains(s.DepartureServiceCentreId)).
                    Select(s => s.GroupWaybillNumber).AsQueryable();

                manifestGroupwaybillMapping = manifestGroupwaybillMapping.Where(s => serviceCentreGroupWaybills.Contains(s.GroupWaybillNumber));
            }


            var manifest = from mgw in manifestGroupwaybillMapping
                           join p in _context.Manifest on mgw.ManifestCode equals p.ManifestCode
                           join s in _context.GroupWaybillNumberMapping on mgw.GroupWaybillNumber equals s.GroupWaybillNumber
                           where p.SuperManifestStatus == Core.Enums.SuperManifestStatus.ArrivedScan || p.SuperManifestStatus == Core.Enums.SuperManifestStatus.Pending
                           select new ManifestDTO
                           {
                               ManifestCode = p.ManifestCode,
                               DateCreated = p.DateCreated,
                               DateModified = p.DateModified,
                               ManifestType = p.ManifestType,
                               DateTime = p.DateTime,
                               IsDispatched = p.IsDispatched,
                               IsReceived = p.IsReceived,
                               HasSuperManifest = p.HasSuperManifest,
                               SuperManifestStatus = p.SuperManifestStatus,
                               IsDeleted = p.IsDeleted,
                               SuperManifestCode = p.SuperManifestCode,
                               DestinationServiceCentre = Context.ServiceCentre.Where(c => c.ServiceCentreId == s.DestinationServiceCentreId).Select(x => new ServiceCentreDTO
                               {
                                   Code = x.Code,
                                   Name = x.Name,
                                 StationId = x.StationId,
                                 StationName = x.Station.StationName
                               }).FirstOrDefault(),
                               DepartureServiceCentre = Context.ServiceCentre.Where(c => c.ServiceCentreId == s.DestinationServiceCentreId).Select(x => new ServiceCentreDTO
                               {
                                   Code = x.Code,
                                   Name = x.Name,
                                   StationId = x.StationId,
                                   StationName = x.Station.StationName
                               }).FirstOrDefault(),

                           };

            return await Task.FromResult(manifest.OrderByDescending(x => x.DateCreated).ToList());
        }

        public async Task<List<ManifestDTO>> GetManifestAvailableForSuperManifest(int[] serviceCentreIds)
        {

            var manifestMapping = Context.Manifest.Where(s => s.IsDeleted == false && s.HasSuperManifest == false && s.SuperManifestStatus == Core.Enums.SuperManifestStatus.ArrivedScan).AsQueryable();

            if (serviceCentreIds.Length > 0)
            {
                manifestMapping = manifestMapping.Where(s => serviceCentreIds.Contains(s.DepartureServiceCentreId));
            }

            var manifest = from p in manifestMapping
                           select new ManifestDTO
                           {
                               ManifestCode = p.ManifestCode,
                               DateCreated = p.DateCreated,
                               DateModified = p.DateModified,
                               ManifestType = p.ManifestType,
                               DateTime = p.DateTime,
                               IsDispatched = p.IsDispatched,
                               IsReceived = p.IsReceived,
                               HasSuperManifest = p.HasSuperManifest,
                               SuperManifestStatus = p.SuperManifestStatus,
                               IsDeleted = p.IsDeleted,
                               SuperManifestCode = p.SuperManifestCode,
                               DestinationServiceCentre = Context.Dispatch.Where(c => c.ManifestNumber == p.ManifestCode).Select(x => new ServiceCentreDTO
                               {
                                   Code = x.Destination.StationCode,
                                   Name = Context.ServiceCentre.Where(d => d.ServiceCentreId == x.DestinationServiceCenterId).Select(y => y.Name).FirstOrDefault(),
                                   StationId = x.Destination.StationId,
                                   StationName = x.Destination.StationName
                               }).FirstOrDefault(),
                           };

            return await Task.FromResult(manifest.OrderByDescending(x => x.DateCreated).ToList());
        }
    }
}
