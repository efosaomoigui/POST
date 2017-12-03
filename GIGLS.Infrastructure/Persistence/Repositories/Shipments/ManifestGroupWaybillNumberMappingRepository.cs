using GIGLS.Core.IRepositories.Shipments;
using GIGLS.Core.Domain;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;
using GIGLS.Core.DTO.Shipments;
using System.Linq;
using System.Collections.Generic;
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
            var manifestGroupwaybillMapping = Context.ManifestGroupWaybillNumberMapping.AsQueryable();

            var serviceCentreGroupWaybills = new List<string>();
            if (serviceCentreIds.Length > 0)
            {
                serviceCentreGroupWaybills = _context.GroupWaybillNumberMapping.Where(s => serviceCentreIds.Contains(s.DepartureServiceCentreId)).
                    Select(s => s.GroupWaybillNumber).ToList();
            }


            manifestGroupwaybillMapping = manifestGroupwaybillMapping.Where(s => serviceCentreGroupWaybills.Contains(s.GroupWaybillNumber));

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
                                                     RowVersion = mgw.RowVersion
                                                 };

            return await Task.FromResult(manifestGroupwaybillMappingDTO.ToList());
        }

    }
}
