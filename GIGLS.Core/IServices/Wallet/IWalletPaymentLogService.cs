using GIGLS.Core.DTO.Wallet;
using GIGLS.CORE.DTO.Report;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Wallet
{
    public interface IWalletPaymentLogService : IServiceDependencyMarker
    {
        Task<IEnumerable<WalletPaymentLogDTO>> GetWalletPaymentLogs();
        Task<WalletPaymentLogDTO> GetWalletPaymentLogById(int walletPaymentLogId);
        Task<object> AddWalletPaymentLog(WalletPaymentLogDTO walletPaymentLog);
        Task UpdateWalletPaymentLog(int walletTransactionId, WalletPaymentLogDTO walletPaymentLog);
        Task RemoveWalletPaymentLog(int walletPaymentLogId);
    }
}
