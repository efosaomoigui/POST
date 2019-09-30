using GIGLS.Core.DTO;
using GIGLS.Core.IServices;
using GIGLS.Services.Implementation;
using GIGLS.WebApi.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace GIGLS.WebApi.Controllers
{
    [Authorize(Roles = "Shipment, ViewAdmin")]
    [RoutePrefix("api/deliverylocation")]
    public class DeliveryLocationController : BaseWebApiController
    {
        private readonly IDeliveryLocationService _deliveryLocationService;
        public DeliveryLocationController(IDeliveryLocationService deliveryLocationService) : base(nameof(DeliveryLocationController))
        {
            _deliveryLocationService = deliveryLocationService;
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<IEnumerable<DeliveryLocationDTO>>> GetDeliveryLocations()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var locations = await _deliveryLocationService.GetDeliveryLocations();

                return new ServiceResponse<IEnumerable<DeliveryLocationDTO>>
                {
                    Object = locations
                };
            });
        }
    }
}