using GIGLS.Core.Domain.Wallet;
using GIGLS.Core.DTO;
using GIGLS.Core.DTO.Wallet;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Wallet
{
    public interface ICODWalletService : IServiceDependencyMarker
    {
        Task<IEnumerable<WalletDTO>> GetCODWallets();
        Task<WalletDTO> GetCODWalletById(int walletId);
        Task<Domain.Wallet.Wallet> GetCODWalletById(string walletNumber);
        Task AddCODWallet(WalletDTO wallet);
        Task UpdateCODWallet(int walletId, WalletTransactionDTO walletTransactionDTO, bool hasServiceCentre = true);
        Task RemoveCODWallet(int walletId);
    }

}
