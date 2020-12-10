using GIGLS.Core;
using GIGLS.Core.DTO;
using GIGLS.Core.IServices;
using System.Threading.Tasks;
using GIGLS.Infrastructure;
using AutoMapper;
using GIGLS.Core.Domain;
using System.Collections.Generic;
using System;
using System.Net;
using System.Linq;
using GIGLS.Core.IServices.User;

namespace GIGLS.Services.Implementation.Messaging
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserService _userService;

        public NotificationService(IUnitOfWork uow, IUserService userService)
        {
            _uow = uow;
            _userService = userService;
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

        //App
        public async Task<object> CreateNotification(NotificationDTO notificationDTO)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(notificationDTO.UserId))
                {
                    throw new GenericException("User Id can not be empty", $"{(int)HttpStatusCode.BadRequest}");
                }

                if (string.IsNullOrWhiteSpace(notificationDTO.Subject))
                {
                    throw new GenericException("Message Subject can not be empty", $"{(int)HttpStatusCode.BadRequest}");
                }

                if (string.IsNullOrWhiteSpace(notificationDTO.Message))
                {
                    throw new GenericException("Message Body can not be empty", $"{(int)HttpStatusCode.BadRequest}");
                }

                var user = await _uow.User.GetUserById(notificationDTO.UserId);
                if(user == null)
                {
                    throw new GenericException("User not found", $"{(int)HttpStatusCode.NotFound}");
                }

                var notification = Mapper.Map<Notification>(notificationDTO);
                notification.IsRead = false;
                _uow.Notification.Add(notification);
                await _uow.CompleteAsync();

                return new { id = notification.NotificationId };
            }
            catch(Exception ex)
            {
                throw;
            }
        }


        public async Task<IEnumerable<NotificationDTO>> GetNotifications(bool? IsRead)
        {
            var userId = await _userService.GetCurrentUserId();

            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new GenericException("User Id can not be empty", $"{(int)HttpStatusCode.BadRequest}");
            }

            var notifications = _uow.Notification.GetAllAsQueryable().Where(x => x.UserId == userId);
            if (IsRead == true)
            {
                notifications = notifications.Where(x => x.IsRead == true);
            }
            else if (IsRead == false)
            {
                notifications = notifications.Where(x => x.IsRead == false);
            }
           
            var notificationDto = Mapper.Map<IEnumerable<NotificationDTO>>(notifications);
            return notificationDto;
        }

        public async Task UpdateNotificationAsRead(int notificationId)
        {
            var notification = await _uow.Notification.GetAsync(notificationId);

            if (notification == null)
            {
                throw new GenericException("Notification does not exist", $"{(int)HttpStatusCode.NotFound}");
            }

            var userId = await _userService.GetCurrentUserId();
            if(notification.UserId == userId)
            {
                notification.IsRead = true;
            }
            else
            {
                throw new GenericException("Notification not for this user", $"{(int)HttpStatusCode.BadRequest}");
            }
            await _uow.CompleteAsync();
        }
    }
}
