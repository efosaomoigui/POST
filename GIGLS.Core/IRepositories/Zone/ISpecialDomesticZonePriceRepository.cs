using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.DTO.Zone;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGL.GIGLS.Core.Domain;

namespace GIGLS.Core.IRepositories.Zone
{
    public interface ISpecialDomesticZonePriceRepository : IRepository<SpecialDomesticZonePrice>
    {
        Task<List<SpecialDomesticZonePriceDTO>> GetSpecialDomesticZonesPrice();
    }
}
