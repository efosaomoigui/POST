using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.IServices;
using GIGLS.Core.IServices.Shipments;
using GIGLS.Services.Implementation;
using GIGLS.WebApi.Filters;
using System.Threading.Tasks;
using System.Web.Http;

namespace GIGLS.WebApi.Controllers.Shipments
{
    [Authorize(Roles = "Shipment, ViewAdmin")]
    [RoutePrefix("api/shipmentreroute")]
    public class ShipmentRerouteController : BaseWebApiController
    {
        private readonly IShipmentRerouteService _service;

        public ShipmentRerouteController(IShipmentRerouteService service) : base(nameof(ShipmentRerouteController))
        {
            _service = service;
        }
        
        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("")]
        public async Task<IServiceResponse<ShipmentDTO>> AddShipmentReroute(ShipmentDTO shipmentDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var shipment = await _service.AddRerouteShipment(shipmentDto);

                return new ServiceResponse<ShipmentDTO>
                {
                    Object = shipment
                };
            });
        }

    }
}
