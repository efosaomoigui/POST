using System;
using System.Threading.Tasks;
using GIGLS.Core.DTO.PaymentTransactions;
using GIGLS.Core.DTO.Stores;
using GIGLS.Core.IServices;
using GIGLS.Core.IServices.Stores;

namespace GIGLS.Services.Business.Pricing
{
    public class StoreService : IPriceCategoryService
    {
        public Task<object> AddPriceCategory(PriceCategoryDTO priceCategory)
        {
            throw new NotImplementedException();
        }

        public Task DeletePriceCategory(int priceCategoryId)
        {
            throw new NotImplementedException();
        }

        public Task<PriceCategoryDTO> GetPriceCategoryById(int priceCategoryId)
        {
            throw new NotImplementedException();
        }

        public Task<PriceCategoryDTO> GetPriceCategorys()
        {
            throw new NotImplementedException();
        }

        public Task UpdatePriceCategory(int priceCategoryId, PriceCategoryDTO priceCategory)
        {
            throw new NotImplementedException();
        }
    }
}
