using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIGLS.Core.Domain;
using GIGLS.Core.IRepositories.Fleets;
using GIGLS.Infrastructure.Persistence.Repository;

namespace GIGLS.Infrastructure.Persistence.Repositories.Fleets
{
    public class FleetDisputeMessageRepository : Repository<FleetDisputeMessage, GIGLSContext>, IFleetDisputeMessageRepository
    {
        private GIGLSContext _context;

        public FleetDisputeMessageRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }
    }
}
