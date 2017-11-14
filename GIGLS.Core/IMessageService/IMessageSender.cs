using GIGLS.Core.IServices;

namespace GIGLS.Core.IMessage
{
    public interface IMessageSender : IServiceDependencyMarker
    {
        //Send Email and Sms Messages
        void SendMessage(string address, string subject, string body, string sender="");
        void InitializeClient();
    }
}
