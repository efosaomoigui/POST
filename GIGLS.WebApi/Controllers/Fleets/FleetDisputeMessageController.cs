using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using GIGLS.Core.DTO.Fleets;
using GIGLS.Core.IServices;
using GIGLS.Core.IServices.Fleets;
using GIGLS.Services.Implementation;
using GIGLS.WebApi.Controllers.FleetJobCards;

namespace GIGLS.WebApi.Controllers.Fleets
{
    [Authorize]
    [RoutePrefix("api/fleetdisputemessage")]
    public class FleetDisputeMessageController : BaseWebApiController
    {
        private readonly IFleetDisputeMessageService _fleetDisputeMessageService;

        public FleetDisputeMessageController(IFleetDisputeMessageService fleetDisputeMessageService) : base(nameof(FleetJobCardController))
        {
            _fleetDisputeMessageService = fleetDisputeMessageService;
        }

        // GET: FleetDisputeMessage
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<IEnumerable<FleetDisputeMessageDto>>> GetFleetDisputeMessages()
        {
            return await HandleApiOperationAsync(async () => {
                var disputeMsg = await _fleetDisputeMessageService.GetAllFleetDisputeMessagesAsync();
                return new ServiceResponse<IEnumerable<FleetDisputeMessageDto>>
                {
                    Object = disputeMsg.ToList()
                };
            });
        }

        // POST: FleetDisputeMessage
        [HttpPost]
        [Route("")]
        public async Task<IServiceResponse<bool>> AddFleetDisputeMessage(FleetDisputeMessageDto fleetDisputeMessage)
        {
            return await HandleApiOperationAsync(async () => {
                var result = await _fleetDisputeMessageService.AddFleetDisputeMessageAsync(fleetDisputeMessage);
                return new ServiceResponse<bool>
                {
                    Object = result
                };
            });
        }
    }
}