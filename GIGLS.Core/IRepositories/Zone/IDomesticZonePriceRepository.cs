using GIGL.POST.Core.Domain;
using GIGL.POST.Core.Repositories;
using POST.Core.DTO.Zone;
using POST.Core.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Core.IRepositories.Zone
{
    public interface IDomesticZonePriceRepository : IRepository<DomesticZonePrice>
    {
        Task<List<DomesticZonePriceDTO>> GetDomesticZones();
        Task<DomesticZonePrice> GetDomesticZonePrice(int zoneId, decimal weight, RegularEcommerceType regularEcommerceType, int countryId);
    }
}
