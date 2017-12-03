using GIGLS.Core.DTO.Haulage;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices
{
    public interface IZoneHaulagePriceService : IServiceDependencyMarker
    {
        Task<IEnumerable<ZoneHaulagePriceDTO>> GetZoneHaulagePrices();
        Task<ZoneHaulagePriceDTO> GetZoneHaulagePriceById(int zoneHaulagePriceId);
        Task<object> AddZoneHaulagePrice(ZoneHaulagePriceDTO zoneHaulagePrice);
        Task UpdateZoneHaulagePrice(int zoneHaulagePriceId, ZoneHaulagePriceDTO zoneHaulagePriceDto);
        Task RemoveZoneHaulagePrice(int zoneHaulagePriceId);
    }
}
