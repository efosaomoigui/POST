using GIGLS.Core.IRepositories.Shipments;
using GIGLS.Core.IRepositories.Fleets;
using GIGLS.Core.Domain;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.DTO.Fleets;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.CORE.DTO.Report;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.Shipments
{
    public class PickupManifestWaybillMappingRepository : Repository<PickupManifestWaybillMapping, GIGLSContext>, IPickupManifestWaybillMappingRepository
    {
        private GIGLSContext _context;
        public PickupManifestWaybillMappingRepository(GIGLSContext context)
            : base(context)
        {
            _context = context;
        }
    }
}
