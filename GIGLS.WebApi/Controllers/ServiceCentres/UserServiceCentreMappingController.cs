using GIGLS.Core.IServices;
using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.Core.IServices.ServiceCentres;
using GIGLS.Services.Implementation;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using GIGLS.Core.DTO.User;
using GIGLS.WebApi.Filters;

namespace GIGLS.WebApi.Controllers.ServiceCentres
{
    [Authorize(Roles = "Admin")]
    [RoutePrefix("api/usermapping")]
    public class UserServiceCentreMappingController : BaseWebApiController
    {
        private readonly IUserServiceCentreMappingService _service;
        public UserServiceCentreMappingController(IUserServiceCentreMappingService service) : base(nameof(UserServiceCentreMappingController))
        {
            _service = service;
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("")]
        public async Task<IServiceResponse<bool>> MappingUserToServiceCentre(string userid, int serviceCentreId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _service.MappingUserToServiceCentre(userid, serviceCentreId);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<List<UserDTO>>> GetUsersInServiceCentre()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var users = await _service.GetUsersInServiceCentre();

                return new ServiceResponse<List<UserDTO>>
                {
                    Object = users
                };
            });
        }
        
        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{userid}/active")]
        public async Task<IServiceResponse<ServiceCentreDTO>> GetUserActiveServiceCentre(string userid)
        {
            return await HandleApiOperationAsync(async () =>
            {

                var centre = await _service.GetUserActiveServiceCentre(userid);

                return new ServiceResponse<ServiceCentreDTO>
                {
                    Object = centre
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{userid}")]
        public async Task<IServiceResponse<List<ServiceCentreDTO>>> GetUserServiceCentre(string userid)
        {
            return await HandleApiOperationAsync(async () =>
            {

                var centre = await _service.GetUserServiceCentres(userid);

                return new ServiceResponse<List<ServiceCentreDTO>>
                {
                    Object = centre
                };
            });
        }
        
        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{serviceCentreId:int}/active")]
        public async Task<IServiceResponse<List<UserDTO>>> GetServiceCentreActiveUsers(int serviceCentreId)
        {
            return await HandleApiOperationAsync(async () =>
            {

                var centre = await _service.GetActiveUsersMapToServiceCentre(serviceCentreId);

                return new ServiceResponse<List<UserDTO>>
                {
                    Object = centre
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{serviceCentreId:int}")]
        public async Task<IServiceResponse<List<UserDTO>>> GetServiceCentreUsers(int serviceCentreId)
        {
            return await HandleApiOperationAsync(async () =>
            {

                var centre = await _service.GetUsersMapToServiceCentre(serviceCentreId);

                return new ServiceResponse<List<UserDTO>>
                {
                    Object = centre
                };
            });
        }

    }
}
