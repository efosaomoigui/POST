using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.DTO.Wallet;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories.Wallet
{
    public interface ICODWalletRepository : IRepository<Domain.Wallet.CODWallet>
    {
        Task<IEnumerable<CODWalletDTO>> GetCODWalletsAsync();
        IQueryable<Core.Domain.Wallet.CODWallet> GetCODWalletsAsQueryable();
    }
}
