using GIGL.POST.Core.Repositories;
using POST.Core.Domain;
using POST.Core.DTO.Shipments;
using POST.Core.DTO.Fleets;
using POST.CORE.DTO.Report;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Core.IRepositories.Shipments
{
    public interface IManifestWaybillMappingRepository : IRepository<ManifestWaybillMapping>
    {
        Task<List<ManifestWaybillMappingDTO>> GetManifestWaybillMappings(int[] serviceCentreIds);
        Task<List<ManifestWaybillMappingDTO>> GetManifestWaybillMappings(int[] serviceCentreIds, DateFilterCriteria dateFilterCriteria);
        Task<List<ManifestWaybillMappingDTO>> GetManifestWaybillWaitingForSignOff(int[] serviceCentreIds, List<string> manifests);
        Task<List<DispatchDTO>> GetWaybillsinManifestMappings(string captainId, DateFilterCriteria dateFilterCriteria);
    }
}