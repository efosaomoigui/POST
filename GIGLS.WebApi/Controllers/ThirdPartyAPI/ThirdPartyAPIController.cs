using EfeAuthen.Models;
using GIGLS.Core.DTO;
using GIGLS.Core.DTO.Account;
using GIGLS.Core.DTO.Partnership;
using GIGLS.Core.DTO.Report;
using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.IServices;
using GIGLS.Core.IServices.ThirdPartyAPI;
using GIGLS.CORE.DTO.Report;
using GIGLS.Infrastructure;
using GIGLS.Services.Implementation;
using GIGLS.WebApi.Filters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;

namespace GIGLS.WebApi.Controllers.ThirdPartyAPI
{
    [Authorize(Roles = "ThirdParty")]
    [RoutePrefix("api/thirdparty")]
    public class ThirdPartyAPIController : BaseWebApiController
    {
        private readonly IThirdPartyAPIService _thirdPartyAPIService;

        public ThirdPartyAPIController(IThirdPartyAPIService portalService) : base(nameof(ThirdPartyAPIController))
        {
            _thirdPartyAPIService = portalService;
        }

        /// <summary>
        /// This api is used to get the price for Shipment Items
        /// </summary>
        /// <param name="thirdPartyPricingDto"></param>
        /// <returns></returns>
        //[ThirdPartyActivityAuthorize(Activity = "View")]
        //[HttpPost]
        //[Route("previousprice")]
        //public async Task<IServiceResponse<decimal>> GetPrice(ThirdPartyPricingDTO thirdPartyPricingDto)
        //{
        //    return await HandleApiOperationAsync(async () =>
        //    {
        //        var price = await _thirdPartyAPIService.GetPrice2(thirdPartyPricingDto);

