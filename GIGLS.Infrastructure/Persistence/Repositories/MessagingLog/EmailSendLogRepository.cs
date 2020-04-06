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

        public Task<List<EmailSendLogDTO>> GetEmailSendLogsAsync(MessageFilterOption filter)
        {
            try
            {
                //get startDate and endDate
                var queryDate = filter.getStartDateAndEndDate();
                var startDate = queryDate.Item1;
                var endDate = queryDate.Item2;

                var messages = Context.EmailSendLog.AsQueryable().Where(s => s.DateCreated >= startDate && s.DateCreated < endDate);

                if (filter.Status.HasValue)
                {
                    messages = messages.Where(x => x.Status == filter.Status);
                }

                var result = messages.OrderByDescending(x => x.DateCreated).ToList();
                var messageDto = Mapper.Map<List<EmailSendLogDTO>>(result);
                return Task.FromResult(messageDto);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
