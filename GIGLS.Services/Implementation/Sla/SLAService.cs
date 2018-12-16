using GIGLS.Core.IServices.Sla;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.Core.DTO.SLA;
using GIGLS.Core.Enums;
using GIGLS.Core;
using AutoMapper;
using GIGLS.Core.Domain.SLA;
using GIGLS.Infrastructure;

namespace GIGLS.Services.Implementation.Sla
{
    public class SLAService : ISLAService
    {
        private readonly IUnitOfWork _uow;

        public SLAService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<object> AddSLA(SLADTO slaDto)
        {
            var sla = Mapper.Map<SLA>(slaDto);
            _uow.SLA.Add(sla);
            await _uow.CompleteAsync();
            return new { id = sla.SLAId };
        }

        public async Task<SLADTO> GetSLAById(int id)
        {
            var sla = await _uow.SLA.GetAsync(id);

            if (sla == null)
            {
                throw new GenericException("SLA INFORMATION DOES NOT EXIST");
            }
            return Mapper.Map<SLADTO>(sla);
        }

        //Assume SLA should only be one for each type
        public async Task<SLADTO> GetSLAByType(SLAType type)
        {
            var sla = await _uow.SLA.GetAsync(x => x.SLAType == type);

            if (sla == null)
            {
                throw new GenericException("SLA INFORMATION DOES NOT EXIST");
            }
            return Mapper.Map<SLADTO>(sla);
        }

        public Task<IEnumerable<SLADTO>> GetSLAs()
        {
            var slas =  _uow.SLA.GetAll();
            return Task.FromResult(Mapper.Map<IEnumerable<SLADTO>>(slas));
        }

        public async Task RemoveSLA(int id)
        {
            var sla = await _uow.SLA.GetAsync(id);

            if (sla == null)
            {
                throw new GenericException("SLA INFORMATION DOES NOT EXIST");
            }
            _uow.SLA.Remove(sla);
            await _uow.CompleteAsync();
        }

        public async Task UpdateSLA(int id, SLADTO slaDto)
        {
            var sla = await _uow.SLA.GetAsync(id);

            if (sla == null)
            {
                throw new GenericException("SLA INFORMATION DOES NOT EXIST");
            }

            sla.Content = slaDto.Content;
            sla.SLAType = slaDto.SLAType;
            await _uow.CompleteAsync();
        }        
    }
}
