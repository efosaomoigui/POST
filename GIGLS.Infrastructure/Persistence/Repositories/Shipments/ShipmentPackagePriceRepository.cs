using GIGLS.Core.Domain;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.IRepositories.Shipments;
using GIGLS.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using GIGLS.Core.DTO;

namespace GIGLS.Infrastructure.Persistence.Repositories.Shipments
{
    public class ShipmentPackagePriceRepository : Repository<ShipmentPackagePrice, GIGLSContext>, IShipmentPackagePriceRepository
    {
        private GIGLSContext _context;

        public ShipmentPackagePriceRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }

        public Task<List<ShipmentPackagePriceDTO>> GetShipmentPackagePrices()
        {
            try
            {
                var packages = _context.ShipmentPackagePrice;
                var packageDto = from p in packages
                                 join c in Context.Country on p.CountryId equals c.CountryId
                                 select new ShipmentPackagePriceDTO
                                 {
                                     ShipmentPackagePriceId = p.ShipmentPackagePriceId,
                                     Price = p.Price,
                                     Description = p.Description,
                                     CountryId = p.CountryId,
                                     Country = new CountryDTO
                                     {
                                         CountryId = c.CountryId,
                                         CountryCode = c.CountryCode,
                                         CountryName = c.CountryName,
                                         CurrencySymbol = c.CurrencySymbol,
                                         CurrencyCode = c.CurrencyCode
                                     }
                                 };

                return Task.FromResult(packageDto.ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        public Task<ShipmentPackagePriceDTO> GetShipmentPackagePriceById(int shipmentPackagePriceId)
        {
            try
            {
                var packages = _context.ShipmentPackagePrice.Where(c => c.ShipmentPackagePriceId == shipmentPackagePriceId);

                var packageDto = from p in packages
                                join c in Context.Country on p.CountryId equals c.CountryId
                                select new ShipmentPackagePriceDTO
                                {
                                    ShipmentPackagePriceId = p.ShipmentPackagePriceId,
                                    Price = p.Price,
                                    Description = p.Description,
                                    CountryId = p.CountryId,
                                    Country = new CountryDTO
                                    {
                                        CountryId = c.CountryId,
                                        CountryCode = c.CountryCode,
                                        CountryName = c.CountryName,
                                        CurrencySymbol = c.CurrencySymbol,
                                        CurrencyCode = c.CurrencyCode
                                    }
                                };

                return Task.FromResult(packageDto.FirstOrDefault());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<List<ShipmentPackagePriceDTO>> GetShipmentPackagePriceByCountry(int countryId)
        {
            try
            {
                var packages = _context.ShipmentPackagePrice.Where(c => c.CountryId == countryId);

                var packageDto = from p in packages
                                 join c in Context.Country on p.CountryId equals c.CountryId
                                 select new ShipmentPackagePriceDTO
                                 {
                                     ShipmentPackagePriceId = p.ShipmentPackagePriceId,
                                     Price = p.Price,
                                     Description = p.Description,
                                     CountryId = p.CountryId,
                                     Country = new CountryDTO
                                     {
                                         CountryId = c.CountryId,
                                         CountryCode = c.CountryCode,
                                         CountryName = c.CountryName,
                                         CurrencySymbol = c.CurrencySymbol,
                                         CurrencyCode = c.CurrencyCode
                                     }
                                 };

                return Task.FromResult(packageDto.ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}