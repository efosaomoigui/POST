using GIGLS.Core.Domain.MessagingLog;
using GIGLS.Core.IRepositories.MessagingLog;
using GIGLS.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GIGLS.Core.DTO.MessagingLog;
using AutoMapper;
using GIGLS.CORE.DTO.Shipments;

namespace GIGLS.Infrastructure.Persistence.Repositories.MessagingLog
{
    public class SmsSendLogRepository : Repository<SmsSendLog, GIGLSContext>, ISmsSendLogRepository
    {
        public SmsSendLogRepository(GIGLSContext context) : base(context)
        {
        }

        public Task<List<SmsSendLogDTO>> GetSmsSendLogsAsync(MessageFilterOption filter)
        {
            try
            {
                //get startDate and endDate
                var queryDate = filter.getStartDateAndEndDate();
                var startDate = queryDate.Item1;
                var endDate = queryDate.Item2;
                
                var messages = Context.SmsSendLog.AsQueryable().Where(s => s.DateCreated >= startDate && s.DateCreated < endDate);

                if (filter.Status.HasValue)
                {
                    messages = messages.Where(x => x.Status == filter.Status);
                }

                var result = messages.OrderByDescending(x => x.DateCreated).ToList();
                var messageDto = Mapper.Map<List<SmsSendLogDTO>>(result);
                return Task.FromResult(messageDto);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Tuple<Task<List<SmsSendLogDTO>>, int> GetSmsSendLogsAsync(FilterOptionsDto filterOptionsDto)
        {
            try
            {
                var smsCollectionDto = new List<SmsSendLogDTO>();
                var pageNumber = filterOptionsDto.page;
                var pageSize = filterOptionsDto.count;

                //build query
                var queryable = Context.SmsSendLog.AsQueryable();

                var filter = filterOptionsDto.filter;
                var filterValue = filterOptionsDto.filterValue;
                if (!string.IsNullOrWhiteSpace(filter) && !string.IsNullOrWhiteSpace(filterValue))
                {
                    var caseObject = new SmsSendLogDTO();
                    var myPropInfo = typeof(SmsSendLog).GetProperty(filter);
                    switch (filter)
                    {
                        case nameof(caseObject.From):
                            queryable = queryable.Where(s => s.From.Contains(filterValue));
                            break;
                        case nameof(caseObject.To):
                            queryable = queryable.Where(s => s.To.Contains(filterValue));
                            break;
                    }
                }
                
                //populate the count variable
                var totalCount = queryable.Count();

                //page the query
                queryable = queryable.OrderByDescending(x => x.DateCreated);
                var result = queryable.Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToList();
                var messageDto = Mapper.Map<IEnumerable<SmsSendLogDTO>>(result);
                return new Tuple<Task<List<SmsSendLogDTO>>, int>(Task.FromResult(messageDto.ToList()), totalCount);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
