using GIGLS.Core.IServices;
using System.Threading.Tasks;

namespace GIGLS.Core.IMessageService
{
    public interface IMessageService : IServiceDependencyMarker
    {
        Task<bool> SendMessage(string address, string subject, string body, string sender = "");
    }
}
