using GIGL.GIGLS.Core.Domain;
using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.DTO.Stores;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories.Stores
{
    public interface IStoreRepository : IRepository<Store>
    {
        Task<List<StoreDTO>> GetStoresByCountryId(int countryId);
    }
}
