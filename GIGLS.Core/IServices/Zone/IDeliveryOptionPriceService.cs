using GIGLS.Core.DTO.Zone;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Zone
{
    public interface IDeliveryOptionPriceService : IServiceDependencyMarker
    {
        Task<IEnumerable<DeliveryOptionPriceDTO>> GetDeliveryOptionPrices();
        Task<DeliveryOptionPriceDTO> GetDeliveryOptionPriceById(int optionId);
        Task<decimal> GetDeliveryOptionPrice(int optionId, int zoneId, int countryId);
        Task<object> AddDeliveryOptionPrice(DeliveryOptionPriceDTO option);
        Task UpdateDeliveryOptionPrice(int optionId, DeliveryOptionPriceDTO option);
        Task DeleteDeliveryOptionPrice(int optionId);
    }
}
