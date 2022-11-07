using GIGL.POST.Core.Repositories;
using POST.Core.Domain;
using POST.Core.Domain.Wallet;
using POST.Core.DTO;
using POST.Core.DTO.Report;
using POST.Core.DTO.Wallet;
using POST.Core.View;
using POST.CORE.DTO.Report;
using POST.CORE.DTO.Shipments;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Core.IRepositories.Wallet
{
    public interface IWalletPaymentLogRepository : IRepository<WalletPaymentLog>
    {
        Task<List<WalletPaymentLogDTO>> GetWalletPaymentLogs();
        Tuple<Task<List<WalletPaymentLogView>>, int> GetWalletPaymentLogs(FilterOptionsDto filterOptionsDto, string WalletNumber = null);
        Tuple<Task<List<WalletPaymentLogDTO>>, int> GetWalletPaymentLogs(FilterOptionsDto filterOptionsDto, int walletId);
        Task<List<WalletPaymentLogView>> GetFromWalletPaymentLogView(DateFilterCriteria filterCriteria);
        Task<List<WalletPaymentLogView>> GetFromWalletPaymentLogViewBySearchParameter(string searchItem);
        Task<List<WalletPaymentLogView>> GetWalletPaymentLogBreakdown(DashboardFilterCriteria dashboardFilter);
        Task LogContentType(LogEntry payload);
    }
}
