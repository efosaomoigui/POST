using GIGLS.Core.DTO.ShipmentScan;
using GIGLS.Core.IServices;
using GIGLS.Core.IServices.ShipmentScan;
using GIGLS.Services.Implementation;
using GIGLS.WebApi.Filters;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace GIGLS.WebApi.Controllers.ShipmentScan
{
    [Authorize(Roles = "Admin, ViewAdmin")]
    [RoutePrefix("api/scanstatus")]
    public class ScanStatusController : BaseWebApiController
    {
        private readonly IScanStatusService _scanService;
        public ScanStatusController(IScanStatusService scanService) : base(nameof(ScanStatusController))
        {
            _scanService = scanService;
        }


        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<IEnumerable<ScanStatusDTO>>> GetScanStatus()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var scanStatus = await _scanService.GetNonHiddenScanStatus();
                
                return new ServiceResponse<IEnumerable<ScanStatusDTO>>
                {
                    Object = scanStatus
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("all")]
        public async Task<IServiceResponse<IEnumerable<ScanStatusDTO>>> GetScanStatusAll()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var scanStatus = await _scanService.GetScanStatus();

                return new ServiceResponse<IEnumerable<ScanStatusDTO>>
                {
                    Object = scanStatus
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("")]
        public async Task<IServiceResponse<object>> AddScanStatus(ScanStatusDTO scanStatusDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var scanStatus = await _scanService.AddScanStatus(scanStatusDto);

                return new ServiceResponse<object>
                {
                    Object = scanStatus
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{scanStatusId:int}")]
        public async Task<IServiceResponse<ScanStatusDTO>> GetScanStatus(int scanStatusId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var scanStatus = await _scanService.GetScanStatusById(scanStatusId);

                return new ServiceResponse<ScanStatusDTO>
                {
                    Object = scanStatus
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("code/{code}")]
        public async Task<IServiceResponse<ScanStatusDTO>> GetScanStatus(string code)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var scanStatus = await _scanService.GetScanStatusByCode(code);

                return new ServiceResponse<ScanStatusDTO>
                {
                    Object = scanStatus
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpDelete]
        [Route("{scanStatusId:int}")]
        public async Task<IServiceResponse<bool>> DeleteScanStatus(int scanStatusId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _scanService.DeleteScanStatus(scanStatusId);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{scanStatusId:int}")]
        public async Task<IServiceResponse<bool>> UpdateScanStatus(int scanStatusId, ScanStatusDTO scanStatusDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _scanService.UpdateScanStatus(scanStatusId, scanStatusDto);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

    }
}
