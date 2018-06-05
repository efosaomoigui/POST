using GIGLS.Core.DTO.Devices;
using GIGLS.Core.IServices;
using GIGLS.Core.IServices.Devices;
using GIGLS.Services.Implementation;
using GIGLS.WebApi.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace GIGLS.WebApi.Controllers.Devices
{
    [Authorize(Roles = "Admin, ViewAdmin")]
    [RoutePrefix("api/devicemanagement")]
    public class DeviceManagementController : BaseWebApiController
    {
        private readonly IDeviceManagementService _deviceService;

        public DeviceManagementController(IDeviceManagementService deviceService) :base(nameof(DeviceManagementController))
        {
            _deviceService = deviceService;
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<IEnumerable<DeviceManagementDTO>>> GetActiveDeviceManagements()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var deviceManagement = await _deviceService.GetActiveDeviceManagements();

                return new ServiceResponse<IEnumerable<DeviceManagementDTO>>
                {
                    Object = deviceManagement
                };
            });
        }
        
        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("all")]
        public async Task<IServiceResponse<IEnumerable<DeviceManagementDTO>>> GetDeviceManagements()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var deviceManagement = await _deviceService.GetDeviceManagements();

                return new ServiceResponse<IEnumerable<DeviceManagementDTO>>
                {
                    Object = deviceManagement
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("{deviceId:int}/assign/{userId}")]
        public async Task<IServiceResponse<object>> AssignDeviceToUser(string userId, int deviceId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _deviceService.AssignDeviceToUser(userId, deviceId);

                return new ServiceResponse<object>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPost]
        [Route("{deviceManagementId:int}/unassign")]
        public async Task<IServiceResponse<object>> UnAssignDeviceFromUser(int deviceManagementId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _deviceService.UnAssignDeviceFromUser(deviceManagementId);

                return new ServiceResponse<object>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{deviceManagementId:int}")]
        public async Task<IServiceResponse<DeviceManagementDTO>> GetDeviceManagement(int deviceManagementId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var DeviceManagement = await _deviceService.GetDeviceManagementById(deviceManagementId);

                return new ServiceResponse<DeviceManagementDTO>
                {
                    Object = DeviceManagement
                };
            });
        }
                
    }
}
