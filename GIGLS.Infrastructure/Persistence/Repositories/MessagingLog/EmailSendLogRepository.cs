using GIGLS.Core.Domain.MessagingLog;
using GIGLS.Core.IRepositories.MessagingLog;
using GIGLS.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GIGLS.Core.DTO.MessagingLog;
using AutoMapper;

namespace GIGLS.Infrastructure.Persistence.Repositories.MessagingLog
{
    public class EmailSendLogRepository : Repository<EmailSendLog, GIGLSContext>, IEmailSendLogRepository
    {
        public EmailSendLogRepository(GIGLSContext context) : base(context)
        {
        }

        public Task<IEnumerable<EmailSendLogDTO>> GetEmailSendLogsAsync()
        {
            try
            {
                var messages = Context.EmailSendLog.ToList();
                var messageDto = Mapper.Map<IEnumerable<EmailSendLogDTO>>(messages);
                return Task.FromResult(messageDto);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
