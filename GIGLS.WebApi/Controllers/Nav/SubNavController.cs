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
    [RoutePrefix("api/subnav")]
    public class SubNavController : BaseWebApiController
    {
        private readonly ISubNavService _subNavService;
        public SubNavController(ISubNavService subNavService):base(nameof(SubNavController))
        {
            _subNavService = subNavService;
        }

        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<IEnumerable<SubNavDTO>>> GetSubNavs()
        {
            return await HandleApiOperationAsync(async () =>
            {                
                var SubNav = await _subNavService.GetSubNavs();

                return new ServiceResponse<IEnumerable<SubNavDTO>>
                {
                    Object = SubNav
                };
            });
        }

        [HttpPost]
        [Route("")]
        public async Task<IServiceResponse<object>> AddSubNav(SubNavDTO subNavDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var SubNav = await _subNavService.AddSubNav(subNavDto);

                return new ServiceResponse<object>
                {
                    Object = SubNav
                };
            });
        }

        [HttpGet]
        [Route("{subNavId:int}")]
        public async Task<IServiceResponse<SubNavDTO>> GetSubNav(int subNavId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var SubNav = await _subNavService.GetSubNavById(subNavId);

                return new ServiceResponse<SubNavDTO>
                {
                    Object = SubNav
                };
            });
        }

        [HttpDelete]
        [Route("{subNavId:int}")]
        public async Task<IServiceResponse<bool>> DeleteSubNav(int subNavId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _subNavService.RemoveSubNav(subNavId);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [HttpPut]
        [Route("{subNavId:int}")]
        public async Task<IServiceResponse<bool>> UpdateSubNav(int subNavId, SubNavDTO subNavDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _subNavService.UpdateSubNav(subNavId, subNavDto);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }
        
    }
}
