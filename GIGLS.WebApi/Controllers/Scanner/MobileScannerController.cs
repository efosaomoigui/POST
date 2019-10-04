using EfeAuthen.Models;
using GIGLS.Core.DTO;
using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.DTO.ShipmentScan;
using GIGLS.Core.Enums;
using GIGLS.Core.IServices;
using GIGLS.Core.IServices.Business;
using GIGLS.Core.IServices.Shipments;
using GIGLS.Core.IServices.ShipmentScan;
using GIGLS.CORE.DTO.Shipments;
using GIGLS.CORE.IServices.Shipments;
using GIGLS.Infrastructure;
using GIGLS.Services.Implementation;
using GIGLS.Services.Implementation.Utility;
using GIGLS.WebApi.Filters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;

namespace GIGLS.WebApi.Controllers.Scanner
{
    [Authorize(Roles = "Shipment, ViewAdmin")]
    [RoutePrefix("api/scanner")]
    public class MobileScannerController : BaseWebApiController
    {
        private readonly IScanService _scanService;
        private readonly IScanStatusService _scanStatusService;
        private readonly IShipmentService _shipmentService;
        private readonly IGroupWaybillNumberService _groupService;
        private readonly IGroupWaybillNumberMappingService _groupMappingservice;
        private readonly IManifestService _manifestService;
        private readonly IManifestGroupWaybillNumberMappingService _manifestGroupMappingService;
        private readonly IManifestWaybillMappingService _manifestWaybillservice;
        private readonly IStateService _stateService;
        private readonly IShipmentCollectionService _collectionservice;
        private readonly ILogVisitReasonService _logService;
        private readonly IManifestVisitMonitoringService _visitService;

        public MobileScannerController(IScanService scanService, IScanStatusService scanStatusService, IShipmentService shipmentService,
            IGroupWaybillNumberService groupService, IGroupWaybillNumberMappingService groupMappingservice,
            IManifestService manifestService, IManifestGroupWaybillNumberMappingService manifestGroupMappingService,
            IManifestWaybillMappingService manifestWaybillservice, IStateService stateService,
            IShipmentCollectionService collectionservice, ILogVisitReasonService logService, IManifestVisitMonitoringService visitService) : base(nameof(MobileScannerController))
        {
            _scanService = scanService;
            _scanStatusService = scanStatusService;
            _shipmentService = shipmentService;
            _groupService = groupService;
            _groupMappingservice = groupMappingservice;
            _manifestService = manifestService;
            _manifestGroupMappingService = manifestGroupMappingService;
            _manifestWaybillservice = manifestWaybillservice;
            _stateService = stateService;
            _collectionservice = collectionservice;
            _logService = logService;
            _visitService = visitService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<IServiceResponse<JObject>> Login(UserloginDetailsModel userLoginModel)
        {
            if (userLoginModel.username != null)
            {
                userLoginModel.username = userLoginModel.username.Trim();
            }

            if (userLoginModel.Password != null)
            {
                userLoginModel.Password = userLoginModel.Password.Trim();
            }

            string apiBaseUri = ConfigurationManager.AppSettings["WebApiUrl"];
            string getTokenResponse;

            return await HandleApiOperationAsync(async () =>
            {
                using (var client = new HttpClient())
                {
                    //setup client
                    client.BaseAddress = new Uri(apiBaseUri);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    //setup login data
                    var formContent = new FormUrlEncodedContent(new[]
                    {
                         new KeyValuePair<string, string>("grant_type", "password"),
                         new KeyValuePair<string, string>("Username", userLoginModel.username),
                         new KeyValuePair<string, string>("Password", userLoginModel.Password),
                     });

                    //setup login data
                    HttpResponseMessage responseMessage = client.PostAsync("token", formContent).Result;

                    if (!responseMessage.IsSuccessStatusCode)
                    {
                        throw new GenericException("Operation could not complete login successfully:");
                    }

                    //get access token from response body
                    var responseJson = await responseMessage.Content.ReadAsStringAsync();
                    var jObject = JObject.Parse(responseJson);

                    getTokenResponse = jObject.GetValue("access_token").ToString();

                    return new ServiceResponse<JObject>
                    {
                        Object = jObject
                    };
                }
            });
        }


        //Scan Status
        //------------
        //1. Get all Scan status  --> ScanTrackService --> scanstatus(get all)
        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("scanstatus")]
        public async Task<IServiceResponse<IEnumerable<ScanStatusDTO>>> GetScanStatus()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var scanStatus = await _scanStatusService.GetScanStatus();

