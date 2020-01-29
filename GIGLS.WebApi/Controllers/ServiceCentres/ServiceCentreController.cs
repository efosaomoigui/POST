                                                                                                               using GIGLS.Core.IServices;
using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.Core.IServices.ServiceCentres;
using GIGLS.Services.Implementation;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using GIGLS.WebApi.Filters;
using System.Linq;
using GIGLS.Core.IServices.User;
using GIGLS.Core.DTO;

namespace GIGLS.WebApi.Controllers.ServiceCentres  
{
    [Authorize(Roles = "Admin, ViewAdmin")]
    [RoutePrefix("api/servicecentre")]
    public class ServiceCentreController : BaseWebApiController
    {
        private readonly IServiceCentreService _service;
        private readonly IUserService _userService;

        public ServiceCentreController(IServiceCentreService service, IUserService userService):base(nameof(ServiceCentreController))
        {
            _userService = userService;
            _service = service;
            _userService = userService;
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

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("hubs")]
        public async Task<IServiceResponse<IEnumerable<ServiceCentreDTO>>> GetHUBServiceCentres()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var centres = await _service.GetHUBServiceCentres();
                //centres = centres.Where(s => s.IsHUB == true);

                return new ServiceResponse<IEnumerable<ServiceCentreDTO>>
                {
                    Object = centres
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("usersServiceCentres")]
        public async Task<IServiceResponse<IEnumerable<ServiceCentreDTO>>> GetUsersServiceCentres()
        {
            return await HandleApiOperationAsync(async () =>
            {
                //1. all service centres
                var allCentres = await _service.GetServiceCentres();

                //2. all priviledged users service centres
                var usersServiceCentresId = await _userService.GetPriviledgeServiceCenters();

                //3.
                var usersServiceCentres = allCentres.Where(s => usersServiceCentresId.Contains(s.ServiceCentreId));
                return new ServiceResponse<IEnumerable<ServiceCentreDTO>>
                {
                    Object = usersServiceCentres
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("local")]
        public async Task<IServiceResponse<IEnumerable<ServiceCentreDTO>>> GetLocalServiceCentres()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var countryIds = await _userService.GetPriviledgeCountryIds();
                var centres = await _service.GetLocalServiceCentres(countryIds);
                return new ServiceResponse<IEnumerable<ServiceCentreDTO>>
                {
                    Object = centres
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("international")]
        public async Task<IServiceResponse<IEnumerable<ServiceCentreDTO>>> GetInternationalServiceCentres()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var centres = await _service.GetInternationalServiceCentres();
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

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("station/{stationId:int}")]
        public async Task<IServiceResponse<IEnumerable<ServiceCentreDTO>>> GetServiceCentreByStationId (int stationId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var serviceCenters = await _service.GetServiceCentresByStationId(stationId);
                return new ServiceResponse<IEnumerable<ServiceCentreDTO>>
                {
                    Object = serviceCenters
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

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("ServiceCentreForHomeDelivery/{servicecentreId:int}")]
        public async Task<IServiceResponse<ServiceCentreDTO>> GetServiceCentreForHomeDelivery(int servicecentreId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var servicecentre = await _service.GetServiceCentreForHomeDelivery(servicecentreId);

                return new ServiceResponse<ServiceCentreDTO>
                {
                    Object = servicecentre
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpPost]
        [Route("ServiceCentresByCountryId")]
        public async Task<IServiceResponse<List<ServiceCentreDTO>>> GetServiceCentresByCountryId(CountryDTO countryDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var servicecentres = await _service.GetServiceCentresByCountryId(countryDTO.CountryId);

                return new ServiceResponse<List<ServiceCentreDTO>>
                {
                    Object = servicecentres
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("ServiceCentresByCountryId/{countryId:int}")]
        public async Task<IServiceResponse<List<ServiceCentreDTO>>> GetServiceCentresByCountryId(int countryId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var servicecentres = await _service.GetServiceCentresByCountryId(countryId);

                return new ServiceResponse<List<ServiceCentreDTO>>
                {
                    Object = servicecentres
                };
            });
        }

    }
}
