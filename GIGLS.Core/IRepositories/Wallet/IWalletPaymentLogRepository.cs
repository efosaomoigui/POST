using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.Domain.Wallet;
using GIGLS.Core.DTO.Report;
using GIGLS.Core.DTO.Wallet;
using GIGLS.Core.View;
using GIGLS.CORE.DTO.Report;
using GIGLS.CORE.DTO.Shipments;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories.Wallet
{
    public interface IWalletPaymentLogRepository : IRepository<WalletPaymentLog>
    {
        Task<List<WalletPaymentLogDTO>> GetWalletPaymentLogs();
        Tuple<Task<List<WalletPaymentLogView>>, int> GetWalletPaymentLogs(FilterOptionsDto filterOptionsDto, string WalletNumber = null);
        Tuple<Task<List<WalletPaymentLogDTO>>, int> GetWalletPaymentLogs(FilterOptionsDto filterOptionsDto, int walletId);
        Task<List<WalletPaymentLogView>> GetFromWalletPaymentLogView(DateFilterCriteria filterCriteria);
        Task<List<WalletPaymentLogView>> GetFromWalletPaymentLogViewBySearchParameter(string searchItem);
        Task<List<WalletPaymentLogView>> GetWalletPaymentLogBreakdown(DashboardFilterCriteria dashboardFilter);
    }
}
