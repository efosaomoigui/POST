using GIGLS.Core.DTO;
using GIGLS.Core.DTO.Report;
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
    //[Authorize(Roles = "Shipment, ViewAdmin")]
    [AllowAnonymous]
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
        public async Task<IServiceResponse<List<RiderDeliveryDTO>>> GetRiderDelivery(string riderId, ShipmentCollectionFilterCriteria dateFilterCriteria)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var riderDelivery = await _riderDeliveryService.GetRiderDelivery(riderId, dateFilterCriteria);

                return new ServiceResponse<List<RiderDeliveryDTO>>
                {
                    Object = riderDelivery
                };
            });
        }

    }
}