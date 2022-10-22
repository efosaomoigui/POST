using GIGL.POST.Core.Repositories;
using POST.Core.Domain;
using POST.Core.DTO.Shipments;
using POST.CORE.DTO.Report;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Core.IRepositories.Shipments
{
    public interface IHUBManifestWaybillMappingRepository : IRepository<HUBManifestWaybillMapping>
    {
        Task<List<HUBManifestWaybillMappingDTO>> GetHUBManifestWaybillMappings(int[] serviceCentreIds);
        Task<List<HUBManifestWaybillMappingDTO>> GetHUBManifestWaybillMappings(int[] serviceCentreIds, DateFilterCriteria dateFilterCriteria);
        Task<List<HUBManifestWaybillMappingDTO>> GetHUBManifestWaybillWaitingForSignOff(int[] serviceCentreIds, List<string> manifests);
    }
}