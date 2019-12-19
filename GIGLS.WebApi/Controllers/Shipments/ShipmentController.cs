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

namespace GIGLS.WebApi.Controllers.Shipments
{
    [Authorize(Roles = "Shipment, ViewAdmin")]
    [RoutePrefix("api/shipment")]
    public class ShipmentController : BaseWebApiController
    {
        private readonly IShipmentService _service;
        private readonly IShipmentReportService _reportService;
        private readonly IUserService _userService;

        public ShipmentController(IShipmentService service, IShipmentReportService reportService,
            IUserService userService) : base(nameof(ShipmentController))
        {
            _service = service;
            _reportService = reportService;
            _userService = userService;
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
        public async Task<IServiceResponse<ShipmentDTO>> AddShipment(ShipmentDTO ShipmentDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {
                //Update SenderAddress for corporate customers
                ShipmentDTO.SenderAddress = null;
                ShipmentDTO.SenderState = null;
                if (ShipmentDTO.Customer[0].CompanyType == CompanyType.Corporate)
                {
                    ShipmentDTO.SenderAddress = ShipmentDTO.Customer[0].Address;
                    ShipmentDTO.SenderState = ShipmentDTO.Customer[0].State;
                }

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

        // Shipment delivery monitor
        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("GetShipmentCreatedSummaryMonitor")]
        public async Task<IServiceResponse<System.Web.Mvc.JsonResult>> GetShipmentCreatedSummaryMonitor()
        {
            return await HandleApiOperationAsync(async () =>
            {

                var today = DateTime.Now.Date; // ;new DateTime(2019, 2, 6)
                var firstDayOfMonth = today.AddDays(-7);
                //var firstDayOfMonth = DateTime.Parse(ConfigurationManager.AppSettings["dashboardstartdate"]); //today.AddDays(limitspan); //DateTime.Now.AddDays(limitspan); //   new DateTime(today.Year, today.Month, 1); 

                var accountFilterCriteria = new AccountFilterCriteria
                {
                    StartDate = firstDayOfMonth,
                    EndDate = today
                };

                // string path = "http:/localhost/GIGLS/uploads/giglsdoc.json";
                var results = await _service.GetShipmentMonitor(accountFilterCriteria);

                return new ServiceResponse<System.Web.Mvc.JsonResult>()
                {
                    Object = new System.Web.Mvc.JsonResult { Data = results.totalZones, JsonRequestBehavior = System.Web.Mvc.JsonRequestBehavior.AllowGet }
                };
            });
        }

        // Shipment monitor
        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("GetShipmentCreatedByDateMonitor")]
        public async Task<IServiceResponse<System.Web.Mvc.JsonResult>> GetShipmentCreatedByDateMonitor(int limitStart, int limitEnd)
        {
            return await HandleApiOperationAsync(async () =>
            {

                var today = DateTime.Now.Date; // ;new DateTime(2019, 2, 6)
                var firstDayOfMonth = today.AddDays(-7);
                //var firstDayOfMonth = DateTime.Parse(ConfigurationManager.AppSettings["dashboardstartdate"]);

                var accountFilterCriteria = new AccountFilterCriteria
                {
                    StartDate = firstDayOfMonth,
                    EndDate = today
                };

                var limitdates = new LimitDates
                {
                    StartLimit = limitStart,
                    EndLimit = limitEnd
                };

                // string path = "http:/localhost/GIGLS/uploads/giglsdoc.json";
                var chartData = await _service.GetShipmentCreatedByDateMonitor(accountFilterCriteria, limitdates);

                return new ServiceResponse<System.Web.Mvc.JsonResult>()
                {
                    Object = new System.Web.Mvc.JsonResult { Data = chartData, JsonRequestBehavior = System.Web.Mvc.JsonRequestBehavior.AllowGet }
                };
            });
        }

        // Shipment monitor
        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("GetShipmentWaybillsByDateMonitor")]
        public async Task<IServiceResponse<System.Web.Mvc.JsonResult>> GetShipmentWaybillsByDateMonitor(int limitStart, int limitEnd, string scname)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var today = DateTime.Now.Date; // ;new DateTime(2019, 2, 6)
                var firstDayOfMonth = today.AddDays(-7);
                //var firstDayOfMonth = DateTime.Parse(ConfigurationManager.AppSettings["dashboardstartdate"]);
                //today.AddDays(limitspan); //DateTime.Now.AddDays(limitspan); //   new DateTime(today.Year, today.Month, 1);  

                var accountFilterCriteria = new AccountFilterCriteria
                {
                    StartDate = firstDayOfMonth,
                    EndDate = today
                };

