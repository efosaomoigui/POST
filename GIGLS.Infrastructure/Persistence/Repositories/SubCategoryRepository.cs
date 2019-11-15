using GIGLS.Core.Domain;
using GIGLS.Core.DTO;
using GIGLS.Core.IRepositories;
using GIGLS.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Infrastructure.Persistence.Repositories
{
    public class SubCategoryRepository : Repository<SubCategory, GIGLSContext>, ISubCategoryRepository
    {
        public SubCategoryRepository(GIGLSContext context) : base(context)
        {
        }


        public Task<List<SubCategoryDTO>> GetSubCategories()
        {
            var subcategories = Context.SubCategory.AsQueryable();

            List<SubCategoryDTO> categoryDto = (from s in subcategories
                                                select new SubCategoryDTO
                                                {
                                                 SubCategoryName = s.SubCategoryName,
                                                 Category = Context.Category.Where(c => c.CategoryId == s.Category.CategoryId).Select(x => new CategoryDTO
                                                 {
                                                     CategoryName = x.CategoryName,
                                                     CategoryId = x.CategoryId
                                                 }).FirstOrDefault(),
                                                 SubCategoryId = s.SubCategoryId,
                                                 Weight = s.Weight,
                                                 WeightRange = s.WeightRange
                                                }).ToList();

            return Task.FromResult(categoryDto);
        }
    }
}

