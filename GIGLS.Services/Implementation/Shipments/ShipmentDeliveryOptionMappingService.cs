using POST.Core.IServices.Shipments;
using System.Collections.Generic;
using System.Threading.Tasks;
using POST.Core.DTO.Shipments;
using POST.Core;
using System;
using POST.Core.Domain;
using AutoMapper;

namespace POST.Services.Implementation.Shipments
{
    public class ShipmentDeliveryOptionMappingService : IShipmentDeliveryOptionMappingService
    {
        private readonly IUnitOfWork _uow;

        public ShipmentDeliveryOptionMappingService(IUnitOfWork uow)
        {
            _uow = uow;
            MapperConfig.Initialize();
        }

        public async Task<object> AddShipmentDeliveryOptionMapping(ShipmentDeliveryOptionMappingDTO mapping)
        {
            try
            {
                var newMapping = Mapper.Map<ShipmentDeliveryOptionMapping>(mapping);
                _uow.ShipmentDeliveryOptionMapping.Add(newMapping);
                await _uow.CompleteAsync();
                return new { Id = newMapping };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ShipmentDeliveryOptionMappingDTO>> GetAllShipmentDeliveryOptionMappings()
        {
            return await _uow.ShipmentDeliveryOptionMapping.GetAllShipmentDeliveryOptionMappings();
        }

        public async Task<List<ShipmentDeliveryOptionMappingDTO>> GetDeliveryOptionInWaybill(string waybill)
        {
            return await _uow.ShipmentDeliveryOptionMapping.GetDeliveryOptionInWaybill(waybill);
        }
    }
}
