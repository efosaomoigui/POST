using GIGL.POST.Core.Repositories;
using POST.Core.DTO.Haulage;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Core.IRepositories
{
    public interface IHaulageRepository : IRepository<Core.Domain.Haulage>
    {
        Task<IEnumerable<HaulageDTO>> GetHaulagesAsync();
        
    }
}
