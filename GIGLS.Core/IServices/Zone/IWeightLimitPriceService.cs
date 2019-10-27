using GIGLS.Core.DTO.Zone;
using GIGLS.Core.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Zone
{
    public interface IWeightLimitPriceService : IServiceDependencyMarker
    {
        Task<List<WeightLimitPriceDTO>> GetWeightLimitPrices();
        Task<WeightLimitPriceDTO> GetWeightLimitPriceById(int weightLimitId);
        Task<WeightLimitPriceDTO> GetWeightLimitPriceByZoneId(int zoneId, int countryId);
        Task<WeightLimitPriceDTO> GetWeightLimitPriceByZoneId(int zoneId, RegularEcommerceType regularEcommerceType, int countryId);
        Task<object> AddWeightLimitPrice(WeightLimitPriceDTO weightLimit);
        Task UpdateWeightLimitPrice(int weightLimitId, WeightLimitPriceDTO weightLimit);
        Task RemoveWeightLimitPrice(int weightLimitId);
    }
}
