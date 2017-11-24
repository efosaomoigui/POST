using GIGLS.Core.IServices;
using GIGLS.Core.DTO.Partnership;
using GIGLS.Core.IServices.Partnership;
using GIGLS.Services.Implementation;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using GIGLS.WebApi.Filters;

namespace GIGLS.WebApi.Controllers.Partnership
{
    [Authorize(Roles = "Admin,Shipment,Account,Report")]
    [RoutePrefix("api/partner")]
    public class PartnerController : BaseWebApiController
    {
        private readonly IPartnerService _partnerService;

        public PartnerController(IPartnerService partnerService) :base(nameof(PartnerController))
        {
            _partnerService = partnerService;
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<IEnumerable<PartnerDTO>>> GetPartners()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var partners = await _partnerService.GetPartners();
                return new ServiceResponse<IEnumerable<PartnerDTO>>
                {
                    Object = partners
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("")]
        public async Task<IServiceResponse<object>> AddPartner(PartnerDTO partnerDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var partner = await _partnerService.AddPartner(partnerDto);

                return new ServiceResponse<object>
                {
                    Object = partner
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{partnerId:int}")]
        public async Task<IServiceResponse<PartnerDTO>> GetPartner(int partnerId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var state = await _partnerService.GetPartnerById(partnerId);

                return new ServiceResponse<PartnerDTO>
                {
                    Object = state
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpDelete]
        [Route("{partnerId:int}")]
        public async Task<IServiceResponse<bool>> DeletePartner(int partnerId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _partnerService.RemovePartner(partnerId);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{partnerId:int}")]
        public async Task<IServiceResponse<bool>> UpdatePartner(int partnerId, PartnerDTO partnerDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _partnerService.UpdatePartner(partnerId, partnerDto);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

    }
}
