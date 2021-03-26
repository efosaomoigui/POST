using GIGLS.Core.DTO.Zone;
using GIGLS.Core.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Zone
{
    public interface ICountryRouteZoneMapService : IServiceDependencyMarker
    {
        Task<IEnumerable<CountryRouteZoneMapDTO>> GetCountryRouteZoneMaps();
        Task<CountryRouteZoneMapDTO> GetCountryRouteZoneMapById(int routeZoneMapId);
        Task<object> AddCountryRouteZoneMap(CountryRouteZoneMapDTO routeZoneMap);
        Task UpdateCountryRouteZoneMap(int routeZoneMapId, CountryRouteZoneMapDTO routeZoneMap);
        Task UpdateStatusCountryRouteZoneMap(int routeZoneMapId, bool status);
        Task DeleteCountryRouteZoneMap(int routeZoneMapId); 
        //Task<CountryRouteZoneMapDTO> GetZone(int departure, int destination);
        Task<CountryRouteZoneMapDTO> GetZone(int departure, int destination, CompanyMap companyMap = CompanyMap.GIG);

    }
}
