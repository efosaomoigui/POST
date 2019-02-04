using GIGLS.Core.DTO.Wallet;
using GIGLS.Core.View;
using GIGLS.CORE.DTO.Shipments;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Wallet
{
    public interface IWalletPaymentLogService : IServiceDependencyMarker
    {
        Task<IEnumerable<WalletPaymentLogDTO>> GetWalletPaymentLogs();
        Task<WalletPaymentLogDTO> GetWalletPaymentLogById(int walletPaymentLogId);
        Task<object> AddWalletPaymentLog(WalletPaymentLogDTO walletPaymentLog);
        Task UpdateWalletPaymentLog(string reference, WalletPaymentLogDTO walletPaymentLog);
        Task RemoveWalletPaymentLog(int walletPaymentLogId);
        Task PaystackPaymentService(WalletPaymentLogDTO WalletPaymentInfo);
        Tuple<Task<List<WalletPaymentLogView>>, int> GetWalletPaymentLogs(FilterOptionsDto filterOptionsDto);
    }
}
