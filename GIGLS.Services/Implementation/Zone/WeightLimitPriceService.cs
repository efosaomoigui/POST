using GIGLS.Core.IServices.Zone;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Zone;
using GIGLS.Core;
using GIGLS.Infrastructure;
using AutoMapper;
using GIGLS.Core.Domain;

namespace GIGLS.Services.Implementation.Zone
{
    public class WeightLimitPriceService : IWeightLimitPriceService
    {
        private readonly IUnitOfWork _uow;

        public WeightLimitPriceService(IUnitOfWork uow)
        {
            _uow = uow;
            MapperConfig.Initialize();
        }

        public async Task<object> AddWeightLimitPrice(WeightLimitPriceDTO weightLimitPrice)
        {
            if (weightLimitPrice == null)
                throw new GenericException("NULL_INPUT");
            var weightExist = await _uow.WeightLimitPrice.ExistAsync(x => x.ZoneId == weightLimitPrice.ZoneId && x.Weight.Equals(weightLimitPrice.Weight));

            if (weightExist == true)
                throw new GenericException("WEIGHT_LIMIT_ALREADY_EXIST");

            var weight = new WeightLimitPrice
            {
                Weight = weightLimitPrice.Weight,
                ZoneId = weightLimitPrice.ZoneId,
                Price = weightLimitPrice.Price
            };
            _uow.WeightLimitPrice.Add(weight);
            await _uow.CompleteAsync();
            return new { Id = weight.WeightLimitPriceId };
        }

        public async Task<WeightLimitPriceDTO> GetWeightLimitPriceById(int weightLimitPriceId)
        {
            var weight = await _uow.WeightLimitPrice.GetAsync(x => x.WeightLimitPriceId == weightLimitPriceId);
            return Mapper.Map<WeightLimitPriceDTO>(weight);
        }

        public async Task<WeightLimitPriceDTO> GetWeightLimitPriceByZoneId(int zoneId)
        {
            var weight = await _uow.WeightLimitPrice.GetAsync(x => x.ZoneId == zoneId);
            return Mapper.Map<WeightLimitPriceDTO>(weight);
        }

        public async Task<List<WeightLimitPriceDTO>> GetWeightLimitPrices()
        {
            return await _uow.WeightLimitPrice.GetWeightLimitPrices();
            //return Task.FromResult(Mapper.Map<IEnumerable<WeightLimitPriceDTO>>(_uow.WeightLimitPrice.GetAll("Zone")));
        }

        public async Task RemoveWeightLimitPrice(int weightLimitId)
        {
            var limit = await _uow.WeightLimitPrice.GetAsync(weightLimitId);

            if (limit == null)
            {
                throw new GenericException("WEIGHT_LIMIT_PRICE_DOES_NOT_EXIST");
            }
            _uow.WeightLimitPrice.Remove(limit);
            await _uow.CompleteAsync();
        }

        public async Task UpdateWeightLimitPrice(int weightLimitId, WeightLimitPriceDTO weightLimitPriceDto)
        {
            if (weightLimitPriceDto == null)
                throw new GenericException("NULL_INPUT");

            var weight = _uow.WeightLimitPrice.Get(weightLimitId);

            if (weight == null)
                throw new GenericException("WEIGHT_LIMIT_DOES_NOT_EXIST");

            weight.Weight = weightLimitPriceDto.Weight;
            weight.Price = weightLimitPriceDto.Price;
            weight.ZoneId = weightLimitPriceDto.ZoneId;

            await _uow.CompleteAsync();
        }
    }
}
