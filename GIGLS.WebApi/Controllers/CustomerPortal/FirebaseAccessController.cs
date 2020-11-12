using GIGLS.Core.DTO;
using GIGLS.Core.DTO.Customers;
using GIGLS.Core.DTO.User;
using GIGLS.Core.IServices;
using GIGLS.Core.IServices.CustomerPortal;
using GIGLS.Infrastructure;
using GIGLS.Services.Implementation;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;

namespace GIGLS.WebApi.Controllers.CustomerPortal
{
    [RoutePrefix("api/firebase")]
    public class FirebaseAccessController : BaseWebApiController
    {
        private readonly ICustomerPortalService _portalService;

        public FirebaseAccessController(ICustomerPortalService portalService) : base(nameof(FirebaseAccessController))
        {

            _portalService = portalService;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("sendpickuprequestmessage/{userid}")]
        public async Task<IServiceResponse<bool>> SendPickUpRequestMessage([FromUri] string userId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var response = new ServiceResponse<bool>();
                var request = Request;
                var headers = request.Headers;
                if (headers.Contains("api_key"))
                {
                    var key = await _portalService.Decrypt();
                    string token = headers.GetValues("api_key").FirstOrDefault();
                    if (token == key)
                    {
                        await _portalService.SendPickUpRequestMessage(userId);
                        response.Object = true;
                    }
                }
                return response;
            });
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("addmobilepickuprequestfortimedoutrequests")]
        public async Task<IServiceResponse<bool>> AddPickupRequestForTimedOutRequest([FromBody] MobilePickUpRequestsDTO PickupRequest)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var response = new ServiceResponse<bool>();
                var request = Request;
                var headers = request.Headers;
                if (headers.Contains("api_key"))
                {
                    var key = await _portalService.Decrypt();
                    string token = headers.GetValues("api_key").FirstOrDefault();
                    if (token == key)
                    {
                        var shipmentItem = await _portalService.AddMobilePickupRequest(PickupRequest);
                        response.Object = true;
                    }
                }
                return response;
            });
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("shipmentreassignment")]
        public async Task<IServiceResponse<bool>> ChangeShipmentOwnershipForPartner(PartnerReAssignmentDTO pickupRequest)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var response = new ServiceResponse<bool>();
                var request = Request;
                var headers = request.Headers;
                if (headers.Contains("api_key"))
                {
                    var key = await _portalService.Decrypt();
                    string token = headers.GetValues("api_key").FirstOrDefault();
                    if (token == key)
                    {
                        var shipmentItem = await _portalService.ChangeShipmentOwnershipForPartner(pickupRequest);
                        response.Object = true;
                    }
                    else
                    {
                        throw new GenericException("Invalid key", $"{(int)HttpStatusCode.Unauthorized}");
                    }
                }
                else
                {
                    throw new GenericException("Unauthorized", $"{(int)HttpStatusCode.Unauthorized}");
                }
                return response;
            });
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("onboarduser")]
        public async Task<IServiceResponse<JObject>> UnboardUser(NewCompanyDTO company)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var response = new ServiceResponse<JObject>();
                var request = Request;
                var headers = request.Headers;
                if (headers.Contains("api_key"))
                {
                    var key = await _portalService.Decrypt();
                    string token = headers.GetValues("api_key").FirstOrDefault();
                    if (token == key)
                    {
                        var responseDTO = await _portalService.UnboardUser(company);
                        if (responseDTO.Succeeded)
                        {
                            using (var client = new HttpClient())
                            {
                                var user = await _portalService.GetUserByEmail(company.Email);
                                string apiBaseUri = ConfigurationManager.AppSettings["WebApiUrl"];
                                string getTokenResponse;
                                //setup client
                                client.BaseAddress = new Uri(apiBaseUri);
                                client.DefaultRequestHeaders.Accept.Clear();
                                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                                //setup login data
                                var formContent = new FormUrlEncodedContent(new[]
                                {
                            new KeyValuePair<string, string>("grant_type", "password"),
                            new KeyValuePair<string, string>("Username", user.Username),
                            new KeyValuePair<string, string>("Password", company.Password),
                        });

                                //setup login data
                                HttpResponseMessage responseMessage = await client.PostAsync("token", formContent);

                                if (!responseMessage.IsSuccessStatusCode)
                                {
                                    throw new GenericException("Incorrect Login Details");
                                }
                                else
                                {
                                    //get access token from response body
                                    var responseJson = await responseMessage.Content.ReadAsStringAsync();
                                    var jObject = JObject.Parse(responseJson);


                                    //jObject.Add(company);
                                    var userResponse = JObject.FromObject(response);
                                    getTokenResponse = jObject.GetValue("access_token").ToString();
                                    return new ServiceResponse<JObject>
                                    {
                                        Object = jObject
                                    };
                                }
                            }
                        }
                        else
                        {
                            var result = JObject.FromObject(responseDTO);
                            return new ServiceResponse<JObject>
                            { 
                                Object = result,
                            };
                        }
                    }
                    else
                    {
                        throw new GenericException("Invalid key", $"{(int)HttpStatusCode.Unauthorized}");
                    }
                }
                else
                {
                    throw new GenericException("Unauthorized", $"{(int)HttpStatusCode.Unauthorized}");
                }
                return response;
            });
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("validateuser")]
        public async Task<IServiceResponse<ResponseDTO>> ValidateUser([FromUri] string userDetail)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var response = new ServiceResponse<ResponseDTO>();
                var request = Request;
                var headers = request.Headers;
                if (headers.Contains("api_key"))
                {
                    var key = await _portalService.Decrypt();
                    string token = headers.GetValues("api_key").FirstOrDefault();
                    if (token == key)
                    {
                        var validateItem = await _portalService.ValidateUser(userDetail);
                        response.Object = validateItem;
                    }
                    else
                    {
                        throw new GenericException("Invalid key", $"{(int)HttpStatusCode.Unauthorized}");
                    }
                }
                else
                {
                    throw new GenericException("Unauthorized", $"{(int)HttpStatusCode.Unauthorized}");
                }
                return response;
            });
        }

        [AllowAnonymous]
        [HttpPut]
        [Route("updaterank")]
        public async Task<IServiceResponse<ResponseDTO>> UpdateUserRank(UserValidationDTO userValidationDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var response = new ServiceResponse<ResponseDTO>();
                var request = Request;
                var headers = request.Headers;
                if (headers.Contains("api_key"))
                {
                    var key = await _portalService.Decrypt();
                    string token = headers.GetValues("api_key").FirstOrDefault();
                    if (token == key)
                    {
                        var userItem = await _portalService.UpdateUserRank(userValidationDTO);
                        response.Object = userItem;
                    }
                    else
                    {
                        throw new GenericException("Invalid key", $"{(int)HttpStatusCode.Unauthorized}");
                    }
                }
                else
                {
                    throw new GenericException("Unauthorized", $"{(int)HttpStatusCode.Unauthorized}");
                }
                return response;
            });
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("sendmessage")]
        public async Task<IServiceResponse<bool>> SendMessage(NewMessageDTO newMessageDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var response = new ServiceResponse<bool>();
                var request = Request;
                var headers = request.Headers;
                if (headers.Contains("api_key"))
                {
                    var key = await _portalService.Decrypt();
                    string token = headers.GetValues("api_key").FirstOrDefault();
                    if (token == key)
                    {
                        var sendItem = await _portalService.SendMessage(newMessageDTO);
                        response.Object = sendItem;
                    }
                    else
                    {
                        throw new GenericException("Invalid key", $"{(int)HttpStatusCode.Unauthorized}");
                    }
                }
                else
                {
                    throw new GenericException("Unauthorized", $"{(int)HttpStatusCode.Unauthorized}");
                }
                return response;
            });
        }


    }

}

