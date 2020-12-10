using GIGL.GIGLS.Core.Domain;
using GIGL.GIGLS.Core.Repositories;
using GIGLS.CORE.DTO.Report;
using GIGLS.CORE.DTO.Shipments;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories.Shipments
{
    public interface IGroupWaybillNumberMappingRepository : IRepository<GroupWaybillNumberMapping>
    {
        Task<List<GroupWaybillNumberMappingDTO>> GetGroupWaybillMappings(int[] serviceCentreIds);
        Task<List<GroupWaybillNumberMappingDTO>> GetGroupWaybillMappings(FilterOptionsDto filterOptionsDto, int[] serviceCentreIds);
        Task<List<GroupWaybillNumberMappingDTO>> GetGroupWaybillMappings(int[] serviceCentreIds, DateFilterCriteria dateFilterCriteria);
        Task<List<string>> GetGroupWaybillMappingWaybills(int[] serviceCentreIds);
    }

    public interface IMovementManifestNumberMappingRepository : IRepository<MovementManifestNumberMapping> 
    {
        Task<List<MovementManifestNumberMappingDTO>> GetMovmentManifestMappings(int[] serviceCentreIds);
        Task<List<MovementManifestNumberMappingDTO>> GetMovmentManifestMappings(FilterOptionsDto filterOptionsDto, int[] serviceCentreIds); 
        Task<List<MovementManifestNumberMappingDTO>> GetMovmentManifestMappings(int[] serviceCentreIds, DateFilterCriteria dateFilterCriteria); 
        Task<List<string>> GetMovmentManifestMappingsManifest(int[] serviceCentreIds); 
    }
}
