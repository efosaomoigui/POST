using GIGLS.Core.DTO;
using GIGLS.Core.DTO.Stores;
using GIGLS.Core.IServices;
using GIGLS.Core.IServices.Stores;
using GIGLS.Services.Implementation;
using GIGLS.WebApi.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace GIGLS.WebApi.Controllers
{
    [Authorize(Roles = "Admin, ViewAdmin")]
    [RoutePrefix("api/store")]
    public class StoreController : BaseWebApiController
    {
        private readonly IStoreService _storeService;
        public StoreController(IStoreService storeService) : base(nameof(StoreController))
        {
            _storeService = storeService;
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<IEnumerable<StoreDTO>>> GetStores()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var store = await _storeService.GetStores();

                return new ServiceResponse<IEnumerable<StoreDTO>>
                {
                    Object = store
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("")]
        public async Task<IServiceResponse<object>> AddStore(StoreDTO storeDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var store = await _storeService.AddStore(storeDTO);

                return new ServiceResponse<object>
                {
                    Object = store
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{storeId:int}")]
        public async Task<IServiceResponse<StoreDTO>> GetStoreById(int storeId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var store = await _storeService.GetStoreById(storeId);

                return new ServiceResponse<StoreDTO>
                {
                    Object = store
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpDelete]
        [Route("{storeId:int}")]
        public async Task<IServiceResponse<bool>> DeleteStore(int storeId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _storeService.DeleteStore(storeId);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{storeId:int}")]
        public async Task<IServiceResponse<bool>> UpdateStore(int storeId, StoreDTO storeDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _storeService.UpdateStore(storeId, storeDTO);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

    }
}
