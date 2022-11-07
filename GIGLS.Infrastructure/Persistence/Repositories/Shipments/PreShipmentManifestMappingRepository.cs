using POST.Core.Domain;
using POST.Core.DTO.Shipments;
using POST.Core.IRepositories.Shipments;
using POST.Infrastructure.Persistence;
using POST.Infrastructure.Persistence.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POST.INFRASTRUCTURE.Persistence.Repositories.Shipments
{
    public class PreShipmentManifestMappingRepository : Repository<PreShipmentManifestMapping, GIGLSContext>, IPreShipmentManifestMappingRepository
    {
        private GIGLSContext _context;
        public PreShipmentManifestMappingRepository(GIGLSContext context)
            : base(context)
        {
            _context = context;
        }
        
        public async Task<List<PreShipmentManifestMappingDTO>> GetManifestWaybillMappings()
        {
            var manifestWaybillMapping = Context.PreShipmentManifestMapping.AsQueryable();

            var manifestWaybillMappingDTO = from mgw in manifestWaybillMapping
                                                 select new PreShipmentManifestMappingDTO
                                                 {
                                                     PreShipmentManifestMappingId = mgw.PreShipmentManifestMappingId,
                                                     ManifestCode = mgw.ManifestCode,
                                                     IsActive = mgw.IsActive,
                                                     DateCreated = mgw.DateCreated,
                                                     DateModified = mgw.DateModified,
                                                     Waybill = mgw.Waybill,
                                                     DispatchedBy = mgw.DispatchedBy,
                                                     DriverDetail = mgw.DriverDetail,
                                                     ReceivedBy = mgw.ReceivedBy,
                                                     RegistrationNumber = mgw.RegistrationNumber
                                                 };

            return await Task.FromResult(manifestWaybillMappingDTO.OrderByDescending(x => x.DateCreated).ToList());
        }
    }
}
