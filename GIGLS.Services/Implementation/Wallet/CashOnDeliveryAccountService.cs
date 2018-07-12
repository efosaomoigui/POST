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
using GIGLS.Core.IServices.Account;
using GIGLS.Core.DTO.Account;
using System.Linq;

namespace GIGLS.Services.Implementation.Wallet
{
    public class CashOnDeliveryAccountService : ICashOnDeliveryAccountService
    {
        private readonly IUnitOfWork _uow;
        private readonly IWalletService _walletService;
        private readonly ICashOnDeliveryBalanceService _cashOnDeliveryBalanceService;
        private readonly IUserService _userService;
        private readonly IGeneralLedgerService _generalLedgerService;

        public CashOnDeliveryAccountService(IUnitOfWork uow, IWalletService walletService, IGeneralLedgerService generalLedgerService,
            ICashOnDeliveryBalanceService cashOnDeliveryBalanceService, IUserService userService)
        {
            _uow = uow;
            _walletService = walletService;
            _cashOnDeliveryBalanceService = cashOnDeliveryBalanceService;
            _userService = userService;
            _generalLedgerService = generalLedgerService;
            MapperConfig.Initialize();
        }

        public async Task AddCashOnDeliveryAccount(CashOnDeliveryAccountDTO cashOnDeliveryAccountDto)
        {
            //Use wallet Number
            var wallet = await _walletService.GetWalletById(cashOnDeliveryAccountDto.Wallet.WalletNumber);

            if (cashOnDeliveryAccountDto.UserId == null)
            {
                cashOnDeliveryAccountDto.UserId = await _userService.GetCurrentUserId();
            }

            //var accountBalance = await _uow.CashOnDeliveryBalance.GetAsync(x => x.WalletId == wallet.WalletId);

            //if (accountBalance == null)
            //{
            //    var newBalance = new CashOnDeliveryBalance
            //    {
            //        WalletId = wallet.WalletId,
            //        Balance = 0,
            //        UserId = cashOnDeliveryAccountDto.UserId
            //    };

            //    _uow.CashOnDeliveryBalance.Add(newBalance);
            //    await _uow.CompleteAsync();

            //    accountBalance = await _uow.CashOnDeliveryBalance.GetAsync(newBalance.CashOnDeliveryBalanceId);
            //}

            //create COD Account and all COD Account for the wallet
            cashOnDeliveryAccountDto.Wallet = null;
            var newCODAccount = Mapper.Map<CashOnDeliveryAccount>(cashOnDeliveryAccountDto);
            newCODAccount.WalletId = wallet.WalletId;
            newCODAccount.UserId = cashOnDeliveryAccountDto.UserId;
            _uow.CashOnDeliveryAccount.Add(newCODAccount);
            await _uow.CompleteAsync();

            ////add to wallet transaction and wallet
            //await _walletService.UpdateWallet(wallet.WalletId, new WalletTransactionDTO
            //{
            //    Amount = cashOnDeliveryAccountDto.Amount,
            //    CreditDebitType = cashOnDeliveryAccountDto.CreditDebitType,
            //    WalletId = wallet.WalletId,
            //    Description = cashOnDeliveryAccountDto.Description,
            //    UserId = cashOnDeliveryAccountDto.UserId,
            //    Waybill = cashOnDeliveryAccountDto.Waybill
            //});


            //create entry in WalletTransaction table
            var serviceCenterIds = await _userService.GetPriviledgeServiceCenters();
            if (serviceCenterIds.Length <= 0)
            {
                serviceCenterIds = new int[] { 0 };
                var defaultServiceCenter = await _userService.GetDefaultServiceCenter();
                serviceCenterIds[0] = defaultServiceCenter.ServiceCentreId;
            }


            CreditDebitType creditType;
            var description = "";
            if (cashOnDeliveryAccountDto.CreditDebitType == CreditDebitType.Credit)
            {
                creditType = CreditDebitType.Debit;
                description = "Debit for COD Payment Settlement";
            }
            else
            {
                creditType = CreditDebitType.Credit;
                description = "Credit for COD Payment Settlement";
            }

            //add to general ledger
            await _generalLedgerService.AddGeneralLedger(new GeneralLedgerDTO
            {
                Amount = cashOnDeliveryAccountDto.Amount,
                CreditDebitType = creditType,
                Description = description,
                UserId = cashOnDeliveryAccountDto.UserId,
                Waybill = cashOnDeliveryAccountDto.Waybill,
                ServiceCentreId = serviceCenterIds[0]
            });

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

            var accountDto = Mapper.Map<List<CashOnDeliveryAccountDTO>>(account.OrderByDescending(x => x.DateCreated));

            //var balance = await _cashOnDeliveryBalanceService.GetCashOnDeliveryBalanceByWalletId(wallet.WalletId);

            var walletDto = Mapper.Map<WalletDTO>(wallet);

            //set the customer name
            // handle Company customers
            if (CustomerType.Company.Equals(wallet.CustomerType))
            {
                var companyDTO = await _uow.Company.GetAsync(s => s.CompanyId == wallet.CustomerId);
                walletDto.CustomerName = companyDTO.Name;
            }
            else
            {
                // handle IndividualCustomers
                var individualCustomerDTO = await _uow.IndividualCustomer.GetAsync(s => s.IndividualCustomerId == wallet.CustomerId);
                walletDto.CustomerName = string.Format($"{individualCustomerDTO.FirstName} " +  $"{individualCustomerDTO.LastName}");
            }
            
            var balance = new CashOnDeliveryBalanceDTO
            {
                Balance = wallet.Balance,
                Wallet = walletDto                
            };
                       
            return new CashOnDeliveryAccountSummaryDTO
            {
                CashOnDeliveryAccount = accountDto,
                CashOnDeliveryDetail = balance
            };
        }

        
        public async Task<CashOnDeliveryAccountSummaryDTO> GetCashOnDeliveryAccountByStatus(string walletNumber, CODStatus status)
        {
            var wallet = await _walletService.GetWalletById(walletNumber);

            var account = await _uow.CashOnDeliveryAccount.FindAsync(c => c.WalletId == wallet.WalletId && c.CODStatus == status);

            if (account == null)
            {
                throw new GenericException("Cash on Delivery Wallet information does not exist");
            }

            var accountDto = Mapper.Map<List<CashOnDeliveryAccountDTO>>(account.OrderByDescending(x => x.DateCreated));
            var walletDto = Mapper.Map<WalletDTO>(wallet);

            //set the customer name
            // handle Company customers
            if (CustomerType.Company.Equals(wallet.CustomerType))
            {
                var companyDTO = await _uow.Company.GetAsync(s => s.CompanyId == wallet.CustomerId);
                walletDto.CustomerName = companyDTO.Name;
            }
            else
            {
                // handle IndividualCustomers
                var individualCustomerDTO = await _uow.IndividualCustomer.GetAsync(s => s.IndividualCustomerId == wallet.CustomerId);
                walletDto.CustomerName = string.Format($"{individualCustomerDTO.FirstName} " + $"{individualCustomerDTO.LastName}");
            }

            var balance = new CashOnDeliveryBalanceDTO
            {
                Balance = wallet.Balance,
                Wallet = walletDto
            };

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

        public Task<IEnumerable<CashOnDeliveryAccountDTO>> GetCashOnDeliveryAccounts(CODStatus cODStatus)
        {
            return _uow.CashOnDeliveryAccount.GetCashOnDeliveryAccountAsync(cODStatus);
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
            var currentUser = await _userService.GetCurrentUserId();

            account.CreditDebitType = cashOnDeliveryAccountDto.CreditDebitType;
            account.UserId = currentUser;
            await _uow.CompleteAsync();
        }


        public async Task ProcessToPending(List<CashOnDeliveryBalanceDTO> data)
        {
            var unprocessedCODAccounts = await _uow.CashOnDeliveryAccount.GetCashOnDeliveryAccountAsync(CODStatus.Unprocessed);

            foreach (var unprocessedAccount in unprocessedCODAccounts)
            {
                //1. update status
                var cashOnDeliveryAccount = await _uow.CashOnDeliveryAccount.GetAsync(unprocessedAccount.CashOnDeliveryAccountId);
                cashOnDeliveryAccount.CODStatus = CODStatus.Pending;

                ////2. update balance
                //var accountBalance = await _uow.CashOnDeliveryBalance.GetAsync(x => x.WalletId == unprocessedAccount.WalletId);

                //if (unprocessedAccount.CreditDebitType == CreditDebitType.Credit)
                //{
                //    accountBalance.Balance -= unprocessedAccount.Amount;
                //}

                //3. complete transaction
                await _uow.CompleteAsync();
            }
        }


        public async Task ProcessCashOnDeliveryPaymentSheet(List<CashOnDeliveryBalanceDTO> data)
        {
            var listOfPendingCashOnDeliverys = await _cashOnDeliveryBalanceService.GetPendingCashOnDeliveryPaymentSheet();

            foreach (var cashOnDeliveryBalance in listOfPendingCashOnDeliverys)
            {
                var walletId = cashOnDeliveryBalance.Wallet.WalletId;
                var cashOnDeliveryAccount = new CashOnDeliveryAccountDTO
                {
                    Amount = cashOnDeliveryBalance.Balance,
                    Wallet = cashOnDeliveryBalance.Wallet,
                    CreditDebitType = CreditDebitType.Debit,
                    Description = "Debit for COD Payment Settlement",
                    CODStatus = CODStatus.Processed
                };

                await AddCashOnDeliveryAccount(cashOnDeliveryAccount);

                //update status to processed
                var CODTransactions =
                    await _uow.CashOnDeliveryAccount.FindAsync(s => s.WalletId == walletId && s.CODStatus == CODStatus.Pending);
                foreach (var item in CODTransactions)
                {
                    item.CODStatus = CODStatus.Processed;
                }
                await _uow.CompleteAsync();

            }
        }
    }
}
