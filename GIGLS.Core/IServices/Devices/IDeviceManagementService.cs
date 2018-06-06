using GIGLS.Core.DTO.Devices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Devices
{
    public interface IDeviceManagementService : IServiceDependencyMarker
    {
        Task<List<DeviceManagementDTO>> GetDeviceManagements();
        Task<List<DeviceManagementDTO>> GetActiveDeviceManagements();
        Task<DeviceManagementDTO> GetDeviceManagementById(int deviceManagementId);
        Task AssignDeviceToUser(string userId, int deviceId);
        Task UnAssignDeviceFromUser(int deviceManagementId);
    }
}
