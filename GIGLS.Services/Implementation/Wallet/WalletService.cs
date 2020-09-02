using AutoMapper;
using GIGLS.Core;
using GIGLS.Core.Domain.Wallet;
using GIGLS.Core.DTO;
using GIGLS.Core.DTO.Customers;
using GIGLS.Core.DTO.Partnership;
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
using System.Net;
using System.Threading.Tasks;

namespace GIGLS.Services.Implementation.Wallet
{
    public class WalletService : IWalletService
    {
        private readonly INumberGeneratorMonitorService _numberGeneratorMonitorService;
        private readonly IUserService _userService;
        private readonly IUnitOfWork _uow;
        private readonly IGlobalPropertyService _globalPropertyService;

        public WalletService(IUserService userService, INumberGeneratorMonitorService numberGeneratorMonitorService, IUnitOfWork uow, IGlobalPropertyService globalPropertyService)
        {
            _numberGeneratorMonitorService = numberGeneratorMonitorService;
            _userService = userService;
            _uow = uow;
            _globalPropertyService = globalPropertyService;
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
                    var companyDTO = await _uow.Company.GetCompanyByIdWithCountry(item.CustomerId);

                    if (companyDTO != null)
                    {
                        item.CustomerName = companyDTO.Name;
                        item.Country = companyDTO.Country;
                        item.UserActiveCountryId = companyDTO.UserActiveCountryId;
                    }
                }
                if (CustomerType.Partner.Equals(item.CustomerType))
                {
                    var partnerDTO = await _uow.Partner.GetPartnerByIdWithCountry(item.CustomerId);
                    item.CustomerName = partnerDTO.PartnerName;
                    item.UserActiveCountryId = partnerDTO.UserActiveCountryId;
                    item.Country = partnerDTO.Country;
                }
                else
                {
                    // handle IndividualCustomers
                    var individualCustomerDTO = await _uow.IndividualCustomer.GetIndividualCustomerByIdWithCountry(item.CustomerId);
                    item.CustomerName = string.Format($"{individualCustomerDTO.FirstName} " + $"{individualCustomerDTO.LastName}");
                    item.UserActiveCountryId = individualCustomerDTO.UserActiveCountryId;
                    item.Country = individualCustomerDTO.Country;
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
                var companyDTO = await _uow.Company.GetCompanyByIdWithCountry(walletDTO.CustomerId);

                if (companyDTO != null)
                {
                    walletDTO.CustomerName = companyDTO.Name;
                    walletDTO.CustomerEmail = companyDTO.Email;
                    walletDTO.Country = companyDTO.Country;
                    walletDTO.UserActiveCountryId = companyDTO.UserActiveCountryId;
                }
            }
            else if (CustomerType.Partner.Equals(wallet.CustomerType))
            {
                var partnerDTO = await _uow.Partner.GetPartnerByIdWithCountry(walletDTO.CustomerId);
                if (partnerDTO != null)
                {
                    walletDTO.CustomerName = partnerDTO.PartnerName;
                    walletDTO.CustomerEmail = partnerDTO.Email;
                    walletDTO.UserActiveCountryId = partnerDTO.UserActiveCountryId;
                    walletDTO.Country = partnerDTO.Country;
                }
            }
            else
            {
                // handle IndividualCustomers
                var individualCustomerDTO = await _uow.IndividualCustomer.GetIndividualCustomerByIdWithCountry(walletDTO.CustomerId);
                if (individualCustomerDTO != null)
                {
                    walletDTO.CustomerName = string.Format($"{individualCustomerDTO.FirstName} " + $"{individualCustomerDTO.LastName}");
                    walletDTO.UserActiveCountryId = individualCustomerDTO.UserActiveCountryId;
                    walletDTO.Country = individualCustomerDTO.Country;
                    walletDTO.CustomerEmail = individualCustomerDTO.Email;
                }
            }

            return walletDTO;
        }

