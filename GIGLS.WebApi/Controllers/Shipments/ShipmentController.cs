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

namespace GIGLS.WebApi.Controllers.Shipments
{
    [Authorize(Roles = "Shipment, ViewAdmin")]
    [RoutePrefix("api/shipment")]
    public class ShipmentController : BaseWebApiController
    {
        private readonly IShipmentService _service;
        private readonly IShipmentReportService _reportService;

        public ShipmentController(IShipmentService service, IShipmentReportService reportService) : base(nameof(ShipmentController))
        {
            _service = service;
            _reportService = reportService;
        }


        //public Task<IEnumerable<StateDTO>> GetStatesAsync(int pageSize = 10, int page = 1)
        //{
        //    var states = Context.State.ToList();
        //    var subresult = states.Skip(pageSize * (page - 1)).Take(pageSize);
        //    var stateDto = Mapper.Map<IEnumerable<StateDTO>>(subresult);
        //    return Task.FromResult(stateDto);
        //}

        //[HttpGet]
        //[Route("all")]
        //public async Task<IServiceResponse<IEnumerable<ShipmentDTO>>> GetShipments()
        //{
        //    return await HandleApiOperationAsync(async () =>
        //    {
        //        var shipments = await _service.GetShipments();
        //        return new ServiceResponse<IEnumerable<ShipmentDTO>>
        //        {
        //            Object = shipments
        //        };
        //    });
        //}

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("")]
        public async Task<IServiceResponse<IEnumerable<ShipmentDTO>>> GetShipments([FromUri]FilterOptionsDto filterOptionsDto)
        {
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
        public async Task<IServiceResponse<ShipmentDTO>> AddShipment(ShipmentDTO ShipmentDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var shipment = await _service.AddShipment(ShipmentDTO);
                return new ServiceResponse<ShipmentDTO>
                {
                    Object = shipment
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{ShipmentId:int}")]
        public async Task<IServiceResponse<ShipmentDTO>> GetShipment(int ShipmentId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var shipment = await _service.GetShipment(ShipmentId);
                return new ServiceResponse<ShipmentDTO>
                {
                    Object = shipment
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{waybill}/waybill")]
        public async Task<IServiceResponse<ShipmentDTO>> GetShipment(string waybill)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var shipment = await _service.GetShipment(waybill);
                return new ServiceResponse<ShipmentDTO>
                {
                    Object = shipment
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpDelete]
        [Route("{ShipmentId:int}")]
        public async Task<IServiceResponse<bool>> DeleteShipment(int ShipmentId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _service.DeleteShipment(ShipmentId);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }


        [GIGLSActivityAuthorize(Activity = "Delete")]
        [HttpDelete]
        [Route("{waybill}/waybill")]
        public async Task<IServiceResponse<bool>> DeleteShipment(string waybill)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _service.DeleteShipment(waybill);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{shipmentId:int}")]
        public async Task<IServiceResponse<bool>> UpdateShipment(int shipmentId, ShipmentDTO ShipmentDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _service.UpdateShipment(shipmentId, ShipmentDTO);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("{waybill}")]
        public async Task<IServiceResponse<bool>> UpdateShipment(string waybill, ShipmentDTO ShipmentDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _service.UpdateShipment(waybill, ShipmentDTO);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("ungroupedwaybillsforservicecentre")]
        public async Task<IServiceResponse<IEnumerable<InvoiceViewDTO>>> GetUnGroupedWaybillsForServiceCentre([FromUri]FilterOptionsDto filterOptionsDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var shipments = await _service.GetUnGroupedWaybillsForServiceCentre(filterOptionsDto, true);
                return new ServiceResponse<IEnumerable<InvoiceViewDTO>>
                {
                    Object = shipments,
                    Total = shipments.Count
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("ungroupmappingservicecentre")]
        public async Task<IServiceResponse<IEnumerable<ServiceCentreDTO>>> GetUnGroupMappingServiceCentres()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var centres = await _service.GetUnGroupMappingServiceCentres();
                return new ServiceResponse<IEnumerable<ServiceCentreDTO>>
                {
                    Object = centres
                };
            });
        }

        //[GIGLSActivityAuthorize(Activity = "View")]
        //[HttpGet]
        //[Route("unmappedgroupedwaybillsforservicecentre")]
        //public async Task<IServiceResponse<IEnumerable<GroupWaybillNumberMappingDTO>>> GetUnmappedGroupedWaybillsForServiceCentre([FromUri]FilterOptionsDto filterOptionsDto)
        //{
        //    return await HandleApiOperationAsync(async () =>
        //    {
        //        var unmappedGroupWaybills = await _service.GetUnmappedGroupedWaybillsForServiceCentre(filterOptionsDto);
        //        return new ServiceResponse<IEnumerable<GroupWaybillNumberMappingDTO>>
        //        {
        //            Object = unmappedGroupWaybills,
        //            Total = unmappedGroupWaybills.Count
        //        };
        //    });
        //}

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("unmappedgroupedwaybillsforservicecentre")]
        public async Task<IServiceResponse<IEnumerable<GroupWaybillNumberDTO>>> GetUnmappedGroupedWaybillsForServiceCentre([FromUri]FilterOptionsDto filterOptionsDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var unmappedGroupWaybills = await _service.GetUnmappedGroupedWaybillsForServiceCentre(filterOptionsDto);
                return new ServiceResponse<IEnumerable<GroupWaybillNumberDTO>>
                {
                    Object = unmappedGroupWaybills,
                    Total = unmappedGroupWaybills.Count
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("unmappedmanifestservicecentre")]
        public async Task<IServiceResponse<IEnumerable<ServiceCentreDTO>>> GetUnmappedManifestServiceCentres()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var centres = await _service.GetUnmappedManifestServiceCentres();
                return new ServiceResponse<IEnumerable<ServiceCentreDTO>>
                {
                    Object = centres
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("zone/{destinationServiceCentreId:int}")]
        public async Task<IServiceResponse<DomesticRouteZoneMapDTO>> GetZone(int destinationServiceCentreId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var zone = await _service.GetZone(destinationServiceCentreId);

                return new ServiceResponse<DomesticRouteZoneMapDTO>
                {
                    Object = zone
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("countryzone/{destinationCountryId:int}")]
        public async Task<IServiceResponse<CountryRouteZoneMapDTO>> GetCountryZone(int destinationCountryId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var zone = await _service.GetCountryZone(destinationCountryId);

                return new ServiceResponse<CountryRouteZoneMapDTO>
                {
                    Object = zone
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpPost]
        [Route("dailysales")]
        public async Task<IServiceResponse<DailySalesDTO>> GetDailySales(AccountFilterCriteria accountFilterCriteria)
        {
            return await HandleApiOperationAsync(async () =>
            {

               // string path = "http:/localhost/GIGLS/uploads/giglsdoc.json";

                var dailySales = await _service.GetDailySales(accountFilterCriteria);

                //create daily files and store in a folder
                //if (!File.Exists(path))
                //{
                //    // Create a file to write to.
                //    var createText = dailySales.Invoices ;
                //    string json = JsonConvert.SerializeObject(createText);
                //    File.WriteAllText(path, json);
                //}

                return new ServiceResponse<DailySalesDTO>
                {
                    Object = dailySales
                };
            });
        }


        //This is use to solve time out from service centre
        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpPost]
        [Route("dailysalesforservicecentre")]
        public async Task<IServiceResponse<DailySalesDTO>> GetSalesForServiceCentre(AccountFilterCriteria accountFilterCriteria)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var dailySales = await _service.GetSalesForServiceCentre(accountFilterCriteria);
                
                return new ServiceResponse<DailySalesDTO>
                {
                    Object = dailySales
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpPost]
        [Route("dailysalesbyservicecentre")]
        public async Task<IServiceResponse<DailySalesDTO>> GetDailySalesByServiceCentre(AccountFilterCriteria accountFilterCriteria)
        {
            return await HandleApiOperationAsync(async () =>
            {

                //string path = "http:/localhost/GIGLS/uploads/giglsdoc.json";

                var dailySalesByServiceCentre = await _service.GetDailySalesByServiceCentre(accountFilterCriteria);

                var reportObject = await _reportService.GetDailySalesByServiceCentreReport(dailySalesByServiceCentre);

                //create daily files and store in a folder
                //if (!File.Exists(path))
                //{
                //    // Create a file to write to.
                //    var createText = dailySales.Invoices;
                //    string json = JsonConvert.SerializeObject(createText);
                //    File.WriteAllText(path, json);
                //}

                dailySalesByServiceCentre.Filename = (string)reportObject;

                return new ServiceResponse<DailySalesDTO>
                {
                    Object = dailySalesByServiceCentre
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("warehouseservicecentre")]
        public async Task<IServiceResponse<IEnumerable<ServiceCentreDTO>>> GetAllWarehouseServiceCenters()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var centres = await _service.GetAllWarehouseServiceCenters();
                return new ServiceResponse<IEnumerable<ServiceCentreDTO>>
                {
                    Object = centres
                };
            });
        }

    }
}
