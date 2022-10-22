using POST.Core.Domain.Wallet;
using POST.Core.IRepositories.Wallet;
using POST.Infrastructure.Persistence.Repository;

namespace POST.Infrastructure.Persistence.Repositories.Wallet
{
    public class WaybillPaymentLogRepository : Repository<WaybillPaymentLog, GIGLSContext>, IWaybillPaymentLogRepository
    {
        private GIGLSContext _context;

        public WaybillPaymentLogRepository( GIGLSContext context) : base(context)
        {
            _context = context;
        }

    }
}
