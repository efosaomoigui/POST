using GIGLS.Core.DTO.Zone;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Zone
{
    public interface IHaulageDistanceMappingService : IServiceDependencyMarker
    {
        Task<IEnumerable<HaulageDistanceMappingDTO>> GetHaulageDistanceMappings();
        Task<HaulageDistanceMappingDTO> GetHaulageDistanceMappingById(int haulageDistanceMappingId);
        Task<object> AddHaulageDistanceMapping(HaulageDistanceMappingDTO haulageDistanceMapping);
        Task UpdateHaulageDistanceMapping(int haulageDistanceMappingId, HaulageDistanceMappingDTO haulageDistanceMapping);
        Task UpdateStatusHaulageDistanceMapping(int haulageDistanceMappingId, bool status);
        Task DeleteHaulageDistanceMapping(int haulageDistanceMappingId);
        Task<HaulageDistanceMappingDTO> GetHaulageDistanceMapping(int departure, int destination);
    }

}
