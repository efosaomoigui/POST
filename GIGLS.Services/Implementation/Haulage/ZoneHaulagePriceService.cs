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
    public class ZoneHaulagePriceService : IZoneHaulagePriceService
    {
        private readonly IUnitOfWork _uow;
        private readonly IZoneService _zoneService;
        private readonly IHaulageService _haulageService;

        public ZoneHaulagePriceService(IUnitOfWork uow, IZoneService zoneService, IHaulageService haulageService)
        {
            _uow = uow;
            _zoneService = zoneService;
            _haulageService = haulageService;
            MapperConfig.Initialize();
        }

        public async Task<IEnumerable<ZoneHaulagePriceDTO>> GetZoneHaulagePrices()
        {
            var zoneHaulagePrices = await _uow.ZoneHaulagePrice.GetZoneHaulagePricesAsync();
            return zoneHaulagePrices;
        }
        
        public async Task<ZoneHaulagePriceDTO> GetZoneHaulagePriceById(int zoneHaulagePriceId)
        {
            var zoneHaulagePrice = await _uow.ZoneHaulagePrice.GetAsync(z => z.ZoneHaulagePriceId == zoneHaulagePriceId, "Haulage,Zone");
            
            if (zoneHaulagePrice == null)
            {
                throw new GenericException("Zone Haulage Price information does not exist");
            }
            var zoneHaulagePriceDto = Mapper.Map<ZoneHaulagePriceDTO>(zoneHaulagePrice);
            zoneHaulagePriceDto.Tonne = zoneHaulagePrice.Haulage.Tonne;
            zoneHaulagePriceDto.ZoneName = zoneHaulagePrice.Zone.ZoneName;
            return zoneHaulagePriceDto;
        }

        public async Task<object> AddZoneHaulagePrice(ZoneHaulagePriceDTO zoneHaulagePriceDto)
        {
            await _haulageService.GetHaulageById(zoneHaulagePriceDto.HaulageId);
            await _zoneService.GetZoneById(zoneHaulagePriceDto.ZoneId);

            if (await _uow.ZoneHaulagePrice.ExistAsync(v => v.HaulageId == zoneHaulagePriceDto.HaulageId && v.ZoneId == zoneHaulagePriceDto.ZoneHaulagePriceId))
            {
                throw new GenericException($"Price already set for this Tonne and Zone");
            }

            var newZoneHaulagePrice = new ZoneHaulagePrice
            {
                HaulageId = zoneHaulagePriceDto.HaulageId,
                ZoneId = zoneHaulagePriceDto.ZoneId,
                Price = zoneHaulagePriceDto.Price
            };

            _uow.ZoneHaulagePrice.Add(newZoneHaulagePrice);
            await _uow.CompleteAsync();
            return new { id = newZoneHaulagePrice.ZoneHaulagePriceId };
        }

        public async Task UpdateZoneHaulagePrice(int zoneHaulagePriceId, ZoneHaulagePriceDTO zoneHaulagePriceDto)
        {
            await _haulageService.GetHaulageById(zoneHaulagePriceDto.HaulageId);
            await _zoneService.GetZoneById(zoneHaulagePriceDto.ZoneId);

            var zoneHaulagePrice = await _uow.ZoneHaulagePrice.GetAsync(zoneHaulagePriceId);

            if (zoneHaulagePrice == null)
            {
                throw new GenericException("Zone Haulage Price information does not exist");
            }

            zoneHaulagePrice.HaulageId = zoneHaulagePriceDto.HaulageId;
            zoneHaulagePrice.ZoneId = zoneHaulagePriceDto.ZoneId;
            zoneHaulagePrice.Price = zoneHaulagePriceDto.Price;
            await _uow.CompleteAsync();
        }

        public async Task RemoveZoneHaulagePrice(int zoneHaulagePriceId)
        {
            var zoneHaulagePrice = await _uow.ZoneHaulagePrice.GetAsync(zoneHaulagePriceId);

            if (zoneHaulagePrice == null)
            {
                throw new GenericException("Zone Haulage Price information does not exist");
            }
            _uow.ZoneHaulagePrice.Remove(zoneHaulagePrice);
            await _uow.CompleteAsync();
        }        
    }
}
