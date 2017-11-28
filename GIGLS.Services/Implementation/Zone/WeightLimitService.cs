using GIGLS.Core.IServices.Zone;
using System.Collections.Generic;
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

            var weightExist = await _uow.WeightLimit.ExistAsync(x => x.Weight.Equals(weightLimit.Weight));
            if (weightExist == true)
                throw new GenericException($"WEIGHT LIMIT {weightLimit.Weight}  ALREADY EXIST");

            //var weightExist = await _uow.WeightLimit.ExistAsync(x => x.WeightLimitId > 0);

            //if (weightExist == true)
            //    throw new GenericException("ONLY ONE WEIGHT LIMIT IS ALLOWED");

            //Get previous active weight limit and disable it
            var activeWeight = await _uow.WeightLimit.GetAsync(x => x.Status == true);
            if(activeWeight != null)
            {
                activeWeight.Status = false;
            }

            var weight = Mapper.Map<WeightLimit>(weightLimit);
            weight.Status = true;
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
            var weight = await _uow.WeightLimit.GetAsync(x => x.Status == true);
            if(weight == null)
            {
                throw new GenericException("WEIGHT LIMIT HAS NOT BEEN SET");
            }

            return await Task.FromResult(Mapper.Map<WeightLimitDTO>(weight));
            //return await Task.FromResult(Mapper.Map<WeightLimitDTO>(_uow.WeightLimit.GetAll().OrderByDescending(x => x.WeightLimitId).First()));
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
                throw new GenericException("WEIGHT LIMIT DOES NOT EXIST");

            if (weightLimitDto.Status)
            {
                //Get previous active weight limit and the update status is true
                var activeWeight = await _uow.WeightLimit.GetAsync(x => x.Status == true);
                if (activeWeight != null)
                {
                    activeWeight.Status = false;
                }
            }

            weight.Weight = weightLimitDto.Weight;
            weight.Status = weightLimitDto.Status;
            await _uow.CompleteAsync();
        }
    }
}
