using GIGLS.Core.IServices;
using GIGLS.Services.Implementation;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using GIGLS.CORE.IServices.Nav;
using GIGLS.CORE.DTO.Nav;

namespace GIGLS.WebApi.Controllers.Nav
{
    //[Authorize]
    [RoutePrefix("api/mainnav")]
    public class MainNavController : BaseWebApiController
    {
        private readonly IMainNavService _mainNavService;
        public MainNavController(IMainNavService mainNavService):base(nameof(MainNavController))
        {
            _mainNavService = mainNavService;
        }

        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<IEnumerable<MainNavDTO>>> GetMainNavs()
        {
            return await HandleApiOperationAsync(async () =>
            {
                
                var mainNav = await _mainNavService.GetMainNavs();

                return new ServiceResponse<IEnumerable<MainNavDTO>>
                {
                    Object = mainNav
                };
            });
        }

        [HttpPost]
        [Route("")]
        public async Task<IServiceResponse<object>> AddMainNav(MainNavDTO mainNavDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var mainNav = await _mainNavService.AddMainNav(mainNavDto);

                return new ServiceResponse<object>
                {
                    Object = mainNav
                };
            });
        }

        [HttpGet]
        [Route("{mainNavId:int}")]
        public async Task<IServiceResponse<MainNavDTO>> GetMainNav(int mainNavId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var mainNav = await _mainNavService.GetMainNavById(mainNavId);

                return new ServiceResponse<MainNavDTO>
                {
                    Object = mainNav
                };
            });
        }

        [HttpDelete]
        [Route("{mainNavId:int}")]
        public async Task<IServiceResponse<bool>> DeleteMainNav(int mainNavId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _mainNavService.RemoveMainNav(mainNavId);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [HttpPut]
        [Route("{mainNavId:int}")]
        public async Task<IServiceResponse<bool>> UpdateMainNav(int mainNavId, MainNavDTO mainNavDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _mainNavService.UpdateMainNav(mainNavId, mainNavDto);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }
        
    }
}
