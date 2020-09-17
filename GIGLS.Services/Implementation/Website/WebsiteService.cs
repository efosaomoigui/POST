using AutoMapper;
using GIGLS.Core;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO;
using GIGLS.Core.DTO.Customers;
using GIGLS.Core.DTO.MessagingLog;
using GIGLS.Core.Enums;
using GIGLS.Core.IMessageService;
using GIGLS.Core.IServices.Customers;
using GIGLS.Core.IServices.Shipments;
using GIGLS.Core.IServices.User;
using GIGLS.Core.DTO.Wallet;
using GIGLS.Core.IServices.Wallet;
using GIGLS.Core.IServices.Utility;
using GIGLS.Core.IServices.Website;
using GIGLS.Infrastructure;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Collections.Generic;
using GIGLS.Core.DTO.Shipments;

namespace GIGLS.Services.Implementation.Website
{
    public class WebsiteService : IWebsiteService
    {
        private readonly IMessageSenderService _messageSenderService;
        private readonly IUnitOfWork _uow;
        private readonly ICompanyService _companyService;
        private readonly IGlobalPropertyService _globalPropertyService;
        private readonly ICustomerService _customerService;
        private readonly IMagayaService _magayaService;
        private readonly IUserService _userService;
        private readonly IWalletService _walletService;
        private readonly IPasswordGenerator _passwordGenerator;

        public WebsiteService(IMessageSenderService messageSenderService, IUnitOfWork uow, ICompanyService companyService, IGlobalPropertyService globalPropertyService,
            ICustomerService customerService, IMagayaService magayaService, IUserService userService, IWalletService walletService, IPasswordGenerator passwordGenerator)
        {
            _messageSenderService = messageSenderService;
            _uow = uow;
            _companyService = companyService;
            _globalPropertyService = globalPropertyService;
            _customerService = customerService;
            _magayaService = magayaService;
            _userService = userService;
            _walletService = walletService;
            _passwordGenerator = passwordGenerator;
        }

