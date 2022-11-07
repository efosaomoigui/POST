using POST.Core.Domain;
using POST.Core.DTO.Account;
using POST.Core.IRepositories.Account;
using POST.Infrastructure.Persistence;
using POST.Infrastructure.Persistence.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using POST.Core.DTO;

namespace POST.INFRASTRUCTURE.Persistence.Repositories.Account
{
    public class WaybillChargeRepository : Repository<WaybillCharge, GIGLSContext>, IWaybillChargeRepository
    {
        private GIGLSContext _context;

        public WaybillChargeRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }

    }
}