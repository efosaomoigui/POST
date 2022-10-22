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
    public class MobileRatingRepository : Repository<MobileRating, GIGLSContext>, IMobileRatingRepository
    {
        private GIGLSContext _context;
        public MobileRatingRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }
    }
}
