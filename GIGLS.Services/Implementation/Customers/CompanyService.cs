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
using GIGLS.Core.DTO.User;
using Newtonsoft.Json.Linq;
using GIGLS.Core.IServices;
using GIGLS.Core.DTO.Report;

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
        private readonly IPasswordGenerator _codegenerator;
        private readonly IUnitOfWork _uow;

        public CompanyService(INumberGeneratorMonitorService numberGeneratorMonitorService, IWalletService walletService, IPasswordGenerator passwordGenerator,
            IUserService userService, IUnitOfWork uow, IMessageSenderService messageSenderService, IGlobalPropertyService globalPropertyService, IPasswordGenerator codegenerator)
        {
            _walletService = walletService;
            _numberGeneratorMonitorService = numberGeneratorMonitorService;
            _passwordGenerator = passwordGenerator;
            _userService = userService;
            _globalPropertyService = globalPropertyService;
            _messageSenderService = messageSenderService;
            _codegenerator = codegenerator;
            _uow = uow;
            MapperConfig.Initialize();
        }

        public async Task<CompanyDTO> AddCompany(CompanyDTO company)
        {
            try
            {
                //block the registration for APP User
                var gigGoEmailUser = await _uow.User.GetUserByEmail(company.Email);

                if (gigGoEmailUser != null)
                {
                    throw new GenericException($"Email already exist");
                }

                if (await _uow.Company.ExistAsync(c => c.Name.ToLower() == company.Name.Trim().ToLower() || c.PhoneNumber == company.PhoneNumber || c.Email == company.Email))
                {
                    throw new GenericException($"{company.Name}, phone number or email detail already exist");
                }

                //check if registration is from Giglgo
                if (company.IsFromMobile == true)
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
                newCompany.RankModificationDate = DateTime.UtcNow;

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

                if (company.EcommerceAgreementId != 0)
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

            if (company.Status == EcommerceAgreementStatus.Pending)
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
            if (countryId < 1)
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
                if (wallet != null)
                {
                    _uow.Wallet.Remove(wallet);
                }

                var user = await _uow.User.GetUserByChannelCode(company.CustomerCode);
                if (user != null)
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

        public Task<List<CompanyDTO>> GetCompanyByEmail(string email, Rank? rank)
        {
            try
            {
                return _uow.Company.GetCompanyByEmail(email, rank);
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

        public async Task<List<CompanyDTO>> GetCompaniesBy(List<string> codes)
        {
            return await _uow.Company.GetCompaniesByCodes(codes);
        }


        public async Task<ResponseDTO> UnboardUser(NewCompanyDTO company)
        {
            try
            {
                var result = new ResponseDTO();
                if (company == null)
                {
                    result.Succeeded = false;
                    result.Message = "Invalid payload";
                    return result;
                }
                //check if user already exist
                var emailExist = await _uow.User.GetUserByEmail(company.Email);

                if (emailExist != null)
                {
                    result.Succeeded = false;
                    result.Exist = true;
                    result.Message = "Email already exist";
                    return result;
                }

                if (await _uow.Company.ExistAsync(c => c.Name.Trim().ToLower() == company.Name.Trim().ToLower() || c.PhoneNumber == company.PhoneNumber || c.Email.ToLower() == company.Email.ToLower()))
                {
                    result.Succeeded = false;
                    result.Message = $"{company.Name}, phone number or email detail already exist";
                    return result;
                }

                //check phone number existence
                var phoneExist = await _uow.User.GetUserByPhoneNumber(company.PhoneNumber);

                if (phoneExist != null)
                {
                    result.Succeeded = false;
                    result.Exist = true;
                    result.Message = $"Phone Number already exist";
                    return result;
                }

                if (company.Rank.ToString().ToLower() == Rank.Class.ToString().ToLower())
                {
                    if (String.IsNullOrEmpty(company.BVN))
                    {
                        result.Succeeded = false;
                        result.Exist = false;
                        result.Message = $"BVN number not provided for class customer";
                        return result;
                    }
                }
                var industry = string.Join(",", company.Industry);
                var productType = string.Join(",", company.ProductType);

                company.Industry = null;
                company.ProductType = null;
                company.Email = company.Email.ToLower();
                //create company object
                var newCompany = JObject.FromObject(company).ToObject<Company>();
                //check if registration is from Giglgo
                if (company.IsFromMobile == true)
                {
                    newCompany.IsRegisteredFromMobile = true;
                }
                newCompany.CompanyStatus = CompanyStatus.Active;
                //Enable Eligibility so that the customer can create shipment on GIGGO APP
                newCompany.IsEligible = true;
                newCompany.IsDeleted = false;
                newCompany.DateCreated = DateTime.UtcNow;
                newCompany.DateModified = DateTime.UtcNow;
                newCompany.IsInternational = company.isInternational;
                newCompany.ProductType = productType;
                newCompany.Industry = industry;
                newCompany.CompanyType = CompanyType.Ecommerce;
                newCompany.CompanyStatus = CompanyStatus.Active;
                newCompany.CustomerCategory = CustomerCategory.Normal;
                newCompany.ReturnOption = PickupOptions.HOMEDELIVERY.ToString();
                newCompany.CustomerCode = await _numberGeneratorMonitorService.GenerateNextNumber(NumberGeneratorType.CustomerCodeEcommerce);
                newCompany.Rank = Rank.Basic;
                newCompany.RankModificationDate = DateTime.UtcNow;

                //get user country by code
                if (!String.IsNullOrEmpty(company.CountryCode))
                {
                    var userCountry = _uow.Country.GetAll().Where(x => x.CountryCode.ToLower() == company.CountryCode.ToLower()).FirstOrDefault();
                    newCompany.UserActiveCountryId = userCountry.CountryId;
                }

                _uow.Company.Add(newCompany);

                if (!String.IsNullOrEmpty(company.FirstName))
                {
                    CompanyContactPersonDTO personDto = new CompanyContactPersonDTO();
                    personDto.FirstName = newCompany.FirstName;
                    personDto.LastName = newCompany.LastName;
                    personDto.Email = newCompany.Email;
                    personDto.PhoneNumber = newCompany.PhoneNumber;
                    var person = Mapper.Map<CompanyContactPerson>(personDto);
                    person.CompanyId = newCompany.CompanyId;
                    _uow.CompanyContactPerson.Add(person);
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

                var aspUser = await _userService.AddUser(new Core.DTO.User.UserDTO()
                {
                    ConfirmPassword = password,
                    Department = newCompany.CompanyType.ToString(),
                    DateCreated = DateTime.Now,
                    Designation = newCompany.CompanyType.ToString(),
                    Email = newCompany.Email,
                    FirstName = newCompany.FirstName,
                    LastName = newCompany.LastName,
                    Organisation = newCompany.Name,
                    Password = password,
                    PhoneNumber = newCompany.PhoneNumber,
                    UserType = UserType.Regular,
                    Username = newCompany.Email,
                    UserChannelCode = newCompany.CustomerCode,
                    UserChannelPassword = password,
                    UserChannelType = userChannelType,
                    PasswordExpireDate = DateTime.Now,
                    UserActiveCountryId = newCompany.UserActiveCountryId,
                    IsActive = true,
                    IsInternational = newCompany.IsInternational,
                });
                //complete
                _uow.Complete();

                //generate user refferal code
                var userDTO = await _userService.GetUserByChannelCode(newCompany.CustomerCode);
                var code = await _codegenerator.Generate(5);
                var ReferrerCodeExists = await _uow.ReferrerCode.GetAsync(s => s.UserCode == userDTO.UserChannelCode);
                if (ReferrerCodeExists == null)
                {
                    var referrerCodeDTO = new ReferrerCodeDTO
                    {
                        Referrercode = code,
                        UserId = userDTO.Id,
                        UserCode = userDTO.UserChannelCode
                    };
                    var referrercode = Mapper.Map<ReferrerCode>(referrerCodeDTO);
                    _uow.ReferrerCode.Add(referrercode);
                    await _uow.CompleteAsync();
                    userDTO.Referrercode = referrercode.Referrercode;
                    userDTO.RegistrationReferrercode = referrercode.Referrercode;
                }
                else
                {
                    userDTO.Referrercode = ReferrerCodeExists.Referrercode;
                    userDTO.RegistrationReferrercode = ReferrerCodeExists.Referrercode;
                }
                await _userService.UpdateUser(userDTO.Id,userDTO);

                // add customer to a wallet
                await _walletService.AddWallet(new WalletDTO
                {
                    CustomerId = newCompany.CompanyId,
                    CustomerType = CustomerType.Company,
                    CustomerCode = newCompany.CustomerCode,
                    CompanyType = newCompany.CompanyType.ToString(),
                });
                var entity = Mapper.Map<CompanyDTO>(newCompany);
                result.Message = "Signup Successful";
                result.Succeeded = true;
                result.Entity = entity;

                //SEND EMAIL TO NEW SIGNEE
                //send a copy to chairman
                var chairmanEmail = await _uow.GlobalProperty.GetAsync(s => s.Key == GlobalPropertyType.ChairmanEmail.ToString() && s.CountryId == 1);
                var companyMessagingDTO = new CompanyMessagingDTO();
                if (chairmanEmail != null)
                {
                    //seperate email by comma and send message to those email
                    string[] chairmanEmails = chairmanEmail.Value.Split(',').ToArray();
                    foreach (string email in chairmanEmails)
                    {
                        companyMessagingDTO.Emails.Add(email);
                    }
                }
                companyMessagingDTO.Name = company.Name;
                companyMessagingDTO.Email = company.Email;
                companyMessagingDTO.PhoneNumber = company.PhoneNumber;
                companyMessagingDTO.Rank = company.Rank;
                companyMessagingDTO.IsFromMobile = company.IsFromMobile;
                companyMessagingDTO.UserChannelType = userChannelType;
                await SendMessageToNewSignUps(companyMessagingDTO);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ResponseDTO> UpdateUserRank(UserValidationDTO userValidationDTO)
        {
            try
            {
                var result = new ResponseDTO();
                if (userValidationDTO == null)
                {
                    result.Succeeded = false;
                    result.Message = "Invalid payload";
                    return result;
                }

                if (String.IsNullOrEmpty(userValidationDTO.UserID) || userValidationDTO.Rank == null)
                {
                    result.Succeeded = false;
                    result.Message = "User or rank not provided";
                    return result;
                }

                var user = await _userService.GetUserById(userValidationDTO.UserID);
                if (user == null)
                {
                    result.Succeeded = false;
                    result.Message = "User does not exist";
                    return result;
                }

                var company = await  _uow.Company.GetAsync(x => x.CustomerCode == user.UserChannelCode);
                if (company == null)
                {
                    result.Succeeded = false;
                    result.Message = "Company information does not exist";
                    return result;
                }

                if (userValidationDTO.Rank == Rank.Class)
                {
                    //make the BVN compulsory
                    if (String.IsNullOrEmpty(userValidationDTO.BVN))
                    {
                        result.Succeeded = false;
                        result.Message = "User BVN not provided";
                        return result;
                    }
                    company.isCodNeeded = true;
                }
                else
                {
                    company.isCodNeeded = false;
                }
                
                company.Rank = userValidationDTO.Rank;
                company.BVN = userValidationDTO.BVN;
                company.RankModificationDate = DateTime.Now;
                var companyDTO = Mapper.Map<CompanyDTO>(company);
                _uow.RankHistory.Add(new RankHistory
                {
                    CustomerName = companyDTO.Name,
                    CustomerCode = companyDTO.CustomerCode,
                    RankType = userValidationDTO.RankType
                });
                await _uow.CompleteAsync();

                //send email for upgrade customers
                if (userValidationDTO.Rank == Rank.Class)
                {
                    //SEND EMAIL TO CLASS CUSTOMERS
                    //send a copy to chairman
                    var chairmanEmail = await _uow.GlobalProperty.GetAsync(s => s.Key == GlobalPropertyType.ChairmanEmail.ToString() && s.CountryId == 1);
                    var companyMessagingDTO = new CompanyMessagingDTO();
                    if (chairmanEmail != null)
                    {
                        //seperate email by comma and send message to those email
                        string[] chairmanEmails = chairmanEmail.Value.Split(',').ToArray();
                        foreach (string email in chairmanEmails)
                        {
                           companyMessagingDTO.Emails.Add(email);
                        }
                    }
                    var userchannelType = (UserChannelType)Enum.Parse(typeof(UserChannelType), company.CompanyType.ToString());
                    companyMessagingDTO.Name = company.Name;
                    companyMessagingDTO.Email = company.Email;
                    companyMessagingDTO.PhoneNumber = company.PhoneNumber;
                    companyMessagingDTO.Rank = company.Rank;
                    companyMessagingDTO.IsFromMobile = company.IsRegisteredFromMobile;
                    companyMessagingDTO.UserChannelType = userchannelType;
                    companyMessagingDTO.IsUpdate = true;
                    await SendMessageToNewSignUps(companyMessagingDTO); 
                }

                result.Message = "User Rank Update Successful";
                result.Succeeded = true;
                result.Entity = companyDTO;
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task SendMessageToNewSignUps(object obj)
        {
            try
            {
                if (obj is CompanyMessagingDTO)
                {
                    var company = (CompanyMessagingDTO)obj;
                    var messageDTO = new MessageDTO
                    {
                        CustomerName = company.Name,
                        To = company.Email,
                        ToEmail = company.Email
                    };
                    if (company.UserChannelType == UserChannelType.IndividualCustomer)
                    {
                        messageDTO.MessageTemplate = "IndividualWelcome";
                        await _messageSenderService.SendCustomerRegistrationMails(messageDTO);
                        //await _messageSenderService.SendMessage(MessageType.ISA, EmailSmsType.Email, company);
                    }
                    else if (company.UserChannelType == UserChannelType.Partner)
                    {
                        await _messageSenderService.SendMessage(MessageType.PSU, EmailSmsType.Email, company);
                    }
                    else if (company.IsFromMobile)
                    {
                        if(company.Rank == Rank.Class)
                        {
                            messageDTO.MessageTemplate = "ClassWelcomeApp";
                            await _messageSenderService.SendCustomerRegistrationMails(messageDTO);
                        }
                        else if (company.Rank == Rank.Basic)
                        {
                            //await _messageSenderService.SendMessage(MessageType.ESBA, EmailSmsType.Email, company);
                            messageDTO.MessageTemplate = "BasicWelcomeApp";
                            await _messageSenderService.SendCustomerRegistrationMails(messageDTO);
                        }
                    }
                    else
                    {
                        if (company.Rank == Rank.Class)
                        {
                            messageDTO.MessageTemplate = "ClassWelcomeWebiste";
                            await _messageSenderService.SendCustomerRegistrationMails(messageDTO);
                        }
                        else if (company.Rank == Rank.Basic)
                        {
                            messageDTO.MessageTemplate = "BasicWelcomeWebsite";
                            await _messageSenderService.SendCustomerRegistrationMails(messageDTO);
                           //await _messageSenderService.SendMessage(MessageType.ESBW, EmailSmsType.Email, company);
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<CompanyDTO>> GetClassCustomers(ShipmentCollectionFilterCriteria filterCriteria)
        {
            return await _uow.Company.GetCompanies(Rank.Class, filterCriteria);
        }

        public async Task<CompanyDTO> UpgradeToEcommerce(UpgradeToEcommerce newCompanyDTO)
        {
            //get the current login user 
            var currentUserId = await _userService.GetCurrentUserId();
            var user = await _uow.User.GetUserById(currentUserId);
            if (user == null)
            {
                throw new GenericException("user does not exist");
            }
            CompanyDTO companyDTO = new CompanyDTO();
            if (UserChannelType.IndividualCustomer.ToString().ToLower() == user.UserChannelType.ToString().ToLower())
            {
                var individualInfo = await _uow.IndividualCustomer.GetAsync(x => x.CustomerCode == user.UserChannelCode);
                if (individualInfo == null)
                {
                    throw new GenericException("This user is not an Individual customer");
                }

                var nameExist = await _uow.Company.GetAsync(x => x.Name.ToLower() == newCompanyDTO.Name.ToLower());
                if (nameExist != null)
                {
                    throw new GenericException($"Business name {newCompanyDTO.Name.ToUpper()} already exist ");
                }
                var industry = string.Join(",", newCompanyDTO.Industry);
                var productType = string.Join(",", newCompanyDTO.ProductType);
                var company = new Company()
                {
                    CompanyStatus = CompanyStatus.Active,
                    CustomerCode = user.UserChannelCode,
                    Rank = Rank.Basic,
                    RankModificationDate = DateTime.Now,
                    IsEligible = true,
                    IsDeleted = false,
                   // IsInternational = newCompanyDTO.isInternational,
                    ProductType = productType,
                    Industry = industry,
                    CompanyType = CompanyType.Ecommerce,
                    CustomerCategory = CustomerCategory.Normal,
                    ReturnOption = newCompanyDTO.ReturnOption.ToString(),
                    ReturnServiceCentre = newCompanyDTO.ReturnServiceCentre,
                    ReturnAddress = newCompanyDTO.ReturnAddress,
                    Name = newCompanyDTO.Name,
                    Email = user.Email,
                    FirstName = newCompanyDTO.FirstName,
                    LastName = newCompanyDTO.LastName,
                    City = individualInfo.City,
                    State = individualInfo.State,
                    Address = individualInfo.Address,
                    UserActiveCountryId = user.UserActiveCountryId,
                    isCodNeeded = false,
                    IsRegisteredFromMobile = individualInfo.IsRegisteredFromMobile,
                    PhoneNumber = user.PhoneNumber

                };
                if (!String.IsNullOrEmpty(user.FirstName))
                {
                    CompanyContactPersonDTO personDto = new CompanyContactPersonDTO();
                    personDto.FirstName = newCompanyDTO.FirstName;
                    personDto.LastName = newCompanyDTO.LastName;
                    personDto.Email = user.Email;
                    personDto.PhoneNumber = user.PhoneNumber;
                    var person = Mapper.Map<CompanyContactPerson>(personDto);
                    person.CompanyId = company.CompanyId;
                    _uow.CompanyContactPerson.Add(person);
                }

                //also update orgnization,designation,department
                individualInfo.IsRegisteredFromMobile = false;
                user.UserChannelType = UserChannelType.Ecommerce;
                user.Organisation = newCompanyDTO.Name;
                user.Department = UserChannelType.Ecommerce.ToString();
                user.Designation = UserChannelType.Ecommerce.ToString();
                user.FirstName = newCompanyDTO.FirstName;
                user.LastName = newCompanyDTO.LastName;
               _uow.Company.Add(company);
                await _uow.CompleteAsync();

                //update customer wallet
                var wallet = await _uow.Wallet.GetAsync(x => x.CustomerCode == individualInfo.CustomerCode);
                if (wallet != null)
                {
                    var newCompany = await _uow.Company.GetAsync(x => x.CustomerCode == individualInfo.CustomerCode);
                    if (newCompany != null)
                    {
                        wallet.CustomerType = CustomerType.Company;
                        wallet.CompanyType = CompanyType.Ecommerce.ToString();
                        wallet.CustomerId = newCompany.CompanyId;
                        await _uow.CompleteAsync();
                    }
                }
                companyDTO = Mapper.Map<CompanyDTO>(company);
            }
            return companyDTO;
        }
    }
        
}