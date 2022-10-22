using EfeAuthen.Models;
using POST.Core.Domain;
using POST.Core.DTO;
using POST.Core.DTO.Fleets;
using POST.Core.DTO.MessagingLog;
using POST.Core.DTO.Partnership;
using POST.Core.DTO.Report;
using POST.Core.DTO.User;
using POST.Core.Enums;
using POST.Core.IServices;
using POST.Core.IServices.CustomerPortal;
using POST.Core.IServices.Partnership;
using POST.Infrastructure;
using POST.Services.Implementation;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;

namespace POST.WebApi.Controllers.Partnership
{
    [Authorize]
    [RoutePrefix("api/fleetpartner")]
    public class FleetPartnerController : BaseWebApiController
    {
        private readonly IFleetPartnerService _fleetPartnerService;
        private readonly ICustomerPortalService _portalService;

        public FleetPartnerController(IFleetPartnerService fleetPartnerService, ICustomerPortalService portalService) : base(nameof(FleetPartnerController))
        {
            _fleetPartnerService = fleetPartnerService;
            _portalService = portalService;
        }

        [HttpGet]
        [Route("getcountofpartnersattachedtofleet")]
        public async Task<IServiceResponse<int>> GetCountOfPartnersUnderFleet()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var partners = await _fleetPartnerService.CountOfPartnersUnderFleet();