        public async Task<bool> SendSchedulePickupMail(WebsiteMessageDTO obj)
        {
            try
            {
                bool result = false;
                var messageType = MessageType.WEBPICKUP;
                var emailSmsType = EmailSmsType.Email;

                var gigMail = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.GIGLogisticsEmail, 1);

                //seperate email by comma and send message to those email
                string[] gigMailList = gigMail.Value.Split(',').ToArray();

                foreach (string email in gigMailList)
                {
                    obj.gigMail = email;
                    result = await _messageSenderService.SendMessage(messageType, emailSmsType, obj);
                }

                return await Task.FromResult(result);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<bool> SendQuoteMail(WebsiteMessageDTO obj)
        {
            try
            {
                bool result = false;
                var messageType = MessageType.WEBQUOTE;
                var emailSmsType = EmailSmsType.Email;

                var gigMail = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.GIGLogisticsEmail, 1);

                //seperate email by comma and send message to those email
                string[] gigMailList = gigMail.Value.Split(',').ToArray();

                foreach (string email in gigMailList)
                {
                    obj.gigMail = email;
                    result = await _messageSenderService.SendMessage(messageType, emailSmsType, obj);
                }

                return await Task.FromResult(result);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<bool> SendGIGGoIssuesMail(AppMessageDTO obj)
        {
            try
            {
                bool result = false;
                var messageType = MessageType.APPREPORT;
                var emailSmsType = EmailSmsType.Email;

                //send meail to ecommerce team
                var gigMail = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.GIGGOPartnerEmail, 1);

                //seperate email by comma and send message to those email
                string[] gigMailList = gigMail.Value.Split(',').ToArray();

                foreach (string email in gigMailList)
                {
                    obj.Recipient = email;
                    result = await _messageSenderService.SendMessage(messageType, emailSmsType, obj);
                }

                return await Task.FromResult(result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<object> AddEcommerceAgreement(EcommerceAgreementDTO ecommerceAgreementDTO)
        {
            try
            {
                if (ecommerceAgreementDTO.BusinessEmail == null)
                {
                    throw new GenericException("Business email field can not be empty", $"{(int)HttpStatusCode.BadRequest}");
                }

                ecommerceAgreementDTO.BusinessEmail = ecommerceAgreementDTO.BusinessEmail.Replace(" ", string.Empty);

                string natureOfBusiness = string.Empty;
                string ecommerceSignature = string.Empty;

                if (ecommerceAgreementDTO.NatureOfBusiness.Any())
                {
                    natureOfBusiness = string.Join(",", ecommerceAgreementDTO.NatureOfBusiness);
                }

                //Check if it is in Pending Requests 
                if (await _uow.EcommerceAgreement.ExistAsync(c => c.BusinessEmail == ecommerceAgreementDTO.BusinessEmail))
                {
                    throw new GenericException("Ecommerce Pending Request information already exists", $"{(int)HttpStatusCode.Forbidden}");
                }

                //block the registration for GIGGO Users using the Email
                var gigGoEmailUser = await _uow.User.GetUserByEmail(ecommerceAgreementDTO.BusinessEmail);

                if (gigGoEmailUser != null)
                {
                    throw new GenericException($"{ecommerceAgreementDTO.BusinessEmail} already exists as Customer", $"{(int)HttpStatusCode.Forbidden}");
                }

                //Check if it is already an E-commerce Customer
                if (await _uow.Company.ExistAsync(c => c.PhoneNumber.Contains(ecommerceAgreementDTO.ContactPhoneNumber) || c.Email == ecommerceAgreementDTO.BusinessEmail || c.Name.ToLower() == ecommerceAgreementDTO.BusinessOwnerName.Trim().ToLower()))
                {
                    throw new GenericException($"{ecommerceAgreementDTO.ContactPhoneNumber}, or {ecommerceAgreementDTO.BusinessEmail} or {ecommerceAgreementDTO.BusinessOwnerName} already exists as Ecommerce Customer", $"{(int)HttpStatusCode.Forbidden}");
                }

                //Check if it is already a GIGGo Individual customer 
                if (await _uow.IndividualCustomer.ExistAsync(c => (c.PhoneNumber.Contains(ecommerceAgreementDTO.ContactPhoneNumber) || c.Email == ecommerceAgreementDTO.BusinessEmail) && (c.IsRegisteredFromMobile)))
                {
                    throw new GenericException($"{ecommerceAgreementDTO.ContactPhoneNumber}, or {ecommerceAgreementDTO.BusinessEmail}  already exists as an Individual GIGGO Customer", $"{(int)HttpStatusCode.Forbidden}");
                }

                //Check if the state is valid
                var state = await _uow.State.GetAsync(x => x.StateName.ToLower() == ecommerceAgreementDTO.State.ToLower());
                if (state == null)
                {
                    throw new GenericException("Invalid State Name", $"{(int)HttpStatusCode.Forbidden}");
                }

                //update the customer phone number to have country code added to it
                if (ecommerceAgreementDTO.ContactPhoneNumber.StartsWith("0"))
                {
                    ecommerceAgreementDTO.ContactPhoneNumber = await _companyService.AddCountryCodeToPhoneNumber(ecommerceAgreementDTO.ContactPhoneNumber, state.CountryId);
                }

                //check phone number existence 
                var gigGoPhoneUser = await _uow.User.GetUserByPhoneNumber(ecommerceAgreementDTO.ContactPhoneNumber);

                if (gigGoPhoneUser != null)
                {
                    throw new GenericException($"{ecommerceAgreementDTO.ContactPhoneNumber} already exists", $"{(int)HttpStatusCode.Forbidden}");
                }

                ecommerceSignature = ecommerceAgreementDTO.EcommerceSignatureName + ecommerceAgreementDTO.EcommerceSignatureAddress;
                var ecommerceAgreement = Mapper.Map<EcommerceAgreement>(ecommerceAgreementDTO);
                ecommerceAgreement.NatureOfBusiness = natureOfBusiness;
                ecommerceAgreement.EcommerceSignature = ecommerceSignature;
                ecommerceAgreement.State = state.StateName;
                ecommerceAgreement.CountryId = state.CountryId;
                ecommerceAgreement.Status = EcommerceAgreementStatus.Pending;
                _uow.EcommerceAgreement.Add(ecommerceAgreement);
                await _uow.CompleteAsync();

                //send meail to ecommerce team
                var ecommerceEmail = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.EcommerceEmail, 1);

                //seperate email by comma and send message to those email
                string[] ecommerceEmails = ecommerceEmail.Value.Split(',').ToArray();

                //customer email, customer phone, receiver email
                EcommerceMessageDTO email = new EcommerceMessageDTO
                {
                    CustomerEmail = ecommerceAgreementDTO.BusinessEmail,
                    CustomerPhoneNumber = ecommerceAgreementDTO.ContactPhoneNumber,
                    CustomerCompanyName = ecommerceAgreementDTO.BusinessOwnerName,
                    BusinessNature = natureOfBusiness
                };

                foreach (string data in ecommerceEmails)
                {
                    email.EcommerceEmail = data;
                    await _messageSenderService.SendGenericEmailMessage(MessageType.ENM, email);
                }

                return HttpStatusCode.Created;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IndividualCustomerDTO> AddUserForInternationalCustomer(CustomerDTO customerDTO) 
        {
            try
            {
                //block the registration for existing User
                var EmailUser = await _uow.User.GetUserByEmail(customerDTO.Email);

                if (EmailUser != null)
                {
                    throw new GenericException($"Email already exist");
                }

                //check if registration is from Giglgo
                if (customerDTO.IsFromMobile == true)
                {
                    //customerDTO.IsRegisteredFromMobile = true;
                }

                //update the customer update to have country code added to it
                if (customerDTO.PhoneNumber.StartsWith("0"))
                {
                    customerDTO.PhoneNumber = await _companyService.AddCountryCodeToPhoneNumber(customerDTO.PhoneNumber, customerDTO.UserActiveCountryId);
                }

                var password = "";
                if (customerDTO.Password == null)
                {
                    password = await _passwordGenerator.Generate();
                }
                else
                {
                    password = customerDTO.Password;
                }

                //check phone number existence
                var PhoneUser = await _uow.User.GetUserByPhoneNumber(customerDTO.PhoneNumber);

                if (PhoneUser != null)
                {
                    throw new GenericException($"Phone Number already exist");
                }

                var userChannelType = UserChannelType.IndividualCustomer;

                var result = await _userService.AddUser(new Core.DTO.User.UserDTO()
                {
                    ConfirmPassword = password,
                    Department = customerDTO.CompanyType.ToString(),
                    DateCreated = DateTime.Now,
                    Designation = customerDTO.CompanyType.ToString(),
                    Email = customerDTO.Email,
                    FirstName = customerDTO.Name,
                    LastName = customerDTO.Name,
                    Organisation = customerDTO.CompanyType.ToString(),
                    Password = password,
                    PhoneNumber = customerDTO.PhoneNumber,
                    UserType = UserType.Regular,
                    Username = customerDTO.Email,
                    UserChannelCode = customerDTO.CustomerCode,
                    UserChannelPassword = password,
                    UserChannelType = userChannelType,
                    PasswordExpireDate = DateTime.Now,
                    UserActiveCountryId = customerDTO.UserActiveCountryId,
                    IsActive = true
                });

                //complete
                _uow.Complete();

                // add customer to a wallet
                await _walletService.AddWallet(new WalletDTO
                {
                    CustomerId = customerDTO.CompanyId,
                    CustomerType = CustomerType.IndividualCustomer,
                    CustomerCode = customerDTO.CustomerCode
                });

                var message = new MessageDTO()
                {
                    CustomerCode = customerDTO.CustomerCode,
                    CustomerName = customerDTO.Name,
                    ToEmail = customerDTO.Email,
                    To = customerDTO.Email,
                    Body = password
                };

                await _messageSenderService.SendGenericEmailMessage(MessageType.USER_LOGIN, message);
                var customer = await _uow.IndividualCustomer.GetIndividualCustomers(customerDTO.PhoneNumber);
                
                return Mapper.Map<IndividualCustomerDTO>(customer);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<object> AddIntlCustomer(CustomerDTO customerDTO)
        {
            try
            {
                //Check if the state is valid
                var state = await _uow.State.GetAsync(x => x.StateName.ToLower() == customerDTO.State.ToLower());
                if (state == null)
                {
                    throw new GenericException("Invalid State Name", $"{(int)HttpStatusCode.Forbidden}");
                }

                //update the customer phone number to have country code added to it
                if (customerDTO.PhoneNumber.StartsWith("0"))
                {
                    customerDTO.PhoneNumber = await _companyService.AddCountryCodeToPhoneNumber(customerDTO.PhoneNumber, state.CountryId);
                }

                //check phone number existence 
                var PhoneUser = await _uow.User.GetUserByPhoneNumber(customerDTO.PhoneNumber);

                if (PhoneUser != null)
                {
                    throw new GenericException($"{customerDTO.PhoneNumber} already exists", $"{(int)HttpStatusCode.Forbidden}");
                }

                //Create customer in Agility
                var customerCreationResult = await _customerService.CreateCustomerIntl(customerDTO);

                //Create Customer in Magaya
                if (customerCreationResult != null)
                {
                    var MagayacustomerCreationResult = await _magayaService.SetEntityIntl(customerDTO);
                }

                //Create User login details
                var CustomerDetails = await AddUserForInternationalCustomer(customerDTO); 

                //update user based on isInternationShipper flag
                if (customerDTO.IsInternational)
                {
                    await UploadImageForCustomer(customerDTO);
                }

                var result = new object();
                return await Task.FromResult(result);

            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IntlShipmentRequestDTO> AddIntlShipmentRequest(IntlShipmentRequestDTO shipmentDTO)
        {
            try
            {
                var addResult = await _magayaService.CreateIntlShipmentRequest(shipmentDTO); 
                return addResult;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //this method returns int as follows 0. successfull 1. type not allowed 2. size not allowed
        public async Task<int> UploadImageForCustomer(CustomerDTO customerDTO)
        {
            string message = string.Empty;
            try
            {
                var httpRequest = HttpContext.Current.Request;

                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];

                    if (postedFile != null && postedFile.ContentLength > 0)
                    {
                        int MaxContentLength = 1024 * 1024 * 1; //Size = 1 MB

                        IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".gif", ".png", ".pdf" };
                        var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                        var extension = ext.ToLower();

                        if (!AllowedFileExtensions.Contains(extension))
                        {
                            throw new GenericException($"{postedFile.FileName} is not allowed");
                        }
                        else if (postedFile.ContentLength > MaxContentLength)
                        {
                            throw new GenericException($"Image size is too Large allowed");
                        }
                        else 
                        {
                            //update user based on isInternationShipper flag
                            if (customerDTO.IsInternational)
                            {
                                var user = await _uow.User.GetUserByEmail(customerDTO.Email);
                                if (user != null)
                                {
                                    user.IdentificationType = customerDTO.IdentificationType;
                                    user.IdentificationNumber = customerDTO.IdentifictionNumber;
                                    user.IdentificationImage = postedFile.FileName;
                                    user.IsInternational = true;

                                    await _uow.User.UpdateUser(user.Id, user);

                                    var filePath = HttpContext.Current.Server.MapPath("~/UserDocuments/" + customerDTO.Email + extension);
                                    postedFile.SaveAs(filePath);
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                //do nothing
            }

            return 0;
        }
    }
}