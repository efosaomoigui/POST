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
                DateTime StartDate = filter.StartDate.GetValueOrDefault().Date;
                DateTime EndDate = filter.EndDate?.Date ?? StartDate;

                var messages = Context.EmailSendLog.AsQueryable();

                //If No Date Supply
                if (!filter.StartDate.HasValue && !filter.EndDate.HasValue)
                {
                    var Today = DateTime.Today;
                    var nextDay = DateTime.Today.AddDays(1).Date;
                    messages = messages.Where(x => x.DateCreated >= Today && x.DateCreated < nextDay);
                }

                if (filter.StartDate.HasValue && filter.EndDate.HasValue)
                {
                    if (filter.StartDate.Equals(filter.EndDate))
                    {
                        var nextDay = DateTime.Today.AddDays(1).Date;
                        messages = messages.Where(x => x.DateCreated >= StartDate && x.DateCreated < nextDay);
                    }
                    else
                    {
                        var dayAfterEndDate = EndDate.AddDays(1).Date;
                        messages = messages.Where(x => x.DateCreated >= StartDate && x.DateCreated < dayAfterEndDate);
                    }
                }

                if (filter.StartDate.HasValue && !filter.EndDate.HasValue)
                {
                    var nextDay = DateTime.Today.AddDays(1).Date;
                    messages = messages.Where(x => x.DateCreated >= StartDate && x.DateCreated < nextDay);
                }

                if (filter.EndDate.HasValue && !filter.StartDate.HasValue)
                {
                    var dayAfterEndDate = EndDate.AddDays(1).Date;
                    messages = messages.Where(x => x.DateCreated < dayAfterEndDate);
                }

                if (filter.Status.HasValue)
                {
                    messages = messages.Where(x => x.Status.Equals(filter.Status));
                }

                var result = messages.ToList();
                var messageDto = Mapper.Map<IEnumerable<EmailSendLogDTO>>(result);
                return Task.FromResult(messageDto.OrderByDescending(x => x.DateCreated).ToList());
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
