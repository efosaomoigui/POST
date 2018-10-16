using GIGLS.Core.IServices;
using GIGLS.Core.DTO;
using GIGLS.Services.Implementation;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using GIGLS.WebApi.Filters;

namespace GIGLS.WebApi.Controllers.Messaging
{
    [Authorize(Roles = "Admin")]
    [RoutePrefix("api/notification")]
    public class NotificationController : BaseWebApiController
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService) : base(nameof(NotificationController))
        {
            _notificationService = notificationService;
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<IEnumerable<NotificationDTO>>> GetNotifications()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var notifications = await _notificationService.GetNotificationAsync();

                return new ServiceResponse<IEnumerable<NotificationDTO>>
                {
                    Object = notifications
                };
            });
        }


        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("")]
        public async Task<IServiceResponse<object>> AddNotification(NotificationDTO notificationDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var notification = await _notificationService.AddNotification(notificationDto);

                return new ServiceResponse<object>
                {
                    Object = notification
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{notificationId:int}")]
        public async Task<IServiceResponse<NotificationDTO>> GetNotification(int notificationId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var notification = await _notificationService.GetNotificationById(notificationId);

                return new ServiceResponse<NotificationDTO>
                {
                    Object = notification
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpDelete]
        [Route("{notificationId:int}")]
        public async Task<IServiceResponse<bool>> DeleteNotification(int notificationId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _notificationService.RemoveNotification(notificationId);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{notificationId:int}")]
        public async Task<IServiceResponse<bool>> UpdateNotification(int notificationId, NotificationDTO notificationDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _notificationService.UpdateNotification(notificationId, notificationDto);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

    }
}
