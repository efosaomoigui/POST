using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using POST.Core.DTO.Fleets;
using POST.Core.IServices;
using POST.Core.IServices.Fleets;
using POST.Services.Implementation;
using POST.Services.Implementation.Shipments;

namespace POST.WebApi.Controllers.FleetJobCards
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
        public async Task<IServiceResponse<IEnumerable<FleetJobCardDto>>> GetAllFleetJobCards()
        {
            return await HandleApiOperationAsync(async () => {
                var jobCards = await _fleetJobCardService.GetFleetJobCardsAsync();
                return new ServiceResponse<IEnumerable<FleetJobCardDto>>
                {
                    Object = jobCards.ToList()
                };
            });
        }
        
        // POST: FleetJobCard/ByFleetManager
        [HttpPost]
        [Route("closejobcard")]
        public async Task<IServiceResponse<bool>> CloseFleetJobCardsById(CloseJobCardDto jobCardDto)
        {
            var fileCheck = jobCardDto.ReceiptUrl.Split(':')[0];
            if (fileCheck.ToLower().Trim() != "https")
            {
                byte[] bytes = Convert.FromBase64String(jobCardDto.ReceiptUrl);

                //Save to AzureBlobStorage
                var picUrl = await AzureBlobServiceUtil.UploadAsync(bytes, $"JobCard-Receipt-For-{jobCardDto.VehicleNumber}-{DateTime.Now.Ticks}.png");
                jobCardDto.ReceiptUrl = picUrl;
            }

            return await HandleApiOperationAsync(async () => {
                var jobCard = await _fleetJobCardService.CloseJobCardAsync(jobCardDto);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        // GET: FleetJobCard/ByFleetManager in current month
        [HttpGet]
        [Route("alljobcards/incurrentmonth")]
        public async Task<IServiceResponse<IEnumerable<FleetJobCardByDateDto>>> GetFleetJobCardsByFleetManagerInCurrentMonth()
        {
            return await HandleApiOperationAsync(async () => {
                var jobCards = await _fleetJobCardService.GetAllFleetJobCardsByInCurrentMonthAsync();
                return new ServiceResponse<IEnumerable<FleetJobCardByDateDto>>
                {
                    Object = jobCards.ToList()
                };
            });
        }
        
        // GET: FleetJobCard/ById
        [HttpGet]
        [Route("{jobcardid}")]
        public async Task<IServiceResponse<FleetJobCardDto>> GetFleetJobCardById(int jobcardid)
        {
            return await HandleApiOperationAsync(async () => {
                var jobCard = await _fleetJobCardService.GetFleetJobCardByIdAsync(jobcardid);
                return new ServiceResponse<FleetJobCardDto>
                {
                    Object = jobCard
                };
            });
        }
    }
}