using GIGL.GIGLS.Core.Domain;
using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO.Shipments;
using GIGLS.CORE.DTO.Report;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories.Shipments
{
    public interface IManifestGroupWaybillNumberMappingRepository : IRepository<ManifestGroupWaybillNumberMapping>
    {
        Task<List<ManifestGroupWaybillNumberMappingDTO>> GetManifestGroupWaybillNumberMappings(int[] serviceCentreIds);
        Task<ManifestGroupWaybillNumberMappingDTO> GetManifestGroupWaybillNumberMappingsUsingGroupWaybill(string groupWaybill);
        Task<List<ManifestGroupWaybillNumberMappingDTO>> GetManifestGroupWaybillNumberMappings(int[] serviceCentreIds, DateFilterCriteria dateFilterCriteria);
        Task<List<ManifestDTO>> GetManifestSuperManifestMappings(int[] serviceCentreIds, DateFilterCriteria dateFilterCriteria);
        Task<List<MovementManifestNumberDTO>> GetManifestMovementNumberMappings(int[] serviceCentreIds, DateFilterCriteria dateFilterCriteria);
    }
}