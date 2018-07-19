using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO.Shipments;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories.Shipments
{
    public interface IShimpmentDeliveryOptionMappingRepository : IRepository<ShimpmentDeliveryOptionMapping>
    {

        Task<List<ShimpmentDeliveryOptionMappingDTO>> GetAllShimpmentDeliveryOptionMappings();
        Task<List<ShimpmentDeliveryOptionMappingDTO>> GetDeliveryOptionInWaybill(string waybill);
    }
}
