using GIGLS.Core.DTO;
using GIGLS.Core.IMessageService;
using GIGLS.Core.IServices;
using GIGLS.Core.IServices.Website;
using GIGLS.Services.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace GIGLS.WebApi.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api/website")]
    public class WebsiteEmailController : BaseWebApiController
    {
        private readonly IWebsiteService _websiteService;
        public WebsiteEmailController(IWebsiteService websiteService): base(nameof(WebsiteEmailController))
        {
            _websiteService = websiteService;
        }

        [HttpPost]
        [Route("schedulePickup")]
        public async Task<IServiceResponse<bool>> SendSchedulePickupMail(WebsiteMessageDTO obj)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _websiteService.SendSchedulePickupMail(obj);

                return new ServiceResponse<bool>
                {
                    Object = result
                };
            });
        }

        [HttpPost]
        [Route("requestQuote")]
        public async Task<IServiceResponse<bool>> SendRequestQuoteMail(WebsiteMessageDTO obj)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _websiteService.SendQuoteMail(obj);

                return new ServiceResponse<bool>
                {
                    Object = result
                };
            });
        }
    }
}