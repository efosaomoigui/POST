using GIGLS.Core.IMessage;
using System;
using GIGLS.Core.Config;


namespace GIGLS.INFRASTRUCTURE.MessageService
{
    public class SmsSender : ISMSMessageSender
    {
        public IMessageConfig _config;

        public SmsSender(IMessageConfig config)
        {
            _config = config;
            InitializeClient();
        }

        public void InitializeClient()
        {
            throw new NotImplementedException();
        }

        public void SendMessage(string address, string subject, string body, string sender)
        {
            throw new NotImplementedException();
        }
    }
}
