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
using GIGLS.Core.Enums;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories
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
