using GIGL.POST.Core.Repositories;
using POST.Core.Domain;
using POST.Core.DTO.Zone;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Core.IRepositories.Zone
{
    public interface IWeightLimitPriceRepository : IRepository<WeightLimitPrice>
    {
        Task<List<WeightLimitPriceDTO>> GetWeightLimitPrices();
    }
}
