using GIGLS.Core.DTO.ServiceCentres;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.ServiceCentres
{
    public interface IRegionService : IServiceDependencyMarker
    {
        Task<IEnumerable<RegionDTO>> GetRegions();
        Task<RegionDTO> GetRegionById(int regionId);
        Task<object> AddRegion(RegionDTO region);
        Task UpdateRegion(int regionId, RegionDTO region);
        Task DeleteRegion(int regionId);
    }
}
