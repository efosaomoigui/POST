using GIGL.POST.Core.Repositories;
using POST.Core.Domain;
using POST.Core.DTO.Shipments;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Core.IRepositories.Shipments
{
    public interface IShipmentDeliveryOptionMappingRepository : IRepository<ShipmentDeliveryOptionMapping>
    {

        Task<List<ShipmentDeliveryOptionMappingDTO>> GetAllShipmentDeliveryOptionMappings();
        Task<List<ShipmentDeliveryOptionMappingDTO>> GetDeliveryOptionInWaybill(string waybill);
    }
}
