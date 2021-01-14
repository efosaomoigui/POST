using GIGLS.Core.IServices;
using GIGLS.Core.IServices.Business;
using GIGLS.Services.Implementation;
using System.Threading.Tasks;
using System.Web.Http;
using GIGLS.Core.DTO.Shipments;
using GIGLS.WebApi.Filters;
using System.Collections.Generic;
using System;

namespace GIGLS.WebApi.Controllers.Business
{
    [Authorize(Roles = "Shipment, ViewAdmin")]
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

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("multiple")]
        public async Task<IServiceResponse<bool>> ScanMultipleShipment(List<ScanDTO> scanList)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _scan.ScanMultipleShipment(scanList);

                return new ServiceResponse<bool>
                {
                    Object = result
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("multiple2")]
        public async Task<IServiceResponse<bool>> ScanMultipleShipment2(List<ScanDTO> scanList)
        {
            return await HandleApiOperationAsync(async () =>
            {
                List<ScanDTO> multipleWaybillList = new List<ScanDTO>();
                string[] ArrWaybills = scanList[0].WaybillNumber.Split(new[] { Environment.NewLine },StringSplitOptions.None);

                foreach (var waybill in ArrWaybills)
                {
                    ScanDTO nItem = new ScanDTO();
                    nItem.WaybillNumber = waybill;
                    nItem.ShipmentScanStatus = scanList[1].ShipmentScanStatus;
                    multipleWaybillList.Add(nItem);
                }

                var result = await _scan.ScanMultipleShipment(multipleWaybillList);

                return new ServiceResponse<bool>
                {
                    Object = result
                };
            });
        }
    }
}
