using GIGL.POST.Core.Repositories;
using POST.Core.Domain.Wallet;
using POST.Core.DTO.Report;
using POST.Core.DTO.Wallet;
using POST.CORE.DTO.Report;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Core.IRepositories.Wallet
{
    public interface IWalletTransactionRepository : IRepository<WalletTransaction>
    {
        Task<List<WalletTransactionDTO>> GetWalletTransactionAsync(int[] serviceCentreIds);
        Task<List<WalletTransactionDTO>> GetWalletTransactionCreditAsync(int[] serviceCentreIds, AccountFilterCriteria accountFilterCriteria);
        Task<List<WalletTransactionDTO>> GetWalletTransactionDateAsync(int[] serviceCentreIds, ShipmentCollectionFilterCriteria dateFilter);
        Task<List<ModifiedWalletTransactionDTO>> GetWalletTransactionMobile(int walletId, ShipmentCollectionFilterCriteria filterCriteria);
        Task<WalletTransactionSummary> GetWalletTransactionSummary(DashboardFilterCriteria dashboardFilterCriteria);
        Task<List<WalletTransactionDTO>> GetWalletTransactionCreditOrDebitAsync(int[] serviceCentreIds, AccountFilterCriteria accountFilterCriteria);
        Task<WalletPaymentLogSummary> GetWalletPaymentSummary(DashboardFilterCriteria dashboardFilterCriteria);
        Task<List<WalletTransactionDTO>> GetWalletTransactionHistoryAsync( ShipmentCollectionFilterCriteria dateFilter);
        Task<WalletCreditTransactionSummaryDTO> GetWalletCreditTransactionHistoryAsync(ShipmentCollectionFilterCriteria dateFilter);
        Task<List<ForexTransactionHistoryDTO>> GetForexTransactionHistoryAsync(ShipmentCollectionFilterCriteria dateFilter);
    }
}
