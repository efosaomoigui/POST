using EfeAuthen.Models;
using GIGLS.Core.DTO;
using GIGLS.Core.DTO.Report;
using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.DTO.ShipmentScan;
using GIGLS.Core.DTO.Zone;
using GIGLS.Core.Enums;
using GIGLS.Core.IServices;
using GIGLS.Core.IServices.Business;
using GIGLS.Core.IServices.CustomerPortal;
using GIGLS.Core.IServices.ServiceCentres;
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
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;

namespace GIGLS.WebApi.Controllers.Scanner
{
    [Authorize(Roles = "Shipment, ViewAdmin, Agent")]
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
        private readonly ICustomerPortalService _portalService;
        private readonly IServiceCentreService _serviceCentre;
        private readonly IHUBManifestWaybillMappingService _hubService;
        private readonly ISuperManifestService _superManifestService;

        public MobileScannerController(IScanService scanService, IScanStatusService scanStatusService, IShipmentService shipmentService,
            IGroupWaybillNumberService groupService, IGroupWaybillNumberMappingService groupMappingservice, IManifestService manifestService,
            IManifestGroupWaybillNumberMappingService manifestGroupMappingService, IManifestWaybillMappingService manifestWaybillservice, 
            IStateService stateService, IShipmentCollectionService collectionservice, ILogVisitReasonService logService, IManifestVisitMonitoringService visitService,
            ICustomerPortalService portalService, IServiceCentreService serviceCentre, IHUBManifestWaybillMappingService hubService, ISuperManifestService superManifestService) : base(nameof(MobileScannerController))
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
            _portalService = portalService;
            _serviceCentre = serviceCentre;
            _hubService = hubService;
            _superManifestService = superManifestService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<IServiceResponse<JObject>> Login(UserloginDetailsModel userLoginModel)
        {
            var user = await _portalService.CheckDetailsForMobileScanner(userLoginModel.username);

            if (user.Username != null)
            {
                user.Username = user.Username.Trim();
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
                         new KeyValuePair<string, string>("Username", user.Username),
                         new KeyValuePair<string, string>("Password", userLoginModel.Password),
                     });

                    //setup login data
                    HttpResponseMessage responseMessage = await client.PostAsync("token", formContent);

