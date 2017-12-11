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
    [Authorize(Roles = "SuperAdmin,SubAdmin,Shipment,Account,Report")]
    public class PartnerApplicationController : BaseWebApiController
    {
        private readonly IPartnerApplicationService _partnerApplicationService;

        public PartnerApplicationController(IPartnerApplicationService partnerApplicationService) :base(nameof(PartnerApplicationController))
        {
            _partnerApplicationService = partnerApplicationService;
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<IEnumerable<PartnerApplicationDTO>>> GetPartnerApplications()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var partners = await _partnerApplicationService.GetPartnerApplications();

                return new ServiceResponse<IEnumerable<PartnerApplicationDTO>>
                {
                    Object = partners
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("")]
        public async Task<IServiceResponse<object>> AddPartnerApplication(PartnerApplicationDTO partnerDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var partner = await _partnerApplicationService.AddPartnerApplication(partnerDto);

                return new ServiceResponse<object>
                {
                    Object = partner
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{partnerId:int}")]
        public async Task<IServiceResponse<PartnerApplicationDTO>> GetPartnerApplication(int partnerId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var state = await _partnerApplicationService.GetPartnerApplicationById(partnerId);

                return new ServiceResponse<PartnerApplicationDTO>
                {
                    Object = state
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpDelete]
        [Route("{partnerId:int}")]
        public async Task<IServiceResponse<bool>> DeletePartnerApplication(int partnerId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _partnerApplicationService.RemovePartnerApplication(partnerId);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{partnerId:int}")]
        public async Task<IServiceResponse<bool>> UpdatePartnerApplication(int partnerId, PartnerApplicationDTO partnerDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _partnerApplicationService.UpdatePartnerApplication(partnerId, partnerDto);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

    }
}
