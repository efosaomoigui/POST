using GIGL.POST.Core.Domain;
using GIGL.POST.Core.Repositories;
using POST.Core.Domain;
using POST.Core.DTO.PaymentTransactions;
using POST.Core.DTO.Stores;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Core.IRepositories
{
    public interface IPriceCategoryRepository : IRepository<PriceCategory>
    {
        Task<List<PriceCategoryDTO>> GetPriceCategoriesByCountryId(int countryId);
        Task<PriceCategoryDTO> GetPriceCategoryById(int priceCategoryId);
        Task<List<PriceCategoryDTO>> GetPriceCategories();
        Task<List<PriceCategoryDTO>> GetPriceCategoriesByCountryId(int destCountryId, int deptCountryID);
    }
}
