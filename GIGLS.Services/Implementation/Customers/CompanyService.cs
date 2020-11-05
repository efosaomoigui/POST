using System;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Customers;
using GIGLS.Core.IServices.Customers;
using GIGLS.Core;
using System.Collections.Generic;
using System.Linq;
using GIGL.GIGLS.Core.Domain;
using GIGLS.Core.Enums;
using GIGLS.Infrastructure;
using AutoMapper;
using GIGLS.Core.DTO.Wallet;
using GIGLS.Core.IServices.Wallet;
using GIGLS.Core.IServices.Utility;
using GIGLS.Core.IServices.User;
using GIGLS.CORE.DTO.Report;
using GIGLS.Core.IMessageService;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO;

namespace GIGLS.Services.Implementation.Customers
{
    public class CompanyService : ICompanyService
    {
        private readonly IWalletService _walletService;
        private readonly INumberGeneratorMonitorService _numberGeneratorMonitorService;
        private readonly IPasswordGenerator _passwordGenerator;
        private readonly IUserService _userService;
        private readonly IMessageSenderService _messageSenderService;
        private readonly IGlobalPropertyService _globalPropertyService;
        private readonly IUnitOfWork _uow;

        public CompanyService(INumberGeneratorMonitorService numberGeneratorMonitorService, IWalletService walletService, IPasswordGenerator passwordGenerator, 
            IUserService userService, IUnitOfWork uow, IMessageSenderService messageSenderService, IGlobalPropertyService globalPropertyService)
        {
            _walletService = walletService;
            _numberGeneratorMonitorService = numberGeneratorMonitorService;
            _passwordGenerator = passwordGenerator;
            _userService = userService;
            _globalPropertyService = globalPropertyService;
            _messageSenderService = messageSenderService;
            _uow = uow;
            MapperConfig.Initialize();
        }

