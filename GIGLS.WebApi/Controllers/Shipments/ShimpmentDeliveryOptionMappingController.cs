using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.IServices;
using GIGLS.Core.IServices.Shipments;
using GIGLS.Services.Implementation;
using GIGLS.WebApi.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace GIGLS.WebApi.Controllers.Shipments
{
    [Authorize(Roles = "Admin, ViewAdmin")]
    [RoutePrefix("api/shimpmentdeliveryoptionmapping")]
    public class ShimpmentDeliveryOptionMappingController : BaseWebApiController
    {
        private readonly IShimpmentDeliveryOptionMappingService _service;

        public ShimpmentDeliveryOptionMappingController(IShimpmentDeliveryOptionMappingService service) : base(nameof(ShimpmentDeliveryOptionMappingController))
        {
            _service = service;
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<List<ShimpmentDeliveryOptionMappingDTO>>> GetShimpmentDeliveryOptionMappings()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var mappings = await _service.GetAllShimpmentDeliveryOptionMappings();
                return new ServiceResponse<List<ShimpmentDeliveryOptionMappingDTO>>
                {
                    Object = mappings
                };
            });
        }


        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{waybill}")]
        public async Task<IServiceResponse<List<ShimpmentDeliveryOptionMappingDTO>>> GetShimpmentDeliveryOptionMappings(string waybill)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var mappings = await _service.GetAllShimpmentDeliveryOptionMappings();
                return new ServiceResponse<List<ShimpmentDeliveryOptionMappingDTO>>
                {
                    Object = mappings
                };
            });
        }

       // [GIGLSActivityAuthorize(Activity = "Create")]
        //[HttpPost]
        //[Route("")]
        //public async Task<IServiceResponse<object>> AddMapping(ShimpmentDeliveryOptionMappingDTO newMapping)
        //{
        //    return await HandleApiOperationAsync(async () =>
        //    {
        //        var mapping = await _service.AddShimpmentDeliveryOptionMapping(newMapping);
        //        return new ServiceResponse<object>
        //        {
        //            Object = newMapping
        //        };
        //    });
        //}

    }
}
