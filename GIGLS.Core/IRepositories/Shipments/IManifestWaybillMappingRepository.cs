using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO.Shipments;
using GIGLS.CORE.DTO.Report;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories.Shipments
{
    public interface IManifestWaybillMappingRepository : IRepository<ManifestWaybillMapping>
    {
        Task<List<ManifestWaybillMappingDTO>> GetManifestWaybillMappings(int[] serviceCentreIds);
        Task<List<ManifestWaybillMappingDTO>> GetManifestWaybillMappings(int[] serviceCentreIds, DateFilterCriteria dateFilterCriteria);
        Task<List<ManifestWaybillMappingDTO>> GetManifestWaybillWaitingForSignOff(int[] serviceCentreIds, List<string> manifests);
    }
}