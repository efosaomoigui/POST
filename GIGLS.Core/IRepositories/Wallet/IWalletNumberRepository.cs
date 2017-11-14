using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.Domain.Wallet;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories.Wallet
{
    public interface IWalletNumberRepository : IRepository<WalletNumber>
    {
        Task<WalletNumber> GetLastValidWalletNumber();
    }
}
