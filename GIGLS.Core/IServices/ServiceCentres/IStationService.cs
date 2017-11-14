using GIGLS.Core.DTO.ServiceCentres;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.ServiceCentres
{
    public interface IStationService : IServiceDependencyMarker
    {
        Task<IEnumerable<StationDTO>> GetStations();
        Task<StationDTO> GetStationById(int stationId);
        Task<object> AddStation(StationDTO station);
        Task UpdateStation(int stationId, StationDTO station);
        Task DeleteStation(int stationId);
    }
}