        public async Task<Core.Domain.Wallet.Wallet> GetWalletById(string walletNumber)
        {
            var wallet = await _uow.Wallet.GetAsync(x => x.WalletNumber.Equals(walletNumber));

            if (wallet == null)
            {
                throw new GenericException("Wallet does not exist", $"{(int)HttpStatusCode.NotFound}");
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

        public async Task UpdateWallet(int walletId, WalletTransactionDTO walletTransactionDTO, bool hasServiceCentre = true)
        {
            var wallet = await _uow.Wallet.GetAsync(walletId);
            if (wallet == null)
            {
                throw new GenericException("Wallet does not exists", $"{(int)HttpStatusCode.NotFound}");
            }

            //verify second time to reduce multiple credit of account
            if (!string.IsNullOrWhiteSpace(walletTransactionDTO.PaymentTypeReference))
            {
                var walletTrans = await _uow.WalletTransaction.GetAsync(x => x.PaymentTypeReference == walletTransactionDTO.PaymentTypeReference);

                if(walletTrans != null)
                {
                    //update wallet payment log
                    var paymentLog = await _uow.WalletPaymentLog.GetAsync(x => x.Reference == walletTransactionDTO.PaymentTypeReference);
                    if(paymentLog != null)
                    {
                        paymentLog.IsWalletCredited = true;
                        await _uow.CompleteAsync();
                    }

                    throw new GenericException("Account Already Credited, Kindly check your wallet", $"{(int)HttpStatusCode.Forbidden}");
                }

            }

            //Manage want every customer to be eligible
            //await CheckIfEcommerceIsEligible(wallet, walletTransactionDTO.Amount);

            if (walletTransactionDTO.UserId == null)
            {
                walletTransactionDTO.UserId = await _userService.GetCurrentUserId();
            }

            var serviceCenterIds = new int[] { };
            if (hasServiceCentre == true)
            {
                serviceCenterIds = await _userService.GetPriviledgeServiceCenters();
            }

            if (serviceCenterIds.Length < 1)
            {
                serviceCenterIds = new int[] { 0 };
                var defaultServiceCenter = await _userService.GetDefaultServiceCenter();
                serviceCenterIds[0] = defaultServiceCenter.ServiceCentreId;
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

            //wallet = await _uow.Wallet.GetAsync(walletId);
            wallet.Balance = balance;
            await _uow.CompleteAsync();
        }

        public async Task RemoveWallet(int walletId)
        {
            var wall = await _uow.Wallet.GetAsync(walletId);

            if (wall == null)
            {
                throw new GenericException("Wallet does not exists", $"{(int)HttpStatusCode.NotFound}");
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
                List<PartnerDTO> partners = new List<PartnerDTO>();
                List<CompanyDTO> companies = new List<CompanyDTO>();
                List<IndividualCustomerDTO> individualCustomer = new List<IndividualCustomerDTO>();

                var walletsQueryable = _uow.Wallet.GetWalletsAsQueryable();

                if (!string.IsNullOrWhiteSpace(searchOption.SearchData))
                {
                    List<string> customerCodes = new List<string>();

                    if (searchOption.CustomerType == FilterCustomerType.Ecommerce || searchOption.CustomerType == FilterCustomerType.Corporate)
                    {
                        companies = await _uow.Company.GetCompanyByEmail(searchOption.SearchData);
                        customerCodes = companies.Select(x => x.CustomerCode).ToList();
                    }
                    else if (searchOption.CustomerType == FilterCustomerType.Partner)
                    {
                        partners = await _uow.Partner.GetPartnerBySearchParameters(searchOption.SearchData);
                        customerCodes = partners.Select(x => x.PartnerCode).ToList();
                    }
                    else if (searchOption.CustomerType == FilterCustomerType.IndividualCustomer)
                    {
                        individualCustomer = await _uow.IndividualCustomer.GetIndividualCustomers(searchOption.SearchData);
                        customerCodes = individualCustomer.Select(x => x.CustomerCode).ToList();
                    }

                    walletsQueryable = walletsQueryable.Where(x => customerCodes.Contains(x.CustomerCode));
                    walletsDto = Mapper.Map<List<WalletDTO>>(walletsQueryable.ToList());
                }

                ////set the customer name
                foreach (var item in walletsDto)
                {
                    // handle Company customers
                    if (CustomerType.Company == item.CustomerType)
                    {
                        if (companies.Any())
                        {
                            var company = companies.Where(x => x.CustomerCode == item.CustomerCode).FirstOrDefault();

                            if (company != null)
                            {
                                item.CustomerName = company.Name;
                                item.Country = company.Country;
                                item.CustomerPhoneNumber = company.PhoneNumber;
                                item.CustomerEmail = company.Email;
                                item.UserActiveCountryId = company.UserActiveCountryId;
                            }
                        }
                        else
                        {
                            var walletDTO = await SearchWalletDetails(item.CustomerType, item.CustomerCode);

                            if(walletDTO != null)
                            {
                                item.CustomerName = walletDTO.CustomerName;
                                item.Country = walletDTO.Country;
                                item.CustomerPhoneNumber = walletDTO.CustomerPhoneNumber;
                                item.CustomerEmail = walletDTO.CustomerEmail;
                                item.UserActiveCountryId = walletDTO.UserActiveCountryId;
                            }
                            
                        }
                    }
                    else if (CustomerType.Partner == item.CustomerType)
                    {
                        if (partners.Any())
                        {
                            var partner = partners.Where(x => x.PartnerCode == item.CustomerCode).FirstOrDefault();

                            if (partner != null)
                            {
                                item.CustomerName = partner.PartnerName;
                                item.CustomerPhoneNumber = partner.PhoneNumber;
                                item.CustomerEmail = partner.Email;
                                item.Country = partner.Country;
                                item.UserActiveCountryId = partner.UserActiveCountryId;
                            }
                        }
                        else
                        {
                            var walletDTO = await SearchWalletDetails(item.CustomerType, item.CustomerCode);

                            if (walletDTO != null)
                            {
                                item.CustomerName = walletDTO.CustomerName;
                                item.CustomerPhoneNumber = walletDTO.CustomerPhoneNumber;
                                item.CustomerEmail = walletDTO.CustomerEmail;
                                item.Country = walletDTO.Country;
                                item.UserActiveCountryId = walletDTO.UserActiveCountryId;
                            }

                        }
                    }
                    else
                    {
                        // handle IndividualCustomers
                        if (individualCustomer.Any())
                        {
                            var individual = individualCustomer.Where(x => x.CustomerCode == item.CustomerCode).FirstOrDefault();

                            if (individual != null)
                            {
                                item.CustomerName = string.Format($"{individual.FirstName} " + $"{individual.LastName}");
                                item.CustomerPhoneNumber = individual.PhoneNumber;
                                item.CustomerEmail = individual.Email;
                                item.UserActiveCountryId = individual.UserActiveCountryId;
                                item.Country = individual.Country;
                            }
                        }
                        else
                        {
                            var walletDTO = await SearchWalletDetails(item.CustomerType, item.CustomerCode);

                            if (walletDTO != null)
                            {
                                item.CustomerName = walletDTO.CustomerName;
                                item.CustomerPhoneNumber = walletDTO.CustomerPhoneNumber;
                                item.CustomerEmail = walletDTO.CustomerEmail;
                                item.UserActiveCountryId = walletDTO.UserActiveCountryId;
                                item.Country = walletDTO.Country;
                            }
                        }
                    }
                }

                return walletsDto.OrderBy(x => x.CustomerName).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<WalletDTO> SearchWalletDetails(CustomerType customerType, string customerCode)
        {
            var item = new WalletDTO();

            if (CustomerType.Company == customerType)
            {
                var companyData = await _uow.Company.GetCompanyByEmail(customerCode);
                var company = companyData.FirstOrDefault();

                if (company != null)
                {
                    item.CustomerName = company.Name;
                    item.Country = company.Country;
                    item.CustomerPhoneNumber = company.PhoneNumber;
                    item.CustomerEmail = company.Email;
                    item.UserActiveCountryId = company.UserActiveCountryId;
                }
            }
            else if (CustomerType.Partner == customerType)
            {

                var partnerData = await _uow.Partner.GetPartnerBySearchParameters(customerCode);
                var partner = partnerData.FirstOrDefault();

                if (partner != null)
                {
                    item.CustomerName = partner.PartnerName;
                    item.CustomerPhoneNumber = partner.PhoneNumber;
                    item.CustomerEmail = partner.Email;
                    item.Country = partner.Country;
                    item.UserActiveCountryId = partner.UserActiveCountryId;
                }
            }
            else
            {

                var individualData = await _uow.IndividualCustomer.GetIndividualCustomers(customerCode);
                var individual = individualData.FirstOrDefault();

                if (individual != null)
                {
                    item.CustomerName = string.Format($"{individual.FirstName} " + $"{individual.LastName}");
                    item.CustomerPhoneNumber = individual.PhoneNumber;
                    item.CustomerEmail = individual.Email;
                    item.UserActiveCountryId = individual.UserActiveCountryId;
                    item.Country = individual.Country;
                }
            }

            return item;

        }

        public async Task<WalletDTO> GetWalletBalance()
        {
            var currentUser = await _userService.GetCurrentUserId();
            var user = await _uow.User.GetUserById(currentUser);
            var wallet = await _uow.Wallet.GetAsync(x => x.CustomerCode == user.UserChannelCode);

            if (wallet == null)
            {
                throw new GenericException("Wallet does not exist", $"{(int)HttpStatusCode.NotFound}");
            }

            var walletDTO = Mapper.Map<WalletDTO>(wallet);
            walletDTO.UserActiveCountryId = user.UserActiveCountryId;
            return walletDTO;
        }

        public async Task<WalletDTO> GetWalletBalance(string userChannelCode)
        {
            var wallet = await _uow.Wallet.GetAsync(x => x.CustomerCode.Equals(userChannelCode));
            if (wallet == null)
            {
                throw new GenericException("Wallet does not exist", $"{(int)HttpStatusCode.NotFound}");
            }

            var walletDTO = Mapper.Map<WalletDTO>(wallet);
            return walletDTO;
        }

        public async Task<WalletDTO> GetWalletBalanceWithName()
        {
            var currentUser = await _userService.GetCurrentUserId();
            var user = await _uow.User.GetUserById(currentUser);
            var wallet = await _uow.Wallet.GetAsync(x => x.CustomerCode.Equals(user.UserChannelCode));
            if (wallet == null)
            {
                throw new GenericException("Wallet does not exist", $"{(int)HttpStatusCode.NotFound}");
            }

            var walletDTO = Mapper.Map<WalletDTO>(wallet);
            var country = await _uow.Country.GetAsync(x => x.CountryId == user.UserActiveCountryId);
            walletDTO.Country = Mapper.Map<CountryDTO>(country);

            if (wallet.CompanyType == CustomerType.IndividualCustomer.ToString())
            {
                var customer = await _uow.IndividualCustomer.GetAsync(x => x.CustomerCode == wallet.CustomerCode);
                walletDTO.CustomerName = customer.FirstName + " " + customer.LastName;
            }
            else
            {
                var customer = await _uow.Company.GetAsync(x => x.CustomerCode == wallet.CustomerCode);
                walletDTO.CustomerName = customer.Name;
            }
            return walletDTO;
        }

        public IQueryable<Core.Domain.Wallet.Wallet> GetWalletAsQueryableService()
        {
            var wallet = _uow.Wallet.GetAllAsQueryable();
            return wallet;
        }

        private async Task CheckIfEcommerceIsEligible(Core.Domain.Wallet.Wallet wallet, decimal amount)
        {
            var company = await _uow.Company.GetAsync(s => s.CustomerCode == wallet.CustomerCode);

            if (company != null)
            {
                if (company.IsEligible == true)
                    return;

                decimal codAmountValue;

                if (company.isCodNeeded)
                {
                    var codAmount = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.EcommerceCodAmount, company.UserActiveCountryId);
                    codAmountValue = Convert.ToDecimal(codAmount.Value);
                }
                else
                {
                    var noCoDAmount = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.EcommerceNoCodAmount, company.UserActiveCountryId);
                    codAmountValue = Convert.ToDecimal(noCoDAmount.Value);
                }

                company.WalletAmount = Convert.ToDecimal(company.WalletAmount) + amount;

                if (company.WalletAmount >= codAmountValue)
                {
                    company.IsEligible = true;
                }
                else
                {
                    company.IsEligible = false;
                }
            }
        }
    }
}