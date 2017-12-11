using GIGLS.Core.IServices;
using GIGLS.Core.IServices.Shipments;
using GIGLS.Services.Implementation;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using GIGLS.Core.DTO.Shipments;
using GIGLS.WebApi.Filters;

namespace GIGLS.WebApi.Controllers.Shipments
{
    [Authorize(Roles = "SuperAdmin,SubAdmin,Shipment,Account,Report")]
    [RoutePrefix("api/waybill")]
    public class WaybillNumberController : BaseWebApiController
    {
        private readonly IWaybillNumberService _service;

        public WaybillNumberController(IWaybillNumberService service) : base(nameof(WaybillNumberController))
        {
            _service = service;
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<IEnumerable<WaybillNumberDTO>>> GetAllWaybills()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var waybills = await _service.GetAllWayBillNumbers();

                return new ServiceResponse<IEnumerable<WaybillNumberDTO>>
                {
                    Object = waybills
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("active")]
        public async Task<IServiceResponse<List<WaybillNumberDTO>>> GetActiveWaybills()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var waybills = await _service.GetActiveWayBillNumbers();

                return new ServiceResponse<List<WaybillNumberDTO>>
                {
                    Object = waybills
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("delivery")]
        public async Task<IServiceResponse<List<WaybillNumberDTO>>> GetDeliveryWaybills()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var waybills = await _service.GetDeliverWayBillNumbers();

                return new ServiceResponse<List<WaybillNumberDTO>>
                {
                    Object = waybills
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{waybillId:int}")]
        public async Task<IServiceResponse<WaybillNumberDTO>> GetWaybillById(int waybillId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var waybills = await _service.GetWayBillNumberById(waybillId);

                return new ServiceResponse<WaybillNumberDTO>
                {
                    Object = waybills
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{waybill}")]
        public async Task<IServiceResponse<WaybillNumberDTO>> GetWaybillByCode(string waybill)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var waybills = await _service.GetWayBillNumberById(waybill);

                return new ServiceResponse<WaybillNumberDTO>
                {
                    Object = waybills
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpDelete]
        [Route("{waybillId:int}")]
        public async Task<IServiceResponse<bool>> DeleteWaybillById(int waybillId)
        {
            return await HandleApiOperationAsync(async () => {

                await _service.RemoveWaybillNumber(waybillId);
                return new ServiceResponse<bool>
                {
                    Object = true
                };

            });
        }

        [GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpDelete]
        [Route("{waybill}")]
        public async Task<IServiceResponse<bool>> DeleteWaybillByCode(string waybill)
        {
            return await HandleApiOperationAsync(async () => {

                await _service.RemoveWaybillNumber(waybill);
                return new ServiceResponse<bool>
                {
                    Object = true
                };

            });
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{waybillId:int}")]
        public async Task<IServiceResponse<bool>> UpdateWaybillById(int waybillId)
        {
            return await HandleApiOperationAsync(async () => {
                await _service.UpdateWaybillNumber(waybillId);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{waybill}")]
        public async Task<IServiceResponse<bool>> UpdateWaybillByCode(string waybill)
        {
            return await HandleApiOperationAsync(async () => {
                await _service.UpdateWaybillNumber(waybill);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }
    }
}
