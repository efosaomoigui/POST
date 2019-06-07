using GIGL.GIGLS.Core.Domain;
using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.DTO.ServiceCentres;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories.ServiceCentres
{
    public interface IServiceCentreRepository : IRepository<ServiceCentre>
    {
        Task<List<ServiceCentreDTO>> GetServiceCentres();
        Task<List<ServiceCentreDTO>> GetLocalServiceCentres();
        Task<List<ServiceCentreDTO>> GetInternationalServiceCentres();
        Task<ServiceCentreDTO> GetServiceCentresByIdForInternational(int serviceCentreId);
        Task<List<ServiceCentreDTO>> GetServiceCentresByStationId(int stationId);

        Task<List<ServiceCentreDTO>> GetLocalServiceCentres(int[] countryIds);
    }
}
