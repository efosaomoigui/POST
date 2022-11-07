using GIGL.POST.Core.Domain;
using GIGL.POST.Core.Repositories;
using POST.Core.DTO.Stores;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Core.IRepositories.Stores
{
    public interface IStoreRepository : IRepository<Store>
    {
        Task<List<StoreDTO>> GetStoresByCountryId(int countryId);
        Task<List<StoreDTO>> GetStores();
        Task<StoreDTO> GetStoreById(int storeId);
    }
}
