using GIGLS.Core.IServices;
using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.Core.IServices.ServiceCentres;
using GIGLS.Services.Implementation;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using GIGLS.WebApi.Filters;

namespace GIGLS.WebApi.Controllers.ServiceCentres  
{
    [Authorize(Roles = "SuperAdmin,SubAdmin,Shipment,Account,Report")]
    [RoutePrefix("api/servicecentre")]
    public class ServiceCentreController : BaseWebApiController
    {
        private readonly IServiceCentreService _service;

        public ServiceCentreController(IServiceCentreService service):base(nameof(ServiceCentreController))
        {
            _service = service;
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<IEnumerable<ServiceCentreDTO>>> GetServiceCentres()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var centres = await _service.GetServiceCentres();
                return new ServiceResponse<IEnumerable<ServiceCentreDTO>>
                {
                    Object = centres
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("")]
        public async Task<IServiceResponse<object>> AddServiceCentre(ServiceCentreDTO centreDto)
        {
            return await HandleApiOperationAsync( async () =>
            {
                var centre = await _service.AddServiceCentre(centreDto);

                return new ServiceResponse<object> {
                        Object = centre
                };
            });

        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{servicecentreId:int}")]
        public async Task<IServiceResponse<ServiceCentreDTO>> GetServiceCentre(int servicecentreId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var servicecentre = await _service.GetServiceCentreById(servicecentreId);

                return new ServiceResponse<ServiceCentreDTO>
                {
                    Object = servicecentre
                };

            });
            
            
        }

        [GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpDelete]
        [Route("{servicecentreId:int}")]
        public async Task<IServiceResponse<bool>> DeleteServiceCentre(int servicecentreId)
        {
            return await HandleApiOperationAsync(async () =>
           {
               await _service.DeleteServiceCentre(servicecentreId);
               return new ServiceResponse<bool>
               {
                   Object = true
               };
           });
           
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{servicecentreId:int}")]
        public async Task<IServiceResponse<bool>> UpdateServiceCentre(int servicecentreId, ServiceCentreDTO centreDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _service.UpdateServiceCentre(servicecentreId, centreDto);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
           
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{servicecentreId:int}/status/{status}")]
        public async Task<IServiceResponse<bool>> UpdateServiceCentreStatus(int servicecentreId, bool status)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _service.ServiceCentreStatus(servicecentreId, status);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
           
        }
    }
}
