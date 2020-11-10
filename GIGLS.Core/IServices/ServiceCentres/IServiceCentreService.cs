using GIGLS.Core.DTO.ServiceCentres;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.ServiceCentres
{
    public interface IServiceCentreService : IServiceDependencyMarker
    {
        Task<IEnumerable<ServiceCentreDTO>> GetServiceCentres();
        Task<ServiceCentreDTO> GetServiceCentreById(int serviceCentreId);
        Task<ServiceCentreDTO> GetServiceCentreForHomeDelivery(int serviceCentreId);
        Task<ServiceCentreDTO> GetServiceCentreByCode(string serviceCentreCode);
        Task<List<ServiceCentreDTO>> GetServiceCentreByCode(string[] code);
        Task<object> AddServiceCentre(ServiceCentreDTO service);
        Task UpdateServiceCentre(int serviceCentreId, ServiceCentreDTO service);
        Task DeleteServiceCentre(int serviceCentreId);
        Task ServiceCentreStatus(int serviceCentreId, bool status);
        Task<List<ServiceCentreDTO>> GetLocalServiceCentres();
        Task<List<ServiceCentreDTO>> GetInternationalServiceCentres();
        Task<ServiceCentreDTO> GetServiceCentreByIdForInternational(int serviceCentreId);
        Task<ServiceCentreDTO> GetDefaultServiceCentre();
        Task<IEnumerable<ServiceCentreDTO>> GetServiceCentresByStationId(int stationId);
        Task<List<ServiceCentreDTO>> GetLocalServiceCentres(int[] countryIds);
        Task<List<ServiceCentreDTO>> GetServiceCentresByCountryId(int countryId);
        Task<ServiceCentreDTO> GetGIGGOServiceCentre();
        Task<List<ServiceCentreDTO>> GetHUBServiceCentres();
        Task<List<ServiceCentreDTO>> GetServiceCentresWithoutHUBForNonLagosStation(int usersServiceCentresId, int countryId);
        Task<List<ServiceCentreDTO>> GetServiceCentresBySingleCountry(int countryId);
        Task ServiceCentreViewState(int serviceCentreId, bool ispublic);
    }
}
