using GIGLS.Core.IServices;
using GIGLS.Core.DTO.Partnership;
using GIGLS.Core.IServices.Partnership;
using GIGLS.Services.Implementation;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using GIGLS.WebApi.Filters;
using GIGLS.Core.DTO.Report;

namespace GIGLS.WebApi.Controllers.Partnership
{
    [Authorize(Roles = "Admin, ViewAdmin")]
    [RoutePrefix("api/partner")]
    public class PartnerController : BaseWebApiController
    {
        private readonly IPartnerService _partnerService;
        private readonly IFleetPartnerService _fleetPartnerService;
        private readonly IPartnerTransactionsService _partnerTransactionsService;

        public PartnerController(IPartnerService partnerService, IFleetPartnerService fleetPartnerService, IPartnerTransactionsService partnerTransactionsService) :base(nameof(PartnerController))
        {
            _partnerService = partnerService;
            _fleetPartnerService = fleetPartnerService;
            _partnerTransactionsService = partnerTransactionsService;
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


        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpPost]
        [Route("getverifiedpartners")]
        public async Task<IServiceResponse<IEnumerable<VehicleTypeDTO>>> GetVerfiedPartners(FleetPartnerDTO fleetCode)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var partners = await _partnerService.GetVerifiedPartners(fleetCode.FleetPartnerCode);
                return new ServiceResponse<IEnumerable<VehicleTypeDTO>>
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

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("getpartner/{partnercode}")]
        public async Task<IServiceResponse<PartnerDTO>> GetPartnerByCode(string partnercode)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var state = await _partnerService.GetPartnerByCode(partnercode);

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

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("partnersunderfleet/{fleetCode}")]
        public async Task<IServiceResponse<IEnumerable<VehicleTypeDTO>>> GetAllPartnersUnderFleetPartner(string fleetCode)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var partners = await _fleetPartnerService.GetVehiclesAttachedToFleetPartner(fleetCode);
                return new ServiceResponse<IEnumerable<VehicleTypeDTO>>
                {
                    Object = partners
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpGet]
        [Route("removepartnerfromfleet/{partnerCode}")]
        public async Task<IServiceResponse<bool>> RemovePartnerFromFleet(string partnerCode)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _fleetPartnerService.RemovePartnerFromFleetPartner(partnerCode);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("getallpartnerswithoutenterprise")]
        public async Task<IServiceResponse<IEnumerable<PartnerDTO>>> GetExternalPartnersNotAttachedToAnyFleetPartner()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var partners = await _fleetPartnerService.GetExternalPartnersNotAttachedToAnyFleetPartner();
                return new ServiceResponse<IEnumerable<PartnerDTO>>
                {
                    Object = partners
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpPost]
        [Route("externalpartnerstransactions")]
        public async Task<IServiceResponse<IEnumerable<ExternalPartnerTransactionsPaymentDTO>>> GetExternalPartnerTransactionsForPayment(ShipmentCollectionFilterCriteria shipmentCollectionFilter)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var partners = await _partnerService.GetExternalPartnerTransactionsForPayment(shipmentCollectionFilter);
                return new ServiceResponse<IEnumerable<ExternalPartnerTransactionsPaymentDTO>>
                {
                    Object = partners
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpPost]
        [Route("processpartnertransactions")]
        public async Task<IServiceResponse<bool>> ProcessPartnerTransactions(List<ExternalPartnerTransactionsPaymentDTO> externalPartnerTransactionsDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _partnerTransactionsService.ProcessPartnerTransactions(externalPartnerTransactionsDTO);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpPost]
        [Route("getpartnerpayouts")]
        public async Task<IServiceResponse<IEnumerable<PartnerPayoutDTO>>> GetPartnerPayouts(ShipmentCollectionFilterCriteria filterCriteria)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var payout = await _partnerTransactionsService.GetPartnersPayout(filterCriteria);
                return new ServiceResponse<IEnumerable<PartnerPayoutDTO>>
                {
                    Object = payout
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("creditpartner")]
        public async Task<IServiceResponse<bool>> CreditPartner(CreditPartnerTransactionsDTO transactionsDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _partnerTransactionsService.CreditPartnerTransactionByAdmin(transactionsDTO);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpPost]
        [Route("getunverifiedpartners")]
        public async Task<IServiceResponse<IEnumerable<VehicleTypeDTO>>> GetUnVerfiedPartners(ShipmentCollectionFilterCriteria filterCriteria)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var partners = await _partnerService.GetUnVerifiedPartners(filterCriteria);
                return new ServiceResponse<IEnumerable<VehicleTypeDTO>>
                {
                    Object = partners
                };
            });
        }

    }
}
