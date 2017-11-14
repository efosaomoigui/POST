using GIGLS.Core.Domain.Wallet;
using GIGLS.Core.DTO.Wallet;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Wallet
{
    public interface IWalletTransactionService : IServiceDependencyMarker
    {
        Task<IEnumerable<WalletTransactionDTO>> GetWalletTransaction();
        Task<WalletTransaction> GetWalletTransactionById(int walletTransactionId);
        Task<WalletTransactionSummaryDTO> GetWalletTransactionByWalletId(int walletId);
        Task AddWalletTranaction(WalletTransactionDTO walletTransaction);
        Task UpdateWalletTransaction(int walletTransactionId, WalletTransactionDTO walletTransaction);
        Task RemoveWalletTransaction(int walletTransactionId);
    }
}
