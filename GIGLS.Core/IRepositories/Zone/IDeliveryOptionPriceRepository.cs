using GIGL.POST.Core.Repositories;
using POST.Core.Domain;
using POST.Core.DTO.Zone;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Core.IRepositories.Zone
{
    public interface IDeliveryOptionPriceRepository : IRepository<DeliveryOptionPrice>
    {
        Task<List<DeliveryOptionPriceDTO>> GetDeliveryOptionPrices();
        Task<DeliveryOptionPriceDTO> GetDeliveryOptionPrices(int optionId);
        Task<DeliveryOptionPriceDTO> GetDeliveryOptionPrices(int optionId, int zoneId, int countryId);
    }
}