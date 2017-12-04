using AutoMapper;
using GIGLS.Core;
using GIGLS.Core.Domain;
using GIGLS.Core.IServices;
using GIGLS.Infrastructure;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Haulage;
using GIGLS.Core.IServices.Zone;

namespace GIGLS.Services.Implementation
{
    public class HaulageDistanceMappingPriceService : IHaulageDistanceMappingPriceService
    {
        private readonly IUnitOfWork _uow;
        private readonly IZoneService _zoneService;
        private readonly IHaulageService _haulageService;

        public HaulageDistanceMappingPriceService(IUnitOfWork uow, IZoneService zoneService, IHaulageService haulageService)
        {
            _uow = uow;
            _zoneService = zoneService;
            _haulageService = haulageService;
            MapperConfig.Initialize();
        }

        public async Task<IEnumerable<HaulageDistanceMappingPriceDTO>> GetHaulageDistanceMappingPrices()
        {
            var haulageDistanceMappingPrices = await _uow.HaulageDistanceMappingPrice.GetHaulageDistanceMappingPricesAsync();
            return haulageDistanceMappingPrices;
        }
        
        public async Task<HaulageDistanceMappingPriceDTO> GetHaulageDistanceMappingPriceById(int haulageDistanceMappingPriceId)
        {
            var haulageDistanceMappingPrice = await _uow.HaulageDistanceMappingPrice.GetAsync(z => z.HaulageDistanceMappingPriceId == haulageDistanceMappingPriceId, "Haulage");
            
            if (haulageDistanceMappingPrice == null)
            {
                throw new GenericException("Haulage Price information does not exist");
            }
            var zoneHaulagePriceDto = Mapper.Map<HaulageDistanceMappingPriceDTO>(haulageDistanceMappingPrice);
            return zoneHaulagePriceDto;
        }

        public async Task<object> AddHaulageDistanceMappingPrice(HaulageDistanceMappingPriceDTO haulageDistanceMappingPriceDto)
        {
            await _haulageService.GetHaulageById(haulageDistanceMappingPriceDto.HaulageId);

            if (await _uow.HaulageDistanceMappingPrice.ExistAsync(v => 
            v.HaulageId == haulageDistanceMappingPriceDto.HaulageId &&
            v.StartRange == haulageDistanceMappingPriceDto.StartRange &&
            v.EndRange == haulageDistanceMappingPriceDto.EndRange))
            {
                throw new GenericException($"Price already set for this Tonne and Distance");
            }

            var newHaulageDistanceMappingPrice = new HaulageDistanceMappingPrice
            {
                HaulageId = haulageDistanceMappingPriceDto.HaulageId,
                StartRange = haulageDistanceMappingPriceDto.StartRange,
                EndRange = haulageDistanceMappingPriceDto.EndRange,
                Price = haulageDistanceMappingPriceDto.Price
            };

            _uow.HaulageDistanceMappingPrice.Add(newHaulageDistanceMappingPrice);
            await _uow.CompleteAsync();
            return new { id = newHaulageDistanceMappingPrice.HaulageDistanceMappingPriceId };
        }

        public async Task UpdateHaulageDistanceMappingPrice(int haulageDistanceMappingPriceId, HaulageDistanceMappingPriceDTO haulageDistanceMappingPriceDto)
        {
            await _haulageService.GetHaulageById(haulageDistanceMappingPriceDto.HaulageId);

            var haulageDistanceMappingPrice = await _uow.HaulageDistanceMappingPrice.GetAsync(haulageDistanceMappingPriceId);

            if (haulageDistanceMappingPrice == null)
            {
                throw new GenericException("Haulage Price information does not exist");
            }

            haulageDistanceMappingPrice.HaulageId = haulageDistanceMappingPriceDto.HaulageId;
            haulageDistanceMappingPrice.StartRange = haulageDistanceMappingPriceDto.StartRange;
            haulageDistanceMappingPrice.EndRange = haulageDistanceMappingPriceDto.EndRange;
            haulageDistanceMappingPrice.Price = haulageDistanceMappingPriceDto.Price;
            await _uow.CompleteAsync();
        }

        public async Task RemoveHaulageDistanceMappingPrice(int haulageDistanceMappingPriceId)
        {
            var haulageDistanceMappingPrice = await _uow.HaulageDistanceMappingPrice.GetAsync(haulageDistanceMappingPriceId);

            if (haulageDistanceMappingPrice == null)
            {
                throw new GenericException("Haulage Price information does not exist");
            }
            _uow.HaulageDistanceMappingPrice.Remove(haulageDistanceMappingPrice);
            await _uow.CompleteAsync();
        }        
    }
}
