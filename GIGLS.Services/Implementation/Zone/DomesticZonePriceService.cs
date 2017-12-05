using System;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Zone;
using GIGLS.Core.IServices.Zone;
using GIGLS.Core;
using GIGL.GIGLS.Core.Domain;
using System.Collections.Generic;
using GIGLS.Infrastructure;

namespace GIGLS.Services.Implementation.Zone
{
    public class DomesticZonePriceService : IDomesticZonePriceService
    {
        private readonly IUnitOfWork _uow;
        private readonly IZoneService _zoneService;

        public DomesticZonePriceService(IUnitOfWork uow, IZoneService zoneService)
        {
            _uow = uow;
            _zoneService = zoneService;
        }

        public async Task<object> AddDomesticZonePrice(DomesticZonePriceDTO domesticZonePrice)
        {
            try
            {
                await _zoneService.GetZoneById(domesticZonePrice.ZoneId);

                if (await _uow.DomesticZonePrice.ExistAsync(c => c.Weight == domesticZonePrice.Weight && c.ZoneId == domesticZonePrice.ZoneId))
                {
                    throw new GenericException("Price already set for this Zone and Weight");
                }

                var newPrice = new DomesticZonePrice
                {
                    Weight = domesticZonePrice.Weight,
                    Price = domesticZonePrice.Price,
                    ZoneId = domesticZonePrice.ZoneId,
                    RegularEcommerceType = domesticZonePrice.RegularEcommerceType
                };

                _uow.DomesticZonePrice.Add(newPrice);

                await _uow.CompleteAsync();

                return new { Id = newPrice.DomesticZonePriceId };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteDomesticZonePrice(int DomesticZoneId)
        {
            try
            {
                var zone = await _uow.DomesticZonePrice.GetAsync(DomesticZoneId);
                if (zone != null)
                {
                    _uow.DomesticZonePrice.Remove(zone);
                    _uow.Complete();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<decimal> GetDomesticZonePrice(int zoneId, decimal weight)
        {
            try
            {
                var zone = await _uow.DomesticZonePrice.GetAsync(d => d.ZoneId == zoneId && d.Weight.Equals(weight));

                if (zone == null)
                {
                    throw new Exception("Price not yet set for this Zone and Weight");
                }
                return zone.Price;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<DomesticZonePriceDTO> GetDomesticZonePriceById(int domesticZonePriceId)
        {
            try
            {
                var zone = await _uow.DomesticZonePrice.GetAsync(d => d.DomesticZonePriceId == domesticZonePriceId, "Zone");

                if (zone == null)
                {
                    throw new Exception("Price for this Zone and Weight does not exist");
                }

                return new DomesticZonePriceDTO
                {
                    DomesticZonePriceId = zone.DomesticZonePriceId,
                    Weight = zone.Weight,
                    Price = zone.Price,
                    ZoneId = zone.ZoneId,
                    ZoneName = zone.Zone.ZoneName,
                    RegularEcommerceType = zone.RegularEcommerceType
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<DomesticZonePriceDTO>> GetDomesticZonePrices()
        {
            try
            {
                return await _uow.DomesticZonePrice.GetDomesticZones();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateDomesticZonePrice(int domesticZoneId, DomesticZonePriceDTO domesticZoneDto)
        {
            try
            {
                await _zoneService.GetZoneById(domesticZoneDto.ZoneId);

                var zone = await _uow.DomesticZonePrice.GetAsync(domesticZoneId);
                if (zone == null || domesticZoneDto.DomesticZonePriceId != domesticZoneId)
                {
                    throw new Exception("Price for this Zone and Weight does not exist");
                }
                zone.Weight = domesticZoneDto.Weight;
                zone.Price = domesticZoneDto.Price;
                zone.ZoneId = domesticZoneDto.ZoneId;
                zone.RegularEcommerceType = domesticZoneDto.RegularEcommerceType;
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
