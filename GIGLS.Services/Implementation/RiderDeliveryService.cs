using GIGLS.Core;
using GIGLS.Core.DTO;
using GIGLS.Core.DTO.Report;
using GIGLS.Core.IServices;
using System.Linq;
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
        
        public async Task<RiderDeliveryViewDTO> GetRiderDelivery (string riderId, ShipmentCollectionFilterCriteria dateFilterCriteria)
        {
            var result = new RiderDeliveryViewDTO();

            var riderDelivery = await _uow.RiderDelivery.GetRiderDelivery(riderId, dateFilterCriteria);

            if(riderDelivery.Count > 0)
            {
                result.RiderDelivery = riderDelivery;
                result.Total = riderDelivery.Sum(s => s.CostOfDelivery);
            }
            
            return result;
        }
    }
}