using POST.Core.IRepositories.Shipments;
using POST.Core.Domain;
using POST.Infrastructure.Persistence;
using POST.Infrastructure.Persistence.Repository;
using POST.Core.DTO.Shipments;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using POST.Core.DTO.ServiceCentres;
using POST.CORE.DTO.Report;

namespace POST.INFRASTRUCTURE.Persistence.Repositories.Shipments
{
    public class PickupManifestWaybillMappingRepository : Repository<PickupManifestWaybillMapping, GIGLSContext>, IPickupManifestWaybillMappingRepository
    {
        private GIGLSContext _context;
        public PickupManifestWaybillMappingRepository(GIGLSContext context)
            : base(context)
        {
            _context = context;
        }

        public async Task<List<PickupManifestWaybillMappingDTO>> GetPickupManifestWaybillMapping (DateFilterCriteria dateFilterCriteria)
        {
            //get startDate and endDate
            var queryDate = dateFilterCriteria.getStartDateAndEndDate();
            var startDate = queryDate.Item1;
            var endDate = queryDate.Item2;

            var pickupmanifestWaybillMapping = Context.PickupManifestWaybillMapping.Where(s => s.DateCreated >= startDate && s.DateCreated < endDate).AsQueryable();

            var pickupmanifestWaybillMappingDTO = from mgw in pickupmanifestWaybillMapping
                                                  select new PickupManifestWaybillMappingDTO
                                                  {
                                                      PickupManifestWaybillMappingId = mgw.PickupManifestWaybillMappingId,
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
                                                      PickupManifestDetails = Context.PickupManifest.Where(x => x.ManifestCode == mgw.ManifestCode).
                                                      Select(p => new PickupManifestDTO
                                                      {
                                                          DateCreated = p.DateCreated,
                                                          DateModified = p.DateModified,
                                                          ManifestCode = p.ManifestCode,
                                                          ManifestType = p.ManifestType,
                                                          ManifestStatus = p.ManifestStatus,
                                                          DateTime = p.DateTime,
                                                          IsDispatched = p.IsDispatched,
                                                          IsReceived = p.IsReceived,
                                                          DispatchedBy = Context.Users.Where(d => d.Id == p.DispatchedById).Select(x => x.LastName + " " + x.FirstName).FirstOrDefault(),
                                                          ReceiverBy = Context.Users.Where(d => d.Id == p.ReceiverById).Select(x => x.LastName + " " + x.FirstName).FirstOrDefault(),
                                                      }).FirstOrDefault()
                                                  };
            return await Task.FromResult(pickupmanifestWaybillMappingDTO.OrderByDescending(x => x.DateCreated).ToList());
        }
    }
}