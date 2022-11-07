using GIGL.POST.Core.Repositories;
using POST.Core.Domain;
using POST.Core.DTO;
using POST.Core.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Core.IRepositories
{
    public interface IMessageRepository : IRepository<Message>
    {
        Task<IEnumerable<MessageDTO>> GetMessageAsync(EmailSmsType type);
    }
}
