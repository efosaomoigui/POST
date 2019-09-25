using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO.ServiceCentres;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories.ServiceCentres
{
    public interface IStationRepository : IRepository<Station>
    {
        Task<List<Station>> GetStationsAsync();
        Task<List<StationDTO>> GetLocalStations(int[] countryIds);
        Task<List<StationDTO>> GetInternationalStations();
        Task<List<Station>> GetAllStationsAsync();
    }
}
