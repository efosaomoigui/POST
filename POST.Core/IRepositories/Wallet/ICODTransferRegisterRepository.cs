using GIGL.POST.Core.Repositories;
using POST.Core.DTO.Wallet;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POST.Core.IRepositories.Wallet
{
    public interface ICODTransferRegisterRepository : IRepository<Domain.Wallet.CODTransferRegister>
    {
        //Task<IEnumerable<CODWalletDTO>> GetCODWalletsAsync();
        //IQueryable<Core.Domain.Wallet.CODWallet> GetCODWalletsAsQueryable();
    }
}
