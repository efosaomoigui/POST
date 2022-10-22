using GIGL.POST.Core.Repositories;
using POST.Core.DTO.Zone;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGL.POST.Core.Domain;

namespace POST.Core.IRepositories.Zone
{
    public interface ISpecialDomesticZonePriceRepository : IRepository<SpecialDomesticZonePrice>
    {
        Task<List<SpecialDomesticZonePriceDTO>> GetSpecialDomesticZonesPrice();
    }
}
