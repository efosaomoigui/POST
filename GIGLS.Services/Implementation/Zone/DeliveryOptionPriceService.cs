using GIGLS.Core.IServices.Zone;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Zone;
using GIGLS.Core;
using GIGLS.Infrastructure;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO;
using System.Net;

namespace GIGLS.Services.Implementation.Zone
{
    public class DeliveryOptionPriceService : IDeliveryOptionPriceService
    {
        private readonly IUnitOfWork _uow;
        private IZoneService _zoneService;
        private IDeliveryOptionService _deliveryService;

        public DeliveryOptionPriceService(IUnitOfWork uow, IZoneService zoneService, IDeliveryOptionService deliveryService)
        {
            _uow = uow;
            _zoneService = zoneService;
            _deliveryService = deliveryService;
        }

        public async Task<object> AddDeliveryOptionPrice(DeliveryOptionPriceDTO option)
        {
            try
            {                
                if (!await _uow.DeliveryOption.ExistAsync(c => c.DeliveryOptionId == option.DeliveryOptionId))
                {
                    throw new GenericException("Deliver Option Not Exist", $"{(int)HttpStatusCode.NotFound}");
                }

                if (!await _uow.Zone.ExistAsync(c => c.ZoneId == option.ZoneId))
                {
                    throw new GenericException("Zone Not Exist", $"{(int)HttpStatusCode.NotFound}");
                }

                if(await _uow.DeliveryOptionPrice.ExistAsync(c => c.ZoneId == option.ZoneId 
                    && c.DeliveryOptionId == option.DeliveryOptionId && c.CountryId == option.CountryId))
                {
                    throw new GenericException("Price has already been set for this Delivery Option");
                }

                var newOption = new DeliveryOptionPrice
                {
                      ZoneId = option.ZoneId,
                      DeliveryOptionId = option.DeliveryOptionId,
                      Price = option.Price,
                      CountryId = option.CountryId
                };
                _uow.DeliveryOptionPrice.Add(newOption);
                await _uow.CompleteAsync();
                return new { Id = newOption.DeliveryOptionPriceId };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteDeliveryOptionPrice(int optionId)
        {
            try
            {
                var option = await _uow.DeliveryOptionPrice.GetAsync(optionId);
                if (option == null)
                {
                    throw new GenericException("Delivery Option Price Not Exist", $"{(int)HttpStatusCode.NotFound}");
                }
                _uow.DeliveryOptionPrice.Remove(option);
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<decimal> GetDeliveryOptionPrice(int optionId, int zoneId, int countryId)
        {
            try
            {
                await _deliveryService.GetDeliveryOptionById(optionId);
                await _zoneService.GetZoneById(zoneId);
                var option = await _uow.DeliveryOptionPrice.GetDeliveryOptionPrices(optionId, zoneId, countryId);

                if (option == null)
                {
                    throw new GenericException("Delivery Option Price Not Exist", $"{(int)HttpStatusCode.NotFound}");
                }

                return option.Price;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<DeliveryOptionPriceDTO> GetDeliveryOptionPriceById(int optionId)
        {

            try
            {
                var option = await _uow.DeliveryOptionPrice.GetDeliveryOptionPrices(optionId);

                if (option == null)
                {
                    throw new GenericException("Delivery Option Price Not Exist", $"{(int)HttpStatusCode.NotFound}");
                }

                return option;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<DeliveryOptionPriceDTO>> GetDeliveryOptionPrices()
        {
            try
            {
                return await _uow.DeliveryOptionPrice.GetDeliveryOptionPrices();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateDeliveryOptionPrice(int optionId, DeliveryOptionPriceDTO optionDto)
        {
            try
            {
                if (!await _uow.DeliveryOption.ExistAsync(c => c.DeliveryOptionId == optionDto.DeliveryOptionId))
                {
                    throw new GenericException("Deliver Option Not Exist", $"{(int)HttpStatusCode.NotFound}");
                }

                if (!await _uow.Zone.ExistAsync(c => c.ZoneId == optionDto.ZoneId))
                {
                    throw new GenericException("Zone Not Exist", $"{(int)HttpStatusCode.NotFound}");
                }

                if (!await _uow.Country.ExistAsync(c => c.CountryId == optionDto.CountryId))
                {
                    throw new GenericException("Country Not Exist", $"{(int)HttpStatusCode.NotFound}");
                }

                var option = await _uow.DeliveryOptionPrice.GetAsync(optionId);
                if (option == null || optionDto.DeliveryOptionPriceId != optionId)
                {
                    throw new GenericException("Delivery Option Price Not Exist", $"{(int)HttpStatusCode.NotFound}");
                }

                option.Price = optionDto.Price;
                option.ZoneId = optionDto.ZoneId;
                option.DeliveryOptionId = optionDto.DeliveryOptionId;
                option.CountryId = optionDto.CountryId;
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}