using GIGLS.Core.Domain;
using GIGLS.Core.IRepositories.Stores;
using GIGLS.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Infrastructure.Persistence.Repositories.Stores
{
    public class ShipmentPackageInflowRepository : Repository<ShipmentPackageInflow, GIGLSContext>, IShipmentPackageInflowRepository
    {
        private GIGLSContext _context;

        public ShipmentPackageInflowRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }
    }
}
