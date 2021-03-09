using GIGLS.Core.DTO.Zone;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Zone
{
    public interface IDomesticRouteZoneMapService : IServiceDependencyMarker
    {
        Task<IEnumerable<DomesticRouteZoneMapDTO>> GetRouteZoneMaps();
        Task<DomesticRouteZoneMapDTO> GetRouteZoneMapById(int routeZoneMapId);
        Task<object> AddRouteZoneMap(DomesticRouteZoneMapDTO routeZoneMap);
        Task UpdateRouteZoneMap(int routeZoneMapId, DomesticRouteZoneMapDTO routeZoneMap);
        Task UpdateStatusRouteZoneMap(int routeZoneMapId, bool status);
        Task DeleteRouteZoneMap(int routeZoneMapId); 
        Task<DomesticRouteZoneMapDTO> GetZone(int departure, int destination);
        Task<DomesticRouteZoneMapDTO> GetZoneMobile(int departure, int destination);
        Task<int> GetZoneETA(int departure, int destination);
    }
}
