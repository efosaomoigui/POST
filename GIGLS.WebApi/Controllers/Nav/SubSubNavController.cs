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
    [RoutePrefix("api/subsubnav")]
    public class SubSubNavController : BaseWebApiController
    {
        private readonly ISubSubNavService _subSubNavService;
        public SubSubNavController(ISubSubNavService subSubNavService):base(nameof(SubSubNavController))
        {
            _subSubNavService = subSubNavService;
        }

        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<IEnumerable<SubSubNavDTO>>> GetSubSubNavs()
        {
            return await HandleApiOperationAsync(async () =>
            {                
                var subSubNav = await _subSubNavService.GetSubSubNavs();

                return new ServiceResponse<IEnumerable<SubSubNavDTO>>
                {
                    Object = subSubNav
                };
            });
        }

        [HttpPost]
        [Route("")]
        public async Task<IServiceResponse<object>> AddSubSubNav(SubSubNavDTO SubSubNavDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var subSubNav = await _subSubNavService.AddSubSubNav(SubSubNavDto);

                return new ServiceResponse<object>
                {
                    Object = subSubNav
                };
            });
        }

        [HttpGet]
        [Route("{subSubNavId:int}")]
        public async Task<IServiceResponse<SubSubNavDTO>> GetSubSubNav(int subSubNavId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var SubSubNav = await _subSubNavService.GetSubSubNavById(subSubNavId);

                return new ServiceResponse<SubSubNavDTO>
                {
                    Object = SubSubNav
                };
            });
        }

        [HttpDelete]
        [Route("{subSubNavId:int}")]
        public async Task<IServiceResponse<bool>> DeleteSubSubNav(int subSubNavId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _subSubNavService.RemoveSubSubNav(subSubNavId);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [HttpPut]
        [Route("{subSubNavId:int}")]
        public async Task<IServiceResponse<bool>> UpdateSubSubNav(int subSubNavId, SubSubNavDTO SubSubNavDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _subSubNavService.UpdateSubSubNav(subSubNavId, SubSubNavDto);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }
        
    }
}
