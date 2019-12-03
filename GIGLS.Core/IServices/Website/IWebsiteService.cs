using GIGLS.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Website
{
    public interface IWebsiteService : IServiceDependencyMarker
    {
        Task<bool> SendSchedulePickupMail(WebsiteMessageDTO obj);
        Task<bool> SendQuoteMail(WebsiteMessageDTO obj);
        Task<bool> SendGIGGoIssuesMail(AppMessageDTO obj);
    }
    
}
