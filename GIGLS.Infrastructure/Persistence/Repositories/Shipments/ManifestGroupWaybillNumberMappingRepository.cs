using GIGLS.Core.Domain;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.IRepositories.Shipments;
using GIGLS.CORE.DTO.Report;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                                                         ReceiverBy = Context.Users.Where(r => r.Id == p.ReceiverById).Select(x => x.LastName + " " + x.FirstName).FirstOrDefault(),
                                                         CargoStatus = p.CargoStatus
                                                     }).FirstOrDefault()
                                                 };

            return await Task.FromResult(manifestGroupwaybillMappingDTO.OrderByDescending(x => x.DateCreated).ToList());
        }

        public async Task<List<MovementManifestNumberDTO>> GetManifestMovementNumberMappings(int[] serviceCentreIds, DateFilterCriteria dateFilterCriteria)
        {
            //get startDate and endDate
            var queryDate = dateFilterCriteria.getStartDateAndEndDate();
            var startDate = queryDate.Item1;
            var endDate = queryDate.Item2;

            var movementManifestNumber = Context.MovementManifestNumber.Where(s => s.IsDeleted == false && s.DateCreated >= startDate && s.DateCreated < endDate).AsQueryable();

            if (serviceCentreIds.Length > 0)
            {
                var serviceCentreMovementManifests = _context.MovementManifestNumber.Where(s => serviceCentreIds.Contains(s.DepartureServiceCentreId)).
                    Select(s => s.MovementManifestCode).AsQueryable();

                movementManifestNumber = movementManifestNumber.Where(s => serviceCentreMovementManifests.Contains(s.MovementManifestCode));
            }

            var movementManifestNumberVals = movementManifestNumber.ToList();
            var allServiceCenters = _context.ServiceCentre.AsQueryable();

            var movementManifestNumberDto = from mgw in movementManifestNumber
                                            select new MovementManifestNumberDTO
                                            {
                                                MovementManifestNumberId = mgw.MovementManifestNumberId,
                                                MovementManifestCode = mgw.MovementManifestCode,
                                                DateCreated = mgw.DateCreated,
                                                DateModified = mgw.DateModified,
                                                IsDeleted = mgw.IsDeleted,
                                                MovementStatus = mgw.MovementStatus,
                                                DestinationServiceCentre = allServiceCenters.Where(x => x.ServiceCentreId == mgw.DepartureServiceCentreId).FirstOrDefault(),
                                                DriverCode = mgw.DriverCode,
                                                DestinationServiceCentreCode = mgw.DestinationServiceCentreCode
                                            };

            return movementManifestNumberDto.ToList();
        }

        public async Task<List<MovementManifestNumberDTO>> GetExpectedManifestMovementNumberMappings(int[] serviceCentreIds, DateFilterCriteria dateFilterCriteria)
        {
            //get startDate and endDate
            var queryDate = dateFilterCriteria.getStartDateAndEndDate();
            var startDate = queryDate.Item1;
            var endDate = queryDate.Item2;

            var movementDispatches = Context.MovementDispatch.Where(s => s.IsDeleted == false && s.DateCreated >= startDate && s.DateCreated < endDate).AsQueryable();
            var serviceCentreMovementManifests2 = new List<MovementManifestNumber>();
            var serviceCentreMovementManifests = Enumerable.Empty <MovementManifestNumber> ().AsQueryable(); ;

            if (serviceCentreIds.Length > 0)
            {
                var serviceCentreMovementDispatch = _context.MovementDispatch.Where(s => serviceCentreIds.Contains(s.DestinationServiceCenterId)).
                Select(s => s.MovementManifestNumber).AsQueryable();

                movementDispatches = movementDispatches.Where(s => serviceCentreMovementDispatch.Contains(s.MovementManifestNumber));
                var dispatchedNumbers = movementDispatches.Select(s => s.MovementManifestNumber).ToList();

                serviceCentreMovementManifests = _context.MovementManifestNumber.Where(s => dispatchedNumbers.Contains(s.MovementManifestCode)).AsQueryable();
            }

            var movementManifestNumberVals = serviceCentreMovementManifests.ToList();
            var movementDispatchedVals = movementDispatches.ToList();

            var allServiceCenters = _context.ServiceCentre.AsQueryable();

            var movementManifestNumberDto = from mgw in movementManifestNumberVals
                                            join md in movementDispatches on mgw.MovementManifestCode equals md.MovementManifestNumber
                                            select new MovementManifestNumberDTO
                                            {
                                                MovementManifestNumberId = mgw.MovementManifestNumberId,
                                                MovementManifestCode = mgw.MovementManifestCode,
                                                DateCreated = mgw.DateCreated,
                                                DateModified = mgw.DateModified,
                                                IsDeleted = mgw.IsDeleted,
                                                MovementStatus = mgw.MovementStatus,
                                                DestinationServiceCentre = allServiceCenters.Where(x => x.ServiceCentreId == mgw.DepartureServiceCentreId).FirstOrDefault(),
                                                DriverCode = mgw.DriverCode,
                                                DestinationServiceCentreCode = mgw.DestinationServiceCentreCode
                                            };

            return movementManifestNumberDto.ToList();
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
                                                      IsDispatched = mgw.IsDispatched,
                                                      IsReceived = mgw.IsReceived,
                                                      DispatchedBy = Context.Users.Where(d => d.Id == mgw.DispatchedById).Select(x => x.LastName + " " + x.FirstName).FirstOrDefault(),
                                                      ReceiverBy = Context.Users.Where(r => r.Id == mgw.ReceiverById).Select(x => x.LastName + " " + x.FirstName).FirstOrDefault()
                                                  };

            return await Task.FromResult(manifestSuperManifestMappingDTO.OrderByDescending(x => x.DateModified).ToList());
        }

    }
}
