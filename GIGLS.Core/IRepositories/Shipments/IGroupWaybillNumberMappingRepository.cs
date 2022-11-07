using GIGL.POST.Core.Domain;
using GIGL.POST.Core.Repositories;
using POST.Core.DTO.Shipments;
using POST.CORE.DTO.Report;
using POST.CORE.DTO.Shipments;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Core.IRepositories.Shipments
{
    public interface IGroupWaybillNumberMappingRepository : IRepository<GroupWaybillNumberMapping>
    {
        Task<List<GroupWaybillNumberMappingDTO>> GetGroupWaybillMappings(int[] serviceCentreIds);
        Task<List<GroupWaybillNumberMappingDTO>> GetGroupWaybillMappings(FilterOptionsDto filterOptionsDto, int[] serviceCentreIds);
        Task<List<GroupWaybillNumberMappingDTO>> GetGroupWaybillMappings(int[] serviceCentreIds, DateFilterCriteria dateFilterCriteria);
        Task<List<string>> GetGroupWaybillMappingWaybills(int[] serviceCentreIds);
        Task<List<AllManifestAndGroupWaybillDTO>> GetGroupWaybillMappings(string groupCode);
    }

    public interface IMovementManifestNumberMappingRepository : IRepository<MovementManifestNumberMapping> 
    {
        Task<List<MovementManifestNumberMappingDTO>> GetMovmentManifestMappings(int[] serviceCentreIds);
        Task<List<MovementManifestNumberMappingDTO>> GetMovmentManifestMappings(FilterOptionsDto filterOptionsDto, int[] serviceCentreIds); 
        Task<List<MovementManifestNumberMappingDTO>> GetMovmentManifestMappings(int[] serviceCentreIds, DateFilterCriteria dateFilterCriteria); 
        Task<List<string>> GetMovmentManifestMappingsManifest(int[] serviceCentreIds); 
    }
}
