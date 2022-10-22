using GIGL.POST.Core.Repositories;
using POST.Core.Domain;
using POST.Core.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Core.IRepositories
{
    public interface INotificationRepository : IRepository<Notification>
    {
        Task<IEnumerable<NotificationDTO>> GetNotificationAsync();
    }
}
