using GIGL.POST.Core.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGL.POST.Core.Domain;
using POST.Core.DTO.Zone;

namespace POST.Core.IRepositories.Zone
{
    public interface IDeliveryOptionRepository : IRepository<DeliveryOption>
    {
        Task<List<DeliveryOptionDTO>> GetDeliveryOptions();
        Task<List<DeliveryOptionDTO>> GetActiveDeliveryOptions();
    }
}
