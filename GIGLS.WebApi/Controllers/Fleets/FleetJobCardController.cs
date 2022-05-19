using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GIGL.GIGLS.Core.Domain;
using GIGLS.Core.DTO.Fleets;
using GIGLS.Core.IServices;
using GIGLS.Core.IServices.Fleets;
using GIGLS.Services.Implementation;
using GIGLS.WebApi.Controllers.Vehicles;

namespace GIGLS.WebApi.Controllers.Fleets
{
    [RoutePrefix("api/fleetjobcard")]
    public class FleetJobCardController : BaseWebApiController
    {
        private readonly IFleetJobCardService _fleetJobCardService;

        public FleetJobCardController(IFleetJobCardService fleetJobCardService) : base(nameof(FleetJobCardController))
        {
            _fleetJobCardService = fleetJobCardService;
        }

        // GET: FleetJobCard
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<IEnumerable<FleetJobCardDto>>> GetFleetJobCards()
        {
            return await HandleApiOperationAsync(async () => {
                var jobCards = await _fleetJobCardService.GetFleetJobCardsAsync();
                return new ServiceResponse<IEnumerable<FleetJobCardDto>>
                {
                    Object = jobCards.ToList()
                };
            });
        }
        
        // POST: FleetJobCard
        [HttpPost]
        [Route("")]
        public async Task<IServiceResponse<bool>> OpenFleetJobCards(OpenFleetJobCardDto fleetJobCard)
        {
            return await HandleApiOperationAsync(async () => {
                var jobCards = await _fleetJobCardService.OpenFleetJobCardsAsync(fleetJobCard);
                return new ServiceResponse<bool>
                {
                    Object = jobCards
                };
            });
        }
    }
}