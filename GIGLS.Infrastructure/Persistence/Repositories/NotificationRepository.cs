using AutoMapper;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO;
using GIGLS.Core.IRepositories;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories
{
    public class NotificationRepository : Repository<Notification, GIGLSContext>, INotificationRepository
    {
        public NotificationRepository(GIGLSContext context) : base(context)
        {
        }

        public Task<IEnumerable<NotificationDTO>> GetNotificationAsync()
        {
            try
            {
                var Notification = Context.Notification.ToList();

                var NotificationDto = Mapper.Map<IEnumerable<NotificationDTO>>(Notification);
                return Task.FromResult(NotificationDto);
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
