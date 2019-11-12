using AutoMapper;
using GIGLS.Core;
using GIGLS.Core.Domain.Wallet;
using GIGLS.Core.DTO.Wallet;
using GIGLS.Core.Enums;
using GIGLS.Core.IServices.User;
using GIGLS.Core.IServices.Utility;
using GIGLS.Core.IServices.Wallet;
using GIGLS.CORE.Enums;
using GIGLS.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GIGLS.Services.Implementation.Wallet
{
    public class WalletService : IWalletService
    {
        private readonly INumberGeneratorMonitorService _numberGeneratorMonitorService;
        private readonly IUserService _userService;
        private readonly IUnitOfWork _uow;

        public WalletService(IUserService userService, INumberGeneratorMonitorService numberGeneratorMonitorService, IUnitOfWork uow)
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

            return wallets.ToList().OrderBy(x => x.CustomerName);
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
            else if (CustomerType.Partner.Equals(wallet.CustomerType))
            {
                var partnerDTO = await _uow.Partner.GetAsync(p => p.PartnerId == walletDTO.CustomerId);
                walletDTO.CustomerName = partnerDTO.PartnerName;
            }
            else
            {
                // handle IndividualCustomers
                var individualCustomerDTO = await _uow.IndividualCustomer.GetAsync(s => s.IndividualCustomerId == walletDTO.CustomerId);
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

        public async Task<WalletDTO> GetSystemWallet()
        {
            var wallet = await _uow.Wallet.GetAsync(x => x.IsSystem == true);

            if (wallet == null)
            {
                throw new GenericException("System Wallet does not exist");
            }

            return Mapper.Map<WalletDTO>(wallet);
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
                CustomerType = wallet.CustomerType,
                CustomerCode = wallet.CustomerCode,
                CompanyType = wallet.CompanyType
            });
            await _uow.CompleteAsync();
        }

        public async Task UpdateWallet(int walletId, WalletTransactionDTO walletTransactionDTO,
            bool hasServiceCentre = true)
        {
            var wallet = await _uow.Wallet.GetAsync(walletId);
            if (wallet == null)
            {
                throw new GenericException("Wallet does not exists");
            }

            if (walletTransactionDTO.UserId == null)
            {
                walletTransactionDTO.UserId = await _userService.GetCurrentUserId();
            }

            ////////////
            var serviceCenterIds = new int[] { };
            if (hasServiceCentre == true)
            {
                serviceCenterIds = await _userService.GetPriviledgeServiceCenters();
            }
            ///////////////

            if (serviceCenterIds.Length <= 0)
            {
                serviceCenterIds = new int[] { 0 };
                var defaultServiceCenter = await _userService.GetDefaultServiceCenter();
                serviceCenterIds[0] = defaultServiceCenter.ServiceCentreId;

                //var currentUser = await _userService.GetUserById(walletTransactionDTO.UserId);
                //throw new GenericException($"User {currentUser.Username} does not have a priviledge claim.");
            }

            var newWalletTransaction = Mapper.Map<WalletTransaction>(walletTransactionDTO);
            newWalletTransaction.WalletId = walletId;
            newWalletTransaction.DateOfEntry = DateTime.Now;
            newWalletTransaction.ServiceCentreId = serviceCenterIds[0];
            newWalletTransaction.UserId = walletTransactionDTO.UserId;

            _uow.WalletTransaction.Add(newWalletTransaction);
            await _uow.CompleteAsync();

            //get balance
            var walletTransactions = await _uow.WalletTransaction.FindAsync(s => s.WalletId == walletId);
            decimal balance = 0;
            foreach (var item in walletTransactions)
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

            wallet = await _uow.Wallet.GetAsync(walletId);
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

        public async Task<List<WalletDTO>> SearchForWallets(WalletSearchOption searchOption)
        {
            try
            {
                List<WalletDTO> walletsDto = new List<WalletDTO>();
                var walletsQueryable = _uow.Wallet.GetWalletsAsQueryable();

                //If searchOption.SearchData is not empty
                if (!string.IsNullOrWhiteSpace(searchOption.SearchData))
                {
                    walletsQueryable = walletsQueryable.Where(x =>
                        x.CustomerCode.Contains(searchOption.SearchData) || x.WalletNumber.Contains(searchOption.SearchData));
                }

                // handle Individual customers
                if (FilterCustomerType.IndividualCustomer.Equals(searchOption.CustomerType))
                {
                    walletsQueryable = walletsQueryable.Where(x => x.CustomerType == CustomerType.IndividualCustomer);
                    walletsDto = Mapper.Map<List<WalletDTO>>(walletsQueryable.ToList());
                }
                else
                {
                    CompanyType companyType;

                    if (FilterCustomerType.Corporate.Equals(searchOption.CustomerType))
                    {
                        companyType = CompanyType.Corporate;
                    }
                    else
                    {
                        companyType = CompanyType.Ecommerce;
                    }
                    walletsQueryable = walletsQueryable.Where(x => x.CompanyType == companyType.ToString());
                    var walletsResult = walletsQueryable.ToList();
                    walletsDto = Mapper.Map<List<WalletDTO>>(walletsResult);
                }

                ////set the customer name
                foreach (var item in walletsDto)
                {
                    // handle Company customers
                    if (CustomerType.Company == item.CustomerType)
                    {
                        var companyDTO = await _uow.Company.GetAsync(s => s.CompanyId == item.CustomerId);
                        if(companyDTO != null)
                        {
                            item.CustomerName = companyDTO.Name;
                        }
                    }
                    else
                    {
                        // handle IndividualCustomers
                        var individualCustomerDTO = await _uow.IndividualCustomer.GetAsync(s => s.IndividualCustomerId == item.CustomerId);
                        item.CustomerName = string.Format($"{individualCustomerDTO.FirstName} " + $"{individualCustomerDTO.LastName}");
                    }
                }

                return walletsDto.OrderBy(x => x.CustomerName).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<WalletDTO> GetWalletBalance()
        {
            var currentUser = await _userService.GetCurrentUserId();
            var user = await _uow.User.GetUserById(currentUser);
            var wallet = await _uow.Wallet.GetAsync(x => x.CustomerCode.Equals(user.UserChannelCode));

            var walletDTO = Mapper.Map<WalletDTO>(wallet);
            if (wallet == null)
            {
                throw new GenericException("Wallet does not exist");
            }

            return walletDTO;
        }

        public IQueryable<Core.Domain.Wallet.Wallet> GetWalletAsQueryableService()
        {
            var wallet = _uow.Wallet.GetAllAsQueryable();
            return wallet;
        }
    }
}