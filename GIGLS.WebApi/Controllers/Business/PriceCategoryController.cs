using GIGLS.Core.DTO;
using GIGLS.Core.DTO.PaymentTransactions;
using GIGLS.Core.IServices;
using GIGLS.Services.Implementation;
using GIGLS.WebApi.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace GIGLS.WebApi.Controllers
{
    [Authorize(Roles = "Admin, ViewAdmin")]
    [RoutePrefix("api/pricecategory")]
    public class PriceCategoryController : BaseWebApiController
    {
        private readonly IPriceCategoryService _priceCategoryService;
        public PriceCategoryController(IPriceCategoryService priceCategoryService) : base(nameof(PriceCategoryController))
        {
            _priceCategoryService = priceCategoryService;
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<IEnumerable<PriceCategoryDTO>>> GetPriceCategories()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var priceCategory = await _priceCategoryService.GetPriceCategorys();

                return new ServiceResponse<IEnumerable<PriceCategoryDTO>>
                {
                    Object = priceCategory
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("")]
        public async Task<IServiceResponse<object>> AddPriceCategory(PriceCategoryDTO priceCategoryDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var priceCategory = await _priceCategoryService.AddPriceCategory(priceCategoryDTO);

                return new ServiceResponse<object>
                {
                    Object = priceCategory
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{priceCategoryId:int}")]
        public async Task<IServiceResponse<PriceCategoryDTO>> GetPriceCategoryById(int priceCategoryId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var priceCategory = await _priceCategoryService.GetPriceCategoryById(priceCategoryId);

                return new ServiceResponse<PriceCategoryDTO>
                {
                    Object = priceCategory
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpDelete]
        [Route("{priceCategoryId:int}")]
        public async Task<IServiceResponse<bool>> DeletePriceCategory(int priceCategoryId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _priceCategoryService.DeletePriceCategory(priceCategoryId);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{priceCategoryId:int}")]
        public async Task<IServiceResponse<bool>> UpdatePriceCategory(int priceCategoryId, PriceCategoryDTO priceCategoryDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _priceCategoryService.UpdatePriceCategory(priceCategoryId, priceCategoryDTO);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

    }
}
