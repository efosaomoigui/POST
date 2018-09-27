using EfeAuthen.Models;
using GIGLS.Core.DTO;
using GIGLS.Core.DTO.Account;
using GIGLS.Core.DTO.Customers;
using GIGLS.Core.DTO.Dashboard;
using GIGLS.Core.DTO.Haulage;
using GIGLS.Core.DTO.PaymentTransactions;
using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.DTO.Wallet;
using GIGLS.Core.DTO.Zone;
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
using System.Linq;
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



        [ThirdPartyActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("haulageprice")]
        private async Task<IServiceResponse<decimal>> GetHaulagePrice(HaulagePricingDTO haulagePricingDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var price = await _thirdPartyAPIService.GetHaulagePrice(haulagePricingDto);

                return new ServiceResponse<decimal>
                {
                    Object = price
                };
            });
        }




        //Route API


        //Track API


        //Invoice API
        [ThirdPartyActivityAuthorize(Activity = "View")]
        //[AllowAnonymous]
        [HttpGet]
        [Route("invoice")]
        private async Task<IServiceResponse<IEnumerable<InvoiceViewDTO>>> GetInvoices()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var invoice = await _thirdPartyAPIService.GetInvoices();

                return new ServiceResponse<IEnumerable<InvoiceViewDTO>>
                {
                    Object = invoice
                };
            });
        }

        [ThirdPartyActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("bywaybill/{waybill}")]
        private async Task<IServiceResponse<InvoiceDTO>> GetInvoiceByWaybill([FromUri]  string waybill)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var invoice = await _thirdPartyAPIService.GetInvoiceByWaybill(waybill);

                return new ServiceResponse<InvoiceDTO>
                {
                    Object = invoice
                };
            });
        }


        //Transaction History API
        [ThirdPartyActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("transaction")]
        private async Task<IServiceResponse<List<InvoiceViewDTO>>> GetShipmentTransactions(ShipmentFilterCriteria f_Criteria)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var invoices = await _thirdPartyAPIService.GetShipmentTransactions(f_Criteria);

                return new ServiceResponse<List<InvoiceViewDTO>>
                {
                    Object = invoices
                };
            });
        }


        //General API
        [ThirdPartyActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("dashboard")]
        private async Task<IServiceResponse<DashboardDTO>> GetDashboard()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var dashboard = await _thirdPartyAPIService.GetDashboard();

                return new ServiceResponse<DashboardDTO>
                {
                    Object = dashboard
                };
            });
        }

        //For Quick Quotes
        [ThirdPartyActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("wallet")]
        private async Task<IServiceResponse<WalletTransactionSummaryDTO>> GetWalletTransactions()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var walletTransactionSummary = await _thirdPartyAPIService.GetWalletTransactions();

                return new ServiceResponse<WalletTransactionSummaryDTO>
                {
                    Object = walletTransactionSummary
                };
            });
        }



        [ThirdPartyActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("cod")]
        private async Task<IServiceResponse<CashOnDeliveryAccountSummaryDTO>> GetCashOnDeliveryAccount()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _thirdPartyAPIService.GetCashOnDeliveryAccount();

                return new ServiceResponse<CashOnDeliveryAccountSummaryDTO>
                {
                    Object = result
                };
            });
        }

        [ThirdPartyActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("partialPaymentTransaction/{waybill}")]
        private async Task<IServiceResponse<IEnumerable<PaymentPartialTransactionDTO>>> GetPartialPaymentTransaction(string waybill)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var payment = await _thirdPartyAPIService.GetPartialPaymentTransaction(waybill);

                return new ServiceResponse<IEnumerable<PaymentPartialTransactionDTO>>
                {
                    Object = payment
                };
            });
        }

        [ThirdPartyActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("state")]
        private async Task<IServiceResponse<IEnumerable<StateDTO>>> GetStates(int pageSize = 10, int page = 1)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var state = await _thirdPartyAPIService.GetStates(pageSize, page);
                var total = _thirdPartyAPIService.GetStatesTotal();

                return new ServiceResponse<IEnumerable<StateDTO>>
                {
                    Total = total,
                    Object = state
                };
            });
        }

        [ThirdPartyActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("localservicecentre")]
        private async Task<IServiceResponse<IEnumerable<ServiceCentreDTO>>> GetLocalServiceCentres()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var centres = await _thirdPartyAPIService.GetLocalServiceCentres();
                return new ServiceResponse<IEnumerable<ServiceCentreDTO>>
                {
                    Object = centres
                };
            });
        }

        [ThirdPartyActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("deliveryoption")]
        private async Task<IServiceResponse<IEnumerable<DeliveryOptionDTO>>> GetDeliveryOptions()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var delivery = await _thirdPartyAPIService.GetDeliveryOptions();

                return new ServiceResponse<IEnumerable<DeliveryOptionDTO>>
                {
                    Object = delivery
                };
            });
        }

        [ThirdPartyActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("specialdomesticpackage")]
        private async Task<IServiceResponse<IEnumerable<SpecialDomesticPackageDTO>>> GetSpecialDomesticPackages()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var packages = await _thirdPartyAPIService.GetSpecialDomesticPackages();

                return new ServiceResponse<IEnumerable<SpecialDomesticPackageDTO>>
                {
                    Object = packages
                };
            });
        }

        [ThirdPartyActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("haulage")]
        private async Task<IServiceResponse<IEnumerable<HaulageDTO>>> GetHaulages()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var haulage = await _thirdPartyAPIService.GetHaulages();

                return new ServiceResponse<IEnumerable<HaulageDTO>>
                {
                    Object = haulage
                };
            });
        }

        [ThirdPartyActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("vat")]
        private async Task<IServiceResponse<VATDTO>> GetVATs()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var vat = await _thirdPartyAPIService.GetVATs();
                return new ServiceResponse<VATDTO>
                {
                    Object = vat.FirstOrDefault()
                };
            });
        }

        [ThirdPartyActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("insurance")]
        private async Task<IServiceResponse<InsuranceDTO>> GetInsurances()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var insurance = await _thirdPartyAPIService.GetInsurances();
                return new ServiceResponse<InsuranceDTO>
                {
                    Object = insurance.FirstOrDefault()
                };
            });
        }

        [ThirdPartyActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{departure:int}/{destination:int}")]
        private async Task<IServiceResponse<DomesticRouteZoneMapDTO>> GetZone(int departure, int destination)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var zone = await _thirdPartyAPIService.GetZone(departure, destination);

                return new ServiceResponse<DomesticRouteZoneMapDTO>
                {
                    Object = zone
                };
            });
        }

        [ThirdPartyActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("user/{userId}")]
        private async Task<IServiceResponse<CustomerDTO>> GetUser(string userId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var user = await _thirdPartyAPIService.GetCustomer(userId);
                return new ServiceResponse<CustomerDTO>
                {
                    Object = user
                };
            });
        }

        [ThirdPartyActivityAuthorize(Activity = "Update")]
        [HttpPut]
        [Route("changepassword/{userid}/{currentPassword}/{newPassword}")]
        private async Task<IServiceResponse<bool>> ChangePassword(string userid, string currentPassword, string newPassword)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _thirdPartyAPIService.ChangePassword(userid, currentPassword, newPassword);

                if (!result.Succeeded)
                {
                    throw new GenericException("Operation could not complete successfully");
                }

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }


        //////////////////////////////PUBLIC API//////////////////////////////////////////////////////////////////
        //Price API
        [ThirdPartyActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("price")]
        public async Task<IServiceResponse<decimal>> GetPrice(PricingDTO pricingDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var price = await _thirdPartyAPIService.GetPrice(pricingDto);

                return new ServiceResponse<decimal>
                {
                    Object = price
                };
            });
        }

        //Capture Shipment API
        [ThirdPartyActivityAuthorize(Activity = "Create")]
        [HttpPost]
        [Route("captureshipment")]
        public async Task<IServiceResponse<PreShipmentDTO>> AddPreShipment(PreShipmentDTO preShipmentDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var shipment = await _thirdPartyAPIService.AddPreShipment(preShipmentDTO);
                return new ServiceResponse<PreShipmentDTO>
                {
                    Object = shipment
                };
            });
        }

        [ThirdPartyActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("{waybillNumber}")]
        public async Task<IServiceResponse<IEnumerable<ShipmentTrackingDTO>>> TrackShipment(string waybillNumber)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _thirdPartyAPIService.TrackShipment(waybillNumber);

                return new ServiceResponse<IEnumerable<ShipmentTrackingDTO>>
                {
                    Object = result
                };
            });
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("public/{waybillNumber}")]
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


        [ThirdPartyActivityAuthorize(Activity = "View")]
        [HttpGet]
        [Route("LocalStations")]
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

        [AllowAnonymous]
        [HttpPost]
        [Route("user/login")]
        public async Task<IServiceResponse<JObject>> Login(UserloginDetailsModel userLoginModel)
        {
            //trim
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

                    //get access token from response body
                    var responseJson = await responseMessage.Content.ReadAsStringAsync();
                    var jObject = JObject.Parse(responseJson);

                    getTokenResponse = jObject.GetValue("access_token").ToString();

                    if (!responseMessage.IsSuccessStatusCode)
                    {
                        throw new GenericException("Operation could not complete login successfully:");
                    }

                    return new ServiceResponse<JObject>
                    {
                        Object = jObject
                    };
                }
            });
        }

    }
}
