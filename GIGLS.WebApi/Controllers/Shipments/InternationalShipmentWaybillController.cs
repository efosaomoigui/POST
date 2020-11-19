using GIGLS.Core.DTO.DHL;
using GIGLS.Core.IServices;
using GIGLS.Core.IServices.Shipments;
using GIGLS.CORE.DTO.Report;
using GIGLS.Services.Implementation;
using GIGLS.WebApi.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace GIGLS.WebApi.Controllers.Shipments
{
    [Authorize(Roles = "Shipment, ViewAdmin")]
    [RoutePrefix("api/internationalwaybill")]
    public class InternationalShipmentWaybillController : BaseWebApiController
    {
        private readonly IInternationalShipmentWaybillService _service;

        public InternationalShipmentWaybillController(IInternationalShipmentWaybillService internationalShipmentWaybillService) : base(nameof(InternationalShipmentWaybillController))
        {
            _service = internationalShipmentWaybillService;
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpPost]
        [Route("")]
        public async Task<IServiceResponse<List<InternationalShipmentWaybillDTO>>> GetInternationalShipmentWaybills(DateFilterCriteria dateFilterCriteria)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var shipments = await _service.GetInternationalWaybills(dateFilterCriteria);
                return new ServiceResponse<List<InternationalShipmentWaybillDTO>>
                {
                    Object = shipments
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{waybill}")]
        public async Task<IServiceResponse<InternationalShipmentWaybillDTO>> GetInternationalShipmentWaybill(string waybill)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var shipment = await _service.GetInternationalWaybill(waybill);
                return new ServiceResponse<InternationalShipmentWaybillDTO>
                {
                    Object = shipment
                };
            });
        }
    }
}