                //filter only the status for display
                scanStatus = scanStatus.Where(s => s.HiddenFlag == false);
                scanStatus = scanStatus.OrderBy(s => s.Reason);

                return new ServiceResponse<IEnumerable<ScanStatusDTO>>
                {
                    Object = scanStatus
                };
            });
        }

        //2. Submit the waybills with the status selected -->ScanTrackService  --> scan/multiple(POST)
        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("scanmultiple")]
        public async Task<IServiceResponse<bool>> ScanMultipleShipment(List<ScanDTO> scanList)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _scanService.ScanMultipleShipment(scanList);

                return new ServiceResponse<bool>
                {
                    Object = result
                };
            });
        }

        //Process Scan
        //---------------
        //1. Get Service Centre --> ShipmentsService --> byservicecentre(shipment/ungroupmappingservicecentre) (GET)
        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("ungroupmappingservicecentre")]
        public async Task<IServiceResponse<IEnumerable<ServiceCentreDTO>>> GetUnGroupMappingServiceCentres()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var centres = await _shipmentService.GetUnGroupMappingServiceCentres();
                return new ServiceResponse<IEnumerable<ServiceCentreDTO>>
                {
                    Object = centres
                };
            });
        }

        //2. Generate GroupWaybill --> ShipmentsService --> groupwaybill/generategroupwaybillnumber(GET)
        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("generategroupwaybillnumber")]
        public async Task<IServiceResponse<string>> GenerateGroupWaybillNumber(GroupWaybillNumberDTO groupWaybillNumberDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var groupwaybills = await _groupService.GenerateGroupWaybillNumber(groupWaybillNumberDTO);

                return new ServiceResponse<string>
                {
                    Object = groupwaybills
                };
            });
        }

        //3. Save --> ShipmentsService --> groupwaybillnumbermapping/mapmultiple(POST)
        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("mapwaybillstogroup")]
        public async Task<IServiceResponse<bool>> MappingWaybillNumberToGroup(GroupWaybillNumberMappingDTO data)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _groupMappingservice.MappingWaybillNumberToGroup(data.GroupWaybillNumber, data.WaybillNumbers);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }



        //Manifest Scan
        //---------------------
        //1. Get Service centre --> ShipmentsService --> byservicecentre(shipment/unmappedmanifestservicecentre (GET)
        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("unmappedmanifestservicecentre")]
        public async Task<IServiceResponse<IEnumerable<ServiceCentreDTO>>> GetUnmappedManifestServiceCentres()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var centres = await _shipmentService.GetUnmappedManifestServiceCentres();
                return new ServiceResponse<IEnumerable<ServiceCentreDTO>>
                {
                    Object = centres
                };
            });
        }


        //2. Generate Manifest --> ShipmentsService --> GenerateMaifestCode(manifest/generateMaifestCode)
        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("generateManifestcode")]
        public async Task<IServiceResponse<string>> GenerateManifestCode()
        {
            return await HandleApiOperationAsync(async () =>
            {
                ManifestDTO manifestDTO = new ManifestDTO();
                var groupwaybills = await _manifestService.GenerateManifestCode(manifestDTO);

                return new ServiceResponse<string>
                {
                    Object = groupwaybills
                };
            });
        }

        //3. Save --> ShipmentsService --> manifestgroupwaybillnumbermapping(manifestgroupwaybillnumbermapping/mapmultiple)
        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("mapgroupwaybilltomanifest")]
        public async Task<IServiceResponse<bool>> MappingManifestToGroupWaybillNumber(ManifestGroupWaybillNumberMappingDTO data)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _manifestGroupMappingService.MappingManifestToGroupWaybillNumber(data.ManifestCode, data.GroupWaybillNumbers);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }


        //Delivery Manifest
        //------------------
        //2.Save-- > ShipmentsService-- > ManifestForWaybillsMapping(manifestwaybillmapping / mapmultiplemobile)(POST)
        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("mapwaybillstomanifest")]
        public async Task<IServiceResponse<bool>> MappingManifestToWaybillsMobile(ManifestWaybillMappingDTO data)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _manifestWaybillservice.MappingManifestToWaybillsMobile(data.ManifestCode, data.Waybills);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        //Overdue Shipment
        //-------------------- -
        //1.Get all Warehouse-- > ShipmentsService-- > Shipment.byWarehouseServicecentre(shipment / warehouseservicecentre)(GET)
        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("warehouseservicecentre")]
        public async Task<IServiceResponse<IEnumerable<ServiceCentreDTO>>> GetAllWarehouseServiceCenters()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var centres = await _shipmentService.GetAllWarehouseServiceCenters();
                return new ServiceResponse<IEnumerable<ServiceCentreDTO>>
                {
                    Object = centres
                };
            });
        }

        //3.Save-- > ShipmentsService-- > groupwaybillnumbermappingForOverdue(groupwaybillnumbermapping / mapmultipleForOverdue)(POST)

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("mapwaybillstogroupforoverdue")]
        public async Task<IServiceResponse<bool>> MappingWaybillNumberToGroupForOverdue(GroupWaybillNumberMappingDTO data)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _groupMappingservice.MappingWaybillNumberToGroupForOverdue(data.GroupWaybillNumber, data.WaybillNumbers);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }


        //Dispatch
        //------------
        //1.Get all waybills to be delivered-- > ShipmentsService-- > ManifestForWaybillsMapping / GetManifestForWayBillMobile(manifestwaybillmapping / waybillsinmanifestfordispatch)(GET)
        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("waybillsinmanifestfordispatch")]
        public async Task<IServiceResponse<List<ManifestWaybillMappingDTO>>> GetWaybillsInManifestForDispatch()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var groupWaybillNumbersInManifest = await _manifestWaybillservice.GetWaybillsInManifestForDispatch();

                return new ServiceResponse<List<ManifestWaybillMappingDTO>>
                {
                    Object = groupWaybillNumbersInManifest
                };
            });
        }

        //ReleaseDetails
        //------------------------
        //1.Get State : StateService-- > get
        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("states")]
        public async Task<IServiceResponse<IEnumerable<StateDTO>>> GetStates(int pageSize = 10, int page = 1)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var state = await _stateService.GetStates(pageSize, page);
                var total = _stateService.GetStatesTotal();

                return new ServiceResponse<IEnumerable<StateDTO>>
                {
                    Total = total,
                    Object = state
                };
            });
        }

        //2.Get Shipment Detail-- > ShipmentsService-- > getShipmentbyWaybill(shipment / waybillNumber / waybill)
        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("waybill/{waybill}")]
        public async Task<IServiceResponse<ShipmentDTO>> GetShipment(string waybill)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var shipment = await _shipmentService.GetShipment(waybill);
                return new ServiceResponse<ShipmentDTO>
                {
                    Object = shipment
                };
            });
        }

        //3.DemuragePaymentTypes-- > Enum
        //[GIGLSActivityAuthorize(Activity = "View")]
        [AllowAnonymous]
        [HttpGet]
        [Route("demuragepaymenttype")]
        public IHttpActionResult DemuragePaymentTypes()
        {
            return Ok(EnumExtensions.GetValues<PaymentType>());
        }

        //4.ReleasePaymentTypes-- > Enum
        //5.Signature pad


        //6.Release-- > ShipmentsService-- > shipmentcollection.savecollection(shipmentcollection / collected)(PUT)
        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("collected")]
        public async Task<IServiceResponse<bool>> ReleaseShipmentForCollection(ShipmentCollectionDTO shipmentCollection)
        {
            shipmentCollection.ShipmentScanStatus = ShipmentScanStatus.OKT;
            if (shipmentCollection.IsComingFromDispatch)
            {
                shipmentCollection.ShipmentScanStatus = ShipmentScanStatus.OKC;
            }

            return await HandleApiOperationAsync(async () => {
                await _collectionservice.ReleaseShipmentForCollection(shipmentCollection);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        //7.Log Visit(Pass Name, Address, PhoneNumber)-- >
        //Log Visit
        //---------------- -
        //1.Get some detail from the previous page
        //2.Get LogVisitReason-- > ScanTrackService-- > logvisitreason(logvisitreason)(GET)
        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("getlogvisit")]
        public async Task<IServiceResponse<List<LogVisitReasonDTO>>> GetLogVisitReasons()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var logVisitReasons = await _logService.GetLogVisitReasons();

                return new ServiceResponse<List<LogVisitReasonDTO>>
                {
                    Object = logVisitReasons
                };
            });
        }

        //3.Save-- > ShipmentsService-- > manifestvisitmonitoring(manifestvisitmonitoring)(POST)
        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("logwaybillvisit")]
        public async Task<IServiceResponse<object>> AddManifest(ManifestVisitMonitoringDTO manifestVisitMonitoringDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var manifest = await _visitService.AddManifestVisitMonitoring(manifestVisitMonitoringDTO);
                return new ServiceResponse<object>
                {
                    Object = manifest
                };
            });
        }
    }
}
