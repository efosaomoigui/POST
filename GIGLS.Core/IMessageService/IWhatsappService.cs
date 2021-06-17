using GIGLS.Core.DTO;
using GIGLS.Core.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.IMessageService
{
    public interface IWhatsappService : IServiceDependencyMarker
    {
        Task<string> SendWhatsappMessageAsync(WhatsAppMessageDTO message);
        Task<string> GetConsentDetailsAsync(WhatsappNumberDTO number);
        Task<string> ManageOptInOutAsync(ManageWhatsappConsentDTO consent);
    }
}
