using GIGLS.Core.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices
{
    public interface ICategoryService : IServiceDependencyMarker
    {
        Task<List<CategoryDTO>> GetCategories();
        Task<CategoryDTO> GetCategoryById(int categoryId);
        Task<object> AddCategory(CategoryDTO categoryDto);
        Task UpdateCategory(int categoryId, CategoryDTO categoryDto);
        Task DeleteCategory(int categoryId);
    }
}