        //        return new ServiceResponse<decimal>
        //        {
        //            Object = price
        //        };
        //    });
        //}
        [ThirdPartyActivityAuthorize(Activity = "View")]
        [HttpPost]
        [Route("price")]
        public async Task<IServiceResponse<MobilePriceDTO>> GetPrice(PreShipmentMobileDTO preshipmentMobile)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var PreshipMentMobile = await _thirdPartyAPIService.GetPrice(preshipmentMobile);
                return new ServiceResponse<MobilePriceDTO>
                {
                    Object = PreshipMentMobile
                };
            });
        }

        //Capture Shipment API
        /// <summary>
        /// This api is used to register a shipment
        /// </summary>
        /// <param name="thirdPartyPreShipmentDTO"></param>
        /// <returns></returns>
        //[ThirdPartyActivityAuthorize(Activity = "Create")]
        //[HttpPost]
        //[Route("previouscaptureshipment")]
        //public async Task<IServiceResponse<bool>> AddPreShipment(ThirdPartyPreShipmentDTO thirdPartyPreShipmentDTO)
        //{
        //    return await HandleApiOperationAsync(async () =>
        //    {
        //        var shipment = await _thirdPartyAPIService.AddPreShipment(thirdPartyPreShipmentDTO);
        //        return new ServiceResponse<bool>
        //        {
        //            Object = true
        //        };
        //    });
        //}

        [ThirdPartyActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("captureshipment")]
        public async Task<IServiceResponse<object>> CreateShipment(CreatePreShipmentMobileDTO preshipmentMobile)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _thirdPartyAPIService.CreatePreShipment(preshipmentMobile);
                return new ServiceResponse<object>
                {
                    Object = result
                };
            });
        }

        /// <summary>
        /// This api is used to track all shipments
        /// </summary>
        /// <param name="waybillNumber"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("TrackShipmentPublic/{waybillNumber}")]
        public async Task<IServiceResponse<IEnumerable<ShipmentTrackingDTO>>> PublicTrackShipment(string waybillNumber)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _thirdPartyAPIService.PublicTrackShipment(waybillNumber);

                return new ServiceResponse<IEnumerable<ShipmentTrackingDTO>>
                {
                    Object = result
                };
            });
        }

        /// <summary>
        /// This api is used to get all local stations
        /// </summary>
        /// <returns></returns>
        [ThirdPartyActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("localStations")]
        public async Task<IServiceResponse<IEnumerable<StationDTO>>> GetLocalStations()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var stations = await _thirdPartyAPIService.GetLocalStations();

                return new ServiceResponse<IEnumerable<StationDTO>>
                {
                    Object = stations
                };
            });
        }

        /// <summary>
        /// This api is used to get all international stations
        /// </summary>
        /// <returns></returns>
        [ThirdPartyActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("InternationalStations")]
        public async Task<IServiceResponse<IEnumerable<StationDTO>>> GetInternationalStations()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var stations = await _thirdPartyAPIService.GetInternationalStations();

                return new ServiceResponse<IEnumerable<StationDTO>>
                {
                    Object = stations
                };
            });
        }

        /// <summary>
        /// This api is used to login and acquire token for subsequent calls
        /// </summary>
        /// <description>This api is used to login and acquire token for subsequent calls</description>
        /// <param name="userLoginModel"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<IServiceResponse<JObject>> Login(UserloginDetailsModel userLoginModel)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var user = await _thirdPartyAPIService.CheckDetailsForLogin(userLoginModel.username);

                //trim
                if (user.Username != null)
                {
                    user.Username = user.Username.Trim();
                }

                string apiBaseUri = ConfigurationManager.AppSettings["WebApiUrl"];
                string getTokenResponse;

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


        /// <summary>
        /// This api is used to get shipments created by user
        /// </summary>
        /// <description>This api is used to get shipments by user</description>
        /// <returns></returns>

        [ThirdPartyActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("PickUpRequests")]
        public async Task<IServiceResponse<List<InvoiceViewDTO>>> GetShipmentTransactions(ShipmentCollectionFilterCriteria f_Criteria)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var preshipMentMobile = await _thirdPartyAPIService.GetShipmentTransactions(f_Criteria);
                return new ServiceResponse<List<InvoiceViewDTO>>
                {
                    Object = preshipMentMobile
                };
            });
        }

        /// <summary>
        /// This api is used to track shipments created by the user
        /// </summary>
        /// <param name="waybillNumber"></param>
        /// <returns></returns>
        [ThirdPartyActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("TrackAllShipment/{waybillNumber}")]
        public async Task<IServiceResponse<MobileShipmentTrackingHistoryDTO>> TrackMobileShipment(string waybillNumber)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _thirdPartyAPIService.TrackShipment(waybillNumber);

                return new ServiceResponse<MobileShipmentTrackingHistoryDTO>
                {
                    Object = result
                };
            });
        }

        /// <summary>
        /// This api is used to get the active lgas for GiG Go shipments 
        /// </summary>
        /// <returns></returns>
        [ThirdPartyActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("getactivelgas")]
        public async Task<IServiceResponse<IEnumerable<LGADTO>>> GetActiveLGAs()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _thirdPartyAPIService.GetActiveLGAs();

                return new ServiceResponse<IEnumerable<LGADTO>>
                {
                    Object = result
                };
            });
        }

        /// <summary>
        /// This api is used to get the active home delivery locations  
        /// </summary>
        /// <returns></returns>
        [ThirdPartyActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("activehomedeliverylocations")]
        public async Task<IServiceResponse<IEnumerable<LGADTO>>> GetActiveHomeDeliveryLocations()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _thirdPartyAPIService.GetActiveHomeDeliveryLocations();
                return new ServiceResponse<IEnumerable<LGADTO>>
                {
                    Object = result
                };
            });
        }

        /// <summary>
        /// This api is used to get the details about a shipment 
        /// </summary>
        /// <param name="waybillNumber"></param>
        /// <returns></returns>
        [ThirdPartyActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("preshipmentmobile/{waybillNumber}")]
        public async Task<IServiceResponse<PreShipmentMobileDTO>> GetPreShipmentMobileByWaybill(string waybillNumber)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _thirdPartyAPIService.GetPreShipmentMobileByWaybill(waybillNumber);

                return new ServiceResponse<PreShipmentMobileDTO>
                {
                    Object = result
                };
            });
        }

        /// <summary>
        /// This api is used to get the lists of manifests belonging to a service center 
        /// </summary>
        /// <returns></returns>
        [ThirdPartyActivityAuthorize(Activity = "View")]
        [HttpPost]
        [Route("manifests")]
        public async Task<IServiceResponse<IEnumerable<ManifestGroupWaybillNumberMappingDTO>>> GetManifestsInServiceCenter(DateFilterCriteria dateFilterCriteria)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _thirdPartyAPIService.GetManifestsInServiceCenter(dateFilterCriteria);

                return new ServiceResponse<IEnumerable<ManifestGroupWaybillNumberMappingDTO>>
                {
                    Object = result
                };
            });
        }

        /// <summary>
        /// This api is used to get the lists of group waybills and waybills assigned to a manifest 
        /// </summary>
        /// <param name="manifestCode"></param>
        /// <returns></returns>
        [ThirdPartyActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("manifests/{manifestCode}")]
        public async Task<IServiceResponse<List<GroupWaybillAndWaybillDTO>>> GetGroupWaybillDataInManifest(string manifestCode)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _thirdPartyAPIService.GetGroupWaybillDataInManifest(manifestCode);

                return new ServiceResponse<List<GroupWaybillAndWaybillDTO>>
                {
                    Object = result
                };
            });
        }

        /// <summary>
        /// This api is used to give "Shipped from the UK" Scan to all the waybills in a manifests
        /// </summary>
        /// <param name="manifestCode"></param>
        /// <returns></returns>
        [ThirdPartyActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("scan/{manifestCode}")]
        public async Task<IServiceResponse<bool>> ScanShipment(string manifestCode)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _thirdPartyAPIService.ItemShippedFromUKScan(manifestCode);

                return new ServiceResponse<bool>
                {
                    Object = result
                };
            });
        }

        [ThirdPartyActivityAuthorize(Activity = "View")]
        [HttpPost]
        [Route("getaddressdetails")]
        public async Task<IServiceResponse<GoogleAddressDTO>> GetGoogleAddressDetails(GoogleAddressDTO location)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _thirdPartyAPIService.GetGoogleAddressDetails(location);

                return new ServiceResponse<GoogleAddressDTO>
                {
                    Object = result
                };
            });
        }
    }
}