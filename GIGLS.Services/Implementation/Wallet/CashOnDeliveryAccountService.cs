using GIGLS.Core.IServices.CashOnDeliveryAccount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Wallet;
using GIGLS.Core;
using GIGLS.Infrastructure;

namespace GIGLS.Services.Implementation.Wallet
{
    public class CashOnDeliveryAccountService : ICashOnDeliveryAccountService
    {
        private readonly IUnitOfWork _uow;

        public CashOnDeliveryAccountService(IUnitOfWork uow)
        {
            _uow = uow;
            MapperConfig.Initialize();
        }

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
            return _uow.CashOnDeliveryAccount.GetCashOnDeliveryAccountAsync();
        }

        public async Task RemoveCashOnDeliveryAccount(int cashOnDeliveryAccountId)
        {
            var account = await _uow.CashOnDeliveryAccount.GetAsync(cashOnDeliveryAccountId);

            if (account == null)
            {
                throw new GenericException("Wallet does not exists");
            }

            _uow.CashOnDeliveryAccount.Remove(account);
            await _uow.CompleteAsync();
        }

        public async Task UpdateCashOnDeliveryAccount(int cashOnDeliveryAccountId, CashOnDeliveryAccountDTO cashOnDeliveryAccountDto)
        {
            var account = await _uow.CashOnDeliveryAccount.GetAsync(cashOnDeliveryAccountId);

            if (account == null)
            {
                throw new GenericException("Cash on Delivery Balance does not exists");
            }

            account.CreditDebitType = cashOnDeliveryAccountDto.CreditDebitType;
            await _uow.CompleteAsync();
        }
    }
}
