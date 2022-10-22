using POST.Core.DTO.OnlinePayment;
using POST.Core.DTO.Wallet;
using POST.Core.View;
using POST.CORE.DTO.Report;
using POST.CORE.DTO.Shipments;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Core.IServices.Wallet
{
    public interface IWalletPaymentLogService : IServiceDependencyMarker
    {
        Task<IEnumerable<WalletPaymentLogDTO>> GetWalletPaymentLogs();
        Task<WalletPaymentLogDTO> GetWalletPaymentLogById(int walletPaymentLogId);
        Task<object> AddWalletPaymentLog(WalletPaymentLogDTO walletPaymentLog);
        Task<object> AddWalletPaymentLogAnonymousUser(WalletPaymentLogDTO walletPaymentLogDto);
        Task<USSDResponse> InitiatePaymentUsingUSSD(WalletPaymentLogDTO walletPaymentLogDto);
        Task UpdateWalletPaymentLog(string reference, WalletPaymentLogDTO walletPaymentLog);
        Task RemoveWalletPaymentLog(int walletPaymentLogId);
        Task PaystackPaymentService(WalletPaymentLogDTO WalletPaymentInfo);
        Tuple<Task<List<WalletPaymentLogView>>, int> GetWalletPaymentLogs(FilterOptionsDto filterOptionsDto);
        Task AddWalletPaymentLogMobile(WalletPaymentLogDTO walletPaymentLogDto);
        Task<List<WalletPaymentLogView>> GetWalletPaymentLogs(DateFilterCriteria baseFilter);
        Task<List<WalletPaymentLogView>> GetFromWalletPaymentLogViewBySearchParameter(string searchItem);
        Task<PaymentResponse> VerifyAndValidatePayment(string referenceCode);
    }
}
