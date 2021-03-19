using EfeAuthen.Models;
using GIGLS.Core.DTO;
using GIGLS.Core.DTO.Account;
using GIGLS.Core.DTO.PaymentTransactions;
using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.DTO.Zone;
using GIGLS.Core.Enums;
using GIGLS.Core.IServices;
using GIGLS.Core.IServices.CustomerPortal;
using GIGLS.Core.IServices.TickectMan;
using GIGLS.CORE.DTO.Report;
using GIGLS.CORE.DTO.Shipments;
using GIGLS.Infrastructure;
using GIGLS.Services.Implementation;
using GIGLS.Services.Implementation.Utility;
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
        private readonly ITickectManService _tickectMan;
       // private readonly ICustomerPortalService _portalService;

        public GIGLExpressController(ITickectManService tickectMan, ICustomerPortalService portalService) : base(nameof(GIGLExpressController))
        {
            _tickectMan = tickectMan;
           // _portalService = portalService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<IServiceResponse<JObject>> Login(UserloginDetailsModel userLoginModel)
        {
            var user = await _tickectMan.CheckDetailsForMobileScanner(userLoginModel.username);

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

                    //ADD SERVICECENTRE OBJ
                    var centreId = await _tickectMan.GetPriviledgeServiceCenters(user.Id);
                    if (centreId != null)
                    {
                        var centreInfo =  await _tickectMan.GetServiceCentreById(centreId[0]);
                        if (centreInfo != null)
                        {
                            var centreInfoJson = JObject.FromObject(centreInfo);
                            jObject.Add(new JProperty("ServiceCentre", centreInfoJson));
                        }
                    }

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
                var deliveryOptionPrices = await _tickectMan.GetDeliveryOptionPrices();

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
                var zone = await _tickectMan.GetZone(departure, destination);

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
                var shipmentPackagePrices = await _tickectMan.GetShipmentPackagePrices();

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
                var customerObj = await _tickectMan.GetCustomerBySearchParam(customerType, option);

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
                var price = await _tickectMan.GetPrice(pricingDto);

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
                 var shipment = await _tickectMan.AddShipment(newShipmentDTO);
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
                var result = await _tickectMan.ProcessPayment(paymentDto);

                return new ServiceResponse<bool>
                {
                    Object = result
                };
            });
        }

        [HttpGet]
        [Route("customerwaybill/{waybill}")]
        public async Task<IServiceResponse<ShipmentDTO>> GetShipment(string waybill)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var shipment = await _tickectMan.GetShipment(waybill);
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
                var shipmentCollection = await _tickectMan.GetShipmentCollectionById(waybill);
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
            return await HandleApiOperationAsync(async () => {
                await _tickectMan.ReleaseShipmentForCollection(shipmentCollectionforDto);
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
                var countries = await _tickectMan.GetActiveCountries();

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
                var centres = await _tickectMan.GetActiveServiceCentresBySingleCountry(countryId);
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
                var price = await _tickectMan.GetGrandPriceForShipment(newShipmentDTO);
                return new ServiceResponse<NewPricingDTO>
                {
                    Object = price
                };
            });
        }

        [HttpGet]
        [Route("paymenttypes")]
        public IHttpActionResult GetPaymentTypes()
        {
            var types = EnumExtensions.GetValues<PaymentType>();
           // types.RemoveAt(3);
            return Ok(types);
        }

        [HttpPost]
        [Route("processpartialpayment")]
        public async Task<IServiceResponse<bool>> ProcessPaymentPartial(PaymentPartialTransactionProcessDTO paymentPartialTransactionProcessDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _tickectMan.ProcessPaymentPartial(paymentPartialTransactionProcessDTO);

                return new ServiceResponse<bool>
                {
                    Object = result
                };
            });
        }

        [HttpGet]
        [Route("lga")]
        public async Task<IServiceResponse<IEnumerable<LGADTO>>> GetLGAs()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var lga = await _tickectMan.GetLGAs();
                return new ServiceResponse<IEnumerable<LGADTO>>
                {
                    Object = lga
                };
            });
        }

        [HttpGet]
        [Route("activespecialpackage")]
        public async Task<IServiceResponse<IEnumerable<SpecialDomesticPackageDTO>>> GetActiveSpecialDomesticPackages()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var packages = await _tickectMan.GetActiveSpecialDomesticPackages();
                return new ServiceResponse<IEnumerable<SpecialDomesticPackageDTO>>
                {
                    Object = packages
                };
            });
        }

        [HttpGet]
        [Route("natureofgoods")]
        public IHttpActionResult GetNatureOfGoods()
        {
            var types = EnumExtensions.GetValues<NatureOfGoods>();
            return Ok(types);
        }

        [HttpGet]
        [Route("preshipment/{code}")]
        public async Task<IServiceResponse<ShipmentDTO>> GetDropOffShipment(string code)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var shipment = await _tickectMan.GetDropOffShipmentForProcessing(code);
                return new ServiceResponse<ShipmentDTO>
                {
                    Object = shipment
                };
            });
        }

        [HttpGet]
        [Route("waybillbyservicecentre/{waybill}")]
        public async Task<IServiceResponse<DailySalesDTO>> GetDailySaleByWaybillForServiceCentre(string waybill)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var shipment = await _tickectMan.GetWaybillForServiceCentre(waybill);
                return new ServiceResponse<DailySalesDTO>
                {
                    Object = shipment
                };
            });
        }

        [HttpPost]
        [Route("dailysalesforservicecentre")]
        public async Task<IServiceResponse<DailySalesDTO>> GetSalesForServiceCentre(DateFilterForDropOff dateFilterCriteria)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var dailySales = await _tickectMan.GetSalesForServiceCentre(dateFilterCriteria);
                return new ServiceResponse<DailySalesDTO>
                {
                    Object = dailySales
                };
            });
        }

        [HttpPost]
        [Route("dropoffprice")]
        public async Task<IServiceResponse<MobilePriceDTO>> GetPriceForDropOff(PreShipmentMobileDTO preshipmentMobile)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var Price = await _tickectMan.GetPriceForDropOff(preshipmentMobile);
                return new ServiceResponse<MobilePriceDTO>
                {
                    Object = Price,
                };
            });
        }

        [HttpGet]
        [Route("getpreshipmentmobiledetailsfromdeliverynumber/{deliverynumber}")]
        public async Task<IServiceResponse<PreShipmentSummaryDTO>> GetPreshipmentmobiledetailsfromdeliverynumber(string deliverynumber)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var preshipment = await _tickectMan.GetShipmentDetailsFromDeliveryNumber(deliverynumber);
                return new ServiceResponse<PreShipmentSummaryDTO>
                {
                    Object = preshipment
                };
            });
        }

        [HttpPost]
        [Route("approveshipment")]
        public async Task<IServiceResponse<bool>> Approveshipment(ApproveShipmentDTO detail)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _tickectMan.ApproveShipment(detail);
                return new ServiceResponse<bool>
                {
                    Object = result
                };
            });
        }

        [HttpGet]
        [Route("servicecenterbystation/{stationId:int}")]
        public async Task<IServiceResponse<IEnumerable<ServiceCentreDTO>>> GetServiceCentresByStation(int stationId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var centres = await _tickectMan.GetServiceCentreByStation(stationId);
                return new ServiceResponse<IEnumerable<ServiceCentreDTO>>
                {
                    Object = centres
                };
            });
        }

        [HttpGet]
        [Route("vehicletypes")]
        public IHttpActionResult GetVehicleTypes()
        {
            var types = EnumExtensions.GetValues<Vehicletype>();
            return Ok(types);
        }

        [HttpPost]
        [Route("giggoextension")]
        public async Task<IServiceResponse<ShipmentDTO>> AddGIGGOShipmentFromAgility(PreShipmentMobileFromAgilityDTO ShipmentDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var shipment = await _tickectMan.AddAgilityShipmentToGIGGo(ShipmentDTO);
                return new ServiceResponse<ShipmentDTO>
                {
                    Object = shipment
                };
            });
        }

        [HttpPost]
        [Route("getgiggoprice")]
        public async Task<IServiceResponse<MobilePriceDTO>> GetGIGGoPrice(PreShipmentMobileDTO preshipmentMobile)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var price = await _tickectMan.GetGIGGOPrice(preshipmentMobile);
                return new ServiceResponse<MobilePriceDTO>
                {
                    Object = price,
                };
            });
        }

        [HttpPost]
        [Route("bulkpayment")]
        public async Task<IServiceResponse<bool>> ProcessBulkPaymentforWaybills(BulkWaybillPaymentDTO bulkWaybillPaymentDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _tickectMan.ProcessBulkPaymentforWaybills(bulkWaybillPaymentDTO);

                return new ServiceResponse<bool>
                {
                    Object = result
                };
            });
        }

        [HttpGet]
        [Route("unpaidwaybillbyservicecentre")]
        public async Task<IServiceResponse<List<InvoiceViewDTO>>> GetInvoiceByServiceCentre()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var invoices = await _tickectMan.GetInvoiceByServiceCentre();
                return new ServiceResponse<List<InvoiceViewDTO>>
                {
                    Object = invoices
                };
            });
        }


    }
}
