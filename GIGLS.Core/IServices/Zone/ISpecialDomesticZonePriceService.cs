using GIGLS.Core.DTO.Zone;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Zone
{
    public interface ISpecialDomesticZonePriceService : IServiceDependencyMarker
    {
        Task<IEnumerable<SpecialDomesticZonePriceDTO>> GetSpecialDomesticZonePrices();
        Task<SpecialDomesticZonePriceDTO> GetSpecialDomesticZonePriceById(int SpecialDomesticZoneId);
        Task<decimal> GetSpecialZonePrice(int package, int zone);
        Task<object> AddSpecialDomesticZonePrice(SpecialDomesticZonePriceDTO newSpecialZonePrice);
        Task UpdateSpecialDomesticZonePrice(int SpecialDomesticZoneId, SpecialDomesticZonePriceDTO SpecialDomestic);
        Task DeleteSpecialDomesticZonePrice(int SpecialDomesticZoneId);
    }
}
