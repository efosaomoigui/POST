using GIGLS.Core.DTO.Wallet;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.CashOnDeliveryBalance
{
    public interface ICashOnDeliveryBalanceService : IServiceDependencyMarker
    {
        Task<IEnumerable<CashOnDeliveryBalanceDTO>> GetCashOnDeliveryBalances();
        Task<CashOnDeliveryBalanceDTO> GetCashOnDeliveryBalanceById(int cashOnDeliveryBalanceId);
        Task<CashOnDeliveryBalanceDTO> GetCashOnDeliveryBalanceByWallet(string walletNumber);
        Task<CashOnDeliveryBalanceDTO> GetCashOnDeliveryBalanceByWalletId(int walletId);
        Task AddCashOnDeliveryBalance(CashOnDeliveryBalanceDTO cashOnDeliveryBalanceDTO);
        Task UpdateCashOnDeliveryBalance(int cashOnDeliveryBalanceId, CashOnDeliveryBalanceDTO cashOnDeliveryBalanceDTO);
        Task RemoveCashOnDeliveryBalance(int cashOnDeliveryBalanceId);
        Task<IEnumerable<CashOnDeliveryBalanceDTO>> GetCashOnDeliveryPaymentSheet();
        Task<IEnumerable<CashOnDeliveryBalanceDTO>> GetPendingCashOnDeliveryPaymentSheet();
        Task<IEnumerable<CashOnDeliveryBalanceDTO>> GetProcessedCashOnDeliveryPaymentSheet();
    }

}
