using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.DTO.Wallet;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories.Wallet
{
    public interface IWalletRepository : IRepository<Domain.Wallet.Wallet>
    {
        Task<IEnumerable<WalletDTO>> GetWalletsAsync();
    }
}
