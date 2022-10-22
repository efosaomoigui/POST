using GIGL.POST.Core.Repositories;
using POST.Core.Domain.Devices;
using POST.Core.DTO.Devices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Core.IRepositories.Devices
{
    public interface IDeviceManagementRepository : IRepository<DeviceManagement>
    {
        Task<List<DeviceManagementDTO>> GetActiveDeviceManagementAsync();
        Task<List<DeviceManagementDTO>> GetDeviceManagementAsync();
        Task<DeviceManagementDTO> GetDeviceManagementById(int deviceManagementId);
    }
}
