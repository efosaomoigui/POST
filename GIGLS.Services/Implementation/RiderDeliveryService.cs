using AutoMapper;
using GIGLS.Core;
using GIGLS.Core.DTO;
using GIGLS.Core.DTO.Report;
using GIGLS.Core.IServices;
using GIGLS.CORE.DTO.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Services.Implementation
{
    public class RiderDeliveryService : IRiderDeliveryService
    {
        private readonly IUnitOfWork _uow;
        public RiderDeliveryService(IUnitOfWork uow)
        {
            _uow = uow;
            MapperConfig.Initialize();
        }
        //public Task<IEnumerable<RiderDeliveryDTO>> GetRiderDelivery(DateFilterCriteria dateFilterCriteria)
        //{
        //    var locations = _uow.DeliveryLocation.GetAll();
        //    return Task.FromResult(Mapper.Map<IEnumerable<DeliveryLocationDTO>>(locations));
        //}
        public async Task<List<RiderDeliveryDTO>> GetRiderDelivery (string riderId, ShipmentCollectionFilterCriteria dateFilterCriteria)
        {
            var result = new List<RiderDeliveryDTO>();

            var riderDelivery = await _uow.RiderDelivery.GetRiderDelivery(riderId, dateFilterCriteria);
            foreach (var item in riderDelivery)
            {
                result.Add(item);
            }
            return result.OrderByDescending(x => x.DateCreated).ToList();

        }
    }
}
