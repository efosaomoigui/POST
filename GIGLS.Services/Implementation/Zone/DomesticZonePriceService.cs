﻿using System;
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

        public DomesticZonePriceService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<object> AddDomesticZonePrice(DomesticZonePriceDTO domesticZonePrice)
        {
            try
            {
                if (await _uow.DomesticZonePrice.ExistAsync(c => c.Weight == domesticZonePrice.Weight && c.ZoneId == domesticZonePrice.ZoneId))
                {
                    throw new GenericException("Price already set for this Zone and Weight");
                }

                var newPrice = new DomesticZonePrice
                {
                    Weight = domesticZonePrice.Weight,
                    Price = domesticZonePrice.Price,
                    ZoneId = domesticZonePrice.ZoneId
                };

                _uow.DomesticZonePrice.Add(newPrice);

                await _uow.CompleteAsync();

                return new { Id = newPrice.DomesticZonePriceId};
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
                var zone = await _uow.DomesticZonePrice.GetAsync(d => d.DomesticZonePriceId == domesticZonePriceId,  "Zone");

                if (zone == null)
                {
                    throw new Exception("Price for this Zone and Weight does not exist");
                }

                return new DomesticZonePriceDTO
                {
                    DomesticZonePriceId = zone.DomesticZonePriceId,
                    Weight  = zone.Weight,
                    Price   = zone.Price,
                    ZoneId  = zone.ZoneId,
                    ZoneName = zone.Zone.ZoneName
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
                var zone = await _uow.DomesticZonePrice.GetAsync(domesticZoneId);
                if (zone == null || domesticZoneDto.DomesticZonePriceId != domesticZoneId)
                {
                    throw new Exception("Price for this Zone and Weight does not exist");
                }
                zone.Weight = domesticZoneDto.Weight;
                zone.Price = domesticZoneDto.Price;
                zone.ZoneId = domesticZoneDto.ZoneId;                
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
