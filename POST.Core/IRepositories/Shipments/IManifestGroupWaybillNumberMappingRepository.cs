using GIGL.POST.Core.Domain;
using GIGL.POST.Core.Repositories;
using POST.Core.Domain;
using POST.Core.DTO.Shipments;
using POST.CORE.DTO.Report;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Core.IRepositories.Shipments
{
    public interface IManifestGroupWaybillNumberMappingRepository : IRepository<ManifestGroupWaybillNumberMapping>
    {
        Task<List<ManifestGroupWaybillNumberMappingDTO>> GetManifestGroupWaybillNumberMappings(int[] serviceCentreIds);
        Task<ManifestGroupWaybillNumberMappingDTO> GetManifestGroupWaybillNumberMappingsUsingGroupWaybill(string groupWaybill);
        Task<List<ManifestGroupWaybillNumberMappingDTO>> GetManifestGroupWaybillNumberMappings(int[] serviceCentreIds, DateFilterCriteria dateFilterCriteria);
        Task<List<ManifestDTO>> GetManifestSuperManifestMappings(int[] serviceCentreIds, DateFilterCriteria dateFilterCriteria);
        Task<List<MovementManifestNumberDTO>> GetManifestMovementNumberMappings(int[] serviceCentreIds, DateFilterCriteria dateFilterCriteria);
        Task<List<MovementManifestNumberDTO>> GetExpectedManifestMovementNumberMappings(int[] serviceCentreIds, DateFilterCriteria dateFilterCriteria);
        Task<List<AllManifestAndGroupWaybillDTO>> GetGroupWaybillNumberMappingsUsingManifestCode(string manifestCode);
    }
}