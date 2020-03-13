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
    [Authorize(Roles = "Admin, ViewAdmin")]
    [RoutePrefix("api/partner")]
    public class PartnerController : BaseWebApiController
    {
        private readonly IPartnerService _partnerService;
        private readonly IFleetPartnerService _fleetPartnerService;

        public PartnerController(IPartnerService partnerService, IFleetPartnerService fleetPartnerService) :base(nameof(PartnerController))
        {
            _partnerService = partnerService;
            _fleetPartnerService = fleetPartnerService;
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

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("getexternaldeliverypartners")]
        public async Task<IServiceResponse<IEnumerable<PartnerDTO>>> GetExternalDeliveryPartners()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var partners = await _partnerService.GetExternalDeliveryPartners();
                return new ServiceResponse<IEnumerable<PartnerDTO>>
                {
                    Object = partners
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("addfleetpartner")]
        public async Task<IServiceResponse<object>> AddFleetPartner(FleetPartnerDTO fleetPartnerDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var partner = await _fleetPartnerService.AddFleetPartner(fleetPartnerDTO);

                return new ServiceResponse<object>
                {
                    Object = partner
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("getfleetpartner/{partnerId:int}")]
        public async Task<IServiceResponse<FleetPartnerDTO>> GetFleetPartner(int fleetPartnerId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var partner = await _fleetPartnerService.GetFleetPartnerById(fleetPartnerId);

                return new ServiceResponse<FleetPartnerDTO>
                {
                    Object = partner
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpDelete]
        [Route("deletefleetpartner/{partnerId:int}")]
        public async Task<IServiceResponse<bool>> DeleteFleetPartner(int fleetPartnerId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _fleetPartnerService.RemoveFleetPartner(fleetPartnerId);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("updatefleetpartner/{partnerId:int}")]
        public async Task<IServiceResponse<bool>> UpdateFleetPartner(int fleetPartnerId, FleetPartnerDTO fleetPartnerDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _fleetPartnerService.UpdateFleetPartner(fleetPartnerId, fleetPartnerDTO);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("getallfleetpartners")]
        public async Task<IServiceResponse<IEnumerable<FleetPartnerDTO>>> GetAllFleetPartners()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var partners = await _fleetPartnerService.GetFleetPartners();
                return new ServiceResponse<IEnumerable<FleetPartnerDTO>>
                {
                    Object = partners
                };
            });
        }
    }
}
