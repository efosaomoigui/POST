using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.DTO.Haulage;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories
{
    public interface IHaulageRepository : IRepository<Core.Domain.Haulage>
    {
        Task<IEnumerable<HaulageDTO>> GetHaulagesAsync();
        
    }
}
