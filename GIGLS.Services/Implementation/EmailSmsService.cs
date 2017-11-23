using GIGLS.Core;
using GIGLS.Core.DTO;
using GIGLS.Core.IServices;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.Core.Enums;
using GIGLS.Infrastructure;
using AutoMapper;
using GIGLS.Core.Domain;

namespace GIGLS.Services.Implementation
{
    public class EmailSmsService : IEmailSmsService
    {
        private readonly IUnitOfWork _uow;

        public EmailSmsService(IUnitOfWork uow)
        {
            _uow = uow;
            MapperConfig.Initialize();
        }

        public Task<IEnumerable<EmailSmsDTO>> GetEmailAsync()
        {
            var emails = _uow.EmailSms.GetEmailSmsAsync(EmailSmsType.Email);
            return emails;
        }
        public Task<IEnumerable<EmailSmsDTO>> GetSmsAsync()
        {
            var sms = _uow.EmailSms.GetEmailSmsAsync(EmailSmsType.SMS);
            return sms;
        }

        public async Task<object> AddEmailSms(EmailSmsDTO emailSmsDto)
        {
            var emailSms = Mapper.Map<EmailSms>(emailSmsDto);
            _uow.EmailSms.Add(emailSms);
            await _uow.CompleteAsync();
            return new { id = emailSms.EmailSmsId };
        }

        public async Task<EmailSmsDTO> GetEmailSmsById(int emailSmsId)
        {
            var emailSms = await _uow.EmailSms.GetAsync(emailSmsId);

            if (emailSms == null)
            {
                throw new GenericException("MESSAGE INFORMATION DOES NOT EXIST");
            }
            return Mapper.Map<EmailSmsDTO>(emailSms);
        }


        public async Task RemoveEmailSms(int emailSmsId)
        {
            var emailSms = await _uow.EmailSms.GetAsync(emailSmsId);

            if (emailSms == null)
            {
                throw new GenericException("MESSAGE INFORMATION DOES NOT EXIST");
            }
            _uow.EmailSms.Remove(emailSms);
            await _uow.CompleteAsync();
        }

        public async Task UpdateEmailSms(int emailSmsId, EmailSmsDTO emailSmsDto)
        {
            var emailSms = await _uow.EmailSms.GetAsync(emailSmsId);

            if (emailSms == null)
            {
                throw new GenericException("MESSAGE INFORMATION DOES NOT EXIST");
            }

            emailSms.Message = emailSmsDto.Message;
            emailSms.Subject = emailSmsDto.Subject;
            emailSms.From = emailSmsDto.From;
            emailSms.To = emailSmsDto.To;
            emailSms.MessageType = emailSmsDto.MessageType;
            emailSms.EmailSmsType = emailSmsDto.EmailSmsType;
            await _uow.CompleteAsync();
        }
    }
}
