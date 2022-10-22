using POST.Core.DTO.Stores;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Core.IServices.Stores
{
    public interface IStoreService : IServiceDependencyMarker
    {
        Task<List<StoreDTO>> GetStores();
        Task<StoreDTO> GetStoreById(int storeId);
        Task<object> AddStore(StoreDTO store);
        Task UpdateStore(int storeId, StoreDTO store);
        Task DeleteStore(int storeId);
    }
}
