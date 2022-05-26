using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using GIGLS.Core.DTO.Fleets;
using GIGLS.Core.IServices;
using GIGLS.Core.IServices.Fleets;
using GIGLS.Services.Implementation;

namespace GIGLS.WebApi.Controllers.FleetJobCards
{
    [Authorize]
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
        //public async Task<IServiceResponse<bool>> OpenFleetJobCards(OpenFleetJobCardDto fleetJobCard)
        public async Task<IServiceResponse<bool>> OpenFleetJobCards(NewJobCard fleetJobCard)
        {
            return await HandleApiOperationAsync(async () => {
                var jobCards = await _fleetJobCardService.OpenFleetJobCardsAsync(fleetJobCard);
                return new ServiceResponse<bool>
                {
                    Object = jobCards
                };
            });
        }

        // GET: FleetJobCard/ByDateAndVehicleNumber
        [HttpGet]
        [Route("jobcardsbydateandvehicle/{vehicleNumber}/{startDate}/{endDate}")]
        public async Task<IServiceResponse<IEnumerable<FleetJobCardByDateDto>>> GetFleetJobCardByDateRange(string vehicleNumber, DateTime? startDate, DateTime? endDate)
        {
            var dto = new GetFleetJobCardByDateRangeDto() { EndDate = endDate, StartDate = startDate, VehicleNumber = vehicleNumber };
            return await HandleApiOperationAsync(async () =>
            {
                var jobCards = await _fleetJobCardService.GetFleetJobCardByDateRangeAsync(dto);
                return new ServiceResponse<IEnumerable<FleetJobCardByDateDto>>
                {
                    Object = jobCards.ToList()
                };
            });
        }

        // GET: FleetJobCard/ByFleetManager
        [HttpGet]
        [Route("byfleetmanager")]
        public async Task<IServiceResponse<IEnumerable<FleetJobCardDto>>> GetFleetJobCardsByFleetManager()
        {
            return await HandleApiOperationAsync(async () => {
                var jobCards = await _fleetJobCardService.GetFleetJobCardsByFleetManagerAsync();
                return new ServiceResponse<IEnumerable<FleetJobCardDto>>
                {
                    Object = jobCards.ToList()
                };
            });
        }
        
        // PATCH: FleetJobCard/ByFleetManager
        [HttpPatch]
        [Route("closejobcard/{jobCardId}")]
        public async Task<IServiceResponse<bool>> CloseFleetJobCardsById(int jobCardId)
        {
            return await HandleApiOperationAsync(async () => {
                var jobCard = await _fleetJobCardService.CloseJobCardByIdAsync(jobCardId);
                return new ServiceResponse<bool>
                {
                    Object = jobCard
                };
            });
        }

        // GET: FleetJobCard/ByFleetManager in current month
        [HttpGet]
        [Route("byfleetmanager/currentmonth")]
        public async Task<IServiceResponse<IEnumerable<FleetJobCardByDateDto>>> GetFleetJobCardsByFleetManagerInCurrentMonth()
        {
            return await HandleApiOperationAsync(async () => {
                var jobCards = await _fleetJobCardService.GetFleetJobCardsByFleetManagerInCurrentMonthAsync();
                return new ServiceResponse<IEnumerable<FleetJobCardByDateDto>>
                {
                    Object = jobCards.ToList()
                };
            });
        }
    }
}