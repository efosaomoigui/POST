using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.Domain.Devices;
using GIGLS.Core.DTO.Devices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories.Devices
{
    public interface IDeviceManagementRepository : IRepository<DeviceManagement>
    {
        Task<List<DeviceManagementDTO>> GetActiveDeviceManagementAsync();
        Task<List<DeviceManagementDTO>> GetDeviceManagementAsync();
        Task<DeviceManagementDTO> GetDeviceManagementById(int deviceManagementId);
    }
}
