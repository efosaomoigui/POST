using GIGLS.Core.DTO.Haulage;
using GIGLS.Core.IServices;
using GIGLS.Core.IServices.Haulage;
using GIGLS.Services.Implementation;
using GIGLS.WebApi.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace GIGLS.WebApi.Controllers.PackingList
{
    [Authorize(Roles = "SuperAdmin,SubAdmin,Shipment,Account,Report")]
    [RoutePrefix("api/packinglist")]
    public class PackingListController : BaseWebApiController
    {
        private readonly IPackingListService _packingListService;

        public PackingListController(IPackingListService packingListService) : base(nameof(PackingListController))
        {
            _packingListService = packingListService;
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<IEnumerable<PackingListDTO>>> GetPackingLists()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var packingLists = await _packingListService.GetPackingLists();

                return new ServiceResponse<IEnumerable<PackingListDTO>>
                {
                    Object = packingLists
                };
            });
        }
        
        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("")]
        public async Task<IServiceResponse<object>> AddPackingList(PackingListDTO packingListDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var packingList = await _packingListService.AddPackingList(packingListDto);

                return new ServiceResponse<object>
                {
                    Object = packingList
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{packingListId:int}")]
        public async Task<IServiceResponse<PackingListDTO>> GetPackingListById(int packingListId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var PackingList = await _packingListService.GetPackingListById(packingListId);

                return new ServiceResponse<PackingListDTO>
                {
                    Object = PackingList
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{waybill}/waybill")]
        public async Task<IServiceResponse<PackingListDTO>> GetPackingListByWaybill(string waybill)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var PackingList = await _packingListService.GetPackingListByWaybill(waybill);

                return new ServiceResponse<PackingListDTO>
                {
                    Object = PackingList
                };
            });
        }


        [GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpDelete]
        [Route("{packingListId:int}")]
        public async Task<IServiceResponse<bool>> DeletePackingList(int packingListId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _packingListService.RemovePackingList(packingListId);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{packingListId:int}")]
        public async Task<IServiceResponse<bool>> UpdatePackingList(int packingListId, PackingListDTO packingListDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _packingListService.UpdatePackingList(packingListId, packingListDto);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

    }
}
