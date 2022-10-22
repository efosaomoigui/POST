using POST.Core.Domain;
using POST.Core.DTO;
using POST.Core.IRepositories;
using POST.Core.IRepositories.User;
using POST.Infrastructure.Persistence;
using POST.Infrastructure.Persistence.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POST.INFRASTRUCTURE.Persistence.Repositories
{
    public class RankHistoryRepository : Repository<RankHistory, GIGLSContext>, IRankHistoryRepository
    {
        public RankHistoryRepository(GIGLSContext context) : base(context)
        {
        }

    }
}
