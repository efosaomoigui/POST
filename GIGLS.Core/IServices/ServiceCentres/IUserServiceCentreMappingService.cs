using POST.Core.DTO.ServiceCentres;
using POST.Core.DTO.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Core.IServices.ServiceCentres
{
    public interface IUserServiceCentreMappingService : IServiceDependencyMarker
    {
        Task MappingUserToServiceCentre(string userId, int serviceCentreId);
        Task<List<UserDTO>> GetUsersInServiceCentre();
        Task<List<UserDTO>> GetActiveUsersMapToServiceCentre(int serviceCentreId);
        Task<List<UserDTO>> GetUsersMapToServiceCentre(int serviceCentreId);
        Task<List<ServiceCentreDTO>> GetUserServiceCentres(string Id);
        Task<ServiceCentreDTO> GetUserActiveServiceCentre(string Id);
    }
}
