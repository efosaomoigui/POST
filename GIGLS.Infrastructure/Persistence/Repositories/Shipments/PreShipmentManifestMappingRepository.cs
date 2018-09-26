using GIGLS.Core.Domain;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.IRepositories.Shipments;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.Shipments
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
