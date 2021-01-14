using GIGLS.Core.DTO.InternationalShipmentDetails;
using GIGLS.Core.IServices;
using GIGLS.Core.IServices.InternationalRequest;
using GIGLS.Services.Implementation;
using GIGLS.WebApi.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace GIGLS.WebApi.Controllers.InternationalRequest
{
    [Authorize(Roles = "ViewAdmin")]
    [RoutePrefix("api/internationalrequest")]
    public class InternationalRequestController : BaseWebApiController
    {
        private IInternationalRequestReceiverService _service;
        public InternationalRequestController(IInternationalRequestReceiverService service) : base(nameof(InternationalRequestController))
        {
            _service = service;
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<IEnumerable<InternationalRequestReceiverDTO>>> GetInternationalRequestReceiver()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var receiver = await _service.GetInternationalRequestReceiver();
                return new ServiceResponse<IEnumerable<InternationalRequestReceiverDTO>>
                {
                    Object = receiver
                };

            });
        }


        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{receiverId:int}")]
        public async Task<IServiceResponse<InternationalRequestReceiverDTO>> GetInternationalRequestReceiverById(int receiverId)
        {
            return await HandleApiOperationAsync(async () =>
           {
               var receiver = await _service.GetInternationalRequestReceiverById(receiverId);

               return new ServiceResponse<InternationalRequestReceiverDTO>
               {
                   Object = receiver
               };
           });
        }


        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("code/{code}")]
        public async Task<IServiceResponse<IEnumerable<InternationalRequestReceiverDTO>>> GetInternationalRequestReceiver(string code)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var receiver = await _service.GetInternationalRequestReceiverByCode(code);
                return new ServiceResponse<IEnumerable<InternationalRequestReceiverDTO>>
                {
                    Object = receiver
                };

            });
        }


        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<object>> AddGetInternationalRequestReceiver(InternationalRequestReceiverDTO internationalRequestReceiver)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var receiver = await _service.AddInternationalRequestReceiver(internationalRequestReceiver);
                return new ServiceResponse<object>
                {
                    Object = receiver
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{receiverId:int}")]
        public async Task<IServiceResponse<object>> UpdateGetInternationalRequestReceiver(int receiverId, InternationalRequestReceiverDTO internationalRequestReceiver)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _service.UpdateInternationalRequestReceiver(receiverId, internationalRequestReceiver);
                return new ServiceResponse<object>
                {
                    Object = true

                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpDelete]
        [Route("{receiverId:int}")]
        public async Task<IServiceResponse<bool>> DeleteInternationalRequestReceiver(int receiverId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _service.DeleteInternationalRequestReceiver(receiverId);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }


    }
}
