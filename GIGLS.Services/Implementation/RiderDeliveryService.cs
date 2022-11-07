using POST.Core;
using POST.Core.DTO;
using POST.Core.DTO.Report;
using POST.Core.IServices;
using System.Linq;
using System.Threading.Tasks;

namespace POST.Services.Implementation
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