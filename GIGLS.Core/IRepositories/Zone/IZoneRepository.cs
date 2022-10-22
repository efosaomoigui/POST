using GIGL.POST.Core.Repositories;
using POST.Core.DTO.Zone;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Core.IRepositories.Zone
{
    public interface IZoneRepository : IRepository<Domain.Zone>
    {
        Task<IEnumerable<ZoneDTO>> GetZonesAsync();
        Task<IEnumerable<ZoneDTO>> GetActiveZonesAsync();
    }
}
