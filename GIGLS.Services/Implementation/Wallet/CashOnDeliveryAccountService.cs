using GIGLS.Core.IServices.CashOnDeliveryAccount;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Wallet;
using GIGLS.Core;
using GIGLS.Infrastructure;
using AutoMapper;
using GIGLS.Core.Enums;
using GIGLS.Core.IServices.Wallet;
using GIGLS.Core.IServices.CashOnDeliveryBalance;
using GIGLS.Core.Domain.Wallet;
using GIGLS.Core.IServices.User;

namespace GIGLS.Services.Implementation.Wallet
{
    public class CashOnDeliveryAccountService : ICashOnDeliveryAccountService
    {
        private readonly IUnitOfWork _uow;
        private readonly IWalletService _walletService;
        private readonly ICashOnDeliveryBalanceService _cashOnDeliveryBalanceService;
        private readonly IUserService _userService;

        public CashOnDeliveryAccountService(IUnitOfWork uow, IWalletService walletService,
            ICashOnDeliveryBalanceService cashOnDeliveryBalanceService, IUserService userService)
        {
            _uow = uow;
            _walletService = walletService;
            _cashOnDeliveryBalanceService = cashOnDeliveryBalanceService;
            _userService = userService;
            MapperConfig.Initialize();
        }

        public async Task AddCashOnDeliveryAccount(CashOnDeliveryAccountDTO cashOnDeliveryAccountDto)
        {
            //still thinking if to used wallet Id or Wallet Number
            //check for wallet id before processing 
            //var wallet = _walletService.GetWalletById(cashOnDeliveryAccountDto.WalletId);

            var accountBalance = await _uow.CashOnDeliveryBalance.GetAsync(x => x.WalletId == cashOnDeliveryAccountDto.WalletId);

            if(accountBalance == null)
            {
                var newBalance = new CashOnDeliveryBalance
                {
                    WalletId = cashOnDeliveryAccountDto.WalletId,
                    Balance = 0,
                    UserId = cashOnDeliveryAccountDto.UserId
                };

                _uow.CashOnDeliveryBalance.Add(newBalance);
                await _uow.CompleteAsync();

                accountBalance = await _uow.CashOnDeliveryBalance.GetAsync(newBalance.CashOnDeliveryBalanceId);
            }
            
            //create COD Account and all COD Account for the wwallet
            var newCODAccount = Mapper.Map<CashOnDeliveryAccount>(cashOnDeliveryAccountDto);
            newCODAccount.UserId = await _userService.GetCurrentUserId();
            _uow.CashOnDeliveryAccount.Add(newCODAccount);
            await _uow.CompleteAsync();

            //calculate balance
            var CODTransactions = await _uow.CashOnDeliveryAccount.FindAsync(s => s.WalletId == cashOnDeliveryAccountDto.WalletId);
            decimal balance = 0;
            foreach (var item in CODTransactions)
            {
                if (item.CreditDebitType == CreditDebitType.Credit)
                {
                    balance += item.Amount;
                }
                else
                {
                    balance -= item.Amount;
                }
            }

            accountBalance.Balance = balance;
            await _uow.CompleteAsync();
        }

        public async Task<CashOnDeliveryAccountDTO> GetCashOnDeliveryAccountById(int cashOnDeliveryAccountId)
        {
            var account = await _uow.CashOnDeliveryAccount.GetAsync(c => c.CashOnDeliveryAccountId == cashOnDeliveryAccountId, "Wallet");

            if (account == null)
            {
                throw new GenericException("Account does not exist");
            }

            var accountDto = Mapper.Map<CashOnDeliveryAccountDTO>(account);

            //set the customer name
            // handle Company customers
            if (CustomerType.Company.Equals(account.Wallet.CustomerType))
            {
                var companyDTO = await _uow.Company.GetAsync(s => s.CompanyId == account.Wallet.CustomerId);
                accountDto.Wallet.CustomerName = companyDTO.Name;
            }
            else
            {
                // handle IndividualCustomers
                var individualCustomerDTO = await _uow.IndividualCustomer.GetAsync(
                    s => s.IndividualCustomerId == account.Wallet.CustomerId);
                accountDto.Wallet.CustomerName = string.Format($"{individualCustomerDTO.FirstName} " +
                    $"{individualCustomerDTO.LastName}");
            }

            return accountDto;
        }

        public async Task<CashOnDeliveryAccountSummaryDTO> GetCashOnDeliveryAccountByWallet(string walletNumber)
        {
            var wallet = await _walletService.GetWalletById(walletNumber);

            var account = await _uow.CashOnDeliveryAccount.FindAsync(c => c.WalletId == wallet.WalletId);

            if (account == null)
            {
                throw new GenericException("Cash on Delivery Wallet information does not exist");
            }

            var balance =  await _cashOnDeliveryBalanceService.GetCashOnDeliveryBalanceByWalletId(wallet.WalletId);

            var accountDto = Mapper.Map<List<CashOnDeliveryAccountDTO>>(account);
            
            return new CashOnDeliveryAccountSummaryDTO
            {
                CashOnDeliveryAccount = accountDto,
                CashOnDeliveryDetail = balance
            };
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
                throw new GenericException("Cash on Delivery account does not exists");
            }

            account.CreditDebitType = cashOnDeliveryAccountDto.CreditDebitType;
            await _uow.CompleteAsync();
        }
    }
}
