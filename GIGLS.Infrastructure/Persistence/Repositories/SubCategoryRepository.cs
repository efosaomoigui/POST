using GIGLS.Core.Domain;
using GIGLS.Core.DTO;
using GIGLS.Core.IRepositories;
using GIGLS.Infrastructure.Persistence.Repository;
using System.Collections.Generic;
using System.Linq;
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
            //Exclude those package from showing on gig go
            int[] excludePackage = new int[] { 12, 14, 15, 16, 17, 18, 19, 34, 38, 39, 40, 41, 42, 43 };

            var subcategories = Context.SubCategory.AsQueryable().Where(x => !excludePackage.Contains(x.SubCategoryId));

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
                                                }).OrderBy(x => x.SubCategoryName).ToList();
            return Task.FromResult(categoryDto);
        }
    }
}