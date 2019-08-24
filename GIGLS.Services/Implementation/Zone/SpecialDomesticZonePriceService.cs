using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Zone;
using GIGLS.Core.IServices.Zone;
using GIGLS.Core;
using GIGL.GIGLS.Core.Domain;
using GIGLS.Infrastructure;

namespace GIGLS.Services.Implementation.Zone
{
    public class SpecialDomesticZonePriceService : ISpecialDomesticZonePriceService
    {
        private readonly IUnitOfWork _uow;
        private readonly ISpecialDomesticPackageService _packageService;
        private readonly IZoneService _zoneService;

        public SpecialDomesticZonePriceService(IUnitOfWork uow, ISpecialDomesticPackageService packageService, IZoneService zoneService)
        {
            _uow = uow;
            _packageService = packageService;
            _zoneService = zoneService;
        }

        public async Task<object> AddSpecialDomesticZonePrice(SpecialDomesticZonePriceDTO newSpecialZonePrice)
        {
            try
            {
                if (await _uow.SpecialDomesticZonePrice
                                .ExistAsync(
                                     c => c.ZoneId == newSpecialZonePrice.ZoneId && c.Weight == newSpecialZonePrice.Weight
                                     && c.SpecialDomesticPackageId == newSpecialZonePrice.SpecialDomesticPackageId
                                     ))
                {
                    throw new GenericException("Price already set for the parameter selected");
                }

                var newPrice = new SpecialDomesticZonePrice
                {
                   Description = newSpecialZonePrice.Description,
                   Price = newSpecialZonePrice.Price,
                   SpecialDomesticPackageId  = newSpecialZonePrice.SpecialDomesticPackageId,
                   Weight = newSpecialZonePrice.Weight,
                   ZoneId = newSpecialZonePrice.ZoneId,
                   //UserId = newSpecialZonePrice.UserId
                };

                _uow.SpecialDomesticZonePrice.Add(newPrice);
                await _uow.CompleteAsync();
                return new { Id = newPrice.SpecialDomesticZonePriceId};
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteSpecialDomesticZonePrice(int specialDomesticZonePriceId)
        {
            try
            {
                var zone = await _uow.SpecialDomesticZonePrice.GetAsync(specialDomesticZonePriceId);
                if (zone != null)
                {
                    // throw new GenericException("");
                    _uow.SpecialDomesticZonePrice.Remove(zone);
                    _uow.Complete();
                }               
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<SpecialDomesticZonePriceDTO> GetSpecialDomesticZonePriceById(int specialDomesticZonePriceId)
        {
            try
            {
                var specialDomestic = await _uow.SpecialDomesticZonePrice.GetAsync( s => s.SpecialDomesticZonePriceId == specialDomesticZonePriceId, "SpecialDomesticPackage,Zone");
                if (specialDomestic == null)
                {
                    throw new GenericException("Not Exist");
                }

                return new SpecialDomesticZonePriceDTO
                {
                    SpecialDomesticZonePriceId = specialDomestic.SpecialDomesticZonePriceId,
                    Description = specialDomestic.Description,
                    Price = specialDomestic.Price,
                    Weight = specialDomestic.Weight??0,
                    ZoneId = specialDomestic.ZoneId,
                    ZoneName = specialDomestic.Zone.ZoneName,
                    SpecialDomesticPackageId = specialDomestic.SpecialDomesticPackageId,
                    SpecialDomesticPackageName = specialDomestic.SpecialDomesticPackage.Name
                    //UserName    = specialDomestic.User.FirstName + " " + specialDomestic.User.LastName,
                    //UserId      = specialDomestic.UserId,
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<SpecialDomesticZonePriceDTO>> GetSpecialDomesticZonePrices()
        {
            try
            {
                return await _uow.SpecialDomesticZonePrice.GetSpecialDomesticZonesPrice();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateSpecialDomesticZonePrice(int SpecialDomesticZoneId, SpecialDomesticZonePriceDTO specialDomestic)
        {
            try
            {
                var zonePrice = await _uow.SpecialDomesticZonePrice.GetAsync(SpecialDomesticZoneId);
                if (zonePrice == null)
                {
                    throw new GenericException("Special Zone Price does not exist");
                }

                if (zonePrice.SpecialDomesticZonePriceId != specialDomestic.SpecialDomesticZonePriceId)
                    throw new GenericException("Invalid Special Zone Price update");

                zonePrice.Description = specialDomestic.Description;                
                zonePrice.Price  = specialDomestic.Price;
                zonePrice.Weight = specialDomestic.Weight;
                zonePrice.ZoneId = specialDomestic.ZoneId;  

                //zonePrice.UserId = specialDomestic.UserId;                              
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<decimal> GetSpecialZonePrice(int package, int zone, int countryId, decimal weight = 0)
        {
            try
            {
                await _zoneService.GetZoneById(zone);

                var specialPackage = await _packageService.GetSpecialDomesticPackageById(package);

                var specialDomestic = await _uow.SpecialDomesticZonePrice.GetAsync(s => 
                    s.SpecialDomesticPackageId == package && 
                    s.ZoneId == zone &&
                    s.CountryId == countryId);
                if (specialDomestic == null)
                {
                    throw new GenericException("Special Zone Price not yet set for selected parameters");
                }

                //Calculate Price for Special Shipment whose weight are greater than 30 KG
                if (specialPackage.SpecialDomesticPackageType == Core.Enums.SpecialDomesticPackageType.Special)
                {
                    if(specialPackage.Weight > weight)
                    {
                        throw new GenericException("Kindly supply correct weight for the package or select another package");
                    }

                    specialDomestic.Price = specialDomestic.Price * weight;
                }

                return specialDomestic.Price;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
