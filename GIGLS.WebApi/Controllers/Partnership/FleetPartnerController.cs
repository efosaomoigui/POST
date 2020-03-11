using EfeAuthen.Models;
using GIGLS.Core.DTO.Partnership;
using GIGLS.Core.DTO.Report;
using GIGLS.Core.IServices;
using GIGLS.Core.IServices.Partnership;
using GIGLS.Infrastructure;
using GIGLS.Services.Implementation;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;

namespace GIGLS.WebApi.Controllers.Partnership
{
    //[Authorize]
    [RoutePrefix("api/fleetpartner")]
    public class FleetPartnerController : BaseWebApiController
    {
        private readonly IFleetPartnerService _fleetPartnerService;
        public FleetPartnerController(IFleetPartnerService fleetPartnerService) : base(nameof(FleetPartnerController))
        {
            _fleetPartnerService = fleetPartnerService;
        }

        [HttpPost]
        [Route("")]
        public async Task<IServiceResponse<object>> AddFleetPartner(FleetPartnerDTO fleetPartnerDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var partner = await _fleetPartnerService.AddFleetPartner(fleetPartnerDTO);

                return new ServiceResponse<object>
                {
                    Object = partner
                };
            });
        }

        [HttpGet]
        [Route("{partnerId:int}")]
        public async Task<IServiceResponse<FleetPartnerDTO>> GetFleetPartner(int fleetPartnerId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var partner = await _fleetPartnerService.GetFleetPartnerById(fleetPartnerId);

                return new ServiceResponse<FleetPartnerDTO>
                {
                    Object = partner
                };
            });
        }

        [HttpDelete]
        [Route("{partnerId:int}")]
        public async Task<IServiceResponse<bool>> DeleteFleetPartner(int fleetPartnerId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _fleetPartnerService.RemoveFleetPartner(fleetPartnerId);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [HttpPut]
        [Route("{partnerId:int}")]
        public async Task<IServiceResponse<bool>> UpdateFleetPartner(int fleetPartnerId, FleetPartnerDTO fleetPartnerDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _fleetPartnerService.UpdateFleetPartner(fleetPartnerId, fleetPartnerDTO);

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }
               
        [HttpGet]
        [Route("getallfleetpartners")]
        public async Task<IServiceResponse<IEnumerable<FleetPartnerDTO>>> GetAllFleetPartners()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var partners = await _fleetPartnerService.GetFleetPartners();
                return new ServiceResponse<IEnumerable<FleetPartnerDTO>>
                {
                    Object = partners
                };
            });
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
        public async Task<IServiceResponse<List<PartnerTransactionsDTO>>> GetFleetTransactions(ShipmentCollectionFilterCriteria filterCriteria)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var transactions = await _fleetPartnerService.GetFleetTransaction(filterCriteria);

                return new ServiceResponse<List<PartnerTransactionsDTO>>
                {
                    Object = transactions
                };
            });
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("fleetLogin")]
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

                    getTokenResponse = jObject.GetValue("access_token").ToString();

                    return new ServiceResponse<JObject>
                    {
                        Object = jObject
                    };
                }
            });
        }

    }
}