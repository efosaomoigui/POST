using GIGLS.Core.DTO.Zone;
using GIGLS.Core.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Zone
{
    public interface IDomesticZonePriceService : IServiceDependencyMarker
    {
        Task<IEnumerable<DomesticZonePriceDTO>> GetDomesticZonePrices();
        Task<DomesticZonePriceDTO> GetDomesticZonePriceById(int domesticZonePriceId);
        Task<decimal> GetDomesticZonePrice(int zoneId, decimal weight, RegularEcommerceType regularEcommerceType, int countryId);
        Task<object> AddDomesticZonePrice(DomesticZonePriceDTO domesticZonePrice);
        Task UpdateDomesticZonePrice(int domesticZonePriceId, DomesticZonePriceDTO domesticZonePrice);
        Task DeleteDomesticZonePrice(int domesticZonePriceId);
    }
}
