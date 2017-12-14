using GIGLS.Core.IServices.CashOnDeliveryBalance;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Wallet;
using GIGLS.Core;
using GIGLS.Infrastructure;
using AutoMapper;
using GIGLS.Core.Enums;
using GIGLS.Core.IServices.Wallet;

namespace GIGLS.Services.Implementation.Wallet
{
    public class CashOnDeliveryBalanceService : ICashOnDeliveryBalanceService
    {
        private readonly IUnitOfWork _uow;
        private readonly IWalletService _walletService;

        public CashOnDeliveryBalanceService(IUnitOfWork uow, IWalletService walletService)
        {
            _uow = uow;
            _walletService = walletService;
            MapperConfig.Initialize();
        }

        public async Task AddCashOnDeliveryBalance(CashOnDeliveryBalanceDTO cashOnDeliveryBalanceDTO)
        {
            _uow.CashOnDeliveryBalance.Add(new Core.Domain.Wallet.CashOnDeliveryBalance
            {
                WalletId = cashOnDeliveryBalanceDTO.WalletId,
                Balance = cashOnDeliveryBalanceDTO.Balance,
                UserId = cashOnDeliveryBalanceDTO.UserId
            });

            await _uow.CompleteAsync();
        }

        public async Task<CashOnDeliveryBalanceDTO> GetCashOnDeliveryBalanceById(int cashOnDeliveryBalanceId)
        {
            var balance = await _uow.CashOnDeliveryBalance.GetAsync(c => c.CashOnDeliveryBalanceId == cashOnDeliveryBalanceId, "Wallet");

            if (balance == null)
            {
                throw new GenericException("Wallet does not exist");
            }

            var balanceDto = Mapper.Map<CashOnDeliveryBalanceDTO>(balance);

            //set the customer name
            // handle Company customers
            if (CustomerType.Company.Equals(balance.Wallet.CustomerType))
            {
                var companyDTO = await _uow.Company.GetAsync(s => s.CompanyId == balance.Wallet.CustomerId);
                balanceDto.Wallet.CustomerName = companyDTO.Name;
            }
            else
            {
                // handle IndividualCustomers
                var individualCustomerDTO = await _uow.IndividualCustomer.GetAsync(
                    s => s.IndividualCustomerId == balance.Wallet.CustomerId);
                balanceDto.Wallet.CustomerName = string.Format($"{individualCustomerDTO.FirstName} " +
                    $"{individualCustomerDTO.LastName}");
            }
            
            return balanceDto;
        }

        public async Task<CashOnDeliveryBalanceDTO> GetCashOnDeliveryBalanceByWallet(string walletNumber)
        {
            var wallet = await _walletService.GetWalletById(walletNumber);

            var balance = await _uow.CashOnDeliveryBalance.GetAsync(c => c.WalletId == wallet.WalletId);

            if (balance == null)
            {
                throw new GenericException("Cash on Delivery Wallet information does not exist");
            }

            var walletDTO = Mapper.Map<WalletDTO>(wallet);
            var balanceDto = Mapper.Map<CashOnDeliveryBalanceDTO>(balance);
            balanceDto.Wallet = walletDTO;
            

            //set the customer name
            // handle Company customers
            if (CustomerType.Company.Equals(wallet.CustomerType))
            {
                var companyDTO = await _uow.Company.GetAsync(s => s.CompanyId == wallet.CustomerId);
                balanceDto.Wallet.CustomerName = companyDTO.Name;
            }
            else
            {
                // handle IndividualCustomers
                var individualCustomerDTO = await _uow.IndividualCustomer.GetAsync(
                    s => s.IndividualCustomerId == wallet.CustomerId);
                balanceDto.Wallet.CustomerName = string.Format($"{individualCustomerDTO.FirstName} " +
                    $"{individualCustomerDTO.LastName}");
            }

            return balanceDto;
        }


        public async Task<CashOnDeliveryBalanceDTO> GetCashOnDeliveryBalanceByWalletId(int walletId)
        {
            var balance = await _uow.CashOnDeliveryBalance.GetAsync(c => c.WalletId == walletId, "Wallet");

            if (balance == null)
            {
                throw new GenericException("Wallet does not exist");
            }

            var balanceDto = Mapper.Map<CashOnDeliveryBalanceDTO>(balance);

            //set the customer name
            // handle Company customers
            if (CustomerType.Company.Equals(balance.Wallet.CustomerType))
            {
                var companyDTO = await _uow.Company.GetAsync(s => s.CompanyId == balance.Wallet.CustomerId);
                balanceDto.Wallet.CustomerName = companyDTO.Name;
            }
            else
            {
                // handle IndividualCustomers
                var individualCustomerDTO = await _uow.IndividualCustomer.GetAsync(
                    s => s.IndividualCustomerId == balance.Wallet.CustomerId);
                balanceDto.Wallet.CustomerName = string.Format($"{individualCustomerDTO.FirstName} " +
                    $"{individualCustomerDTO.LastName}");
            }

            return balanceDto;
        }

        public async Task<IEnumerable<CashOnDeliveryBalanceDTO>> GetCashOnDeliveryBalances()
        {
            var balances = await _uow.CashOnDeliveryBalance.GetCashOnDeliveryBalanceAsync();

            foreach (var item in balances)
            {
                // handle Company customers
                if (CustomerType.Company.Equals(item.Wallet.CustomerType))
                {
                    var companyDTO = await _uow.Company.GetAsync(s => s.CompanyId == item.Wallet.CustomerId);
                    item.Wallet.CustomerName = companyDTO.Name;
                }
                else
                {
                    // handle IndividualCustomers
                    var individualCustomerDTO = await _uow.IndividualCustomer.GetAsync(
                        s => s.IndividualCustomerId == item.Wallet.CustomerId);
                    item.Wallet.CustomerName = string.Format($"{individualCustomerDTO.FirstName} " +
                        $"{individualCustomerDTO.LastName}");
                }
            }

            return balances;
        }

        public async Task RemoveCashOnDeliveryBalance(int cashOnDeliveryBalanceId)
        {
            var balance = await _uow.CashOnDeliveryBalance.GetAsync(cashOnDeliveryBalanceId);

            if (balance == null)
            {
                throw new GenericException("Wallet does not exists");
            }

            _uow.CashOnDeliveryBalance.Remove(balance);
            await _uow.CompleteAsync();
        }

        public async Task UpdateCashOnDeliveryBalance(int cashOnDeliveryBalanceId, CashOnDeliveryBalanceDTO cashOnDeliveryBalanceDTO)
        {
            var balance = await _uow.CashOnDeliveryBalance.GetAsync(cashOnDeliveryBalanceId);

            if (balance == null)
            {
                throw new GenericException("Cash on Delivery Balance does not exists");
            }

            balance.Balance = cashOnDeliveryBalanceDTO.Balance;
            balance.UserId = cashOnDeliveryBalanceDTO.UserId;
            await _uow.CompleteAsync();
        }
        
    }
}
