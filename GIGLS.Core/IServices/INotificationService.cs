using GIGLS.Core.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices
{
    public interface INotificationService : IServiceDependencyMarker
    {
        Task<IEnumerable<NotificationDTO>> GetNotificationAsync();
        Task<NotificationDTO> GetNotificationById(int notificationId);
        Task<object> AddNotification(NotificationDTO notificationDto);
        Task UpdateNotification(int notificationId, NotificationDTO notificationDto);
        Task RemoveNotification(int notificationId);

        Task<object> CreateNotification(NotificationDTO notificationDTO);
        Task<IEnumerable<NotificationDTO>> GetNotifications(bool? IsRead);
        Task UpdateNotificationAsRead(int notificationId);
    }
}
