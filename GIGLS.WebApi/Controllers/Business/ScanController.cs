using GIGLS.Core.IServices;
using GIGLS.Core.IServices.Business;
using GIGLS.Services.Implementation;
using System.Threading.Tasks;
using System.Web.Http;
using GIGLS.Core.DTO.Shipments;
using GIGLS.WebApi.Filters;

namespace GIGLS.WebApi.Controllers.Business
{
    [Authorize(Roles = "Shipment")]
    [RoutePrefix("api/scan")]
    public class ScanController : BaseWebApiController
    {
        private readonly IScanService _scan;

        public ScanController(IScanService scan) : base(nameof(ScanController))
        {
            _scan = scan;
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("")]
        public async Task<IServiceResponse<bool>> ScanShipment(ScanDTO scanStatus)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _scan.ScanShipment(scanStatus);

                return new ServiceResponse<bool>
                {
                    Object = result
                };
            });
        }

        //[GIGLSActivityAuthorize(Activity = "Create")]
        //[HttpPost]
        //[Route("")]
        //public async Task<IServiceResponse<bool>> ScanShipment(string waybillNumber, ShipmentScanStatus scanStatus)
        //{
        //    return await HandleApiOperationAsync(async () =>
        //    {
        //        var result = await _scan.ScanShipment(waybillNumber, scanStatus);

        //        return new ServiceResponse<bool>
        //        {
        //            Object = result
        //        };
        //    });
        //}
    }
}
