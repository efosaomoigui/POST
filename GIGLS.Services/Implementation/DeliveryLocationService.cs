using AutoMapper;
using GIGLS.Core;
using GIGLS.Core.DTO;
using GIGLS.Core.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public Task<IEnumerable<DeliveryLocationDTO>> GetLocations()
        {
            var locations = _uow.DeliveryLocation.GetAll();
            return Task.FromResult(Mapper.Map<IEnumerable<DeliveryLocationDTO>>(locations));
        }
    }
}
