using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGL.GIGLS.Core.Domain;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.IRepositories.Shipments;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;
using System.Linq;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.Shipments
{
    public class PickupManifestRepository : Repository<PickupManifest, GIGLSContext>, IPickupManifestRepository
    {
        private GIGLSContext _context;
        public PickupManifestRepository(GIGLSContext context)
            : base(context)
        {
            _context = context;
        }

        
    }
}
