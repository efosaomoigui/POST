using POST.Core.DTO;
using POST.Core.DTO.Report;
using POST.Core.IServices;
using POST.Services.Implementation;
using POST.WebApi.Filters;
using System.Threading.Tasks;
using System.Web.Http;

namespace POST.WebApi.Controllers
{
    [Authorize(Roles = "Shipment, ViewAdmin")]
    [RoutePrefix("api/riderdelivery")]
    public class RiderDeliveryController : BaseWebApiController
    {
        
        private readonly IRiderDeliveryService _riderDeliveryService;
        public RiderDeliveryController(IRiderDeliveryService riderDeliveryService) : base(nameof(RiderDeliveryController))
        {
            _riderDeliveryService = riderDeliveryService;
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpPost]
        [Route("riderdeliverybydate/{riderId}")]
        public async Task<IServiceResponse<RiderDeliveryViewDTO>> GetRiderDelivery(string riderId, ShipmentCollectionFilterCriteria dateFilterCriteria)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var riderDelivery = await _riderDeliveryService.GetRiderDelivery(riderId, dateFilterCriteria);

                return new ServiceResponse<RiderDeliveryViewDTO>
                {
                    Object = riderDelivery
                };
            });
        }

    }
}