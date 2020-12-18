using AutoMapper;
using EfeAuthen.Models;
using GIGLS.Core.DTO;
using GIGLS.Core.DTO.Customers;
using GIGLS.Core.DTO.PaymentTransactions;
using GIGLS.Core.DTO.Report;
using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.DTO.ShipmentScan;
using GIGLS.Core.DTO.Zone;
using GIGLS.Core.Enums;
using GIGLS.Core.IServices;
using GIGLS.Core.IServices.Business;
using GIGLS.Core.IServices.CustomerPortal;
using GIGLS.Core.IServices.Customers;
using GIGLS.Core.IServices.ServiceCentres;
using GIGLS.Core.IServices.Shipments;
using GIGLS.Core.IServices.ShipmentScan;
using GIGLS.Core.IServices.User;
using GIGLS.Core.IServices.Zone;
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
    [RoutePrefix("api/giglexpress")]
    public class GIGLExpressController : BaseWebApiController
    {
        private readonly IDeliveryOptionPriceService _deliveryOptionPriceService;
        private readonly IDomesticRouteZoneMapService _domesticRouteZoneMapService;
        private readonly IShipmentService _shipmentService;
        private readonly IShipmentPackagePriceService _packagePriceService;
        private readonly ICustomerService _customerService;
        private readonly IPricingService _pricing;
        private readonly IPaymentService _paymentService;
        private readonly ICustomerPortalService _portalService;
        private readonly IShipmentCollectionService _shipmentCollectionService;
        private readonly IServiceCentreService _serviceCentreService;
        private readonly IUserService _userService;
        private readonly ICountryService _countryService;

        public GIGLExpressController(IDeliveryOptionPriceService deliveryOptionPriceService, IDomesticRouteZoneMapService domesticRouteZoneMapService, IShipmentService shipmentService,
            IShipmentPackagePriceService packagePriceService, ICustomerService customerService, IPricingService pricing,
            IPaymentService paymentService, ICustomerPortalService portalService, IShipmentCollectionService shipmentCollectionService, IServiceCentreService serviceCentreService, IUserService userService,ICountryService countryService) : base(nameof(MobileScannerController))
        {
            _deliveryOptionPriceService = deliveryOptionPriceService;
            _domesticRouteZoneMapService = domesticRouteZoneMapService;
            _shipmentService = shipmentService;
            _packagePriceService = packagePriceService;
            _customerService = customerService;
            _pricing = pricing;
            _paymentService = paymentService;
            _portalService = portalService;
            _shipmentCollectionService = shipmentCollectionService;
            _serviceCentreService = serviceCentreService;
            _userService = userService;
            _countryService = countryService;

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

        [HttpGet]
        [Route("deliveryoptionprice")]
        public async Task<IServiceResponse<IEnumerable<DeliveryOptionPriceDTO>>> GetDeliveryOptionPrices()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var deliveryOptionPrices = await _deliveryOptionPriceService.GetDeliveryOptionPrices();

                return new ServiceResponse<IEnumerable<DeliveryOptionPriceDTO>>
                {
                    Object = deliveryOptionPrices
                };
            });
        }

        [HttpGet]
        [Route("zonemapping/{departure:int}/{destination:int}")]
        public async Task<IServiceResponse<DomesticRouteZoneMapDTO>> GetZone(int departure, int destination)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var zone = await _domesticRouteZoneMapService.GetZone(departure, destination);

                return new ServiceResponse<DomesticRouteZoneMapDTO>
                {
                    Object = zone
                };
            });
        }

        [HttpGet]
        [Route("pickupoptions")]
        public IHttpActionResult GetPickUpOptions()
        {
            var types = EnumExtensions.GetValues<PickupOptions>();
            return Ok(types);
        }

        [HttpGet]
        [Route("packageoptions")]
        public async Task<IServiceResponse<IEnumerable<ShipmentPackagePriceDTO>>> GetShipmentPackagePrices()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var shipmentPackagePrices = await _packagePriceService.GetShipmentPackagePrices();

                return new ServiceResponse<IEnumerable<ShipmentPackagePriceDTO>>
                {
                    Object = shipmentPackagePrices
                };
            });
        }

        [HttpPost]
        [Route("customer/{customerType}")]
        public async Task<IServiceResponse<object>> GetCustomerByPhoneNumber(string customerType, SearchOption option)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var customerObj = await _customerService.GetCustomerBySearchParam(customerType, option);

                return new ServiceResponse<object>
                {
                    Object = customerObj
                };
            });
        }

        [HttpPost]
        [Route("pricing")]
        public async Task<IServiceResponse<decimal>> GetPrice(PricingDTO pricingDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var userCountryId = await _pricing.GetUserCountryId();
                pricingDto.CountryId = userCountryId;
                var price = await _pricing.GetPrice(pricingDto);

                return new ServiceResponse<decimal>
                {
                    Object = price
                };
            });
        }


        [HttpPost]
        [Route("createshipment")]
        public async Task<IServiceResponse<ShipmentDTO>> AddShipment(NewShipmentDTO newShipmentDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {
                //map to real shipmentdto
                var ShipmentDTO = JObject.FromObject(newShipmentDTO).ToObject<ShipmentDTO>();
                //Update SenderAddress for corporate customers
                ShipmentDTO.SenderAddress = null;
                ShipmentDTO.SenderState = null;
                if (ShipmentDTO.Customer[0].CompanyType == CompanyType.Corporate)
                {
                    ShipmentDTO.SenderAddress = ShipmentDTO.Customer[0].Address;
                    ShipmentDTO.SenderState = ShipmentDTO.Customer[0].State;
                }

                //set some data to null
                ShipmentDTO.ShipmentCollection = null;
                ShipmentDTO.Demurrage = null;
                ShipmentDTO.Invoice = null;
                ShipmentDTO.ShipmentCancel = null;
                ShipmentDTO.ShipmentReroute = null;
                ShipmentDTO.DeliveryOption = null;

                var shipment = await _shipmentService.AddShipment(ShipmentDTO);
                return new ServiceResponse<ShipmentDTO>
                {
                    Object = shipment
                };
            });
        }


        [HttpPost]
        [Route("processpayment")]
        public async Task<IServiceResponse<bool>> ProcessPayment(PaymentTransactionDTO paymentDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _paymentService.ProcessPayment(paymentDto);

                return new ServiceResponse<bool>
                {
                    Object = result
                };
            });
        }

        [HttpGet]
        [Route("{customerwaybill}/waybill")]
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


        [HttpGet]
        [Route("shipmentcollection/{waybill}")]
        public async Task<IServiceResponse<ShipmentCollectionDTO>> GetShipmentCollectionByWaybill(string waybill)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var shipmentCollection = await _shipmentCollectionService.GetShipmentCollectionById(waybill);

                return new ServiceResponse<ShipmentCollectionDTO>
                {
                    Object = shipmentCollection
                };
            });
        }

        [HttpPut]
        [Route("releaseshipment")]
        public async Task<IServiceResponse<bool>> ReleaseShipment(ShipmentCollectionDTOForFastTrack shipmentCollectionforDto)
        {

            //map to real shipmentdto
            var shipmentCollection = JObject.FromObject(shipmentCollectionforDto).ToObject<ShipmentCollectionDTO>();
            shipmentCollection.ShipmentScanStatus = Core.Enums.ShipmentScanStatus.OKT;
            if (shipmentCollection.IsComingFromDispatch)
            {
                shipmentCollection.ShipmentScanStatus = Core.Enums.ShipmentScanStatus.OKC;
            }

            return await HandleApiOperationAsync(async () => {
                await _shipmentCollectionService.ReleaseShipmentForCollection(shipmentCollection);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [HttpGet]
        [Route("destinationcountry")]
        public async Task<IServiceResponse<IEnumerable<CountryDTO>>> GetDestinationCountry()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var countries = await _countryService.GetActiveCountries();

                return new ServiceResponse<IEnumerable<CountryDTO>>
                {
                    Object = countries
                };
            });
        }

        [HttpGet]
        [Route("servicecenterbycountry/{countryId:int}")]
        public async Task<IServiceResponse<IEnumerable<ServiceCentreDTO>>> GetServiceCentresWithoutHUBForNonLagosStation(int countryId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                //2. priviledged users service centres
                var usersServiceCentresId = await _userService.GetPriviledgeServiceCenters();

                var centres = await _portalService.GetServiceCentresBySingleCountry(countryId);
                return new ServiceResponse<IEnumerable<ServiceCentreDTO>>
                {
                    Object = centres
                };
            });
        }

        [HttpPost]
        [Route("getshipmentprice")]
        public async Task<IServiceResponse<NewPricingDTO>> GetShipmentPrice(NewShipmentDTO newShipmentDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var userCountryId = await _pricing.GetUserCountryId();
                newShipmentDTO.DepartureCountryId = userCountryId;
                var price = await _pricing.GetGrandPriceForShipment(newShipmentDTO);

                return new ServiceResponse<NewPricingDTO>
                {
                    Object = price
                };
            });
        }

    }
}
