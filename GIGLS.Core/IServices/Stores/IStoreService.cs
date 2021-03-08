using GIGLS.Core.DTO.Stores;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Stores
{
    public interface IStoreService : IServiceDependencyMarker
    {
        Task<IEnumerable<StoreDTO>> GetStores();
        Task<StoreDTO> GetStoreById(int storeId);
        Task<object> AddStore(StoreDTO store);
        Task UpdateStore(int storeId, StoreDTO store);
        Task DeleteStore(int storeId);
    }
}
