using GIGL.GIGLS.Core.Domain;
using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.DTO.Dashboard;
using GIGLS.Core.DTO.ServiceCentres;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories.ServiceCentres
{
    public interface IServiceCentreRepository : IRepository<ServiceCentre>
    {
        Task<List<ServiceCentreDTO>> GetServiceCentres();
        Task<List<ServiceCentreDTO>> GetServiceCentresWithoutStation();
        Task<List<ServiceCentreDTO>> GetLocalServiceCentres();
        Task<List<ServiceCentreDTO>> GetInternationalServiceCentres();
        Task<ServiceCentreDTO> GetServiceCentresByIdForInternational(int serviceCentreId);
        Task<List<ServiceCentreDTO>> GetServiceCentresByStationId(int stationId);

        Task<List<ServiceCentreDTO>> GetLocalServiceCentres(int[] countryIds);
        Task<List<ServiceCentreDTO>> GetServiceCentreByCode(string[] code);
        Task<List<ServiceCentreDTO>> GetServiceCentres(int[] countryIds, bool excludeHub, int stationId = 0);
        Task<List<ServiceCentreDTO>> GetServiceCentresBySingleCountry(int countryId);
        Task<List<ServiceCentreDTO>> GetActiveServiceCentres();
        Task<ServiceCentreBreakdownDTO> GetServiceCentresData(int countryId);
        Task<List<ServiceCentreDTO>> GetActiveServiceCentresBySingleCountry(int countryId, int stationId = 0);
    }
}
