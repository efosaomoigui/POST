using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.IServices;
using GIGLS.Core.IServices.Shipments;
using GIGLS.CORE.DTO.Shipments;
using GIGLS.Services.Implementation;
using GIGLS.WebApi.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace GIGLS.WebApi.Controllers.Shipments
{
    [Authorize(Roles = "Shipment, ViewAdmin")]
    [RoutePrefix("api/preshipment")]
    public class PreShipmentController : BaseWebApiController
    {
        private readonly IPreShipmentService _service;

        public PreShipmentController(IPreShipmentService service) : base(nameof(ShipmentController))
        {
            _service = service;
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<IEnumerable<PreShipmentDTO>>> GetPreShipments([FromUri]FilterOptionsDto filterOptionsDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var preShipments = _service.GetPreShipments(filterOptionsDto);
                return new ServiceResponse<IEnumerable<PreShipmentDTO>>
                {
                    Object = await preShipments.Item1,
                    Total = preShipments.Item2
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("")]
        public async Task<IServiceResponse<PreShipmentDTO>> AddShipment(PreShipmentDTO preShipmentDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var preShipment = await _service.AddPreShipment(preShipmentDTO);
                return new ServiceResponse<PreShipmentDTO>
                {
                    Object = preShipment
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{PreShipmentId:int}")]
        public async Task<IServiceResponse<PreShipmentDTO>> GetPreShipment(int preShipmentId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var preShipment = await _service.GetPreShipment(preShipmentId);
                return new ServiceResponse<PreShipmentDTO>
                {
                    Object = preShipment
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{waybill}/waybill")]
        public async Task<IServiceResponse<PreShipmentDTO>> GetPreShipment(string waybill)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var preShipment = await _service.GetPreShipment(waybill);
                return new ServiceResponse<PreShipmentDTO>
                {
                    Object = preShipment
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpDelete]
        [Route("{PreShipmentId:int}")]
        public async Task<IServiceResponse<bool>> DeletePreShipment(int preShipmentId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _service.DeletePreShipment(preShipmentId);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }


        [GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpDelete]
        [Route("{waybill}/waybill")]
        public async Task<IServiceResponse<bool>> DeletePreShipment(string waybill)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _service.DeletePreShipment(waybill);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{preShipmentId:int}")]
        public async Task<IServiceResponse<bool>> UpdatePreShipment(int preShipmentId, PreShipmentDTO preShipmentDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _service.UpdatePreShipment(preShipmentId, preShipmentDTO);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{waybill}")]
        public async Task<IServiceResponse<bool>> UpdatePreShipment(string waybill, PreShipmentDTO preShipmentDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _service.UpdatePreShipment(waybill, preShipmentDTO);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }



    }
}
