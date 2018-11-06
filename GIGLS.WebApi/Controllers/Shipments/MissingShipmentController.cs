using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.IServices;
using GIGLS.Core.IServices.Shipments;
using GIGLS.Services.Implementation;
using GIGLS.WebApi.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace GIGLS.WebApi.Controllers.Shipments
{
    [Authorize(Roles = "Admin, ViewAdmin, Shipment")]
    [RoutePrefix("api/missingshipment")]
    public class MissingShipmentController : BaseWebApiController
    {

        private IMissingShipmentService _missingService;
        public MissingShipmentController(IMissingShipmentService missingService) : base(nameof(MissingShipmentController))
        {
            _missingService = missingService;
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<IEnumerable<MissingShipmentDTO>>> GetMissingShipments()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var missingShipments = await _missingService.GetMissingShipments();
                return new ServiceResponse<IEnumerable<MissingShipmentDTO>>
                {
                    Object = missingShipments
                };
            });
        }
        
        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{missingShipmentId:int}")]
        public async Task<IServiceResponse<MissingShipmentDTO>> GetMissingShipmentById(int missingShipmentId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var missingShipment = await _missingService.GetMissingShipmentById(missingShipmentId);

                return new ServiceResponse<MissingShipmentDTO>
                {
                    Object = missingShipment
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("")]
        public async Task<IServiceResponse<object>> AddMissingShipment(MissingShipmentDTO newMissingShipment)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var missingShipment = await _missingService.AddMissingShipment(newMissingShipment);
                return new ServiceResponse<object>
                {
                    Object = missingShipment
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{missingShipmentId:int}")]
        public async Task<IServiceResponse<object>> UpdateMissingShipment(int missingShipmentId, MissingShipmentDTO missingShipmentDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _missingService.UpdateMissingShipment(missingShipmentId, missingShipmentDto);
                return new ServiceResponse<object>
                {
                    Object = true
                };
            });
        }
        
        [GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpDelete]
        [Route("{missingShipmentId:int}")]
        public async Task<IServiceResponse<bool>> DeletemissingShipment(int missingShipmentId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _missingService.DeleteMissingShipment(missingShipmentId);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }
    }
}