                var limitdates = new LimitDates
                {
                    StartLimit = limitStart,
                    EndLimit = limitEnd,
                    ScName = scname
                };

                // string path = "http:/localhost/GIGLS/uploads/giglsdoc.json";
                var chartData = await _service.GetShipmentWaybillsByDateMonitor(accountFilterCriteria, limitdates);

                return new ServiceResponse<System.Web.Mvc.JsonResult>()
                {
                    Object = new System.Web.Mvc.JsonResult { Data = chartData, JsonRequestBehavior = System.Web.Mvc.JsonRequestBehavior.AllowGet }
                };
            });
        }


        // Shipment delivery monitor
        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("GetShipmentCreatedSummaryMonitorx")]
        public async Task<IServiceResponse<System.Web.Mvc.JsonResult>> GetShipmentCreatedSummaryMonitorx()
        {
            return await HandleApiOperationAsync(async () =>
            {

                var today = DateTime.Now.Date; // ;new DateTime(2019, 2, 6)
                var firstDayOfMonth = today.AddDays(-7);
                //var firstDayOfMonth = DateTime.Parse(ConfigurationManager.AppSettings["dashboardstartdate"]);
                //today.AddDays(limitspan); //DateTime.Now.AddDays(limitspan); //   new DateTime(today.Year, today.Month, 1);  

                var accountFilterCriteria = new AccountFilterCriteria
                {
                    StartDate = firstDayOfMonth,
                    EndDate = today
                };

                // string path = "http:/localhost/GIGLS/uploads/giglsdoc.json";
                var results = await _service.GetShipmentMonitorx(accountFilterCriteria);

                return new ServiceResponse<System.Web.Mvc.JsonResult>()
                {
                    Object = new System.Web.Mvc.JsonResult { Data = results.totalZones, JsonRequestBehavior = System.Web.Mvc.JsonRequestBehavior.AllowGet }
                };
            });
        }

        // Shipment monitor
        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("GetShipmentCreatedByDateMonitorx")]
        public async Task<IServiceResponse<System.Web.Mvc.JsonResult>> GetShipmentCreatedByDateMonitorx(int limitStart, int limitEnd)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var today = DateTime.Now.Date; // ;new DateTime(2019, 2, 6)
                var firstDayOfMonth = today.AddDays(-7);
                //var firstDayOfMonth = DateTime.Parse(ConfigurationManager.AppSettings["dashboardstartdate"]);
                //today.AddDays(limitspan); //DateTime.Now.AddDays(limitspan); //   new DateTime(today.Year, today.Month, 1);  

                var accountFilterCriteria = new AccountFilterCriteria
                {
                    StartDate = firstDayOfMonth,
                    EndDate = today
                };

                var limitdates = new LimitDates
                {
                    StartLimit = limitStart,
                    EndLimit = limitEnd
                };

                // string path = "http:/localhost/GIGLS/uploads/giglsdoc.json";
                var chartData = await _service.GetShipmentCreatedByDateMonitorx(accountFilterCriteria, limitdates);

                return new ServiceResponse<System.Web.Mvc.JsonResult>()
                {
                    Object = new System.Web.Mvc.JsonResult { Data = chartData, JsonRequestBehavior = System.Web.Mvc.JsonRequestBehavior.AllowGet }
                };
            });
        }

        // Shipment monitor
        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("GetShipmentWaybillsByDateMonitorx")]
        public async Task<IServiceResponse<System.Web.Mvc.JsonResult>> GetShipmentWaybillsByDateMonitorx(int limitStart, int limitEnd, string scname = null)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var today = DateTime.Now.Date; // ;new DateTime(2019, 2, 6)
                var firstDayOfMonth = today.AddDays(-7);
                //var firstDayOfMonth = DateTime.Parse(ConfigurationManager.AppSettings["dashboardstartdate"]);

                var accountFilterCriteria = new AccountFilterCriteria
                {
                    StartDate = firstDayOfMonth,
                    EndDate = today
                };

                var limitdates = new LimitDates
                {
                    StartLimit = limitStart,
                    EndLimit = limitEnd,
                    ScName = scname
                };

                // string path = "http:/localhost/GIGLS/uploads/giglsdoc.json";
                var chartData = await _service.GetShipmentWaybillsByDateMonitorx(accountFilterCriteria, limitdates);

                return new ServiceResponse<System.Web.Mvc.JsonResult>()
                {
                    Object = new System.Web.Mvc.JsonResult { Data = chartData, JsonRequestBehavior = System.Web.Mvc.JsonRequestBehavior.AllowGet }
                };
            });
        }

    }
}
