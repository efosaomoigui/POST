using AutoMapper;
using POST.Core.Domain;
using POST.Core.DTO.Haulage;
using POST.Core.IRepositories;
using POST.Infrastructure.Persistence;
using POST.Infrastructure.Persistence.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POST.INFRASTRUCTURE.Persistence.Repositories
{
    public class HaulageRepository : Repository<Haulage, GIGLSContext>, IHaulageRepository
    {
        public HaulageRepository(GIGLSContext context) : base(context)
        {
        }

        public Task<IEnumerable<HaulageDTO>> GetHaulagesAsync()
        {
            var haulages = Context.Haulage.ToList();
            var haulageDto = Mapper.Map<IEnumerable<HaulageDTO>>(haulages);
            return Task.FromResult(haulageDto);
        }
        
    }
}
