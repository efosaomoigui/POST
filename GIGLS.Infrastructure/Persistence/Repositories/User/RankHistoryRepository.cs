using GIGLS.Core.Domain;
using GIGLS.Core.DTO;
using GIGLS.Core.IRepositories;
using GIGLS.Core.IRepositories.User;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories
{
    public class RankHistoryRepository : Repository<RankHistory, GIGLSContext>, IRankHistoryRepository
    {
        public RankHistoryRepository(GIGLSContext context) : base(context)
        {
        }

    }
}
