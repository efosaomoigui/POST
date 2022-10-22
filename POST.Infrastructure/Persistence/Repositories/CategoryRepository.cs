using POST.Core.Domain;
using POST.Core.IRepositories;
using POST.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POST.Infrastructure.Persistence.Repositories
{
    public class CategoryRepository : Repository<Category, GIGLSContext>, ICategoryRepository
    {
        public CategoryRepository(GIGLSContext context) : base(context)
        {
        }

    }
}
