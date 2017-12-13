using GIGLS.Core.IServices.CashOnDeliveryAccount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Wallet;

namespace GIGLS.Services.Implementation.Wallet
{
    public class CashOnDeliveryAccountService : ICashOnDeliveryAccountService
    {
        public Task AddCashOnDeliveryAccount(CashOnDeliveryAccountDTO cashOnDeliveryAccountDto)
        {
            throw new NotImplementedException();
        }

        public Task<CashOnDeliveryAccountDTO> GetCashOnDeliveryAccountById(int cashOnDeliveryAccountId)
        {
            throw new NotImplementedException();
        }

        public Task<List<CashOnDeliveryAccountDTO>> GetCashOnDeliveryAccountByWallet(string walletNumber)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CashOnDeliveryAccountDTO>> GetCashOnDeliveryAccounts()
        {
            throw new NotImplementedException();
        }

        public Task RemoveCashOnDeliveryAccount(int cashOnDeliveryAccountId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateCashOnDeliveryAccount(int cashOnDeliveryAccountId, CashOnDeliveryAccountDTO cashOnDeliveryAccountDto)
        {
            throw new NotImplementedException();
        }
    }
}
