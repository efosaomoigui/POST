using System;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Stores;
using GIGLS.Core.IServices.Stores;

namespace GIGLS.Services.Implementation.Stores
{
    public class StoreService : IStoreService
    {
        public Task<object> AddStore(StoreDTO store)
        {
            throw new NotImplementedException();
        }

        public Task DeleteStore(int storeId)
        {
            throw new NotImplementedException();
        }

        public Task<StoreDTO> GetStoreById(int storeId)
        {
            throw new NotImplementedException();
        }

        public Task<StoreDTO> GetStores()
        {
            throw new NotImplementedException();
        }

        public Task UpdateStore(int storeId, StoreDTO store)
        {
            throw new NotImplementedException();
        }
    }
}
