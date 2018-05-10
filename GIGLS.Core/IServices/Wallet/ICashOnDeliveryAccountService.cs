using GIGLS.Core.DTO.Wallet;
using GIGLS.Core.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.CashOnDeliveryAccount
{
    public interface ICashOnDeliveryAccountService : IServiceDependencyMarker
    {
        Task<IEnumerable<CashOnDeliveryAccountDTO>> GetCashOnDeliveryAccounts();
        Task<CashOnDeliveryAccountDTO> GetCashOnDeliveryAccountById(int cashOnDeliveryAccountId);
        Task<CashOnDeliveryAccountSummaryDTO> GetCashOnDeliveryAccountByWallet(string walletNumber);
        Task AddCashOnDeliveryAccount(CashOnDeliveryAccountDTO cashOnDeliveryAccountDto);
        Task UpdateCashOnDeliveryAccount(int cashOnDeliveryAccountId, CashOnDeliveryAccountDTO cashOnDeliveryAccountDto);
        Task RemoveCashOnDeliveryAccount(int cashOnDeliveryAccountId);
        Task ProcessCashOnDeliveryPaymentSheet(List<CashOnDeliveryBalanceDTO> data);
        Task ProcessToPending(List<CashOnDeliveryBalanceDTO> data);
        Task<IEnumerable<CashOnDeliveryAccountDTO>> GetCashOnDeliveryAccounts(CODStatus cODStatus);
    }

}
