using AutoMapper;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO;
using GIGLS.Core.IRepositories;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using GIGLS.Core.Enums;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories
{
    public class EmailSmsRepository : Repository<EmailSms, GIGLSContext>, IEmailSmsRepository
    {
        public EmailSmsRepository(GIGLSContext context) : base(context)
        {
        }
        
        public Task<IEnumerable<EmailSmsDTO>> GetEmailSmsAsync(EmailSmsType type)
        {
            try
            {
                var emailSms = Context.EmailSms.Where(x => x.EmailSmsType == type).ToList();

                var emailSmsDto = Mapper.Map<IEnumerable<EmailSmsDTO>>(emailSms);
                return Task.FromResult(emailSmsDto);
            }
            catch (Exception)
            {
                throw;
            }
        }
        
    }
}
