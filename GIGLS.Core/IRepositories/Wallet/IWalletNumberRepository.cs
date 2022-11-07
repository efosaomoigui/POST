using GIGL.POST.Core.Repositories;
using POST.Core.Domain.Wallet;
using System.Threading.Tasks;

namespace POST.Core.IRepositories.Wallet
{
    public interface IWalletNumberRepository : IRepository<WalletNumber>
    {
        Task<WalletNumber> GetLastValidWalletNumber();
    }
}
