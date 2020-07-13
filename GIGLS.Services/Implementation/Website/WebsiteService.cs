using AutoMapper;
using GIGLS.Core;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO;
using GIGLS.Core.DTO.Customers;
using GIGLS.Core.Enums;
using GIGLS.Core.IMessageService;
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

        public WebsiteService(IMessageSenderService messageSenderService, IUnitOfWork uow)
        {
            _messageSenderService = messageSenderService;
            _uow = uow;
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
                    throw new GenericException("Business email field can not be empty", $"{(int)HttpStatusCode.Forbidden}");
                }

                ecommerceAgreementDTO.BusinessEmail = ecommerceAgreementDTO.BusinessEmail.Replace(" ", string.Empty);

                string natureOfBusiness = string.Empty;
                string ecommerceSignature = string.Empty;

                if (ecommerceAgreementDTO.NatureOfBusiness.Any())
                {
                    natureOfBusiness = string.Join(",", ecommerceAgreementDTO.NatureOfBusiness);
                }

                ecommerceSignature = ecommerceAgreementDTO.EcommerceSignatureName + ecommerceAgreementDTO.EcommerceSignatureAddress;

                if (await _uow.EcommerceAgreement.ExistAsync(c => c.BusinessEmail == ecommerceAgreementDTO.BusinessEmail))
                {
                    throw new GenericException("Ecommerce information already exist", $"{(int)HttpStatusCode.Forbidden}");
                }

                var state = await _uow.State.GetAsync(x => x.StateName.ToLower() == ecommerceAgreementDTO.State.ToLower());
                if(state == null)
                {
                    throw new GenericException("Invalid State Name", $"{(int)HttpStatusCode.Forbidden}");
                }

                var ecommerceAgreement = Mapper.Map<EcommerceAgreement>(ecommerceAgreementDTO);
                ecommerceAgreement.NatureOfBusiness = natureOfBusiness;
                ecommerceAgreement.EcommerceSignature = ecommerceSignature;
                ecommerceAgreement.State = state.StateName;
                ecommerceAgreement.CountryId = state.CountryId;
                ecommerceAgreement.Status = EcommerceAgreementStatus.Pending;
                _uow.EcommerceAgreement.Add(ecommerceAgreement);
                await _uow.CompleteAsync();
                return HttpStatusCode.Created;
            }
            catch (Exception)
            {
                throw;
            }
        }



    }
}
