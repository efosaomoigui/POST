using GIGLS.Core.DTO.Wallet;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.CashOnDeliveryAccount
{
    public interface ICashOnDeliveryAccountService : IServiceDependencyMarker
    {
        Task<IEnumerable<CashOnDeliveryAccountDTO>> GetCashOnDeliveryAccounts();
        Task<CashOnDeliveryAccountDTO> GetCashOnDeliveryAccountById(int cashOnDeliveryAccountId);
        Task<List<CashOnDeliveryAccountDTO>> GetCashOnDeliveryAccountByWallet(string walletNumber);
        Task AddCashOnDeliveryAccount(CashOnDeliveryAccountDTO cashOnDeliveryAccountDto);
        Task UpdateCashOnDeliveryAccount(int cashOnDeliveryAccountId, CashOnDeliveryAccountDTO cashOnDeliveryAccountDto);
        Task RemoveCashOnDeliveryAccount(int cashOnDeliveryAccountId);
    }

}
