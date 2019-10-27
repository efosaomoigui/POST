using GIGLS.Core.DTO;
using GIGLS.Core.IServices;
using GIGLS.Services.Implementation;
using GIGLS.WebApi.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace GIGLS.WebApi.Controllers
{
    [Authorize(Roles = "Shipment, ViewAdmin")]
    [RoutePrefix("api/deliverylocation")]
    public class DeliveryLocationController : BaseWebApiController
    {
        private readonly IDeliveryLocationService _deliveryLocationService;
        public DeliveryLocationController(IDeliveryLocationService deliveryLocationService) : base(nameof(DeliveryLocationController))
        {
            _deliveryLocationService = deliveryLocationService;
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<IEnumerable<DeliveryLocationDTO>>> GetDeliveryLocations()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var locations = await _deliveryLocationService.GetDeliveryLocations();

                return new ServiceResponse<IEnumerable<DeliveryLocationDTO>>
                {
                    Object = locations
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("")]
        public async Task<IServiceResponse<object>> AddDeliveryLocations(DeliveryLocationDTO deliveryLocationDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var deliveryLocation = await _deliveryLocationService.AddDeliveryLocationPrice(deliveryLocationDTO);

                return new ServiceResponse<object>
                {
                    Object = deliveryLocation
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{deliveryLocationPriceId:int}")]
        public async Task<IServiceResponse<DeliveryLocationDTO>> GetDeliveryLocationId(int deliveryLocationPriceId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var delivery = await _deliveryLocationService.GetDeliveryLocationById(deliveryLocationPriceId);

                return new ServiceResponse<DeliveryLocationDTO>
                {
                    Object = delivery
                };
            });
        }


        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{deliveryLocationPriceId:int}")]
        public async Task<IServiceResponse<object>> UpdateDeliveryLocationPrice(int deliveryLocationPriceId, DeliveryLocationDTO deliveryLocationDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _deliveryLocationService.UpdateDeliveryLocationPrice(deliveryLocationPriceId, deliveryLocationDTO);
                return new ServiceResponse<object>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpDelete]
        [Route("{deliveryLocationPriceId:int}")]
        public async Task<IServiceResponse<bool>> DeleteDeliveryLocationPrice(int deliveryLocationPriceId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _deliveryLocationService.RemoveDeliveryLocationPrice(deliveryLocationPriceId);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }


    }
}