using GIGLS.Core.Domain;
using GIGLS.Core.IRepositories.Haulage;
using GIGLS.Infrastructure.Persistence.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Haulage;
using AutoMapper;

namespace GIGLS.Infrastructure.Persistence.Repositories.Haulage
{
    public class PackingListRepository : Repository<PackingList, GIGLSContext>, IPackingListRepository
    {
        public PackingListRepository(GIGLSContext context) : base(context)
        {
        }

        public Task<IEnumerable<PackingListDTO>> GetPackingListAsync()
        {
            var packingLists = Context.PackingList.ToList();
            var packingListDto = Mapper.Map<IEnumerable<PackingListDTO>>(packingLists);
            return Task.FromResult(packingListDto);
        }
    }
}
