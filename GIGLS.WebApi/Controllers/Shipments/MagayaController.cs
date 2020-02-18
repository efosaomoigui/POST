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
using System;
using System.Web.Http.Results;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace GIGLS.WebApi.Controllers.Shipments
{
    [Authorize(Roles = "Shipment, ViewAdmin")]
    [RoutePrefix("api/shipment/magaya")]
    public class MagayaController : BaseWebApiController
    {
        private readonly IMagayaService _service;
        private readonly IShipmentReportService _reportService;
        private readonly IUserService _userService;

        public MagayaController(IMagayaService service, IUserService userService) : base(nameof(ShipmentController)) 
        {
            _service = service;
            _userService = userService;
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<IEnumerable<ShipmentDTO>>> GetShipments([FromUri]FilterOptionsDto filterOptionsDto)
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
        public async Task<IServiceResponse<IEnumerable<InvoiceViewDTO>>> GetIncomingShipments([FromUri]FilterOptionsDto filterOptionsDto)
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

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("")]
        public async Task<IServiceResponse<string>> AddShipment(MagayaShipmentDTO MagayaShipmentDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {

                //1. Get the XML from XSD on Local file and fill in the object
                //String path = HttpContext.Current.Server.MapPath("~/GIGLS.Services/Business/Magaya/shipment.cs");
                HostingEnvironment.MapPath("~/GIGLS.Services/Business/Magaya/shipment.cs");


                //2. Call the Magaya SetTransaction Method from MagayaService


                //3. Pass the return to the view or caller


                //Update SenderAddress for corporate customers
                return new ServiceResponse<ShipmentDTO>
                {
                    Object = shipment
                };
            });
        }

    }
}
