using GIGLS.Core.Domain.Wallet;
using GIGLS.Core.IRepositories.Wallet;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.Wallet
{
    public class WalletNumberRepository : Repository<WalletNumber, GIGLSContext>, IWalletNumberRepository
    {
        public WalletNumberRepository(GIGLSContext context) : base(context)
        {
        }

        public Task<WalletNumber> GetLastValidWalletNumber()
        {
            var wallets =
                from walletNumber in Context.WalletNumbers
                orderby walletNumber.WalletPan descending
                select walletNumber;

            return wallets.FirstOrDefaultAsync();
        }
    }
}
