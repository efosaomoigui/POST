using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.Core.IServices;
using GIGLS.Core.IServices.ServiceCentres;
using GIGLS.Services.Implementation;
using GIGLS.WebApi.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace GIGLS.WebApi.Controllers.ServiceCentres
{
    [Authorize(Roles = "Admin, ViewAdmin")]
    [RoutePrefix("api/lga")]
    public class LGAController : BaseWebApiController
    {
        private ILGAService _lgaService;
        public LGAController(ILGAService lgaService) : base(nameof(LGAController))
        {
            _lgaService = lgaService;
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<IEnumerable<LGADTO>>> GetLGAs()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var lga = await _lgaService.GetLGAs();
                return new ServiceResponse<IEnumerable<LGADTO>>
                {
                    Object = lga
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("active")]
        public async Task<IServiceResponse<IEnumerable<LGADTO>>> GetActiveLGAs()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var lga = await _lgaService.GetActiveLGAs();
                return new ServiceResponse<IEnumerable<LGADTO>>
                {
                    Object = lga

                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{lgaId:int}")]
        public async Task<IServiceResponse<LGADTO>> GetLGAById(int lgaId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var lga = await _lgaService.GetLGAById(lgaId);
                return new ServiceResponse<LGADTO>
                {
                    Object = lga
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("")]
        public async Task<IServiceResponse<object>> AddLGA(LGADTO lgaDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var lga = await _lgaService.AddLGA(lgaDto);
                return new ServiceResponse<object>
                {
                    Object = lga
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{lgaId:int}")]
        public async Task<IServiceResponse<object>> UpdateLGA(int lgaId, LGADTO lgaDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _lgaService.UpdateLGA(lgaId, lgaDto);
                return new ServiceResponse<object>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{lgaId:int}/status/{status}")]
        public async Task<IServiceResponse<object>> UpdateLGA(int lgaId, bool status)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _lgaService.UpdateLGA(lgaId, status);
                return new ServiceResponse<object>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpDelete]
        [Route("{lgaId:int}")]
        public async Task<IServiceResponse<bool>> DeleteLGA(int lgaId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _lgaService.DeleteLGA(lgaId);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{lgaId:int}/homedeliverystatus/{status}")]
        public async Task<IServiceResponse<object>> UpdateHomeDeliveryStatus(int lgaId, bool status)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _lgaService.UpdateHomeDeliveryLocation(lgaId, status);
                return new ServiceResponse<object>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("activehomedeliverylocations")]
        public async Task<IServiceResponse<IEnumerable<LGADTO>>> GetActiveHomeDeliveryLocations()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var locations = await _lgaService.GetActiveHomeDeliveryLocations();
                return new ServiceResponse<IEnumerable<LGADTO>>
                {
                    Object = locations

                };
            });
        }


        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("homedeliverybystate/{stateId}")]
        public async Task<IServiceResponse<IEnumerable<LGADTO>>> GetHomeDeliveryLocationByState(int stateId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var locations = await _lgaService.GetLGAByState(stateId);
                return new ServiceResponse<IEnumerable<LGADTO>>
                {
                    Object = locations

                };
            });
        }

    }
}