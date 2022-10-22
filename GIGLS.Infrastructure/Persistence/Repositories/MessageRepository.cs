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
using POST.Core.Enums;

namespace POST.INFRASTRUCTURE.Persistence.Repositories
{
    public class MessageRepository : Repository<Message, GIGLSContext>, IMessageRepository
    {
        public MessageRepository(GIGLSContext context) : base(context)
        {
        }
        
        public Task<IEnumerable<MessageDTO>> GetMessageAsync(EmailSmsType type)
        {
            try
            {
                var Message = Context.Message.Where(x => x.EmailSmsType == type).ToList();

                var MessageDto = Mapper.Map<IEnumerable<MessageDTO>>(Message);
                return Task.FromResult(MessageDto);
            }
            catch (Exception)
            {
                throw;
            }
        }
        
    }
}
