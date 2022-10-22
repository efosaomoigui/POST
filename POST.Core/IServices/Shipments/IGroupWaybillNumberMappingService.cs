using POST.Core.DTO.Shipments;
using POST.CORE.DTO.Report;
using POST.CORE.DTO.Shipments;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Core.IServices.Shipments
{
    public interface IGroupWaybillNumberMappingService : IServiceDependencyMarker
    {
        Task<IEnumerable<GroupWaybillNumberMappingDTO>> GetAllGroupWayBillNumberMappings();
        Task<IEnumerable<GroupWaybillNumberMappingDTO>> GetAllGroupWayBillNumberMappings(DateFilterCriteria dateFilterCriteria);
        Task MappingWaybillNumberToGroup(string groupWaybillNumber, List<string> waybillNumber);
        Task MappingWaybillNumberToGroup(List<GroupWaybillNumberMappingDTO> groupingData);
        Task<GroupWaybillNumberDTO> GetGroupForWaybillNumber(string waybillNumber);
        Task<GroupWaybillNumberDTO> GetGroupForWaybillNumberByServiceCentre(string waybillNumber);
        Task<GroupWaybillNumberMappingDTO> GetWaybillNumbersInGroup(int groupWaybillNumberId);
        Task<GroupWaybillNumberMappingDTO> GetWaybillNumbersInGroup(string groupWaybillNumber);
        Task RemoveWaybillNumberFromGroup(string groupWaybillNumber, string waybillNumber);
        Task MappingWaybillNumberToGroupForOverdue(string groupWaybillNumber, List<string> waybillNumber);
    }
}