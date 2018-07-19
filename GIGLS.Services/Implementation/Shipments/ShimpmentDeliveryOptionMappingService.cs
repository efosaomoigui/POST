using GIGLS.Core.IServices.Shipments;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core;
using System;
using GIGLS.Core.Domain;
using AutoMapper;

namespace GIGLS.Services.Implementation.Shipments
{
    public class ShimpmentDeliveryOptionMappingService : IShimpmentDeliveryOptionMappingService
    {
        private readonly IUnitOfWork _uow;

        public ShimpmentDeliveryOptionMappingService(IUnitOfWork uow)
        {
            _uow = uow;
            MapperConfig.Initialize();
        }

        public async Task<object> AddShimpmentDeliveryOptionMapping(ShimpmentDeliveryOptionMappingDTO mapping)
        {
            try
            {
                var newMapping = Mapper.Map<ShimpmentDeliveryOptionMapping>(mapping);
                _uow.ShimpmentDeliveryOptionMapping.Add(newMapping);
                await _uow.CompleteAsync();
                return new { Id = newMapping };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ShimpmentDeliveryOptionMappingDTO>> GetAllShimpmentDeliveryOptionMappings()
        {
            return await _uow.ShimpmentDeliveryOptionMapping.GetAllShimpmentDeliveryOptionMappings();
        }

        public async Task<List<ShimpmentDeliveryOptionMappingDTO>> GetDeliveryOptionInWaybill(string waybill)
        {
            return await _uow.ShimpmentDeliveryOptionMapping.GetDeliveryOptionInWaybill(waybill);
        }
    }
}
