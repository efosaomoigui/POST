using GIGL.GIGLS.Core.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGL.GIGLS.Core.Domain;
using GIGLS.Core.DTO.Zone;

namespace GIGLS.Core.IRepositories.Zone
{
    public interface IDeliveryOptionRepository : IRepository<DeliveryOption>
    {
        Task<List<DeliveryOptionDTO>> GetDeliveryOptions();
        Task<List<DeliveryOptionDTO>> GetActiveDeliveryOptions();
    }
}
