using GIGLS.Core.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices
{
    public interface IEmailSmsService : IServiceDependencyMarker
    {
        Task<IEnumerable<EmailSmsDTO>> GetSmsAsync();
        Task<IEnumerable<EmailSmsDTO>> GetEmailAsync();
        Task<EmailSmsDTO> GetEmailSmsById(int EmailSmsId);
        Task<object> AddEmailSms(EmailSmsDTO EmailSms);
        Task UpdateEmailSms(int EmailSmsId, EmailSmsDTO EmailSms);
        Task RemoveEmailSms(int EmailSmsId);
    }
}
