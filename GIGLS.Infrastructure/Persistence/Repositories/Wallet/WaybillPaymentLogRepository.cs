using GIGLS.Core.Domain.Wallet;
using GIGLS.Core.IRepositories.Wallet;
using GIGLS.Infrastructure.Persistence.Repository;

namespace GIGLS.Infrastructure.Persistence.Repositories.Wallet
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
