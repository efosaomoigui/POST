using GIGLS.Core.DTO.Partnership;
using GIGLS.Core.DTO.Report;
using GIGLS.Core.IServices;
using GIGLS.Core.IServices.Partnership;
using GIGLS.Services.Implementation;
using GIGLS.WebApi.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace GIGLS.WebApi.Controllers.Partnership
{
    [RoutePrefix("api/fleetpartner")]
    public class FleetPartnerController : BaseWebApiController
    {
        private readonly IFleetPartnerService _fleetPartnerService;
        public FleetPartnerController(IFleetPartnerService fleetPartnerService) : base(nameof(FleetPartnerController))
        {
            _fleetPartnerService = fleetPartnerService;
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("")]
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
        [Route("{partnerId:int}")]
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
        [Route("{partnerId:int}")]
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
        [Route("{partnerId:int}")]
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

        
        [HttpGet]
        [Route("getcountofpartnersattachedtofleet")]
        public async Task<IServiceResponse<int>> GetCountOfPartnersUnderFleet()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var partners = await _fleetPartnerService.CountOfPartnersUnderFleet();

                return new ServiceResponse<int>
                {
                    Object = partners
                };
            });
        }

        
        [HttpGet]
        [Route("getvehiclesinfleet")]
        public async Task<IServiceResponse<IEnumerable<VehicleTypeDTO>>> GetVehiclesAttachedToFleetPartner()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var partners = await _fleetPartnerService.GetVehiclesAttachedToFleetPartner();
                return new ServiceResponse<IEnumerable<VehicleTypeDTO>>
                {
                    Object = partners
                };
            });
        }

        [HttpPost]
        [Route("transaction")]
        public async Task<IServiceResponse<List<PartnerTransactionsDTO>>> GetFleetTransactions(ShipmentCollectionFilterCriteria filterCriteria)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var transactions = await _fleetPartnerService.GetFleetTransaction(filterCriteria);

                return new ServiceResponse<List<PartnerTransactionsDTO>>
                {
                    Object = transactions
                };
            });
        }



    }
}