using POST.Core.DTO.Devices;
using POST.Core.IServices;
using POST.Core.IServices.Devices;
using POST.Services.Implementation;
using POST.WebApi.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace POST.WebApi.Controllers.Devices
{
    [Authorize(Roles = "Admin, ViewAdmin")]
    [RoutePrefix("api/device")]
    public class DeviceController : BaseWebApiController
    {
        private readonly IDeviceService _deviceService;

        public DeviceController(IDeviceService deviceService) :base(nameof(DeviceController))
        {
            _deviceService = deviceService;
        }
        
        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<IEnumerable<DeviceDTO>>> GetDevices()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var devices = await _deviceService.GetDevices();
                return new ServiceResponse<IEnumerable<DeviceDTO>>
                {
                    Object = devices
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("")]
        public async Task<IServiceResponse<object>> AddDevice(DeviceDTO deviceDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var Device = await _deviceService.AddDevice(deviceDTO);

                return new ServiceResponse<object>
                {
                    Object = Device
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{deviceId:int}")]
        public async Task<IServiceResponse<DeviceDTO>> GetDevice(int deviceId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var Device = await _deviceService.GetDeviceById(deviceId);

                return new ServiceResponse<DeviceDTO>
                {
                    Object = Device
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpDelete]
        [Route("{deviceId:int}")]
        public async Task<IServiceResponse<bool>> DeleteDevice(int deviceId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _deviceService.RemoveDevice(deviceId);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{deviceId:int}")]
        public async Task<IServiceResponse<bool>> UpdateDevice(int deviceId, DeviceDTO deviceDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _deviceService.UpdateDevice(deviceId, deviceDTO);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{deviceId:int}/status/{status}")]
        public async Task<IServiceResponse<bool>> UpdateDeliveryOption(int deviceId, bool status)
        {
            return await HandleApiOperationAsync(async () => {
                await _deviceService.UpdateDeviceStatus(deviceId, status);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }
    }
}
