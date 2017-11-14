using GIGLS.Core.DTO.Zone;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Zone
{
    public interface IZoneService : IServiceDependencyMarker
    {
        Task<IEnumerable<ZoneDTO>> GetZones();
        Task<IEnumerable<ZoneDTO>> GetActiveZones();
        Task<ZoneDTO> GetZoneById(int zoneId);
        Task<object> AddZone(ZoneDTO Zone);
        Task UpdateZone(int zoneId, ZoneDTO zone);
        Task UpdateZone(int zoneId, bool status);
        Task DeleteZone(int zoneId);
    }
}
