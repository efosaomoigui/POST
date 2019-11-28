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

namespace GIGLS.Services.Implementation.Customers
{
    public class CompanyService : ICompanyService
    {
        private readonly IWalletService _walletService;
        private readonly INumberGeneratorMonitorService _numberGeneratorMonitorService;
        private readonly IPasswordGenerator _passwordGenerator;
        private readonly IUserService _userService;

        private readonly IUnitOfWork _uow;

        public CompanyService(INumberGeneratorMonitorService numberGeneratorMonitorService,
            IWalletService walletService, IPasswordGenerator passwordGenerator, IUserService userService, IUnitOfWork uow)
        {
            _walletService = walletService;
            _numberGeneratorMonitorService = numberGeneratorMonitorService;
            _passwordGenerator = passwordGenerator;
            _userService = userService;
            _uow = uow;
            MapperConfig.Initialize();
        }

        public async Task<CompanyDTO> AddCompany(CompanyDTO company)
        {
            try
            {
                if (await _uow.Company.ExistAsync(c => c.Name.ToLower() == company.Name.Trim().ToLower() || c.PhoneNumber == company.PhoneNumber))
                {
                    throw new GenericException($"{company.Name} or phone number already exist");
                }

                //check if registration is from Giglgo
                if(company.IsFromMobile==true)
                {
                    company.IsRegisteredFromMobile = true;
                }

                var newCompany = Mapper.Map<Company>(company);
                newCompany.CompanyStatus = CompanyStatus.Active;

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
                
                return Mapper.Map<CompanyDTO>(newCompany);
            }
            catch (Exception)
            {
                throw;
            }
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
                //company.UserActiveCountryId = companyDto.UserActiveCountryId;
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
                //user.UserActiveCountryId = companyDto.UserActiveCountryId;

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

                return companyDto;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}