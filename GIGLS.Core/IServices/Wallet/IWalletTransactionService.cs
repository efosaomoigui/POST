using GIGLS.Core.DTO.Report;
using GIGLS.Core.DTO.User;
using GIGLS.Core.DTO.Wallet;
using GIGLS.CORE.DTO.Report;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Wallet
{
    public interface IWalletTransactionService : IServiceDependencyMarker
    {
        Task<IEnumerable<WalletTransactionDTO>> GetWalletTransactions();
        Task<List<WalletTransactionDTO>> GetWalletTransactionsCredit(AccountFilterCriteria accountFilterCriteria);
        Task<WalletTransactionDTO> GetWalletTransactionById(int walletTransactionId);
        Task<WalletTransactionSummaryDTO> GetWalletTransactionByWalletId(int walletId);
        Task<object> AddWalletTransaction(WalletTransactionDTO walletTransaction);
        Task UpdateWalletTransaction(int walletTransactionId, WalletTransactionDTO walletTransaction);
        Task RemoveWalletTransaction(int walletTransactionId);
        Task<WalletTransactionSummaryDTO> GetWalletTransactionsForMobile();
        Task<IEnumerable<WalletTransactionDTO>> GetWalletTransactionsByDate(ShipmentCollectionFilterCriteria dateFilter);
        Task<WalletTransactionSummaryDTO> GetWalletTransactionsForMobile(UserDTO customer, ShipmentCollectionFilterCriteria filterCriteria);
    }
}
