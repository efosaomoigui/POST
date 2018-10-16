using GIGLS.Core;
using GIGLS.Core.DTO;
using GIGLS.Core.IServices;
using System.Threading.Tasks;
using GIGLS.Infrastructure;
using AutoMapper;
using GIGLS.Core.Domain;
using System.Collections.Generic;

namespace GIGLS.Services.Implementation.Messaging
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _uow;

        public NotificationService(IUnitOfWork uow)
        {
            _uow = uow;
            MapperConfig.Initialize();
        }


        public async Task<IEnumerable<NotificationDTO>> GetNotificationAsync()
        {
            var notification = await _uow.Notification.GetNotificationAsync();
            var notificationDto = Mapper.Map< IEnumerable<NotificationDTO>>(notification);
            return notificationDto;
        }

        public async Task<object> AddNotification(NotificationDTO notificationDto)
        {
            var notification = Mapper.Map<Notification>(notificationDto);
            _uow.Notification.Add(notification);
            await _uow.CompleteAsync();
            return new { id = notification.NotificationId };
        }

        public async Task<NotificationDTO> GetNotificationById(int notificationId)
        {
            var notification = await _uow.Notification.GetAsync(notificationId);

            if (notification == null)
            {
                throw new GenericException("NOTIFICATION INFORMATION DOES NOT EXIST");
            }
            return Mapper.Map<NotificationDTO>(notification);
        }


        public async Task RemoveNotification(int notificationId)
        {
            var notification = await _uow.Notification.GetAsync(notificationId);

            if (notification == null)
            {
                throw new GenericException("NOTIFICATION INFORMATION DOES NOT EXIST");
            }
            _uow.Notification.Remove(notification);
            await _uow.CompleteAsync();
        }

        public async Task UpdateNotification(int notificationId, NotificationDTO notificationDto)
        {
            var notification = await _uow.Notification.GetAsync(notificationId);

            if (notification == null)
            {
                throw new GenericException("NOTIFICATION INFORMATION DOES NOT EXIST");
            }

            notification.Subject = notificationDto.Subject;
            notification.Message = notificationDto.Message;
            await _uow.CompleteAsync();
        }
    }
}
