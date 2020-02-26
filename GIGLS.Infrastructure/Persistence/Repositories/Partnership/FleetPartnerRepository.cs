using GIGLS.Core.Domain.Partnership;
using GIGLS.Core.IRepositories.Partnership;
using GIGLS.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Infrastructure.Persistence.Repositories.Partnership
{
    public class FleetPartnerRepository : Repository<FleetPartner, GIGLSContext>, IFleetPartnerRepository
    {
        private GIGLSContext _context;
        public FleetPartnerRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }
    }
}
