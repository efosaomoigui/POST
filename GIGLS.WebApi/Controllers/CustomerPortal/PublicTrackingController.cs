using GIGLS.Core.DTO.Admin;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.IServices;
using GIGLS.Core.IServices.CustomerPortal;
using GIGLS.Services.Implementation;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace GIGLS.WebApi.Controllers.CustomerPortal
{
    [RoutePrefix("api/webtracking")]
    public class PublicTrackingController : BaseWebApiController
    {
        private readonly ICustomerPortalService _portalService;

        public PublicTrackingController(ICustomerPortalService portalService) : base(nameof(PublicTrackingController))
        {
            _portalService = portalService;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("reportsummary")]
        public async Task<IServiceResponse<AdminReportDTO>> GetWebsiteData()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var data = await _portalService.WebsiteData();
                return new ServiceResponse<AdminReportDTO>
                {
                    Object = data

                };
            });
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("track/{waybillNumber}")]
        public async Task<IServiceResponse<IEnumerable<ShipmentTrackingDTO>>> PublicTrackShipment(string waybillNumber)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _portalService.PublicTrackShipment(waybillNumber);

                return new ServiceResponse<IEnumerable<ShipmentTrackingDTO>>
                {
                    Object = result
                };
            });
        }
    }
}
