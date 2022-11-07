using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGL.POST.Core.Domain;
using POST.Core.DTO.Shipments;
using POST.Core.IRepositories.Shipments;
using POST.Infrastructure.Persistence;
using POST.Infrastructure.Persistence.Repository;
using System.Linq;

namespace POST.INFRASTRUCTURE.Persistence.Repositories.Shipments
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
