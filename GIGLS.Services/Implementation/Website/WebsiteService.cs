using AutoMapper;
using GIGLS.Core;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO;
using GIGLS.Core.DTO.Customers;
using GIGLS.Core.DTO.MessagingLog;
using GIGLS.Core.Enums;
using GIGLS.Core.IMessageService;
using GIGLS.Core.IServices.Customers;
using GIGLS.Core.IServices.Utility;
using GIGLS.Core.IServices.Website;
using GIGLS.Infrastructure;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace GIGLS.Services.Implementation.Website
{
    public class WebsiteService : IWebsiteService
    {
        private readonly IMessageSenderService _messageSenderService;
        private readonly IUnitOfWork _uow;
        private readonly ICompanyService _companyService;
        private readonly IGlobalPropertyService _globalPropertyService;

        public WebsiteService(IMessageSenderService messageSenderService, IUnitOfWork uow, ICompanyService companyService, IGlobalPropertyService globalPropertyService)
        {
            _messageSenderService = messageSenderService;
            _uow = uow;
            _companyService = companyService;
            _globalPropertyService = globalPropertyService;
        }

        public async Task<bool> SendSchedulePickupMail(WebsiteMessageDTO obj)
        {
            try
            {
                var messageType = MessageType.WEBPICKUP;
                var emailSmsType = EmailSmsType.Email;
                obj.gigMail = "info@giglogistics.com";

                var result = await _messageSenderService.SendMessage(messageType, emailSmsType, obj);
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
                var messageType = MessageType.WEBQUOTE;
                var emailSmsType = EmailSmsType.Email;
                obj.gigMail = "info@giglogistics.com";

                var result = await _messageSenderService.SendMessage(messageType, emailSmsType, obj);
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
                var messageType = MessageType.APPREPORT;
                var emailSmsType = EmailSmsType.Email;

                obj.Recipient = "gopartners@giglogistics.ng";

                var result = await _messageSenderService.SendMessage(messageType, emailSmsType, obj);
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
                if (await _uow.Company.ExistAsync(c => c.PhoneNumber.Contains(ecommerceAgreementDTO.ContactPhoneNumber) || c.Email == ecommerceAgreementDTO.BusinessEmail || c.Name.ToLower() == ecommerceAgreementDTO.BusinessOwnerName.Trim().ToLower()) )
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
                if(state == null)
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
    }
}