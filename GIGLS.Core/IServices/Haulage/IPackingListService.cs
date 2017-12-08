using GIGLS.Core.DTO.Haulage;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Haulage
{
    public interface IPackingListService : IServiceDependencyMarker
    {
        Task<IEnumerable<PackingListDTO>> GetPackingLists();
        Task<PackingListDTO> GetPackingListById(int packingListId);
        Task<PackingListDTO> GetPackingListByWaybill(string waybill);
        Task<object> AddPackingList(PackingListDTO packingListDto);
        Task UpdatePackingList(int packingListId, PackingListDTO packingListDto);
        Task RemovePackingList(int packingListId);
    }
}
