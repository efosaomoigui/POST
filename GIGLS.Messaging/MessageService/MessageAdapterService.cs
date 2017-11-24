using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Messaging.MessageService
{
    public class MessageAdapterService
    {
        public async Task<bool> SendMessage(string messageType, string emailSmsType)
        {

            return await Task.FromResult(true);
        }
    }
}
