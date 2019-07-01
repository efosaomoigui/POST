using GIGLS.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices
{
    public interface ISubCategoryService: IServiceDependencyMarker
    {
        Task<List<SubCategoryDTO>> GetSubCategories();
        Task<object> AddSubCategory(SubCategoryDTO subcategory);
        Task UpdateSubCategory(int subcategoryId, SubCategoryDTO subcategory);
        Task RemoveSubCategory(int subcategoryId);
    }
}