                    if (!responseMessage.IsSuccessStatusCode)
                    {
                        throw new GenericException("Incorrect Login Details");
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

        [AllowAnonymous]
        [HttpPost]
        [Route("agentlogin")]
        public async Task<IServiceResponse<JObject>> LoginForAgentApp(UserloginDetailsModel userLoginModel)
        {
            var user = await _portalService.CheckDetailsForAgentApp(userLoginModel.username);

            if (user.Username != null)
            {
                user.Username = user.Username.Trim();
            }

            if (userLoginModel.Password != null)
            {
                userLoginModel.Password = userLoginModel.Password.Trim();
            }

            if (user.SystemUserRole != "FastTrack Agent")
            {
                throw new GenericException("You are not authorized to use this application. You can download the GIGGO customer app and make shipment request", $"{(int)System.Net.HttpStatusCode.Forbidden}");
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
                         new KeyValuePair<string, string>("Username", user.Username),
                         new KeyValuePair<string, string>("Password", userLoginModel.Password),
                     });

                    //setup login data
                    HttpResponseMessage responseMessage = await client.PostAsync("token", formContent);

                    if (!responseMessage.IsSuccessStatusCode)
                    {
                        throw new GenericException("Incorrect Login Details");
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

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("scanstatus")]
        public async Task<IServiceResponse<IEnumerable<ScanStatusDTO>>> GetScanStatus()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var scanStatus = await _scanStatusService.GetNonHiddenScanStatus();
                
                return new ServiceResponse<IEnumerable<ScanStatusDTO>>
                {
                    Object = scanStatus
                };
            });
        }

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

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpGet]
        [Route("generategroupwaybillnumber/{serviceCentreCode}")]
        public async Task<IServiceResponse<string>> GenerateGroupWaybillNumber(string serviceCentreCode)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var groupwaybills = await _groupService.GenerateGroupWaybillNumber(serviceCentreCode);

                return new ServiceResponse<string>
                {
                    Object = groupwaybills
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("mapwaybillstogroup")]
        public async Task<IServiceResponse<bool>> MappingWaybillNumberToGroup(List<GroupWaybillNumberMappingDTO> groupingData)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _groupMappingservice.MappingWaybillNumberToGroup(groupingData);
                return new ServiceResponse<bool>
                {
                    Object = true
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
                var centres = await _shipmentService.GetUnmappedManifestServiceCentres();
                return new ServiceResponse<IEnumerable<ServiceCentreDTO>>
                {
                    Object = centres
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpGet]
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

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpGet]
        [Route("generatesupermanifestcode")]
        public async Task<IServiceResponse<string>> GenerateSuperManifestCode()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var code = await _superManifestService.GenerateSuperManifestCode();

                return new ServiceResponse<string>
                {
                    Object = code
                };
            });
        }

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

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("mapmanifesttosupermanifest")]
        public async Task<IServiceResponse<bool>> MappingSuperManifestToManifest(ManifestDTO data)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _manifestGroupMappingService.MappingSuperManifestToManifest(data.SuperManifestCode, data.ManifestCodes);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("unmappedgroupedwaybillsforservicecentre/{serviceCentreId}")]
        public async Task<IServiceResponse<IEnumerable<GroupWaybillNumberDTO>>> GetUnmappedGroupedWaybillsForServiceCentre(int serviceCentreId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                FilterOptionsDto filterOptionsDto = new FilterOptionsDto
                {
                    filterValue = serviceCentreId.ToString(),
                    filter = "DestinationServiceCentreId"
                };

                var unmappedGroupWaybills = await _shipmentService.GetUnmappedGroupedWaybillsForServiceCentre(filterOptionsDto);
                return new ServiceResponse<IEnumerable<GroupWaybillNumberDTO>>
                {
                    Object = unmappedGroupWaybills,
                    Total = unmappedGroupWaybills.Count
                };
            });
        }

        //Super Manifest
        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("unmappedmanifestlistforservicecentre/{serviceCentreId}")]
        public async Task<IServiceResponse<IEnumerable<ManifestDTO>>> GetUnmappedManifestListForServiceCentre(int serviceCentreId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                FilterOptionsDto filterOptionsDto = new FilterOptionsDto
                {
                    filterValue = serviceCentreId.ToString(),
                    filter = "DestinationServiceCentreId"
                };

                var unmappedManifests = await _shipmentService.GetUnmappedManifestListForServiceCentre(filterOptionsDto);
                return new ServiceResponse<IEnumerable<ManifestDTO>>
                {
                    Object = unmappedManifests,
                    Total = unmappedManifests.Count
                };
            });
        }

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

        [AllowAnonymous]
        [HttpGet]
        [Route("demuragepaymenttype")]
        public IHttpActionResult DemuragePaymentTypes()
        {
            return Ok(EnumExtensions.GetValues<PaymentType>());
        }

        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("collected")]
        public async Task<IServiceResponse<bool>> ReleaseShipmentForCollection(ShipmentCollectionDTO shipmentCollection)
        {
            return await HandleApiOperationAsync(async () => {
                await _collectionservice.ReleaseShipmentForCollectionOnScanner(shipmentCollection);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        //used to fix the scanner problem
        [GIGLSActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("releaseshipment")]
        public async Task<IServiceResponse<bool>> ReleaseShipmentForCollectionFromDispatch(ShipmentCollectionDTO shipmentCollection)
        {
            //Set Request from this endpoint as from Dispatch Rider
            shipmentCollection.IsComingFromDispatch = true;
            shipmentCollection.ShipmentScanStatus = ShipmentScanStatus.OKC;

            return await HandleApiOperationAsync(async () => {
                await _collectionservice.ReleaseShipmentForCollection(shipmentCollection);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

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

        [GIGLSActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("hubservicecentres")]
        public async Task<IServiceResponse<List<ServiceCentreDTO>>> GetHUBServiceCenters()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var centres = await _serviceCentre.GetHUBServiceCentres();
                return new ServiceResponse<List<ServiceCentreDTO>>
                {
                    Object = centres
                };
            });
        }

        [GIGLSActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("mapmultipleHubManifest")]
        public async Task<IServiceResponse<bool>> MappingHUBManifestToWaybills(HUBManifestWaybillMappingDTO data)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _hubService.MappingHUBManifestToWaybillsForScanner(data.ManifestCode, data.Waybills, data.DestinationServiceCentreId);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [HttpPost]
        [Route("dropoffs")]
        public async Task<IServiceResponse<List<PreShipmentDTO>>> GetDropOffsOfUser(ShipmentCollectionFilterCriteria filterCriteria)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var dropoffs = await _portalService.GetDropOffsForUser(filterCriteria);

                return new ServiceResponse<List<PreShipmentDTO>>
                {
                    Object = dropoffs
                };
            });
        }

        [HttpPost]
        [Route("createdropoff")]
        public async Task<IServiceResponse<bool>> CreateOrUpdateDropOff(PreShipmentDTO preShipmentDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var preshipMentMobile = await _portalService.CreateOrUpdateDropOffForAgent(preShipmentDTO);

                return new ServiceResponse<bool>
                {
                    Object = preshipMentMobile
                };
            });
        }

        [HttpPost]
        [Route("dropoffprice")]
        public async Task<IServiceResponse<MobilePriceDTO>> GetPriceForDropOff(PreShipmentMobileDTO preshipmentMobile)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var Price = await _portalService.GetPriceForDropOff(preshipmentMobile);

                return new ServiceResponse<MobilePriceDTO>
                {
                    Object = Price,
                };
            });
        }

        [HttpGet]
        [Route("getspecialpackages")]
        public async Task<IServiceResponse<SpecialResultDTO>> GetSpecialPackages()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var packages = await _portalService.GetSpecialPackages();

                return new ServiceResponse<SpecialResultDTO>
                {
                    Object = packages
                };
            });
        }

        [HttpGet]
        [Route("getstations")]
        public async Task<IServiceResponse<List<GiglgoStationDTO>>> GetGostations()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var stations = await _portalService.GetGoStations();

                return new ServiceResponse<List<GiglgoStationDTO>>
                {
                    Object = stations
                };
            });
        }

        [HttpGet]
        [Route("itemtypes")]
        public async Task<IServiceResponse<List<string>>> GetItemTypes()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var ItemTypes = await _portalService.GetItemTypes();
                return new ServiceResponse<List<string>>
                {
                    Object = ItemTypes,
                };
            });
        }
    }
}
