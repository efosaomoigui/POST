using GIGLS.Core.DTO.Haulage;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices
{
    public interface IHaulageDistanceMappingPriceService : IServiceDependencyMarker
    {
        Task<IEnumerable<HaulageDistanceMappingPriceDTO>> GetHaulageDistanceMappingPrices();
        Task<HaulageDistanceMappingPriceDTO> GetHaulageDistanceMappingPriceById(int haulageDistanceMappingPriceId);
        Task<object> AddHaulageDistanceMappingPrice(HaulageDistanceMappingPriceDTO haulageDistanceMappingPrice);
        Task UpdateHaulageDistanceMappingPrice(int haulageDistanceMappingPriceId, HaulageDistanceMappingPriceDTO haulageDistanceMappingPriceDto);
        Task RemoveHaulageDistanceMappingPrice(int haulageDistanceMappingPriceId);
    }
}
