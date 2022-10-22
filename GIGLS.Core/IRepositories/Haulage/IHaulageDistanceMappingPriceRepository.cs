using GIGL.POST.Core.Repositories;
using POST.Core.Domain;
using POST.Core.DTO.Haulage;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Core.IRepositories
{
    public interface IHaulageDistanceMappingPriceRepository : IRepository<HaulageDistanceMappingPrice>
    {
        Task<List<HaulageDistanceMappingPriceDTO>> GetHaulageDistanceMappingPricesAsync();
    }
}
