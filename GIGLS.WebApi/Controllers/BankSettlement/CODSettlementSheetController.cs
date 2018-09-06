using GIGLS.Core.DTO.BankSettlement;
using GIGLS.Core.IServices;
using GIGLS.Core.IServices.BankSettlement;
using GIGLS.Services.Implementation;
using GIGLS.WebApi.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace GIGLS.WebApi.Controllers.CODSettlementSheets
{
    [Authorize(Roles = "Admin, ViewAdmin")]
    [RoutePrefix("api/CODSettlementSheet")]
    public class CODSettlementSheetController : BaseWebApiController
    {
        private readonly ICODSettlementSheetService _codSettlementSheetService;

        public CODSettlementSheetController(ICODSettlementSheetService codSettlementSheetService) : base(nameof(CODSettlementSheetController))
        {
            _codSettlementSheetService = codSettlementSheetService;
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<IEnumerable<CODSettlementSheetDTO>>> GetCODSettlementSheets()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var codSettlementSheets = await _codSettlementSheetService.GetCODSettlementSheets();
                return new ServiceResponse<IEnumerable<CODSettlementSheetDTO>>
                {
                    Object = codSettlementSheets
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("")]
        public async Task<IServiceResponse<object>> AddCODSettlementSheet(CODSettlementSheetDTO codSettlementSheetDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var CODSettlementSheet = await _codSettlementSheetService.AddCODSettlementSheet(codSettlementSheetDTO);

                return new ServiceResponse<object>
                {
                    Object = CODSettlementSheet
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{codSettlementSheetId:int}")]
        public async Task<IServiceResponse<CODSettlementSheetDTO>> GetCODSettlementSheet(int codSettlementSheetId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var CODSettlementSheet = await _codSettlementSheetService.GetCODSettlementSheetById(codSettlementSheetId);

                return new ServiceResponse<CODSettlementSheetDTO>
                {
                    Object = CODSettlementSheet
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpDelete]
        [Route("{codSettlementSheetId:int}")]
        public async Task<IServiceResponse<bool>> DeleteCODSettlementSheet(int codSettlementSheetId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _codSettlementSheetService.DeleteCODSettlementSheet(codSettlementSheetId);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{codSettlementSheetId:int}")]
        public async Task<IServiceResponse<bool>> UpdateCODSettlementSheet(int codSettlementSheetId, CODSettlementSheetDTO codSettlementSheetDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _codSettlementSheetService.UpdateCODSettlementSheet(codSettlementSheetId, codSettlementSheetDTO);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("updatemultiplestatus")]
        public async Task<IServiceResponse<bool>> UpdateMultipleStatusCODSettlementSheet(List<string> WaybillNumbers)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _codSettlementSheetService.UpdateMultipleStatusCODSettlementSheet(WaybillNumbers);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("UnbankedCODShipmentSettlement")]
        public async Task<IServiceResponse<IEnumerable<CODSettlementSheetDTO>>> GetUnbankedCODShipmentSettlement()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var codSettlementSheets = await _codSettlementSheetService.GetUnbankedCODShipmentSettlement();
                return new ServiceResponse<IEnumerable<CODSettlementSheetDTO>>
                {
                    Object = codSettlementSheets
                };
            });
        }

    }
}
