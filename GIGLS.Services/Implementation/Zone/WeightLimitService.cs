using GIGLS.Core.IServices.Zone;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Zone;
using GIGLS.Core;
using GIGLS.Infrastructure;
using GIGLS.Core.Domain;
using AutoMapper;

namespace GIGLS.Services.Implementation.Zone
{
    public class WeightLimitService : IWeightLimitService
    {
        private readonly IUnitOfWork _uow;

        public WeightLimitService(IUnitOfWork uow)
        {
            _uow = uow;
            MapperConfig.Initialize();
        }

        public async Task<object> AddWeightLimit(WeightLimitDTO weightLimit)
        {
            if (weightLimit == null)
                throw new GenericException("NULL_INPUT");

            //var weightExist = await _uow.WeightLimit.ExistAsync(x => x.Weight.Equals(weightLimit.Weight));
            
            var weightExist = await _uow.WeightLimit.ExistAsync(x => x.WeightLimitId > 0);

            if (weightExist == true)
                throw new GenericException("ONLY ONE WEIGHT LIMIT IS ALLOWED");

            var weight = new WeightLimit
            {
                Weight = weightLimit.Weight
            };
            _uow.WeightLimit.Add(weight);
            await _uow.CompleteAsync();
            return new { Id = weight.WeightLimitId };
        }

        public async Task<WeightLimitDTO> GetWeightLimitById(int weightLimitId)
        {
            var weight = await _uow.WeightLimit.GetAsync(x => x.WeightLimitId == weightLimitId);
            return Mapper.Map<WeightLimitDTO>(weight);
        }

        public Task<IEnumerable<WeightLimitDTO>> GetWeightLimits()
        {
            return Task.FromResult(Mapper.Map<IEnumerable<WeightLimitDTO>>(_uow.WeightLimit.GetAll()));
        }

        public async Task<WeightLimitDTO> GetActiveWeightLimits()
        {
            return await Task.FromResult(Mapper.Map<WeightLimitDTO>(_uow.WeightLimit.GetAll().OrderByDescending(x => x.WeightLimitId).First()));
        }

        public async Task RemoveWeightLimit(int weightLimitId)
        {
            var limit = await _uow.WeightLimit.GetAsync(weightLimitId);

            if (limit == null)
            {
                throw new GenericException("WEIGHT LIMIT DOES NOT EXIST");
            }
            _uow.WeightLimit.Remove(limit);
            await _uow.CompleteAsync();
        }

        public async Task UpdateWeightLimit(int weightLimitId, WeightLimitDTO weightLimitDto)
        {
            if (weightLimitDto == null)
                throw new GenericException("NULL_INPUT");

            var weight = _uow.WeightLimit.Get(weightLimitId);
            if (weight == null)
                throw new GenericException("WEIGHT_LIMIT_DOES_NOT_EXIST");

            weight.Weight = weightLimitDto.Weight;
            await _uow.CompleteAsync();
        }
    }
}
