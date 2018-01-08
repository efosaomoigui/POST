using GIGL.GIGLS.Core.Domain;
using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.DTO.Zone;
using GIGLS.Core.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories.Zone
{
    public interface IDomesticZonePriceRepository : IRepository<DomesticZonePrice>
    {
        Task<List<DomesticZonePriceDTO>> GetDomesticZones();
        Task<DomesticZonePrice> GetDomesticZonePrice(int zoneId, decimal weight, RegularEcommerceType regularEcommerceType);
    }
}
