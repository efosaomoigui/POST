using AutoMapper;
using POST.Core.Domain;
using POST.Core.DTO;
using POST.Core.IRepositories;
using POST.Infrastructure.Persistence;
using POST.Infrastructure.Persistence.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace POST.INFRASTRUCTURE.Persistence.Repositories
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
