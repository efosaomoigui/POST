using GIGLS.Core.Domain.Wallet;
using GIGLS.Core.DTO.Wallet;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Wallet
{
    public interface IWalletService : IServiceDependencyMarker
    {
        Task<IEnumerable<WalletDTO>> GetWallets();
        Task<WalletDTO> GetWalletById(int walletId);
        Task<Domain.Wallet.Wallet> GetWalletById(string walletNumber);
        Task AddWallet(WalletDTO wallet);
        Task UpdateWallet(int walletId, WalletTransactionDTO walletTransactionDTO, bool hasServiceCentre = true);
        Task RemoveWallet(int walletId);
        Task<WalletNumber> GenerateNextValidWalletNumber();
        Task<WalletDTO> GetSystemWallet();
        Task<List<WalletDTO>> SearchForWallets(WalletSearchOption searchOption);

        Task<WalletDTO> GetWalletByCustomerCode(string CustomerCode);
        Task<WalletDTO> GetWalletByUserId(string Userid);

    }

}
