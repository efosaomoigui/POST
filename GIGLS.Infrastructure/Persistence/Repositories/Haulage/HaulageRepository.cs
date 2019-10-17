using AutoMapper;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO.Haulage;
using GIGLS.Core.IRepositories;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories
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
        public Task<IEnumerable<Haulagedto>> GetHaulages()
        {
            var haulages = Context.Haulage.ToList();
            var haulageDto = Mapper.Map<IEnumerable<Haulagedto>>(haulages);
            return Task.FromResult(haulageDto);
        }
    }
}
