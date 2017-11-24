using GIGLS.Core.IMessageService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Messaging.MessageService
{
    public class MessageService : IMessageService
    {
        public async Task<bool> SendMessage(string address, string subject, string body, string sender = "")
        {
            var result = false;




            return await Task.FromResult(result);
        }
    }
}
