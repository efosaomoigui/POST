using GIGLS.Core.Domain;
using GIGLS.Core.DTO.Shipments;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Shipments
{
    public interface IGroupWaybillNumberService : IServiceDependencyMarker
    {
        Task<IEnumerable<GroupWaybillNumberDTO>> GetAllGroupWayBillNumbers();
        Task<List<GroupWaybillNumberDTO>> GetActiveGroupWayBillNumbers();
        Task<List<GroupWaybillNumberDTO>> GetDeliverGroupWayBillNumbers();
        Task<GroupWaybillNumberDTO> GetGroupWayBillNumberById(int groupwaybillId);
        Task<GroupWaybillNumberDTO> GetGroupWayBillNumberById(string groupwaybillNumber);
        Task<string> GenerateGroupWaybillNumber(GroupWaybillNumberDTO groupWaybillNumberDTO);
        Task UpdateGroupWaybillNumber(int groupwaybillId);
        Task UpdateGroupWaybillNumber(string groupwaybillNumber);
        Task RemoveGroupWaybillNumber(int groupwaybillId);
        Task RemoveGroupWaybillNumber(string groupwaybillId);
        Task<GroupWaybillNumber> GetGroupWayBillNumberForScan(string groupwaybillNumber);
    }
}
