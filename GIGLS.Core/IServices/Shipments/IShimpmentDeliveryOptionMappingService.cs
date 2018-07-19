using GIGLS.Core.DTO.Shipments;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Shipments
{
    public interface IShimpmentDeliveryOptionMappingService : IServiceDependencyMarker
    {
        Task<List<ShimpmentDeliveryOptionMappingDTO>> GetAllShimpmentDeliveryOptionMappings();
        Task<List<ShimpmentDeliveryOptionMappingDTO>> GetDeliveryOptionInWaybill(string waybill);
        Task<object> AddShimpmentDeliveryOptionMapping(ShimpmentDeliveryOptionMappingDTO mapping);
    }
}
