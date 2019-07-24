using AutoMapper;
using GIGL.GIGLS.Core.Domain;
using GIGLS.Core;
using GIGLS.Core.DTO.Zone;
using GIGLS.Core.Enums;
using GIGLS.Core.IServices.Zone;
using GIGLS.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GIGLS.Services.Implementation.Zone
{
    public class ZoneService : IZoneService
    {
        private readonly IUnitOfWork _uow;
        public ZoneService(IUnitOfWork uow)
        {
            _uow = uow;
            MapperConfig.Initialize();
        }

        public async Task<object> AddZone(ZoneDTO zone)
        {
            try
            {
                if (await _uow.Zone.ExistAsync(c => c.ZoneName.ToLower() == zone.ZoneName.Trim().ToLower()))
                {
                    throw new GenericException("Zone information already exist");
                }
                var newZone = Mapper.Map<Core.Domain.Zone>(zone);
                _uow.Zone.Add(newZone);
                await _uow.CompleteAsync();
                return new { Id = newZone.ZoneId };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteZone(int zoneId)
        {
            try
            {
                var zone = await _uow.Zone.GetAsync(zoneId);
                if (zone == null)
                {
                    throw new GenericException("Zone information does not exist");
                }
                _uow.Zone.Remove(zone);
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<ZoneDTO>> GetActiveZones()
        {
            try
            {
                return await _uow.Zone.GetActiveZonesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ZoneDTO> GetZoneById(int zoneId)
        {
            try
            {
                var zone = await _uow.Zone.GetAsync(zoneId);
                if (zone == null)
                {
                    throw new GenericException("Zone information does not exist");
                }

                var zoneDTO = Mapper.Map<ZoneDTO>(zone);
                return zoneDTO;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<ZoneDTO>> GetZones()
        {
            try
            {
                return await _uow.Zone.GetZonesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateZone(int zoneId, ZoneDTO zoneDto)
        {
            try
            {
                var zone = await _uow.Zone.GetAsync(zoneId);
                if (zone == null || zoneDto.ZoneId != zoneId)
                {
                    throw new GenericException("Zone information does not exist");
                }
                zone.ZoneName = zoneDto.ZoneName;
                zone.Status = zoneDto.Status;
                // zone.RowVersion = zoneDto.RowVersion;
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateZone(int zoneId, bool status)
        {
            try
            {
                var zone = await _uow.Zone.GetAsync(zoneId);
                if (zone == null)
                {
                    throw new GenericException("Zone information does not exist");
                }
                zone.Status = status;
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> UpdateAllPriceByZone(ZonePercentDTO zonePercent)
        {
            try
            {
                //For Regular Category
                if (zonePercent.PriceType == "Regular")
                {
                    if (zonePercent.CustomerType == "ECommerce")
                    {
                        var priceList = _uow.DomesticZonePrice.GetAllAsQueryable().Where(x => x.RegularEcommerceType == RegularEcommerceType.Ecommerce).ToList();
                        UpdateAllDomesticZonePrice(priceList, zonePercent);
                    }

                    if (zonePercent.CustomerType == "Individual")
                    {
                        var priceList = _uow.DomesticZonePrice.GetAllAsQueryable().Where(x => x.RegularEcommerceType == RegularEcommerceType.Regular).ToList();
                        UpdateAllDomesticZonePrice(priceList, zonePercent);
                    }
                }

                //For Special Category
                if (zonePercent.PriceType == "Special")
                {
                    var priceList = _uow.SpecialDomesticZonePrice.GetAll().ToList();
                    UpdateAllSpecialDomesticZonePrice(priceList, zonePercent);
                }                             

                await _uow.CompleteAsync();
                return true;
            }
            
            catch (Exception)
            {
                throw;
            }
        }
               
        public async Task<bool> UpdateAllPriceByWeight(WeightPercentDTO weightPercentDTO)
        {
            try
            {
                if (weightPercentDTO.PriceType == "Regular")
                {
                    if (weightPercentDTO.CustomerType == "ECommerce")
                    {
                        if(weightPercentDTO.WeightOne > 0)
                        {
                            var priceList = _uow.DomesticZonePrice.FindAsync(x => x.RegularEcommerceType == RegularEcommerceType.Ecommerce && (x.Weight <= 2.00M)).Result.ToList();
                            if (weightPercentDTO.ModificationType == "Increase")
                            {
                                UpdateAllDomesticZonePriceUsingWeightIncrement(priceList, weightPercentDTO.WeightOne);
                            }
                            
                            else if (weightPercentDTO.ModificationType == "Decrease")
                            {
                                UpdateAllDomesticZonePriceUsingWeightDecrement(priceList, weightPercentDTO.WeightOne);
                            }
                        }

                        if (weightPercentDTO.WeightTwo > 0)
                        {
                            var priceList = _uow.DomesticZonePrice.FindAsync(x => x.RegularEcommerceType == RegularEcommerceType.Ecommerce && (x.Weight > 2.00M && x.Weight <= 4.00M)).Result.ToList();
                            if (weightPercentDTO.ModificationType == "Increase")
                            {
                                UpdateAllDomesticZonePriceUsingWeightIncrement(priceList, weightPercentDTO.WeightTwo);
                            }
                            else if (weightPercentDTO.ModificationType == "Decrease")
                            {
                                UpdateAllDomesticZonePriceUsingWeightDecrement(priceList, weightPercentDTO.WeightTwo);
                            }

                        }
                        if (weightPercentDTO.WeightThree > 0)
                        {
                            var priceList = _uow.DomesticZonePrice.FindAsync(x => x.RegularEcommerceType == RegularEcommerceType.Ecommerce && (x.Weight > 4.00M)).Result.ToList();
                            if (weightPercentDTO.ModificationType == "Increase")
                            {
                                UpdateAllDomesticZonePriceUsingWeightIncrement(priceList, weightPercentDTO.WeightThree);
                            }
                            else if (weightPercentDTO.ModificationType == "Decrease")
                            {
                                UpdateAllDomesticZonePriceUsingWeightDecrement(priceList, weightPercentDTO.WeightThree);
                            }
                        }
                    }
                    if (weightPercentDTO.CustomerType == "Individual")
                    {
                        if (weightPercentDTO.WeightOne > 0)
                        {
                            var priceList = _uow.DomesticZonePrice.FindAsync(x => x.RegularEcommerceType == RegularEcommerceType.Regular && (x.Weight <= 2.00M)).Result.ToList();
                            if (weightPercentDTO.ModificationType == "Increase")
                            {
                                UpdateAllDomesticZonePriceUsingWeightIncrement(priceList, weightPercentDTO.WeightOne);
                            }
                            else if (weightPercentDTO.ModificationType == "Decrease")
                            {
                                UpdateAllDomesticZonePriceUsingWeightDecrement(priceList, weightPercentDTO.WeightOne);
                            }
                        }
                        if (weightPercentDTO.WeightTwo > 0)
                        {
                            var priceList = _uow.DomesticZonePrice.FindAsync(x => x.RegularEcommerceType == RegularEcommerceType.Regular && (x.Weight > 2.00M && x.Weight <= 4.00M)).Result.ToList();
                            if (weightPercentDTO.ModificationType == "Increase")
                            {
                                UpdateAllDomesticZonePriceUsingWeightIncrement(priceList, weightPercentDTO.WeightTwo);
                            }
                            else if (weightPercentDTO.ModificationType == "Decrease")
                            {
                                UpdateAllDomesticZonePriceUsingWeightDecrement(priceList, weightPercentDTO.WeightTwo);
                            }
                        }
                        if (weightPercentDTO.WeightThree > 0)
                        {
                            var priceList = _uow.DomesticZonePrice.FindAsync(x => x.RegularEcommerceType == RegularEcommerceType.Regular && (x.Weight > 4.00M)).Result.ToList();
                            if (weightPercentDTO.ModificationType == "Increase")
                            {
                                UpdateAllDomesticZonePriceUsingWeightIncrement(priceList, weightPercentDTO.WeightThree);
                            }
                            else if (weightPercentDTO.ModificationType == "Decrease")
                            {
                                UpdateAllDomesticZonePriceUsingWeightDecrement(priceList, weightPercentDTO.WeightThree);
                            }
                        }
                    }
                }

                if (weightPercentDTO.PriceType == "Special")
                {
                    if (weightPercentDTO.WeightOne > 0)
                    {
                        var priceList = _uow.SpecialDomesticPackage.Find(x => x.Weight <= 2.00M)
                                                                    .Select(x => x.SpecialDomesticPackageId)
                                                                    .ToList();

                        UpdateAllSpecialDomesticZonePriceUsingWeight(priceList, weightPercentDTO.WeightOne,weightPercentDTO);
                    }

                    if (weightPercentDTO.WeightTwo > 0)
                    {
                        var priceList = _uow.SpecialDomesticPackage.Find(x => x.Weight > 2.00M && x.Weight <= 4.00M)
                                                                    .Select(x => x.SpecialDomesticPackageId)
                                                                    .ToList();

                        UpdateAllSpecialDomesticZonePriceUsingWeight(priceList, weightPercentDTO.WeightTwo, weightPercentDTO);
                    }

                    if (weightPercentDTO.WeightThree > 0)
                    {
                        var priceList = _uow.SpecialDomesticPackage.Find(x => x.Weight > 4.00M)
                                                                    .Select(x => x.SpecialDomesticPackageId)
                                                                    .ToList();

                        UpdateAllSpecialDomesticZonePriceUsingWeight(priceList, weightPercentDTO.WeightThree, weightPercentDTO);
                    }
                }

                await _uow.CompleteAsync();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void UpdateAllDomesticZonePrice(List<DomesticZonePrice> priceList, ZonePercentDTO zonePercent)
        {
            foreach (var entry in priceList)
            {
                var entryPrice = entry.Price;
                var entryPercent = zonePercent.Zones.Where(x => x.ZoneId == entry.ZoneId).Select(x => x.ZonePercent).FirstOrDefault();
                var newPrice = 0.0M;
                if (zonePercent.ModificationType == "Increase")
                {
                    newPrice = entryPrice * (1 + (entryPercent / 100));
                }
                else if (zonePercent.ModificationType == "Decrease")
                {
                    newPrice = entryPrice * (1 - (entryPercent / 100));
                }
                //set new Price
                entry.Price = newPrice;
            }
        }

        private void UpdateAllSpecialDomesticZonePrice(List<SpecialDomesticZonePrice> priceList, ZonePercentDTO zonePercent)
        {
            foreach (var entry in priceList)
            {
                var entryPrice = entry.Price;
                var entryPercent = zonePercent.Zones.Where(x => x.ZoneId == entry.ZoneId).Select(x => x.ZonePercent).FirstOrDefault();
                var newPrice = 0.0M;
                if (zonePercent.ModificationType == "Increase")
                {
                    newPrice = entryPrice * (1 + (entryPercent / 100));
                }
                else if (zonePercent.ModificationType == "Decrease")
                {
                    newPrice = entryPrice * (1 - (entryPercent / 100));
                }

                //set new Price
                entry.Price = newPrice;
            }
        }

        private void UpdateAllDomesticZonePriceUsingWeightIncrement(List<DomesticZonePrice> priceList, decimal weightPercent)
        {
            foreach (var entry in priceList)
            {
                var entryPrice = entry.Price;

                var newPrice = entryPrice * (1 + (weightPercent / 100));
                entry.Price = newPrice;
            }
        }
        public void UpdateAllDomesticZonePriceUsingWeightDecrement(List<DomesticZonePrice> priceList, decimal weightPercent)
        {
            foreach(var entry in priceList)
            {
                var entryPrice = entry.Price;

                var newPrice = entryPrice * (1 - (weightPercent / 100));
                entry.Price = newPrice;
            }
        }

        private void UpdateAllSpecialDomesticZonePriceUsingWeight(List<int> priceList, decimal weightPercent, WeightPercentDTO weightPercentDTOs)
        {

            foreach (var entry in priceList)
            {
                var prices = _uow.SpecialDomesticZonePrice.GetAll().Where(x => x.SpecialDomesticPackageId == entry)
                                                                    .ToList();
                foreach(var price in prices)
                {
                    var entryPrice = price.Price;
                    if (weightPercentDTOs.ModificationType == "Increase")
                    {
                        var newPrice = entryPrice * (1 + (weightPercent / 100));
                        price.Price = newPrice;
                    }
                    else if (weightPercentDTOs.ModificationType == "Decrease")
                    {
                        var newPrice = entryPrice * (1 - (weightPercent / 100));
                        price.Price = newPrice;
                    }
                }
            }
        }
    }
}