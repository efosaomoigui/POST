using GIGLS.Core.DTO.Shipments;
using GIGLS.CORE.DTO.Report;
using GIGLS.CORE.DTO.Shipments;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Shipments
{
    public interface IManifestGroupWaybillNumberMappingService : IServiceDependencyMarker
    {
        Task<IEnumerable<ManifestGroupWaybillNumberMappingDTO>> GetAllManifestGroupWayBillNumberMappings();
        Task<IEnumerable<ManifestGroupWaybillNumberMappingDTO>> GetAllManifestGroupWayBillNumberMappings(DateFilterCriteria dateFilterCriteria);
        Task MappingManifestToGroupWaybillNumber(string manifest, List<string> groupWaybillNumber);
        Task<ManifestDTO> GetManifestForGroupWaybillNumber(int groupWaybillNumberId);
        Task<ManifestDTO> GetManifestForGroupWaybillNumber(string groupWaybillNumber);
        Task<List<GroupWaybillNumberDTO>> GetGroupWaybillNumbersInManifest(int manifestId);
        Task<List<GroupWaybillNumberDTO>> GetGroupWaybillNumbersInManifest(string manifest);
        Task RemoveGroupWaybillNumberFromManifest(string manifest, string groupWaybillNumber);
        Task RemoveManifestFromSuperManifest(string superManifest, string manifest);
        Task<ManifestGroupWaybillNumberMappingDTO> GetManifestForWaybill(string waybill);
        Task<ManifestDTO> GetManifestSearch(string manifestCode);
        Task<List<ManifestWaybillMappingDTO>> GetWaybillsInListOfManifest(string captainId);
        Task<List<ManifestWaybillMappingDTO>> GetAllWaybillsinListOfManifest(string captainId, DateFilterCriteria dateFilterCriteria);
        Task<string> MoveManifestDetailToNewManifest(string manifestCode);
        Task MappingSuperManifestToManifest(string superManifest, List<string> manifestList);
        Task<IEnumerable<ManifestDTO>> GetAllManifestSuperManifestMappings(DateFilterCriteria dateFilterCriteria);
        Task<List<ManifestDTO>> GetManifestsInSuperManifest(string superManifestCode);
        Task<ManifestDTO> GetSuperManifestForManifest(string manifest);
        Task<List<ManifestDTO>> GetManifestsInSuperManifestDetails(string superManifestCode);
        Task MappingMovementManifestToManifest(string movementmanifestCode, List<string> manifestList, int destinationid);
        Task UpdateMappingMovementManifestToManifest(string movementmanifestCode, List<string> manifestList, int destinationScId);
        Task<IEnumerable<MovementManifestNumberDTO>> GetAllManifestMovementManifestNumberMappings(DateFilterCriteria dateFilterCriteria);
        Task<IEnumerable<MovementManifestNumberDTO>> GetExpectedManifestMovementManifestNumberMappings(DateFilterCriteria dateFilterCriteria); 
        Task<List<MovementManifestNumberMappingDTOTwo>> GetManifestNumbersInMovementManifest(string movementmanifestCode);
        Task<List<GroupWaybillAndWaybillDTO>> GetGroupWaybillDataInManifest(string manifest);
        Task RemoveManifestFromMovementManifest(string manifest, string groupWaybillNumber);
    }
}