        public async Task<CompanyDTO> AddCompany(CompanyDTO company)
        {
            try
            {
                //block the registration for APP User
                var gigGoEmailUser = await _uow.User.GetUserByEmail(company.Email);

                if(gigGoEmailUser != null)
                {
                    throw new GenericException($"Email already exist");
                }

                if (await _uow.Company.ExistAsync(c => c.Name.ToLower() == company.Name.Trim().ToLower() || c.PhoneNumber == company.PhoneNumber || c.Email == company.Email))
                {
                    throw new GenericException($"{company.Name}, phone number or email detail already exist");
                }

                //check if registration is from Giglgo
                if(company.IsFromMobile == true)
                {
                    company.IsRegisteredFromMobile = true;
                }

                //update the customer update to have country code added to it
                if (company.PhoneNumber.StartsWith("0"))
                {
                    company.PhoneNumber = await AddCountryCodeToPhoneNumber(company.PhoneNumber, company.UserActiveCountryId);
                }

                //check phone number existence
                var gigGoPhoneUser = await _uow.User.GetUserByPhoneNumber(company.PhoneNumber);

                if (gigGoPhoneUser != null)
                {
                    throw new GenericException($"Phone Number already exist");
                }

                var newCompany = Mapper.Map<Company>(company);
                newCompany.CompanyStatus = CompanyStatus.Active;

                //Enable Eligibility so that the customer can create shipment on GIGGO APP
                newCompany.IsEligible = true;

                //get the CompanyType
                var companyType = "";

                //generate customer code
                if (newCompany.CompanyType == CompanyType.Corporate)
                {
                    var customerCode = await _numberGeneratorMonitorService.GenerateNextNumber(
                        NumberGeneratorType.CustomerCodeCorporate);
                    newCompany.CustomerCode = customerCode;
                    companyType = CompanyType.Corporate.ToString();
                }
                else
                {
                    var customerCode = await _numberGeneratorMonitorService.GenerateNextNumber(
                        NumberGeneratorType.CustomerCodeEcommerce);
                    newCompany.CustomerCode = customerCode;
                    companyType = CompanyType.Ecommerce.ToString();
                }

                _uow.Company.Add(newCompany);

                if (company.ContactPersons.Any())
                {
                    foreach (CompanyContactPersonDTO personDto in company.ContactPersons)
                    {
                        var person = Mapper.Map<CompanyContactPerson>(personDto);
                        person.CompanyId = newCompany.CompanyId;
                        _uow.CompanyContactPerson.Add(person);
                    }
                }

                //-- add to user table for login
                //1. set the userChannelType
                var userChannelType = UserChannelType.Corporate;
                if (newCompany.CompanyType == CompanyType.Ecommerce)
                {
                    userChannelType = UserChannelType.Ecommerce;
                }

                //2. If userEmail is null, use CustomerCode
                if (String.IsNullOrEmpty(newCompany.Email))
                {
                    newCompany.Email = newCompany.CustomerCode;
                }
                var password = "";
                if (newCompany.Password == null)
                {
                    password = await _passwordGenerator.Generate();
                }
                else
                {
                    password = newCompany.Password;
                }

                if(company.EcommerceAgreementId != 0)
                {
                    var pendingRequest = await CheckEcommerceAgreement(company.EcommerceAgreementId);
                }

                var result = await _userService.AddUser(new Core.DTO.User.UserDTO()
                {
                    ConfirmPassword = password,
                    Department = newCompany.CompanyType.ToString(),
                    DateCreated = DateTime.Now,
                    Designation = newCompany.CompanyType.ToString(),
                    Email = newCompany.Email,
                    FirstName = newCompany.Name,
                    LastName = newCompany.Name,
                    Organisation = newCompany.CompanyType.ToString(),
                    Password = password,
                    PhoneNumber = newCompany.PhoneNumber,
                    UserType = UserType.Regular,
                    Username = newCompany.Email,
                    UserChannelCode = newCompany.CustomerCode,
                    UserChannelPassword = password,
                    UserChannelType = userChannelType,
                    PasswordExpireDate = DateTime.Now,
                    UserActiveCountryId = newCompany.UserActiveCountryId,
                    IsActive = true
                });

                //complete
                _uow.Complete();

                // add customer to a wallet
                await _walletService.AddWallet(new WalletDTO
                {
                    CustomerId = newCompany.CompanyId,
                    CustomerType = CustomerType.Company,
                    CustomerCode = newCompany.CustomerCode,
                    CompanyType = companyType
                });

                var message = new MessageDTO()
                {
                    CustomerCode = newCompany.CustomerCode,
                    CustomerName = newCompany.Name,
                    ToEmail = newCompany.Email,
                    To = newCompany.Email,
                    Body = password
                };
                await _messageSenderService.SendEcommerceRegistrationNotificationAsync(message);

                //send mail to ecommerce team
                var ecommerceEmail = await _uow.GlobalProperty.GetAsync(s => s.Key == GlobalPropertyType.EcommerceEmail.ToString() && s.CountryId == 1);

                if (ecommerceEmail != null)
                {
                    //seperate email by comma and send message to those email
                    string[] ecommerceEmails = ecommerceEmail.Value.Split(',').ToArray();

                    foreach (string data in ecommerceEmails)
                    {
                        message.ToEmail = data;
                        await _messageSenderService.SendEcommerceRegistrationNotificationAsync(message);
                    }
                }

                return Mapper.Map<CompanyDTO>(newCompany);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<EcommerceAgreement> CheckEcommerceAgreement(int companyId)
        {
            var company = await _uow.EcommerceAgreement.GetAsync(x => x.EcommerceAgreementId == companyId);

            if(company.Status == EcommerceAgreementStatus.Pending)
            {
                company.Status = EcommerceAgreementStatus.Registerd;
            }
            else
            {
                throw new GenericException("Customer already Registered");
            }
            return company;
        }

        public async Task<string> AddCountryCodeToPhoneNumber(string phoneNumber, int countryId)
        {
            if(countryId < 1)
            {
                int getUserActiveCountry = await _userService.GetUserActiveCountryId();
                countryId = getUserActiveCountry;
            }

            var country = await _uow.Country.GetAsync(x => x.CountryId == countryId);
            if (country != null)
            {
                phoneNumber = phoneNumber.Substring(1, phoneNumber.Length - 1);
                string phone = $"{country.PhoneNumberCode}{phoneNumber}";
                phoneNumber = phone;

            }
            return phoneNumber;
        }

        public async Task DeleteCompany(int companyId)
        {
            try
            {
                //Delete user, wallet and customer table
                
                var company = await _uow.Company.GetAsync(companyId);
                if (company == null)
                {
                    throw new GenericException("Company information does not exist");
                }
                _uow.Company.Remove(company);

                var wallet = await _uow.Wallet.GetAsync(x => x.CustomerCode == company.CustomerCode);
                if(wallet != null)
                {
                    _uow.Wallet.Remove(wallet);
                }
                
                var user = await _uow.User.GetUserByChannelCode(company.CustomerCode);
                if(user != null)
                {
                   await _uow.User.Remove(user.Id);
                }

                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<List<CompanyDTO>> GetCompanies()
        {
            return _uow.Company.GetCompanies();
        }

        public Task<List<CompanyDTO>> GetCompanies(BaseFilterCriteria filterCriteria)
        {
            return _uow.Company.GetCompanies(filterCriteria);
        }

        public async Task<List<EcommerceAgreementDTO>> GetPendingEcommerceRequest(BaseFilterCriteria filterCriteria)
        {
            return await _uow.Company.GetPendingEcommerceRequest(filterCriteria);
        }

        public Task<List<CompanyDTO>> GetCompaniesWithoutWallet()
        {
            var companies = _uow.Company.GetAll().ToList();
            return Task.FromResult(Mapper.Map<List<CompanyDTO>>(companies));
        }

        public async Task<List<CompanyDTO>> GetEcommerceWithoutWallet()
        {
            var companies = await _uow.Company.FindAsync(x => x.CompanyType == CompanyType.Ecommerce);
            return Mapper.Map<List<CompanyDTO>>(companies);
        }

        public async Task<List<CompanyDTO>> GetCorporateWithoutWallet()
        {
            var companies = await _uow.Company.FindAsync(x => x.CompanyType == CompanyType.Corporate);
            return Mapper.Map<List<CompanyDTO>>(companies);
        }

        public async Task<CompanyDTO> GetCompanyById(int companyId)
        {
            try
            {
                var company = await _uow.Company.GetCompanyById(companyId);
                
                if (company == null)
                {
                    throw new GenericException("Company information does not exist");
                }

                CompanyDTO companyDto = Mapper.Map<CompanyDTO>(company);
                companyDto.UserActiveCountryName = companyDto.Country.CountryName;
                
                return companyDto;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateCompany(int companyId, CompanyDTO companyDto)
        {
            try
            {
                var company = await _uow.Company.GetAsync(companyId);
                if (company == null || companyId != companyDto.CompanyId)
                {
                    throw new GenericException("Company information does not exist");
                }
                company.Name = companyDto.Name;
                company.PhoneNumber = companyDto.PhoneNumber;
                company.Address = companyDto.Address;
                company.City = companyDto.City;
                company.CompanyType = companyDto.CompanyType;
                company.Discount = companyDto.Discount;
                company.Email = companyDto.Email;
                company.Industry = companyDto.Industry;
                company.CompanyStatus = companyDto.CompanyStatus;
                company.State = companyDto.State;
                company.SettlementPeriod = companyDto.SettlementPeriod;
                company.CustomerCategory = companyDto.CustomerCategory;
                company.ReturnOption = companyDto.ReturnOption;
                company.ReturnServiceCentre = companyDto.ReturnServiceCentre;
                company.ReturnAddress = companyDto.ReturnAddress;
                company.RcNumber = companyDto.RcNumber;
                company.isCodNeeded = companyDto.isCodNeeded;

                if (companyDto.ContactPersons.Any())
                {
                    foreach (CompanyContactPersonDTO personDto in companyDto.ContactPersons)
                    {
                        var person = await _uow.CompanyContactPerson.GetAsync(personDto.CompanyContactPersonId);
                        person.FirstName = personDto.FirstName;
                        person.LastName = personDto.LastName;
                        person.Email = personDto.Email;
                        person.Designation = personDto.Designation;
                        person.PhoneNumber = personDto.PhoneNumber;
                        person.CompanyId = personDto.CompanyId;
                    }
                }

                //Update user 
                var user = await _userService.GetUserByChannelCode(company.CustomerCode);
                user.PhoneNumber = companyDto.PhoneNumber;
                user.LastName = companyDto.Name;
                user.FirstName = companyDto.Name;
                user.Email = companyDto.Email;

                await _userService.UpdateUser(user.Id, user);
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateCompanyStatus(int companyId, CompanyStatus status)
        {
            try
            {
                var company = await _uow.Company.GetAsync(companyId);
                if (company == null)
                {
                    throw new GenericException("Company information does not exist");
                }
                company.CompanyStatus = status;
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<CompanyDTO>> GetCompanies(CompanyType companyType, CustomerSearchOption searchOption)
        {
            return await _uow.Company.GetCompanies(companyType, searchOption);
        }

        private async Task<int> CodeToAddCorporateUsersToAspNetUsersTable()
        {
            var listOfCompanies = await _uow.Company.GetCompanies(); ;

            var listOfUsers = await _userService.GetUsers();

            //only add those companies whose CustomerCode do not exists in AspNet Users table
            foreach (var company in listOfCompanies)    // Start Foreach loop
            {
                if (listOfUsers.Select(s => s.UserChannelCode).Contains(company.CustomerCode))
                {
                    //user already in the system
                    continue;
                }

                try
                {
                    var newCompany = Mapper.Map<Company>(company);

                    //get the CompanyType
                    var companyType = "";

                    if (newCompany.CompanyType == CompanyType.Corporate)
                    {
                        companyType = CompanyType.Corporate.ToString();
                    }
                    else
                    {
                        companyType = CompanyType.Ecommerce.ToString();
                    }

                    //-- add to user table for login
                    //1. set the userChannelType
                    var userChannelType = UserChannelType.Corporate;
                    if (newCompany.CompanyType == CompanyType.Ecommerce)
                    {
                        userChannelType = UserChannelType.Ecommerce;
                    }

                    //2. If userEmail is null, use CustomerCode
                    if (String.IsNullOrEmpty(newCompany.Email))
                    {
                        //newCompany.Email = newCompany.CustomerCode;
                        newCompany.Email = null;
                    }

                    try
                    {
                        var password = await _passwordGenerator.Generate();
                        var result = await _userService.AddUser(new Core.DTO.User.UserDTO()
                        {
                            ConfirmPassword = password,
                            Department = newCompany.CompanyType.ToString(),
                            DateCreated = DateTime.Now,
                            Designation = newCompany.CompanyType.ToString(),
                            Email = newCompany.Email,
                            FirstName = newCompany.Name,
                            LastName = newCompany.Name,
                            Organisation = newCompany.CompanyType.ToString(),
                            Password = password,
                            PhoneNumber = newCompany.PhoneNumber,
                            UserType = UserType.Regular,
                            Username = newCompany.CustomerCode,
                            UserChannelCode = newCompany.CustomerCode,
                            UserChannelPassword = password,
                            UserChannelType = userChannelType,
                            UserActiveCountryId = newCompany.UserActiveCountryId
                        });
                    }
                    catch (Exception)
                    {
                        //throw;
                    }

                }
                catch (Exception)
                {
                    throw;
                }

            }   // End Foreach loop

            return await Task.FromResult(0);
        }

        public async Task<CompanyDTO> GetCompanyByCode(string customerCode)
        {
            try
            {
                var company = await _uow.Company.GetCompanyByCode(customerCode);

                if (company == null)
                {
                    return new CompanyDTO { };
                }

                CompanyDTO companyDto = Mapper.Map<CompanyDTO>(company);
                companyDto.UserActiveCountryName = companyDto.Country.CountryName;
                companyDto.CurrencySymbol = companyDto.Country.CurrencySymbol;
                return companyDto;
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        public async Task<EcommerceWalletDTO> GetECommerceWalletById(int companyId)
        {
            try
            {
                var company = await _uow.Company.GetWalletDetailsForCompany(companyId);

                if (company == null)
                {
                    throw new GenericException("Wallet information does not exist");
                }
                return company;                
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<List<CompanyDTO>> GetCompanyByEmail(string email)
        {
            try
            {
                return _uow.Company.GetCompanyByEmail(email);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<EcommerceAgreementDTO> GetCustomerPendingRequestsById(int companyId)
        {
            try
            {
                var company = await _uow.Company.GetPendingEcommerceRequestById(companyId);

                if (company == null)
                {
                    throw new GenericException("Company information does not exist");
                }

                var companyDto = Mapper.Map<EcommerceAgreementDTO>(company);

                return companyDto;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CompanyDTO> UpdateCompanyRank(string customerCode,Rank rank)
        {
            try
            {
                if (String.IsNullOrEmpty(customerCode))
                {
                    throw new GenericException("Customer code not provided");
                }
                var company = await _uow.Company.GetCompanyByCode(customerCode);
                if (company == null)
                {
                    throw new GenericException("Company information does not exist");
                }
                company.Rank = rank;             
                _uow.Complete();
                var companyDto = Mapper.Map<CompanyDTO>(company);

                return companyDto;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}