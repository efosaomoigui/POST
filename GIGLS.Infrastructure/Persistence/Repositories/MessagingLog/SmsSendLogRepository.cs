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
    public class SmsSendLogRepository : Repository<SmsSendLog, GIGLSContext>, ISmsSendLogRepository
    {
        public SmsSendLogRepository(GIGLSContext context) : base(context)
        {
        }

        public Task<IEnumerable<SmsSendLogDTO>> GetSmsSendLogsAsync()
        {
            try
            {
                var messages = Context.SmsSendLog.ToList();
                var messageDto = Mapper.Map<IEnumerable<SmsSendLogDTO>>(messages);
                return Task.FromResult(messageDto);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
