using POST.Core.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Core.IServices
{
    public interface ISubCategoryService: IServiceDependencyMarker
    {
        Task<List<SubCategoryDTO>> GetSubCategories();
        Task<object> AddSubCategory(SubCategoryDTO subcategory);
        Task UpdateSubCategory(int subcategoryId, SubCategoryDTO subcategory);
        Task RemoveSubCategory(int subcategoryId);
    }
}
