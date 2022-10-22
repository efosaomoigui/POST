using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POST.Core.Domain;
using POST.Core.IRepositories.Fleets;
using POST.Infrastructure.Persistence.Repository;

namespace POST.Infrastructure.Persistence.Repositories.Fleets
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
