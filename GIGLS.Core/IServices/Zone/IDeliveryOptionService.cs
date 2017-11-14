using GIGLS.Core.DTO.Zone;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Zone
{
    public interface IDeliveryOptionService : IServiceDependencyMarker
    {
        Task<IEnumerable<DeliveryOptionDTO>> GetDeliveryOptions();
        Task<IEnumerable<DeliveryOptionDTO>> GetActiveDeliveryOptions();
        Task<DeliveryOptionDTO> GetDeliveryOptionById(int optionId);
        Task<object> AddDeliveryOption(DeliveryOptionDTO option);
        Task UpdateDeliveryOption(int optionId, DeliveryOptionDTO option);
        Task UpdateStatusDeliveryOption(int optionId, bool status);
        Task DeleteDeliveryOption(int optionId);
    }
}
