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
    [Authorize(Roles = "SuperAdmin,SubAdmin,Shipment,Account,Report")]
    [RoutePrefix("api/deliveryoption")]
    public class DeliveryOptionController : BaseWebApiController
    {
        private readonly IDeliveryOptionService _service;

        public DeliveryOptionController(IDeliveryOptionService service):base (nameof(DeliveryOptionController))
        {
            _service = service;
        }

        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<IEnumerable<DeliveryOptionDTO>>> GetDeliveryOptions()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var delivery = await _service.GetDeliveryOptions();

                return new ServiceResponse<IEnumerable<DeliveryOptionDTO>>
                {
                    Object = delivery
                };
            });         
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("active")]
        public async Task<IServiceResponse<IEnumerable<DeliveryOptionDTO>>> GetActiveDeliveryOptions()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var delivery = await _service.GetActiveDeliveryOptions();

                return new ServiceResponse<IEnumerable<DeliveryOptionDTO>>
                {
                    Object = delivery
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("")]
        public async Task<IServiceResponse<object>> AddDeliveryOption(DeliveryOptionDTO optionDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var option = await _service.AddDeliveryOption(optionDto);

                return new ServiceResponse<object>
                {
                    Object = option
                };
            });           
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{optionId:int}")]
        public async Task<IServiceResponse<DeliveryOptionDTO>> GetDeliveryOption(int optionId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var option = await _service.GetDeliveryOptionById(optionId);
                return new ServiceResponse<DeliveryOptionDTO>
                {
                    Object = option
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpDelete]
        [Route("{optionId:int}")]
        public async Task<IServiceResponse<bool>> DeleteDeliveryOption(int optionId)
        {
            return await HandleApiOperationAsync(async () => {

                await _service.DeleteDeliveryOption(optionId);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
                
            });
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{optionId:int}")]
        public async Task<IServiceResponse<bool>> UpdateDeliveryOption(int optionId, DeliveryOptionDTO optionDto)
        {
            return await HandleApiOperationAsync(async () => {
                await _service.UpdateDeliveryOption(optionId, optionDto);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{optionId:int}/status/{status}")]
        public async Task<IServiceResponse<bool>> UpdateDeliveryOption(int optionId, bool status)
        {
            return await HandleApiOperationAsync(async () => {
                await _service.UpdateStatusDeliveryOption(optionId, status);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

    }
}
