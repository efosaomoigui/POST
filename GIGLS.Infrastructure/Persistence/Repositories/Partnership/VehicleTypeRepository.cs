using GIGLS.Core.Domain;
using GIGLS.Core.IRepositories.Partnership;
using GIGLS.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Infrastructure.Persistence.Repositories.Partnership
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