                return new ServiceResponse<int>
                {
                    Object = partners
                };
            });
        }
                
        [HttpGet]
        [Route("getvehiclesinfleet")]
        public async Task<IServiceResponse<IEnumerable<VehicleTypeDTO>>> GetVehiclesAttachedToFleetPartner()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var partners = await _fleetPartnerService.GetVehiclesAttachedToFleetPartner();
                return new ServiceResponse<IEnumerable<VehicleTypeDTO>>
                {
                    Object = partners
                };
            });
        }

        [HttpPost]
        [Route("transaction")]
        public async Task<IServiceResponse<List<FleetPartnerTransactionsDTO>>> GetFleetTransactions(ShipmentCollectionFilterCriteria filterCriteria)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var transactions = await _fleetPartnerService.GetFleetTransaction(filterCriteria);

                return new ServiceResponse<List<FleetPartnerTransactionsDTO>>
                {
                    Object = transactions
                };
            });
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("fleetlogin")]
        public async Task<IServiceResponse<JObject>> FleetLogin(UserloginDetailsModel userLoginModel)
        {
            return await HandleApiOperationAsync(async () =>
            {
                //I WOULD DO THIS PART TOMORROW
                //var user = await _portalService.CheckDetailsForCustomerPortal(userLoginModel.username);

                //if (user.Username != null)
                //{
                //    user.Username = user.Username.Trim();
                //}

                var user = await _portalService.CheckDetailsForCustomerPortal(userLoginModel.username);

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
                    HttpResponseMessage responseMessage = await client.PostAsync("token", formContent);

                    if (!responseMessage.IsSuccessStatusCode)
                    {
                        throw new GenericException("Incorrect Login Details");
                    }

                    //get access token from response body
                    var responseJson = await responseMessage.Content.ReadAsStringAsync();
                    var jObject = JObject.Parse(responseJson);

                    //Get country detail
                    var country = await _portalService.GetUserCountryCode(user);
                    var countryJson = JObject.FromObject(country);

                    //jObject.Add(countryJson);
                    jObject.Add(new JProperty("Country", countryJson));
                    getTokenResponse = jObject.GetValue("access_token").ToString();

                    return new ServiceResponse<JObject>
                    {
                        Object = jObject
                    };
                }
            });
        }

        [HttpPost]
        [Route("earnings")]
        public async Task<IServiceResponse<List<object>>> GetFleetEarnings(ShipmentCollectionFilterCriteria filterCriteria)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var earnings = await _fleetPartnerService.GetEarningsOfPartnersAttachedToFleet(filterCriteria);

                return new ServiceResponse<List<object>>
                {
                    Object = earnings
                };
            });
        }

        [HttpPost]
        [Route("partnerresponse")]
        public async Task<IServiceResponse<List<FleetMobilePickUpRequestsDTO>>> GetPartnersResponses(ShipmentCollectionFilterCriteria filterCriteria)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var earnings = await _fleetPartnerService.GetPartnerResponseAttachedToFleet(filterCriteria);

                return new ServiceResponse<List<FleetMobilePickUpRequestsDTO>>
                {
                    Object = earnings
                };
            });
        }

        [HttpGet]
        [Route("getverifiedpartners")]
        public async Task<IServiceResponse<IEnumerable<VehicleTypeDTO>>> GetVerifiedPartners()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var partners = await _fleetPartnerService.GetVerifiedPartners();
                return new ServiceResponse<IEnumerable<VehicleTypeDTO>>
                {
                    Object = partners
                };
            });
        }

        //Old Flow
        [AllowAnonymous]
        [HttpPost]
        [Route("forgotpassword")]
        public async Task<IServiceResponse<bool>> ForgotPassword(ForgotPasswordDTO user)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _portalService.ForgotPasswordV3(user);

                return new ServiceResponse<bool>
                {
                    Code = $"{(int)HttpStatusCode.OK}",
                    Object = result
                };
            });
        }

        [HttpPost]
        [Route("changepassword")]
        public async Task<IServiceResponse<bool>> ChangePassword(ChangePasswordDTO passwordDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _portalService.ChangePassword(passwordDTO);

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

        [HttpGet]
        [Route("getenterprisepartnersasset")]
        public async Task<IServiceResponse<IEnumerable<AssetDTO>>> GetFleetAttachedToEnterprisePartner()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var assets = await _fleetPartnerService.GetFleetAttachedToEnterprisePartner();
                return new ServiceResponse<IEnumerable<AssetDTO>>
                {
                    Object = assets
                };
            });
        }

        [HttpGet]
        [Route("getpartnerfleetbyid/{fleetid:int}")]
        public async Task<IServiceResponse<AssetDetailsDTO>> GetFleetAttachedToEnterprisePartnerById(int fleetid)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var assetDetails = await _fleetPartnerService.GetFleetAttachedToEnterprisePartnerById(fleetid);
                return new ServiceResponse<AssetDetailsDTO>
                {
                    Object = assetDetails
                };
            });
        }

        [HttpGet]
        [Route("getpartnerfleettrips/{fleetid:int}")]
        public async Task<IServiceResponse<IEnumerable<AssetTripDTO>>> GetFleetTrips(int fleetid)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var fleetTrips = await _fleetPartnerService.GetFleetTrips(fleetid);
                return new ServiceResponse<IEnumerable<AssetTripDTO>>
                {
                    Object = fleetTrips
                };
            });
        }

        [HttpGet]
        [Route("getpartnerwalletbalance")]
        public async Task<IServiceResponse<FleetPartnerWalletDTO>> GetPartnerWalletBalance()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _fleetPartnerService.GetPartnerWalletBalance();
                return new ServiceResponse<FleetPartnerWalletDTO>
                {
                    Object = result
                };
            });
        }

        [HttpGet]
        [Route("getpartnerfleettrips")]
        public async Task<IServiceResponse<IEnumerable<AssetTripDTO>>> GetPartnersFleetTrips()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var fleetTrips = await _fleetPartnerService.GetFleetTripsByPartner();
                return new ServiceResponse<IEnumerable<AssetTripDTO>>
                {
                    Object = fleetTrips
                };
            });
        }

        [HttpPost]
        [Route("getpartnertransactionhistory")]
        public async Task<IServiceResponse<IEnumerable<FleetPartnerTransactionDTO>>> GetPartnersTransactionHistory(FleetFilterCriteria filterCriteria)
        {
            return await HandleApiOperationAsync(async () =>
            {
                IEnumerable < FleetPartnerTransactionDTO > transactions = new List<FleetPartnerTransactionDTO>();
                
                if(filterCriteria == null)
                {
                    transactions = await _fleetPartnerService.GetFleetPartnerTransaction();
                }
                else if (filterCriteria.EndDate == null && filterCriteria.StartDate == null)
                {
                    transactions = await _fleetPartnerService.GetFleetPartnerTransaction();
                }
                else
                {
                    transactions = await _fleetPartnerService.GetFleetPartnerTransactionByDateRange(filterCriteria);
                }
                
                return new ServiceResponse<IEnumerable<FleetPartnerTransactionDTO>>
                {
                    Object = transactions
                };
            });
        }

        [HttpGet]
        [Route("getpartnercredittransactionhistory")]
        public async Task<IServiceResponse<IEnumerable<FleetPartnerTransactionDTO>>> GetPartnersCreditTransactionHistory()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var transactions = await _fleetPartnerService.GetFleetPartnerCreditTransaction();
                return new ServiceResponse<IEnumerable<FleetPartnerTransactionDTO>>
                {
                    Object = transactions
                };
            });
        }

        [HttpGet]
        [Route("getpartnerdebittransactionhistory")]
        public async Task<IServiceResponse<IEnumerable<FleetPartnerTransactionDTO>>> GetPartnersDebitTransactionHistory()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var transactions = await _fleetPartnerService.GetFleetPartnerDebitTransaction();
                return new ServiceResponse<IEnumerable<FleetPartnerTransactionDTO>>
                {
                    Object = transactions
                };
            });
        }
    }
}