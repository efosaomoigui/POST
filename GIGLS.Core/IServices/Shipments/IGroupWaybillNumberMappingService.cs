using GIGLS.Core.DTO.Shipments;
using GIGLS.CORE.DTO.Shipments;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Shipments
{
    public interface IGroupWaybillNumberMappingService : IServiceDependencyMarker
    {
        Task<IEnumerable<GroupWaybillNumberMappingDTO>> GetAllGroupWayBillNumberMappings();
        Task MappingWaybillNumberToGroup(string groupWaybillNumber, List<string> waybillNumber);
        Task<GroupWaybillNumberDTO> GetGroupForWaybillNumber(string waybillNumber);
        Task<GroupWaybillNumberDTO> GetGroupForWaybillNumberByServiceCentre(string waybillNumber);
        Task<GroupWaybillNumberMappingDTO> GetWaybillNumbersInGroup(int groupWaybillNumberId);
        Task<GroupWaybillNumberMappingDTO> GetWaybillNumbersInGroup(string groupWaybillNumber);
        Task RemoveWaybillNumberFromGroup(string groupWaybillNumber, string waybillNumber);
        Task MappingWaybillNumberToGroupForOverdue(string groupWaybillNumber, List<string> waybillNumber);
    }
}
