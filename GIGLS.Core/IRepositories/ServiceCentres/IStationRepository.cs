using GIGL.POST.Core.Repositories;
using POST.Core.Domain;
using POST.Core.DTO.ServiceCentres;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Core.IRepositories.ServiceCentres
{
    public interface IStationRepository : IRepository<Station>
    {
        Task<List<Station>> GetStationsAsync();
        Task<List<StationDTO>> GetLocalStations(int[] countryIds);
        Task<List<StationDTO>> GetLocalStationsWithoutSuperServiceCentre(int[] countryIds);
        Task<List<StationDTO>> GetInternationalStations();
        Task<List<Station>> GetAllStationsAsync();
        Task<List<StationDTO>> GetActiveGIGGoStations();
        Task<List<StationDTO>> GetStationsByCountry(int countryId);
        Task<List<ServiceCentreDTO>> GetServiceCentresByStation(int stationId);
        Task<List<StationDTO>> GetStationsByUserCountry(int[] countryIds);
    }
}
