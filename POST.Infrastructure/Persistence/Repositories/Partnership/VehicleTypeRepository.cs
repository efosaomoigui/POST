using POST.Core.Domain;
using POST.Core.IRepositories.Partnership;
using POST.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POST.Infrastructure.Persistence.Repositories.Partnership
{
    public class VehicleTypeRepository : Repository<VehicleType, GIGLSContext>, IVehicleTypeRepository
    {
        private GIGLSContext _context;
        public VehicleTypeRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }

    }
}
