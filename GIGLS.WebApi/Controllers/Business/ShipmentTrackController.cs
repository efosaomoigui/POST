using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.IServices;
using GIGLS.Core.IServices.Business;
using GIGLS.Services.Implementation;
using GIGLS.WebApi.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace GIGLS.WebApi.Controllers.Business
{
    [Authorize(Roles = "Admin,Shipment,Account,Report")]
    [RoutePrefix("api/shipmenttrack")]
    public class ShipmentTrackController : BaseWebApiController
    {
        private readonly IShipmentTrackService _shipmentTrack;

        public ShipmentTrackController(IShipmentTrackService shipmentTrack) : base(nameof(ShipmentTrackController))
        {
            _shipmentTrack = shipmentTrack;
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<IEnumerable<ShipmentTrackingDTO>>> TrackShipment(string waybillNumber)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _shipmentTrack.TrackShipment(waybillNumber);

                return new ServiceResponse<IEnumerable<ShipmentTrackingDTO>>
                {
                    Object = result
                };
            });
        }
    }
}
