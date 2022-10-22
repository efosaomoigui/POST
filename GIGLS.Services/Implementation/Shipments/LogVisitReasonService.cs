using POST.Core.IServices.Shipments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using POST.Core.DTO.Shipments;
using POST.Core;
using AutoMapper;
using POST.Core.Domain;
using POST.Infrastructure;

namespace POST.Services.Implementation.Shipments
{
    public class LogVisitReasonService : ILogVisitReasonService
    {
        private readonly IUnitOfWork _uow;

        public LogVisitReasonService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<object> AddLogVisitReason(LogVisitReasonDTO logVisitReasonDto)
        {
            try
            {
                var newLog = Mapper.Map<LogVisitReason>(logVisitReasonDto);
                _uow.LogVisitReason.Add(newLog);
                await _uow.CompleteAsync();
                return new { Id = newLog.LogVisitReasonId };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteLogVisitReason(int logVisitReasonId)
        {
            try
            {
                var log = await _uow.LogVisitReason.GetAsync(logVisitReasonId);
                if(log == null)
                {
                    throw new GenericException("Log Visit reason does not exist");
                }

                _uow.LogVisitReason.Remove(log);
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<LogVisitReasonDTO> GetLogVisitReasonById(int logVisitReasonId)
        {
            try
            {
                var log = await _uow.LogVisitReason.GetAsync(logVisitReasonId);
                if (log == null)
                {
                    throw new GenericException("Log Visit reason does not exist");
                }

                var logDto = Mapper.Map<LogVisitReasonDTO>(log);
                return logDto;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<List<LogVisitReasonDTO>> GetLogVisitReasons()
        {
            var logs = _uow.LogVisitReason.GetAll().ToList();
            return Task.FromResult(Mapper.Map<List<LogVisitReasonDTO>>(logs));
        }

        public async Task UpdateLogVisitReason(int logVisitReasonId, LogVisitReasonDTO logVisitReasonDto)
        {
            try
            {
                var log = await _uow.LogVisitReason.GetAsync(logVisitReasonId);
                if (log == null || logVisitReasonDto.LogVisitReasonId != logVisitReasonId)
                {
                    throw new GenericException("Log Visit reason does not exist");
                }

                log.Message = logVisitReasonDto.Message;
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}