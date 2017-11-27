using GIGLS.Core.IServices.Zone;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Zone;
using GIGLS.Core;
using GIGLS.Infrastructure;
using GIGLS.Core.Domain;

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
                    throw new GenericException("Deliver Option Not Exist");
                }

                if (!await _uow.Zone.ExistAsync(c => c.ZoneId == option.ZoneId))
                {
                    throw new GenericException("Zone Not Exist");
                }

                if(await _uow.DeliveryOptionPrice.ExistAsync(
                        c => c.ZoneId == option.ZoneId && c.DeliveryOptionId == option.DeliveryOptionId))
                {
                    throw new GenericException("Price has already been set for this Delivery Option");
                }

                var newOption = new DeliveryOptionPrice
                {
                      ZoneId = option.ZoneId,
                      DeliveryOptionId = option.DeliveryOptionId,
                      Price = option.Price                                     
                };
                _uow.DeliveryOptionPrice.Add(newOption);
                await _uow.CompleteAsync();
                return new { Id = newOption.DeliveryOptionPriceId };
            }
            catch (Exception ex)
            {
                throw new GenericException(ex.Message);
            }
        }

        public async Task DeleteDeliveryOptionPrice(int optionId)
        {
            try
            {
                var option = await _uow.DeliveryOptionPrice.GetAsync(optionId);
                if (option == null)
                {
                    throw new GenericException("Delivery Option Price Not Exist");
                }
                _uow.DeliveryOptionPrice.Remove(option);
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<decimal> GetDeliveryOptionPrice(int optionId, int zoneId)
        {
            try
            {
                await _deliveryService.GetDeliveryOptionById(optionId);
                await _zoneService.GetZoneById(zoneId);

                var option = await _uow.DeliveryOptionPrice.GetAsync(o => o.DeliveryOptionId == optionId && o.ZoneId == zoneId, "Zone, DeliveryOption");
                if (option == null)
                {
                    throw new GenericException("Delivery Option Price Not Exist");
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
                var option = await _uow.DeliveryOptionPrice.GetAsync(o => o.DeliveryOptionPriceId == optionId, "Zone, DeliveryOption");
                if (option == null)
                {
                    throw new GenericException("Delivery Option Price Not Exist");
                }
                return new DeliveryOptionPriceDTO
                {
                    DeliveryOptionPriceId = option.DeliveryOptionPriceId,
                    Price = option.Price,
                    DeliveryOptionId = option.DeliveryOptionId,
                    ZoneId = option.ZoneId,
                    DeliveryOption = option.DeliveryOption.Description,
                    ZoneName = option.Zone.ZoneName
                };
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
                    throw new GenericException("Deliver Option Not Exist");
                }

                if (!await _uow.Zone.ExistAsync(c => c.ZoneId == optionDto.ZoneId))
                {
                    throw new GenericException("Zone Not Exist");
                }

                var option = await _uow.DeliveryOptionPrice.GetAsync(optionId);
                if (option == null || optionDto.DeliveryOptionPriceId != optionId)
                {
                    throw new GenericException("Delivery Option Price Not Exist");
                }
                option.Price = optionDto.Price;
                option.ZoneId = optionDto.ZoneId;
                option.DeliveryOptionId = optionDto.DeliveryOptionId;
                _uow.Complete();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
