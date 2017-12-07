using AutoMapper;
using GIGLS.Core;
using GIGLS.Core.Domain.Wallet;
using GIGLS.Core.DTO.Wallet;
using GIGLS.Core.Enums;
using GIGLS.Core.IServices.User;
using GIGLS.Core.IServices.Utility;
using GIGLS.Core.IServices.Wallet;
using GIGLS.Infrastructure;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Services.Implementation.Wallet
{
    public class WalletService : IWalletService
    {
        private readonly INumberGeneratorMonitorService _numberGeneratorMonitorService;

        private readonly IUserService _userService;
        private readonly IUnitOfWork _uow;

        public WalletService(IUserService userService,
            INumberGeneratorMonitorService numberGeneratorMonitorService, IUnitOfWork uow)
        {
            _numberGeneratorMonitorService = numberGeneratorMonitorService;
            _userService = userService;
            _uow = uow;
            MapperConfig.Initialize();
        }

        public async Task<IEnumerable<WalletDTO>> GetWallets()
        {
            var wallets = await _uow.Wallet.GetWalletsAsync();

            //set the customer name
            foreach (var item in wallets)
            {
                // handle Company customers
                if (CustomerType.Company.Equals(item.CustomerType))
                {
                    var companyDTO = await _uow.Company.GetAsync(s => s.CompanyId == item.CustomerId);
                    item.CustomerName = companyDTO.Name;
                }
                else
                {
                    // handle IndividualCustomers
                    var individualCustomerDTO = await _uow.IndividualCustomer.GetAsync(
                        s => s.IndividualCustomerId == item.CustomerId);
                    item.CustomerName = string.Format($"{individualCustomerDTO.FirstName} " +
                        $"{individualCustomerDTO.LastName}");
                }
            }


            return wallets;
        }


        public async Task<WalletDTO> GetWalletById(int walletid)
        {
            var wallet = await _uow.Wallet.GetAsync(walletid);

            if (wallet == null)
            {
                throw new GenericException("Wallet does not exist");
            }
            var walletDTO = Mapper.Map<WalletDTO>(wallet);

            //set the customer name
            // handle Company customers
            if (CustomerType.Company.Equals(wallet.CustomerType))
            {
                var companyDTO = await _uow.Company.GetAsync(s => s.CompanyId == walletDTO.CustomerId);
                walletDTO.CustomerName = companyDTO.Name;
            }
            else
            {
                // handle IndividualCustomers
                var individualCustomerDTO = await _uow.IndividualCustomer.GetAsync(
                    s => s.IndividualCustomerId == walletDTO.CustomerId);
                walletDTO.CustomerName = string.Format($"{individualCustomerDTO.FirstName} " +
                    $"{individualCustomerDTO.LastName}");
            }

            return walletDTO;
        }

        public async Task<Core.Domain.Wallet.Wallet> GetWalletById(string walletNumber)
        {
            var wallet = await _uow.Wallet.GetAsync(x => x.WalletNumber.Equals(walletNumber));

            if (wallet == null)
            {
                throw new GenericException("Wallet does not exist");
            }
            
            return wallet;
        }

        public async Task AddWallet(WalletDTO wallet)
        {
            var walletNumber = await _numberGeneratorMonitorService.GenerateNextNumber(NumberGeneratorType.Wallet);
            wallet.WalletNumber = walletNumber;

            _uow.Wallet.Add(new Core.Domain.Wallet.Wallet
            {
                WalletId = wallet.WalletId,
                WalletNumber = wallet.WalletNumber,
                Balance = wallet.Balance,
                CustomerId = wallet.CustomerId,
                CustomerType = wallet.CustomerType
            });
            await _uow.CompleteAsync();
        }

        public async Task UpdateWallet(int walletId, WalletTransactionDTO walletTransactionDTO)
        {
            var wallet = await _uow.Wallet.GetAsync(walletId);
            if (wallet == null)
            {
                throw new GenericException("Wallet does not exists");
            }

            //create entry in WalletTransaction table
            var serviceCenterIds = await _userService.GetPriviledgeServiceCenters();

            var newWalletTransaction = Mapper.Map<WalletTransaction>(walletTransactionDTO);
            newWalletTransaction.WalletId = walletId;
            newWalletTransaction.DateCreated = DateTime.Now;
            newWalletTransaction.DateModified = DateTime.Now;
            newWalletTransaction.DateOfEntry = DateTime.Now;
            newWalletTransaction.ServiceCentreId = serviceCenterIds[0];
            newWalletTransaction.UserId = await _userService.GetCurrentUserId();

            _uow.WalletTransaction.Add(newWalletTransaction);

            //get balance
            var walletTransactions = await _uow.WalletTransaction.FindAsync(s => s.WalletId == walletId);
            decimal balance = 0;
            foreach (var item in walletTransactions)
            {
                if (item.CreditDebitType == CreditDebitType.Credit)
                {
                    balance += balance + item.Amount;
                }
                else
                {
                    balance += balance - item.Amount;
                }
            }

            wallet.Balance = balance;
            await _uow.CompleteAsync();
        }

        public async Task RemoveWallet(int walletId)
        {
            var wall = await _uow.Wallet.GetAsync(walletId);

            if (wall == null)
            {
                throw new GenericException("Wallet does not exists");
            }

            _uow.Wallet.Remove(wall);
            await _uow.CompleteAsync();
        }

        public async Task<WalletNumber> GenerateNextValidWalletNumber()
        {
            //1. Get the last wallet number
            var walletNumber = await _uow.WalletNumber.GetLastValidWalletNumber();

            // At this point, walletNumber can only be null if it's the first time we're
            // creating a wallet. If that's the case, we assume our wallet PAN to be "0".
            var walletPan = walletNumber?.WalletPan ?? "0";

            //2. Increment and pad walletPan to get the next available wallet number
            var number = long.Parse(walletPan) + 1;
            var numberStr = number.ToString("0000000000");

            //3. Return New Wallet Number
            return new WalletNumber
            {
                WalletPan = numberStr,
                IsActive = true
            };
        }

    }
}
