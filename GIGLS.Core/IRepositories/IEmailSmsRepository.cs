using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO;
using GIGLS.Core.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories
{
    public interface IEmailSmsRepository : IRepository<EmailSms>
    {
        Task<IEnumerable<EmailSmsDTO>> GetEmailSmsAsync(EmailSmsType type);
    }
}
