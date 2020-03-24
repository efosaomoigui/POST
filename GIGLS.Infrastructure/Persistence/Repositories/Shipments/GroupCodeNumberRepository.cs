using GIGLS.Core.Domain;
using GIGLS.Core.IRepositories.Shipments;
using GIGLS.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Infrastructure.Persistence.Repositories.Shipments
{
    public class GroupCodeNumberRepository : Repository<GroupCodeNumber, GIGLSContext>, IGroupCodeNumberRepository
    {
        private GIGLSContext _context;

        public GroupCodeNumberRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }

    }
}
