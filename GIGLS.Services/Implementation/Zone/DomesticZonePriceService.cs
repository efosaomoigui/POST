using System;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Zone;
using GIGLS.Core.IServices.Zone;
using GIGLS.Core;
using GIGL.GIGLS.Core.Domain;
using System.Collections.Generic;
using GIGLS.Infrastructure;
using GIGLS.Core.Enums;
using AutoMapper;
using System.Linq;
using GIGLS.Core.DTO;
using System.Net;

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

                if (await _uow.DomesticZonePrice.ExistAsync(c => c.Weight == domesticZonePrice.Weight && c.ZoneId == domesticZonePrice.ZoneId && c.RegularEcommerceType == domesticZonePrice.RegularEcommerceType))
                {
                    throw new GenericException("Price already set for this Zone and Weight");
                }

                var newPrice = new DomesticZonePrice
                {
                    Weight = domesticZonePrice.Weight,
                    Price = domesticZonePrice.Price,
                    ZoneId = domesticZonePrice.ZoneId,
                    RegularEcommerceType = domesticZonePrice.RegularEcommerceType,
                    CountryId = domesticZonePrice.CountryId
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

        public async Task<decimal> GetDomesticZonePrice(int zoneId, decimal weight, RegularEcommerceType regularEcommerceType, int countryId)
        {
            try
            {
                var zone = await _uow.DomesticZonePrice.GetDomesticZonePrice(zoneId, weight, regularEcommerceType, countryId);

                if (zone == null)
                {
                    throw new GenericException($"Price not yet set for this Zone {zoneId} and Weight {weight}", $"{(int)HttpStatusCode.NotFound}");
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

                var countries = _uow.Country.GetAll().ToList();
                var countriesDTO = Mapper.Map<IEnumerable<CountryDTO>>(countries);

                var country = countriesDTO.FirstOrDefault(s => s.CountryId == zone.CountryId);

                return new DomesticZonePriceDTO
                {
                    DomesticZonePriceId = zone.DomesticZonePriceId,
                    Weight = zone.Weight,
                    Price = zone.Price,
                    ZoneId = zone.ZoneId,
                    ZoneName = zone.Zone.ZoneName,
                    CountryId = zone.CountryId,
                    Country = country,
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
                var domesticZones = await _uow.DomesticZonePrice.GetDomesticZones();

                var countries = _uow.Country.GetAll().ToList();
                var countriesDTO = Mapper.Map<IEnumerable<CountryDTO>>(countries);

                foreach (var domesticZone in domesticZones)
                {
                    var country = countriesDTO.FirstOrDefault(s => s.CountryId == domesticZone.CountryId);
                    domesticZone.Country = country;
                }


                return domesticZones;
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
                zone.CountryId = domesticZoneDto.CountryId;
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
