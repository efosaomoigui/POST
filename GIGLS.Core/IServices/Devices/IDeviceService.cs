using GIGLS.Core.DTO.Devices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Devices
{
    public interface IDeviceService : IServiceDependencyMarker
    {
        Task<IEnumerable<DeviceDTO>> GetDevices();
        Task<DeviceDTO> GetDeviceById(int deviceId);
        Task<object> AddDevice(DeviceDTO deviceDto);
        Task UpdateDevice(int deviceId, DeviceDTO deviceDto);
        Task UpdateDeviceStatus(int deviceId, bool status);
        Task RemoveDevice(int deviceId);
    }
}
