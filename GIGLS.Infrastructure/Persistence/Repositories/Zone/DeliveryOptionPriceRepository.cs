using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO.Zone;
using GIGLS.Core.IRepositories.Zone;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;
using System.Linq;
using GIGLS.Core.DTO;
using System;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.Zone
{
    public class DeliveryOptionPriceRepository : Repository<DeliveryOptionPrice, GIGLSContext>, IDeliveryOptionPriceRepository
    {
        public DeliveryOptionPriceRepository(GIGLSContext context) : base(context)
        {
        }

        public Task<List<DeliveryOptionPriceDTO>> GetDeliveryOptionPrices()
        {
            try
            {
                var options = Context.DeliveryOptionPrice.Include("Zone").Include("DeliveryOption");
                var optionDto = from r in options
                                join c in Context.Country on r.CountryId equals c.CountryId
                                select new DeliveryOptionPriceDTO
                                {
                                    DeliveryOptionPriceId = r.DeliveryOptionPriceId,
                                    ZoneId = r.ZoneId,
                                    DeliveryOptionId = r.DeliveryOptionId,
                                    Price = r.Price,
                                    ZoneName = r.Zone.ZoneName,
                                    DeliveryOption = r.DeliveryOption.Description,
                                    CountryId = c.CountryId,
                                    CountryDTO = new CountryDTO
                                    {
                                        CountryId = c.CountryId,
                                        CountryCode = c.CountryCode,
                                        CountryName = c.CountryName,
                                        CurrencySymbol = c.CurrencySymbol,
                                        CurrencyCode = c.CurrencyCode
                                    }
                                };
                return Task.FromResult(optionDto.ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<DeliveryOptionPriceDTO> GetDeliveryOptionPrices(int optionId)
        {
            try
            {
                var options = Context.DeliveryOptionPrice.Include("Zone").Include("DeliveryOption")
                                .Where(c => c.DeliveryOptionPriceId == optionId);

                var optionDto = from r in options
                                join c in Context.Country on r.CountryId equals c.CountryId
                                select new DeliveryOptionPriceDTO
                                {
                                    DeliveryOptionPriceId = r.DeliveryOptionPriceId,
                                    ZoneId = r.ZoneId,
                                    DeliveryOptionId = r.DeliveryOptionId,
                                    Price = r.Price,
                                    ZoneName = r.Zone.ZoneName,
                                    DeliveryOption = r.DeliveryOption.Description,
                                    CountryId = c.CountryId,
                                    CountryDTO = new CountryDTO
                                    {
                                        CountryId = c.CountryId,
                                        CountryCode = c.CountryCode,
                                        CountryName = c.CountryName,
                                        CurrencySymbol = c.CurrencySymbol,
                                        CurrencyCode = c.CurrencyCode
                                    }
                                };

                return Task.FromResult(optionDto.FirstOrDefault());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<DeliveryOptionPriceDTO> GetDeliveryOptionPrices(int optionId, int zoneId, int countryId)
        {
            try
            {
                var options = Context.DeliveryOptionPrice.Include("Zone").Include("DeliveryOption")
                                .Where(c => c.DeliveryOptionId == optionId && c.ZoneId == zoneId && c.CountryId == countryId);

                var optionDto = from r in options
                                join c in Context.Country on r.CountryId equals c.CountryId
                                select new DeliveryOptionPriceDTO
                                {
                                    DeliveryOptionPriceId = r.DeliveryOptionPriceId,
                                    ZoneId = r.ZoneId,
                                    DeliveryOptionId = r.DeliveryOptionId,
                                    Price = r.Price,
                                    ZoneName = r.Zone.ZoneName,
                                    DeliveryOption = r.DeliveryOption.Description,
                                    CountryId = c.CountryId,
                                    CountryDTO = new CountryDTO
                                    {
                                        CountryId = c.CountryId,
                                        CountryCode = c.CountryCode,
                                        CountryName = c.CountryName,
                                        CurrencySymbol = c.CurrencySymbol,
                                        CurrencyCode = c.CurrencyCode
                                    }
                                };

                return Task.FromResult(optionDto.FirstOrDefault());
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}