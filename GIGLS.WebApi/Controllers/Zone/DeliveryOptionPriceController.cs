using GIGLS.Core.IServices;
using GIGLS.Core.DTO.Zone;
using GIGLS.Core.IServices.Zone;
using GIGLS.Services.Implementation;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using GIGLS.WebApi.Filters;

namespace GIGLS.WebApi.Controllers.Zone
{
    [Authorize(Roles = "Admin, ViewAdmin")]
    [RoutePrefix("api/deliveryoptionprice")]
    public class DeliveryOptionPriceController : BaseWebApiController
    {
        private readonly IDeliveryOptionPriceService _service;

        public DeliveryOptionPriceController(IDeliveryOptionPriceService service) : base(nameof(DeliveryOptionPriceController))
        {
            _service = service;
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<IEnumerable<DeliveryOptionPriceDTO>>> GetDeliveryOptionPrices()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var delivery = await _service.GetDeliveryOptionPrices();

                return new ServiceResponse<IEnumerable<DeliveryOptionPriceDTO>>
                {
                    Object = delivery
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("")]
        public async Task<IServiceResponse<object>> AddDeliveryOptionPrice(DeliveryOptionPriceDTO optionDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var option = await _service.AddDeliveryOptionPrice(optionDto);

                return new ServiceResponse<object>
                {
                    Object = option
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{optionId:int}")]
        public async Task<IServiceResponse<DeliveryOptionPriceDTO>> GetDeliveryOptionPrice(int optionId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var option = await _service.GetDeliveryOptionPriceById(optionId);
                return new ServiceResponse<DeliveryOptionPriceDTO>
                {
                    Object = option
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpDelete]
        [Route("{optionId:int}")]
        public async Task<IServiceResponse<bool>> DeleteDeliveryOptionPrice(int optionId)
        {
            return await HandleApiOperationAsync(async () => {

                await _service.DeleteDeliveryOptionPrice(optionId);
                return new ServiceResponse<bool>
                {
                    Object = true
                };

            });
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{optionId:int}")]
        public async Task<IServiceResponse<bool>> UpdateDeliveryOptionPrice(int optionId, DeliveryOptionPriceDTO optionDto)
        {
            return await HandleApiOperationAsync(async () => {
                await _service.UpdateDeliveryOptionPrice(optionId, optionDto);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

    }
}
