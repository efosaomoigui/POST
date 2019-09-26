using GIGLS.Core.Domain;
using GIGLS.Core.DTO;
using GIGLS.Core.IServices;
using GIGLS.Services.Implementation;
using GIGLS.WebApi.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace GIGLS.WebApi.Controllers
{
    //[Authorize(Roles = "Admin, ViewAdmin")]
    [AllowAnonymous]
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
        public async Task<IServiceResponse<IEnumerable<DeliveryLocationDTO>>> GetLocations()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var locations = await _deliveryLocationService.GetLocations();

                return new ServiceResponse<IEnumerable<DeliveryLocationDTO>>
                {
                    Object = locations
                };
            });
        }
    }
}