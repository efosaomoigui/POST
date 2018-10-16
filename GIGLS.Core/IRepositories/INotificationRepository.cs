using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories
{
    public interface INotificationRepository : IRepository<Notification>
    {
        Task<IEnumerable<NotificationDTO>> GetNotificationAsync();
    }
}
