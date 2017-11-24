using GIGLS.Core.IServices;

namespace GIGLS.Core.IMessage
{
    public interface ISMSMessageSender : IServiceDependencyMarker
    {
        //Send Sms Messages
        void SendMessage(string address, string subject, string body, string sender = "");
        void InitializeClient();
    }
}
