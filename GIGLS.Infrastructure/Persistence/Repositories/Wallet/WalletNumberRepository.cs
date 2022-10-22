using POST.Core.Domain.Wallet;
using POST.Core.IRepositories.Wallet;
using POST.Infrastructure.Persistence;
using POST.Infrastructure.Persistence.Repository;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace POST.INFRASTRUCTURE.Persistence.Repositories.Wallet
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
