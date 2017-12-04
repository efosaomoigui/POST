using AutoMapper;
using GIGLS.Core;
using GIGLS.Core.IServices;
using GIGLS.Infrastructure;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Haulage;

namespace GIGLS.Services.Implementation
{
    public class HaulageService : IHaulageService
    {
        private readonly IUnitOfWork _uow;

        public HaulageService(IUnitOfWork uow)
        {
            _uow = uow;
            MapperConfig.Initialize();
        }

        public async Task<IEnumerable<HaulageDTO>> GetHaulages()
        {
            var haulages = await _uow.Haulage.GetHaulagesAsync();
            return haulages;
        }

        public async Task<IEnumerable<HaulageDTO>> GetActiveHaulages()
        {
            var haulages = await _uow.Haulage.GetAsync(x => x.Status == true);
            return Mapper.Map<IEnumerable<HaulageDTO>>(haulages);
        }

        public async Task<HaulageDTO> GetHaulageById(int haulageId)
        {
            var haulage = await _uow.Haulage.GetAsync(haulageId);

            if (haulage == null)
            {
                throw new GenericException("HAULAGE INFORMATION DOES NOT EXIST");
            }
            return Mapper.Map<HaulageDTO>(haulage);
        }

        public async Task<object> AddHaulage(HaulageDTO haulageDto)
        {
            if (await _uow.Haulage.ExistAsync(v => v.Tonne == haulageDto.Tonne))
            {
                throw new GenericException($"{haulageDto.Tonne} Tonne already exist");
            }

            var newHaulage = new Core.Domain.Haulage
            {
                Tonne = haulageDto.Tonne,
                Status = true
            };

            _uow.Haulage.Add(newHaulage);
            await _uow.CompleteAsync();
            return new { id = newHaulage.HaulageId };
        }

        public async Task UpdateHaulage(int haulageId, HaulageDTO haulageDto)
        {
            var haulage = await _uow.Haulage.GetAsync(haulageId);

            if (haulage == null)
            {
                throw new GenericException("HAULAGE INFORMATION DOES NOT EXIST");
            }

            haulage.Tonne = haulageDto.Tonne;
            haulage.Status = haulageDto.Status;
            await _uow.CompleteAsync();
        }

        public async Task UpdateHaulageStatus(int haulageId, bool status)
        {
            var haulage = await _uow.Haulage.GetAsync(haulageId);

            if (haulage == null)
            {
                throw new GenericException("HAULAGE INFORMATION DOES NOT EXIST");
            }
            
            haulage.Status = status;
            await _uow.CompleteAsync();
        }

        public async Task RemoveHaulage(int haulageId)
        {
            var haulage = await _uow.Haulage.GetAsync(haulageId);

            if (haulage == null)
            {
                throw new GenericException("HAULAGE INFORMATION DOES NOT EXIST");
            }
            _uow.Haulage.Remove(haulage);
            await _uow.CompleteAsync();
        }        
    }
}
