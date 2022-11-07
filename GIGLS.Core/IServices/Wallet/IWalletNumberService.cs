using POST.Core.DTO.Wallet;
using System.Threading.Tasks;

namespace POST.Core.IServices.Wallet
{
    public interface IWalletNumberService
    {
        Task AddWalletNumber(WalletNumberDTO wallet);
        Task UpdateWalletNumber(int walletId, WalletNumberDTO wallet);
        Task RemoveWalletNumber(int walletId);
    }
}
