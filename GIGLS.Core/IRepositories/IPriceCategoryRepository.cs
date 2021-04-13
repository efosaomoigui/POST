using GIGL.GIGLS.Core.Domain;
using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO.PaymentTransactions;
using GIGLS.Core.DTO.Stores;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories
{
    public interface IPriceCategoryRepository : IRepository<PriceCategory>
    {
        Task<List<PriceCategoryDTO>> GetPriceCategoriesByCountryId(int countryId);
        Task<PriceCategoryDTO> GetPriceCategoryById(int priceCategoryId);
        Task<List<PriceCategoryDTO>> GetPriceCategories();
    }
}
