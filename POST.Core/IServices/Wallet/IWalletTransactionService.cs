using POST.Core.DTO;
using POST.Core.DTO.Report;
using POST.Core.DTO.User;
using POST.Core.DTO.Wallet;
using POST.CORE.DTO.Report;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Core.IServices.Wallet
{
    public interface IWalletTransactionService : IServiceDependencyMarker
    {
        Task<IEnumerable<WalletTransactionDTO>> GetWalletTransactions();
        Task<List<WalletTransactionDTO>> GetWalletTransactionsCredit(AccountFilterCriteria accountFilterCriteria);
        Task<WalletTransactionDTO> GetWalletTransactionById(int walletTransactionId);
        Task<WalletTransactionSummaryDTO> GetWalletTransactionByWalletId(int walletId);
        Task<WalletTransactionSummaryDTO> GetWalletTransactionByWalletId(int walletId,PaginationDTO pagination);
        Task<object> AddWalletTransaction(WalletTransactionDTO walletTransaction);
        Task UpdateWalletTransaction(int walletTransactionId, WalletTransactionDTO walletTransaction);
        Task RemoveWalletTransaction(int walletTransactionId);
        Task<WalletTransactionSummaryDTO> GetWalletTransactionsForMobile();
        Task<IEnumerable<WalletTransactionDTO>> GetWalletTransactionsByDate(ShipmentCollectionFilterCriteria dateFilter);
        Task<ModifiedWalletTransactionSummaryDTO> GetWalletTransactionsForMobile(UserDTO customer, ShipmentCollectionFilterCriteria filterCriteria);
        Task<List<WalletTransactionDTO>> GetWalletTransactionsForMobilePaginated(ShipmentAndPreShipmentParamDTO shipmentAndPreShipmentParamDTO);
        Task<List<WalletTransactionDTO>> GetWalletTransactionCreditOrDebit(AccountFilterCriteria accountFilterCriteria);
        Task<IEnumerable<WalletTransactionDTO>> GetWalletTransactionHistoryByDate(ShipmentCollectionFilterCriteria dateFilter);
        Task<WalletCreditTransactionSummaryDTO> GetWalletCreditTransactionHistoryByDate(ShipmentCollectionFilterCriteria dateFilter);
        Task<IEnumerable<ForexTransactionHistoryDTO>> GetForexTransactionHistory(ShipmentCollectionFilterCriteria dateFilter);
    }
}
