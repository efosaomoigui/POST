using GIGLS.Core.IServices;
using System.Threading.Tasks;
using GIGLS.Core.DTO;
using GIGLS.Services.Implementation;
using GIGLS.WebApi.Filters;
using System.Web.Http;
using GIGLS.Core.DTO.Captains;
using System;

namespace GIGLS.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/captain")]
    public class CaptainController : BaseWebApiController
    {

        private readonly ICaptainService _captainService;

        public CaptainController(ICaptainService captainService) : base(nameof(CaptainController))
        {
            _captainService = captainService;
        }

        // GET: Captain
        //public ActionResult Index()
        //{
        //    return View();
        //}

        //[GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("register")]
        public async Task<IServiceResponse<object>> RegisterCaptain(CaptainDTO captainDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _captainService.RegisterCaptainAsync(captainDTO);

                return new ServiceResponse<object>
                {
                    Object = result
                };
            });
        }

        //[GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{date}")]
        public async Task<IServiceResponse<object>> GetAllCaptainsByDate(DateTime date)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _captainService.GetCaptainsByDateAsync(date);

                return new ServiceResponse<object>
                {
                    Object = result
                };
            });
        }

        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<object>> GetAllCaptainsByDate()
        {
            
            return await HandleApiOperationAsync(async () =>
            {
                DateTime? date = null;
                var result = await _captainService.GetCaptainsByDateAsync(date);

                return new ServiceResponse<object>
                {
                    Object = result
                };
            });
        }

        [HttpGet]
        [Route("captainid/{captainId}")]
        public async Task<IServiceResponse<object>> GetCaptainById(int captainId)
        {

            return await HandleApiOperationAsync(async () =>
            {
                var result = await _captainService.GetCaptainByIdAsync(captainId);

                return new ServiceResponse<object>
                {
                    Object = result
                };
            });
        }

        [HttpDelete]
        [Route("{captainId}")]
        public async Task<IServiceResponse<bool>> DeleteCaptainById(int captainId)
        {

            return await HandleApiOperationAsync(async () =>
            {
                await _captainService.DeleteCaptainByIdAsync(captainId);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }
    }
}