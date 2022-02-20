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
using System.Data.Entity;
using GIGLS.Core.DTO.OnlinePayment;
using GIGLS.Core.IServices.Node;
using System.Net;
using GIGLS.Core.IServices.Node;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.IServices.Alpha;
using System.Configuration;

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
        private readonly IPaystackPaymentService _paystackPaymentService;
        private readonly INodeService _nodeService;
        private readonly IAlphaService _alphsService;
        private readonly IUnitOfWork _uow;

        public CompanyService(INumberGeneratorMonitorService numberGeneratorMonitorService, IWalletService walletService, IPasswordGenerator passwordGenerator,
            IUserService userService, IUnitOfWork uow, IMessageSenderService messageSenderService, IGlobalPropertyService globalPropertyService, IPasswordGenerator codegenerator, IPaystackPaymentService paystackPaymentService, INodeService nodeService)
        {
            _walletService = walletService;
            _numberGeneratorMonitorService = numberGeneratorMonitorService;
            _passwordGenerator = passwordGenerator;
            _userService = userService;
            _globalPropertyService = globalPropertyService;
            _messageSenderService = messageSenderService;
            _codegenerator = codegenerator;
            _paystackPaymentService = paystackPaymentService;
            _nodeService = nodeService;
            _alphsService = alphsService;
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

                if (newCompany.CompanyType == CompanyType.Corporate)
                {
                    var user = await _userService.GetUserByChannelCode(newCompany.CustomerCode);
                    var corporate = await _uow.Company.GetAsync(x => x.CustomerCode == newCompany.CustomerCode);

                    //first create customer on paystack if customer doesnt exist already
                    var nubanAcc = new CreateNubanAccountDTO()
                    {
                        customer = 0,
                        email = company.Email,
                        preferred_bank = company.PrefferedNubanBank,
                        first_name = company.Name,
                        last_name = company.Name,
                        phone = company.PhoneNumber
                    };

                    var nubanCustomer = await _paystackPaymentService.CreateNubanCustomer(nubanAcc);
                    if (nubanCustomer.succeeded)
                    {
                        //then call api to create the nuban account for corporate customers
                        newCompany.NUBANCustomerId = nubanCustomer.data.id;
                        newCompany.NUBANCustomerCode = nubanCustomer.data.customer_code;
                        nubanAcc.customer = nubanCustomer.data.id;
                        var customerNubanAccount = await _paystackPaymentService.CreateUserNubanAccount(nubanAcc);
                        if (customerNubanAccount.succeeded)
                        {
                            if (company != null)
                            {
                                newCompany.PrefferedNubanBank = company.PrefferedNubanBank;
                                newCompany.NUBANAccountNo = customerNubanAccount.data.account_number;
                                newCompany.NUBANCustomerName = customerNubanAccount.data.account_name;
                                message.BankName = company.PrefferedNubanBank;
                                message.AccountName = customerNubanAccount.data.account_name;
                                message.AccountNo = customerNubanAccount.data.account_number;
                                message.ToEmail = company.Email;
                                message.IsCoporate = true;
                            }
                        }
                    }
                    var msgObj = new MessageDTO();
                    msgObj.Body = password;
                    msgObj.ToEmail = company.Email;
                    msgObj.IsCoporate = true;
                    msgObj.CustomerCode = newCompany.CustomerCode;
                    msgObj.CustomerName = newCompany.Name;
                    msgObj.BankName = message.BankName;
                    msgObj.AccountName = message.AccountName;
                    msgObj.AccountNo = message.AccountNo;
                    msgObj.MessageTemplate = "CorporateWelcome";
                    _uow.Complete();
                    await _messageSenderService.SendConfigCorporateSignUpMessage(msgObj);
                }
                if (newCompany.CompanyType == CompanyType.Ecommerce)
                {
                    if (company.Rank == Rank.Class)
                    {
                        await SendEmailToAssignEcommerceCustomerRep(newCompany);
                    }
                    await _messageSenderService.SendEcommerceRegistrationNotificationAsync(message);
                }
                return Mapper.Map<CompanyDTO>(newCompany);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task SendEmailToAssignEcommerceCustomerRep(Company newCompany)
        {
            var message = new MessageDTO()
            {
                ToEmail = newCompany.Email,
                To = newCompany.Email,
            };

            //Send mail to ecommerce customer with an assigned customer rep
            //1. Get a customer rep to assign to ecommerce customer

            var customerRepDTO = await GetEcommerceCustomerRep();
            if (customerRepDTO == null)
            {
                throw new GenericException("No customer rep to be assigned");
            }

            message.EcommerceMessage = new EcommerceCustomerRepMessageDTO
            {
                AccountOfficerName = $"{customerRepDTO?.FirstName} {customerRepDTO?.LastName}",
                AccountOfficerNumber = customerRepDTO?.PhoneNumber,
                AccountOfficerEmail = customerRepDTO?.Email
            };

            //2. Send Mail
            message.MessageTemplate = "AssignEcommerceToCustomerRep";
            await _messageSenderService.SendMailsEcommerceCustomerRep(message);

            //3. After successful email sending
            //Store customer rep id on Company AssignedCustomerRep
            newCompany.AssignedCustomerRep = customerRepDTO.Id;
            await UpdateCompany(newCompany.CompanyId, Mapper.Map<CompanyDTO>(newCompany));

            //Increase Customer rep assigned ecommerce
            customerRepDTO.AssignedEcommerceCustomer += 1;
            await _userService.UpdateUser(customerRepDTO.Id, customerRepDTO);
            //complete
            await _uow.CompleteAsync();
        }

        private async Task<UserDTO> GetEcommerceCustomerRep()
        {
            var ecommerceCustomerRepEmail = await _uow.GlobalProperty.GetAsync(s => s.Key == GlobalPropertyType.EcommerceCustomerRep.ToString() && s.CountryId == 1);
            string[] ecommerceCustomerRepEmails = { };
            if (ecommerceCustomerRepEmail != null)
            {
                ecommerceCustomerRepEmails = ecommerceCustomerRepEmail.Value.Split(',').ToArray();
            }
            var customerRep = _uow.User.GetCorporateCustomerUsersAsQueryable().Where(s => ecommerceCustomerRepEmails.Contains(s.Email)).OrderBy(s => s.AssignedEcommerceCustomer).FirstOrDefault();
            //customerReps = customerReps.AsEnumerable();
            // var customerRep = customerReps.;

            if (customerRep == null)
            {
                throw new GenericException("No customer rep to be assigned");
            }
            return Mapper.Map<UserDTO>(customerRep);
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

        public Task<List<CompanyDTO>> GetCompaniesByEmailOrCode(string searchParams)
        {
            return _uow.Company.GetCompaniesByEmailOrCode(searchParams);
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
                var user = await _uow.User.GetUserByChannelCode(company.CustomerCode);
                if (user != null)
                {
                    UserDTO userDto = Mapper.Map<UserDTO>(user);
                    user.PhoneNumber = companyDto.PhoneNumber;
                    user.LastName = companyDto.Name;
                    user.FirstName = companyDto.Name;
                    user.Email = companyDto.Email;
                    await _userService.UpdateUser(user.Id, userDto);
                }

                if (companyDto.CompanyType == CompanyType.Corporate)
                {

                    var msgObj = new MessageDTO();
                    msgObj.ToEmail = company.Email;
                    msgObj.IsCoporate = true;
                    msgObj.CustomerCode = company.CustomerCode;
                    msgObj.Body = company.Password;
                    msgObj.MessageTemplate = "CorporateEditDetails";

                    //first create customer on paystack if customer doesnt exist already
                    var nubanAcc = new CreateNubanAccountDTO()
                    {
                        customer = 0,
                        email = company.Email,
                        preferred_bank = companyDto.PrefferedNubanBank,
                        first_name = company.Name,
                        last_name = company.Name,
                        phone = company.PhoneNumber
                    };
                    if (String.IsNullOrEmpty(company.NUBANCustomerCode))
                    {
                        var nubanCustomer = await _paystackPaymentService.CreateNubanCustomer(nubanAcc);
                        if (nubanCustomer.succeeded)
                        {
                            company.NUBANCustomerId = nubanCustomer.data.id;
                            company.NUBANCustomerCode = nubanCustomer.data.customer_code;
                            nubanAcc.customer = nubanCustomer.data.id;
                            var customerNubanAccount = await _paystackPaymentService.CreateUserNubanAccount(nubanAcc);
                            if (customerNubanAccount.succeeded)
                            {
                                if (company != null)
                                {
                                    company.PrefferedNubanBank = companyDto.PrefferedNubanBank;
                                    company.NUBANAccountNo = customerNubanAccount.data.account_number;
                                    company.AccountName = customerNubanAccount.data.account_name;
                                    msgObj.AccountName = company.NUBANCustomerName;
                                    msgObj.AccountNo = company.NUBANAccountNo;
                                    msgObj.BankName = company.PrefferedNubanBank;
                                    msgObj.CustomerName = company.Name;
                                    await _messageSenderService.SendConfigCorporateNubanAccMessage(msgObj);
                                    company.NUBANCustomerName = customerNubanAccount.data.account_name;
                                }
                            }
                        }
                    }
                    else if (!String.IsNullOrEmpty(company.NUBANCustomerCode) && String.IsNullOrEmpty(company.NUBANAccountNo))
                    {
                        nubanAcc.customer = company.NUBANCustomerId;
                        var customerNubanAccount = await _paystackPaymentService.CreateUserNubanAccount(nubanAcc);
                        if (customerNubanAccount.succeeded)
                        {
                            if (company != null)
                            {
                                company.PrefferedNubanBank = company.PrefferedNubanBank;
                                company.NUBANAccountNo = customerNubanAccount.data.account_number;
                                company.NUBANCustomerName = customerNubanAccount.data.account_name;
                                msgObj.AccountName = company.NUBANCustomerName;
                                msgObj.AccountNo = company.NUBANAccountNo;
                                msgObj.BankName = company.PrefferedNubanBank;
                                msgObj.CustomerName = company.Name;
                                await _messageSenderService.SendConfigCorporateNubanAccMessage(msgObj);
                            }
                        }
                    }

                }

                _uow.Complete();
            }
            catch (Exception ex)
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
                await _userService.UpdateUser(userDTO.Id, userDTO);

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
                var companyMessagingDTO = new CompanyMessagingDTO();
                //SEND EMAIL TO NEW SIGNEE
                //send a copy to chairman
                // var chairmanEmail = await _uow.GlobalProperty.GetAsync(s => s.Key == GlobalPropertyType.ChairmanEmail.ToString() && s.CountryId == 1);
                //if (chairmanEmail != null)
                //{
                //    //seperate email by comma and send message to those email
                //    string[] chairmanEmails = chairmanEmail.Value.Split(',').ToArray();
                //    foreach (string email in chairmanEmails)
                //    {
                //        companyMessagingDTO.Emails.Add(email);
                //    }
                //}
                companyMessagingDTO.Name = company.Name;
                companyMessagingDTO.Email = company.Email;
                companyMessagingDTO.PhoneNumber = company.PhoneNumber;
                companyMessagingDTO.Rank = company.Rank;
                companyMessagingDTO.IsFromMobile = company.IsFromMobile;
                companyMessagingDTO.UserChannelType = userChannelType;
                await SendMessageToNewSignUps(companyMessagingDTO);

                //Send Email To Assign Ecommerce Customer Rep to class customers
                if (company.Rank.ToString().ToLower() == Rank.Class.ToString().ToLower())
                {
                    await SendEmailToAssignEcommerceCustomerRep(newCompany);
                }
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

                if (String.IsNullOrEmpty(userValidationDTO.UserID) && String.IsNullOrEmpty(userValidationDTO.UserCode))
                {
                    result.Succeeded = false;
                    result.Message = "User not provided";
                    return result;
                }

                if (userValidationDTO.Rank == null)
                {
                    result.Succeeded = false;
                    result.Message = "Rank not provided";
                    return result;
                }

                var user = new UserDTO();
                if (!String.IsNullOrEmpty(userValidationDTO.UserID) && String.IsNullOrEmpty(userValidationDTO.UserCode))
                {
                    user = await _userService.GetUserById(userValidationDTO.UserID);
                    if (user == null || String.IsNullOrEmpty(user.UserChannelCode))
                    {
                        result.Succeeded = false;
                        result.Message = "User does not exist";
                        return result;
                    }
                }
                else if (String.IsNullOrEmpty(userValidationDTO.UserID) && !String.IsNullOrEmpty(userValidationDTO.UserCode))
                {
                    user = await _userService.GetUserByChannelCode(userValidationDTO.UserCode);
                    if (user == null || String.IsNullOrEmpty(user.UserChannelCode))
                    {
                        result.Succeeded = false;
                        result.Message = "User does not exist";
                        return result;
                    }
                }
                else
                {
                    user = await _userService.GetUserById(userValidationDTO.UserID);
                    if (user == null || String.IsNullOrEmpty(user.UserChannelCode))
                    {
                        result.Succeeded = false;
                        result.Message = "User does not exist";
                        return result;
                    }
                }

                var company = await _uow.Company.GetAsync(x => x.CustomerCode == user.UserChannelCode);
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

                //Call Alpha Api to Notify them of merchant subscription
                await _alphsService.UpdateUserSubscription(new Core.DTO.Alpha.AlphaSubscriptionUpdateDTO
                {
                    Amount = Convert.ToInt32(ConfigurationManager.AppSettings["AlphaSubAmount"]),
                    CustomerCode = String.IsNullOrEmpty(company?.CustomerCode) ? "" : company?.CustomerCode,
                    SubscriptionPlan = company.Rank.ToString().ToLower(),
                    ExpiryDate = DateTime.Now.AddMonths(1),
                    FirstName = String.IsNullOrEmpty(company?.FirstName) ? "" : company?.FirstName,
                    LastName = String.IsNullOrEmpty(company?.LastName) ? "" : company?.LastName,
                    Email = String.IsNullOrEmpty(company?.Email) ? "" : company?.Email,
                    Phone = String.IsNullOrEmpty(company?.PhoneNumber) ? "" : company?.PhoneNumber
                }) ;
                
                //send email for upgrade customers
                if (userValidationDTO.Rank == Rank.Class)
                {
                    //SEND EMAIL TO CLASS CUSTOMERS
                    var companyMessagingDTO = new CompanyMessagingDTO();
                    //send a copy to chairman
                    //var chairmanEmail = await _uow.GlobalProperty.GetAsync(s => s.Key == GlobalPropertyType.ChairmanEmail.ToString() && s.CountryId == 1);
                    //if (chairmanEmail != null)
                    //{
                    //    //seperate email by comma and send message to those email
                    //    string[] chairmanEmails = chairmanEmail.Value.Split(',').ToArray();
                    //    foreach (string email in chairmanEmails)
                    //    {
                    //        companyMessagingDTO.Emails.Add(email);
                    //    }
                    //}
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

                //Send mail to class customers with an assigned customer rep
                if (userValidationDTO.Rank == Rank.Class)
                {
                    //CHeck if already assigned a customer rep
                    if (string.IsNullOrEmpty(company.AssignedCustomerRep))
                    {
                        await SendEmailToAssignEcommerceCustomerRep(company);
                    }
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
                        if (company.Rank == Rank.Class)
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

                if (String.IsNullOrEmpty(newCompanyDTO.Name))
                {
                    throw new GenericException($"Business name is required");
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
                    IsInternational = user.IsInternational,
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

                //Call Alpha Api to Notify them of merchant subscription
                await _alphsService.UpdateUserSubscription(new Core.DTO.Alpha.AlphaSubscriptionUpdateDTO
                {
                    Amount = Convert.ToInt32(ConfigurationManager.AppSettings["AlphaSubAmount"]),
                    CustomerCode = String.IsNullOrEmpty(company?.CustomerCode) ? "": company?.CustomerCode,
                    SubscriptionPlan = company.Rank.ToString().ToLower(),
                    ExpiryDate = DateTime.Now.AddMonths(1),
                    FirstName = String.IsNullOrEmpty(company?.FirstName) ? "" : company?.FirstName,
                    LastName = String.IsNullOrEmpty(company?.LastName) ? "" : company?.LastName,
                    Email = String.IsNullOrEmpty(company?.Email) ? "" : company?.Email,
                    Phone = String.IsNullOrEmpty(company?.PhoneNumber) ? "" : company?.PhoneNumber
                });

                companyDTO = Mapper.Map<CompanyDTO>(company);
            }
            return companyDTO;
        }

        public async Task<List<CompanyDTO>> GetAssignedCustomers(BaseFilterCriteria filterCriteria)
        {
            //get the current login user 
            var currentUserId = await _userService.GetCurrentUserId();
            var user = await _uow.User.GetUserById(currentUserId);
            if (user == null)
            {
                throw new GenericException("user does not exist");
            }
            filterCriteria.AssignedCustomerRep = currentUserId;
            return await _uow.Company.GetAssignedCustomers(filterCriteria);
        }

        //create a nuban account
        public async Task<CreateNubanAccountResponseDTO> CraeteNubanAccount(CreateNubanAccountDTO nubanAccount)
        {
            try
            {

                var customerNubanAccount = await _paystackPaymentService.CreateUserNubanAccount(nubanAccount);
                return customerNubanAccount;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<JObject> GetNubanProviders()
        {
            try
            {

                var providers = await _paystackPaymentService.GetNubanAccountProviders();
                return providers;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<CompanyDTO>> GetAssignedCustomersByCustomerRepEmail(BaseFilterCriteria filterCriteria)
        {
            //get the user by email 
            if (!string.IsNullOrEmpty(filterCriteria.AssignedCustomerRepEmail))
            {
                filterCriteria.AssignedCustomerRepEmail = filterCriteria.AssignedCustomerRepEmail.Trim();
            }
            else
            {
                throw new GenericException("Email is invalid");
            }

            var user = await _uow.User.GetUserByEmail(filterCriteria.AssignedCustomerRepEmail);
            if (user == null)
            {
                throw new GenericException("user does not exist");
            }
            filterCriteria.AssignedCustomerRep = user.Id;
            return await _uow.Company.GetAssignedCustomersByCustomerRep(filterCriteria);
        }

        public async Task<ResponseDTO> AddSubscriptionToCustomer(string customercode)
        {
            try
            {
                var result = new ResponseDTO();
                if (string.IsNullOrEmpty(customercode))
                {
                    result.Succeeded = false;
                    result.Message = "Customer code cannot be empty";
                    return result;
                }

                var user = await _userService.GetUserByChannelCode(customercode);
                if (user == null || String.IsNullOrEmpty(user.UserChannelCode))
                {
                    result.Succeeded = false;
                    result.Message = "User does not exist";
                    return result;
                }

                var company = await _uow.Company.GetAsync(x => x.CustomerCode == user.UserChannelCode);
                if (company == null)
                {
                    result.Succeeded = false;
                    result.Message = "Company information does not exist";
                    return result;
                }

                //Get user wallet
                var wallet = await _uow.Wallet.GetAsync(x => x.CustomerCode.Equals(user.UserChannelCode));
                if (wallet == null)
                {
                    result.Succeeded = false;
                    result.Message = $"Wallet does not exist";
                    return result;
                }

                //Charge Wallet for Subscription
                var referenceNo = $"{user.UserChannelCode}{DateTime.Now.ToString("ddMMyyyss")}";
                await _walletService.UpdateWallet(wallet.WalletId, new WalletTransactionDTO()
                {
                    WalletId = wallet.WalletId,
                    Amount = 3999m,
                    CreditDebitType = CreditDebitType.Debit,
                    Description = "Customer subscription",
                    PaymentType = PaymentType.Wallet,
                    PaymentTypeReference = referenceNo,
                    UserId = user.Id,
                    ServiceCharge = 0m,
                }, false);

                company.isCodNeeded = true;
                company.Rank = Rank.Class;
                company.RankModificationDate = DateTime.Now;
                var companyDTO = Mapper.Map<CompanyDTO>(company);
                _uow.RankHistory.Add(new RankHistory
                {
                    CustomerName = companyDTO.Name,
                    CustomerCode = companyDTO.CustomerCode,
                    RankType = RankType.Upgrade
                });
                await _uow.CompleteAsync();

                //Call Node to Update User subscription
                await _nodeService.UpdateMerchantSubscription(new UpdateNodeMercantSubscriptionDTO
                {
                    UserId = user.Id,
                    MerchantCode = companyDTO.CustomerCode
                });

                //send email for upgrade customers
                if (company.Rank == Rank.Class)
                {
                    //SEND EMAIL TO CLASS CUSTOMERS
                    var companyMessagingDTO = new CompanyMessagingDTO();

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

                //Send mail to class customers with an assigned customer rep
                if (company.Rank == Rank.Class)
                {
                    //Check if already assigned a customer rep
                    if (string.IsNullOrEmpty(company.AssignedCustomerRep))
                    {
                        await SendEmailToAssignEcommerceCustomerRep(company);
                    }
                }

                result.Message = "Class Subscription Successful";
                result.Succeeded = true;
                result.Entity = companyDTO;
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ResponseDTO> UpdateUserRankForAlpha(string merchantcode)
        {
            try
            {
                var result = new ResponseDTO();
                var user = new UserDTO();

                if (String.IsNullOrEmpty(merchantcode))
                {
                    throw new GenericException("Invalid merchant code.", $"{(int)HttpStatusCode.BadRequest}");
                }

                user = await _userService.GetUserByChannelCode(merchantcode);
                if (user == null || String.IsNullOrEmpty(user.UserChannelCode))
                {
                    throw new GenericException("User does not exist.", $"{(int)HttpStatusCode.NotFound}");
                }

                var userValidationDTO = new UserValidationDTO
                {
                    Rank = Rank.Class,
                    UserCode = user.UserChannelCode,
                    UserID = user.Id,
                    RankType = RankType.Upgrade
                };

                if (String.IsNullOrEmpty(userValidationDTO.UserID) && String.IsNullOrEmpty(userValidationDTO.UserCode))
                {
                    throw new GenericException("User not provided.", $"{(int)HttpStatusCode.BadRequest}");
                }

                if (userValidationDTO.Rank == null)
                {
                    throw new GenericException("Rank not provided.", $"{(int)HttpStatusCode.BadRequest}");
                }

                var company = await _uow.Company.GetAsync(x => x.CustomerCode == user.UserChannelCode);
                if (company == null)
                {
                    throw new GenericException("Company information does not exist.", $"{(int)HttpStatusCode.NotFound}");
                }

                if (userValidationDTO.Rank == Rank.Class)
                {
                    //make the BVN compulsory
                    if (String.IsNullOrEmpty(company.BVN))
                    {
                        throw new GenericException("User BVN not provided.", $"{(int)HttpStatusCode.BadRequest}");
                    }
                    company.isCodNeeded = true;
                }
                else
                {
                    company.isCodNeeded = false;
                }

                company.Rank = userValidationDTO.Rank;
                company.RankModificationDate = DateTime.Now;
                var companyDTO = Mapper.Map<CompanyDTO>(company);
                _uow.RankHistory.Add(new RankHistory
                {
                    CustomerName = companyDTO.Name,
                    CustomerCode = companyDTO.CustomerCode,
                    RankType = userValidationDTO.RankType
                });
                await _uow.CompleteAsync();

                //Call Node to Update User subscription
                await _nodeService.UpdateMerchantSubscription(new UpdateNodeMercantSubscriptionDTO
                {
                    UserId = user.Id,
                    MerchantCode = companyDTO.CustomerCode
                });

                //send email for upgrade customers
                if (userValidationDTO.Rank == Rank.Class)
                {
                    //SEND EMAIL TO CLASS CUSTOMERS
                    var companyMessagingDTO = new CompanyMessagingDTO();
                  
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

                //Send mail to class customers with an assigned customer rep
                if (userValidationDTO.Rank == Rank.Class)
                {
                    //CHeck if already assigned a customer rep
                    if (string.IsNullOrEmpty(company.AssignedCustomerRep))
                    {
                        await SendEmailToAssignEcommerceCustomerRep(company);
                    }
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
    }

}