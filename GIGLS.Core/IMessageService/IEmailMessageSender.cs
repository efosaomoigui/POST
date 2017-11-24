using GIGLS.Core.IServices;

namespace GIGLS.Core.IMessage
{
    public interface IEmailMessageSender : IServiceDependencyMarker
    {
        //Send Email Messages
        void SendMessage(string address, string subject, string body, string sender="");
        void InitializeClient();
    }
}
