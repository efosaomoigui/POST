using GIGLS.Core.DTO.Shipments;
using GIGLS.CORE.DTO.Shipments;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Shipments
{
    public interface IGroupWaybillNumberMappingService : IServiceDependencyMarker
    {
        Task<IEnumerable<GroupWaybillNumberMappingDTO>> GetAllGroupWayBillNumberMappings();
        //Task MappingWaybillNumberToGroup(int groupWaybillNumberId, int waybillNumberId);
        Task MappingWaybillNumberToGroup(string groupWaybillNumber, string waybillNumber);
        Task MappingWaybillNumberToGroup(string groupWaybillNumber, List<string> waybillNumber);
        //Task<GroupWaybillNumberDTO> GetGroupForWaybillNumber(int waybillNumberId);
        Task<GroupWaybillNumberDTO> GetGroupForWaybillNumber(string waybillNumber);
        Task<List<WaybillNumberDTO>> GetWaybillNumbersInGroup(int groupWaybillNumberId);
        Task<List<WaybillNumberDTO>> GetWaybillNumbersInGroup(string groupWaybillNumber);
        Task RemoveWaybillNumberFromGroup(string groupWaybillNumber, string waybillNumber);
    }
}
