using POST.Core.DTO.User;
using POST.Core.DTO.Zone;
using POST.Core.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Core.IServices.Zone
{
    public interface ICountryRouteZoneMapService : IServiceDependencyMarker
    {
        Task<IEnumerable<CountryRouteZoneMapDTO>> GetCountryRouteZoneMaps();
        Task<CountryRouteZoneMapDTO> GetCountryRouteZoneMapById(int routeZoneMapId);
        Task<object> AddCountryRouteZoneMap(CountryRouteZoneMapDTO routeZoneMap);
        Task UpdateCountryRouteZoneMap(int routeZoneMapId, CountryRouteZoneMapDTO routeZoneMap);
        Task UpdateStatusCountryRouteZoneMap(int routeZoneMapId, bool status);
        Task DeleteCountryRouteZoneMap(int routeZoneMapId); 
        Task<CountryRouteZoneMapDTO> GetZone(int departure, int destination, CompanyMap companyMap = CompanyMap.GIG);
        Task<CountryRouteZoneMapDTO> GetBasicZone(int departure, int destination, CompanyMap companyMap);

        Task<CountryRouteZoneMapDTO> GetZone(int departure, int destination);
    }
}
