using GIGLS.Core.DTO.Account;
using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.DTO.Zone;
using GIGLS.Core.IServices;
using GIGLS.Core.IServices.Shipments;
using GIGLS.CORE.DTO.Report;
using GIGLS.CORE.DTO.Shipments;
using GIGLS.Services.Implementation;
using GIGLS.WebApi.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using GIGLS.CORE.IServices.Report;
using GIGLS.Core.Enums;
using GIGLS.Core.IServices.User;

namespace GIGLS.WebApi.Controllers.Shipments
{
    [Authorize(Roles = "Shipment, ViewAdmin")]
    [RoutePrefix("api/shipment")]
    public class ShipmentDeliveryMonitorController : BaseWebApiController 
    {
        private readonly IShipmentService _service;
        private readonly IShipmentReportService _reportService;
        private readonly IUserService _userService;

        public ShipmentDeliveryMonitorController(IShipmentService service, IShipmentReportService reportService,
            IUserService userService) : base(nameof(ShipmentController))
        {
            _service = service;
            _reportService = reportService;
            _userService = userService;
        }


        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<IEnumerable<ShipmentDTO>>> getShipmentCountForDeliveryInGreen([FromUri]FilterOptionsDto filterOptionsDto)
        {
            //filter by User Active Country
            var userActiveCountry = await _userService.GetUserActiveCountry();
            filterOptionsDto.CountryId = userActiveCountry?.CountryId;


            return await HandleApiOperationAsync(async () =>
            {
                var shipments = _service.GetShipments(filterOptionsDto);
                return new ServiceResponse<IEnumerable<ShipmentDTO>>
                {
                    Object = await shipments.Item1,
                    Total = shipments.Item2
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("incomingshipments")]
        public async Task<IServiceResponse<IEnumerable<InvoiceViewDTO>>> getShipmentCountForDeliveryInBlue([FromUri]FilterOptionsDto filterOptionsDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var shipments = await _service.GetIncomingShipments(filterOptionsDto);
                return new ServiceResponse<IEnumerable<InvoiceViewDTO>>
                {
                    Object = shipments,
                    Total = shipments.Count
                };
            });
        }


        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<IEnumerable<ShipmentDTO>>> getShipmentCountForDeliveryInRed([FromUri]FilterOptionsDto filterOptionsDto)
        {
            //filter by User Active Country
            var userActiveCountry = await _userService.GetUserActiveCountry();
            filterOptionsDto.CountryId = userActiveCountry?.CountryId;


            return await HandleApiOperationAsync(async () =>
            {
                var shipments = _service.GetShipments(filterOptionsDto);
                return new ServiceResponse<IEnumerable<ShipmentDTO>>
                {
                    Object = await shipments.Item1,
                    Total = shipments.Item2
                };
            });
        }

    }
}
