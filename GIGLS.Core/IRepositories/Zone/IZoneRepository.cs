using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.DTO.Zone;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories.Zone
{
    public interface IZoneRepository : IRepository<Domain.Zone>
    {
        Task<IEnumerable<ZoneDTO>> GetZonesAsync();
        Task<IEnumerable<ZoneDTO>> GetActiveZonesAsync();
    }
}
