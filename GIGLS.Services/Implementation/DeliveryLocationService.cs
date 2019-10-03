using AutoMapper;
using GIGLS.Core;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO;
using GIGLS.Core.IServices;
using GIGLS.Infrastructure;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Services.Implementation
{
    public class DeliveryLocationService : IDeliveryLocationService
    {
        private readonly IUnitOfWork _uow;

        public DeliveryLocationService(IUnitOfWork uow)
        {
            _uow = uow;
            MapperConfig.Initialize();
        }

        public Task<IEnumerable<DeliveryLocationDTO>> GetDeliveryLocations()
        {
            var locations = _uow.DeliveryLocation.GetAll();
            return Task.FromResult(Mapper.Map<IEnumerable<DeliveryLocationDTO>>(locations));
        }

        public async Task<object> AddDeliveryLocationPrice(DeliveryLocationDTO deliveryLocationDTO)
        {
            var deliveryExist = await _uow.DeliveryLocation.ExistAsync(x => x.Location.ToUpper() == deliveryLocationDTO.Location.ToUpper());

            if(deliveryExist == true)
            {
                throw new GenericException("Delivery Location already exist");
            }

            var delivery = new DeliveryLocation
            {
                Location = deliveryLocationDTO.Location,
                Tariff = deliveryLocationDTO.Tariff
            };
            _uow.DeliveryLocation.Add(delivery);
            await _uow.CompleteAsync();
            return new { Id = delivery.DeliveryLocationId };
        }

        public async Task<DeliveryLocationDTO> GetDeliveryLocationById(int deliveryLocationId)
        {
            var delivery = await _uow.DeliveryLocation.GetAsync(x => x.DeliveryLocationId == deliveryLocationId);
            if (delivery == null)
            {
                throw new GenericException("Delivery location price does not exist");
            }
            return Mapper.Map<DeliveryLocationDTO>(delivery);
        }

        public async Task UpdateDeliveryLocationPrice(int deliveryLocationId, DeliveryLocationDTO deliveryLocationDTO)
        {
            if (deliveryLocationDTO == null)
                throw new GenericException("Null Input");

            var delivery = _uow.DeliveryLocation.Get(deliveryLocationId);

            if(delivery == null)
            {
                throw new GenericException("Delivery Location Price does not exist");
            }
            delivery.Location = deliveryLocationDTO.Location;
            delivery.Tariff = deliveryLocationDTO.Tariff;
            await _uow.CompleteAsync();
        }

        public async Task RemoveDeliveryLocationPrice(int deliveryLocationId)
        {
            var delivery = await _uow.DeliveryLocation.GetAsync(deliveryLocationId);

            if(delivery == null)
            {
                throw new GenericException("Delivery Location Price does not exist");
            }
            _uow.DeliveryLocation.Remove(delivery);
            await _uow.CompleteAsync();
        }



    }
}
