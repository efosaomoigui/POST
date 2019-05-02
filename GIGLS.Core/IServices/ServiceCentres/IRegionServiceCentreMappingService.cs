using GIGLS.Core.DTO.ServiceCentres;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.ServiceCentres
{
    public interface IRegionServiceCentreMappingService : IServiceDependencyMarker
    {
        Task<IEnumerable<RegionServiceCentreMappingDTO>> GetAllRegionServiceCentreMappings();
        Task MappingServiceCentreToRegion(int regionId, List<int> serviceCentreId);
        Task<RegionServiceCentreMappingDTO> GetRegionForServiceCentre(int serviceCentreId);
        Task<RegionServiceCentreMappingDTO> GetServiceCentresInRegion(int regionId);
        Task RemoveServiceCentreFromRegion(int regionId, int serviceCentreId);
    }
}
