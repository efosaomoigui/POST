using GIGLS.Core.DTO.Stores;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Stores
{
    public interface IStoreService : IServiceDependencyMarker
    {
        Task<StoreDTO> GetStores();
        Task<StoreDTO> GetStoreById(int storeId);
        Task<object> AddStore(StoreDTO store);
        Task UpdateStore(int storeId, StoreDTO store);
        Task DeleteStore(int storeId);
    }
}
