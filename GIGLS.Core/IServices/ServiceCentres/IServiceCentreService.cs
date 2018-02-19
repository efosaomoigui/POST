using GIGLS.Core.DTO.ServiceCentres;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.ServiceCentres
{
    public interface IServiceCentreService : IServiceDependencyMarker
    {
        Task<IEnumerable<ServiceCentreDTO>> GetServiceCentres();
        Task<ServiceCentreDTO> GetServiceCentreById(int serviceCentreId);
        Task<ServiceCentreDTO> GetServiceCentreByCode(string serviceCentreCode);
        Task<object> AddServiceCentre(ServiceCentreDTO service);
        Task UpdateServiceCentre(int serviceCentreId, ServiceCentreDTO service);
        Task DeleteServiceCentre(int serviceCentreId);
        Task ServiceCentreStatus(int serviceCentreId, bool status);
        Task<List<ServiceCentreDTO>> GetLocalServiceCentres();
        Task<List<ServiceCentreDTO>> GetInternationalServiceCentres();
    }
}
