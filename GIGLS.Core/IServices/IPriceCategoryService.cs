using GIGLS.Core.DTO.PaymentTransactions;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices
{
    public interface IPriceCategoryService
    {
        Task<PriceCategoryDTO> GetPriceCategorys();
        Task<PriceCategoryDTO> GetPriceCategoryById(int priceCategoryId);
        Task<object> AddPriceCategory(PriceCategoryDTO priceCategory);
        Task UpdatePriceCategory(int priceCategoryId, PriceCategoryDTO priceCategory);
        Task DeletePriceCategory(int priceCategoryId);
    }
}
