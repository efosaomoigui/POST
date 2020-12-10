using GIGLS.Core.DTO;
using GIGLS.Core.DTO.Admin;
using GIGLS.Core.DTO.Customers;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.DTO.User;
using GIGLS.Core.DTO.Wallet;
using GIGLS.Core.Enums;
using GIGLS.Core.IServices;
using GIGLS.Core.IServices.CustomerPortal;
using GIGLS.Core.IServices.Website;
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
    [AllowAnonymous]
    [RoutePrefix("api/webtracking")]
    public class PublicTrackingController : BaseWebApiController
    {
        private readonly ICustomerPortalService _portalService;
        private readonly IWebsiteService _websiteService;

        public PublicTrackingController(ICustomerPortalService portalService, IWebsiteService websiteService) : base(nameof(PublicTrackingController))
        {
            _portalService = portalService;
            _websiteService = websiteService;
        }

        [HttpGet]
        [Route("reportsummary")]
        public async Task<IServiceResponse<AdminReportDTO>> GetWebsiteData()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var data = await _portalService.WebsiteData();
                return new ServiceResponse<AdminReportDTO>
                {
                    Object = data

                };
            });
        }

        [HttpGet]
        [Route("track/{waybillNumber}")]
        public async Task<IServiceResponse<IEnumerable<ShipmentTrackingDTO>>> PublicTrackShipment(string waybillNumber)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _portalService.PublicTrackShipment(waybillNumber);

                return new ServiceResponse<IEnumerable<ShipmentTrackingDTO>>
                {
                    Object = result
                };
            });
        }

        [HttpPost]
        [Route("schedulePickup")]
        public async Task<IServiceResponse<bool>> SendSchedulePickupMail(WebsiteMessageDTO obj)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _websiteService.SendSchedulePickupMail(obj);

                return new ServiceResponse<bool>
                {
                    Object = result
                };
            });
        }

        [HttpPost]
        [Route("requestQuote")]
        public async Task<IServiceResponse<bool>> SendRequestQuoteMail(WebsiteMessageDTO obj)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _websiteService.SendQuoteMail(obj);

                return new ServiceResponse<bool>
                {
                    Object = result
                };
            });
        }

        [HttpGet]
        [Route("trackshipment/{waybillNumber}")]
        public async Task<IServiceResponse<MobileShipmentTrackingHistoryDTO>> TrackMobileShipment(string waybillNumber)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _portalService.trackShipment(waybillNumber);

                return new ServiceResponse<MobileShipmentTrackingHistoryDTO>
                {
                    Object = result
                };
            });
        }

        [HttpPost]
        [Route("reportIssues")]
        public async Task<IServiceResponse<bool>> SendGIGGoIssuesMail(AppMessageDTO obj)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _websiteService.SendGIGGoIssuesMail(obj);

                return new ServiceResponse<bool>
                {
                    Object = result
                };
            });
        }

        [HttpGet]
        [Route("giggopresentdayshipments")]
        public async Task<IServiceResponse<List<LocationDTO>>> GetPresentDayShipmentLocations()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var preshipment = await _portalService.GetPresentDayShipmentLocations();
                return new ServiceResponse<List<LocationDTO>>
                {
                    Object = preshipment
                };
            });
        }

        [HttpGet]
        [Route("getwaybill/{waybillNumber}")]
        public async Task<IServiceResponse<ShipmentDetailDanfoDTO>> GetShipmentDetailForDanfo(string waybillNumber)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _portalService.GetShipmentDetailForDanfo(waybillNumber);

                return new ServiceResponse<ShipmentDetailDanfoDTO>
                {
                    Object = result
                };
            });
        }

        [HttpPost]
        [Route("ecommerceagreement")]
        public async Task<IServiceResponse<object>> AddEcommerceAgreement(EcommerceAgreementDTO ecommerceAgreementDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var response = new ServiceResponse<object>();
                var request = Request;
                var headers = request.Headers;
                var result = new object();
                if (headers.Contains("api_key"))
                {
                    
                    var key = await _portalService.EncryptWebsiteKey();
                    string token = headers.GetValues("api_key").FirstOrDefault();
                    if (token == key)
                    {
                        result = await _websiteService.AddEcommerceAgreement(ecommerceAgreementDTO);
                        response.Object = result;
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

        [HttpPost]
        [Route("AddIntlCustomer")]
        public async Task<IServiceResponse<object>> AddIntlCustomer(CustomerDTO customerDTO) 
        {
            return await HandleApiOperationAsync(async () =>
            {
                var response = new ServiceResponse<object>();
                var request = Request;
                var headers = request.Headers;
                var result = new object();

                if (!headers.Contains("api_key"))
                {
                    var key = "2";  // await _portalService.EncryptWebsiteKey();
                    string token = "2"; // headers.GetValues("api_key").FirstOrDefault();
                    if (token == key)
                    {
                        result = await _websiteService.AddIntlCustomer(customerDTO);
                        response.Object = result;
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

        [HttpPost]
        [Route("AddIntlShipmentTransactions")]
        public async Task<IServiceResponse<object>> AddIntlShipmentTransactions(IntlShipmentRequestDTO TransactionDTO)  
        {
            return await HandleApiOperationAsync(async () =>
            {
                var response = new ServiceResponse<object>();
                var request = Request;
                var headers = request.Headers;
                var result = new object();

                if (headers.Contains("api_key"))
                {
                    var key = await _portalService.EncryptWebsiteKey();
                    string token = headers.GetValues("api_key").FirstOrDefault();
                    if (token == key)
                    {
                        result = await _websiteService.AddIntlShipmentRequest(TransactionDTO);
                        response.Object = result;
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

        //Login for GIGGO App
        [HttpPost]
        [Route("login")]
        public async Task<IServiceResponse<JObject>> Login(MobileLoginDTO logindetail)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var user = await _portalService.CheckDetails(logindetail.UserDetail, logindetail.UserChannelType);

                if (user.RequiresCod == null)
                    user.RequiresCod = false;

                if (user.IsUniqueInstalled == null)
                    user.IsUniqueInstalled = false;

                if (user.IsEligible == null)
                    user.IsEligible = false;


                var vehicle = user.VehicleType;
                var vehicleDetails = user.VehicleDetails;
                var bankName = "";
                var accountName = "";
                var accountNumber = "";
                var partnerType = "";

                if (user.Username != null)
                {
                    user.Username = user.Username.Trim();
                }

                if (logindetail.Password != null)
                {
                    logindetail.Password = logindetail.Password.Trim();
                }

                if (user.UserChannelType == UserChannelType.Employee && user.SystemUserRole != "Dispatch Rider")
                {
                    throw new GenericException("You are not authorized to login on this platform.", $"{(int)HttpStatusCode.Forbidden}");
                }

                if (user != null && user.IsActive == true)
                {
                    using (var client = new HttpClient())
                    {
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
                            new KeyValuePair<string, string>("Password", logindetail.Password),
                        });

                        //setup login data
                        HttpResponseMessage responseMessage = await client.PostAsync("token", formContent);

                        if (!responseMessage.IsSuccessStatusCode)
                        {
                            throw new GenericException("Incorrect Login Details");
                        }
                        else
                        {
                            if (logindetail.UserChannelType == UserChannelType.IndividualCustomer.ToString())
                            {
                                var response = await _portalService.CreateCustomer(user.UserChannelCode);
                            }

                            if (logindetail.UserChannelType == UserChannelType.Partner.ToString())
                            {
                                var partner = await _portalService.CreatePartner(user.UserChannelCode);
                                partnerType = partner.PartnerType.ToString();
                                bankName = partner.BankName;
                                accountName = partner.AccountName;
                                accountNumber = partner.AccountNumber;
                                if (partnerType == PartnerType.InternalDeliveryPartner.ToString())
                                {
                                    user.IsVerified = true;
                                    await _portalService.AddWallet(new WalletDTO
                                    {
                                        CustomerId = partner.PartnerId,
                                        CustomerType = CustomerType.Partner,
                                        CustomerCode = user.UserChannelCode,
                                        CompanyType = CustomerType.Partner.ToString()
                                    });
                                }
                                if (logindetail.UserChannelType == UserChannelType.Ecommerce.ToString())
                                {
                                    var response = await _portalService.CreateCompany(user.UserChannelCode);
                                }
                            }

                            //get access token from response body
                            var responseJson = await responseMessage.Content.ReadAsStringAsync();
                            var jObject = JObject.Parse(responseJson);

                            getTokenResponse = jObject.GetValue("access_token").ToString();
                            return new ServiceResponse<JObject>
                            {
                                VehicleType = vehicle,
                                VehicleDetails = vehicleDetails,
                                Object = jObject,
                                ReferrerCode = user.Referrercode,
                                AverageRatings = user.AverageRatings,
                                IsVerified = user.IsVerified,
                                PartnerType = partnerType,
                                IsEligible = (bool)user.IsEligible,
                                BankName = bankName,
                                AccountName = accountName,
                                AccountNumber = accountNumber
                            };
                        }
                    }
                }
                else
                {
                    var data = new { IsActive = false };
                    var jObject = JObject.FromObject(data);

                    return new ServiceResponse<JObject>
                    {
                        Code = $"{(int)HttpStatusCode.BadRequest}",
                        ShortDescription = "User has not been verified",
                        Object = jObject
                    };
                }
            });
        }

        [HttpPost]
        [Route("signup")]
        public async Task<IServiceResponse<SignResponseDTO>> SignUp(UserDTO user)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var signUp = await _portalService.SignUp(user);

                return new ServiceResponse<SignResponseDTO>
                {
                    Object = signUp
                };
            });
        }

        [HttpPost]
        [Route("verifyotp")]
        public async Task<IServiceResponse<JObject>> ValidateOTP(OTPDTO otp)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var userDto = await _portalService.ValidateOTP(otp);
                if (userDto != null && userDto.IsActive == true)
                {
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
                            new KeyValuePair<string, string>("Username", userDto.Username),
                            new KeyValuePair<string, string>("Password", userDto.UserChannelPassword),
                        });

                        //setup login data
                        HttpResponseMessage responseMessage = await client.PostAsync("token", formContent);

                        if (!responseMessage.IsSuccessStatusCode)
                        {
                            throw new GenericException("Incorrect Login Details", $"{(int)HttpStatusCode.Forbidden}");
                        }
                        else
                        {
                            userDto = await _portalService.GenerateReferrerCode(userDto);
                        }

                        //get access token from response body
                        var responseJson = await responseMessage.Content.ReadAsStringAsync();
                        var jObject = JObject.Parse(responseJson);

                        getTokenResponse = jObject.GetValue("access_token").ToString();

                        return new ServiceResponse<JObject>
                        {
                            Object = jObject,
                            ReferrerCode = userDto.Referrercode
                        };
                    }
                }
                else
                {
                    var data = new { IsActive = false };

                    var jObject = JObject.FromObject(data);

                    return new ServiceResponse<JObject>
                    {
                        Code = $"{(int)HttpStatusCode.BadRequest}",
                        ShortDescription = "User has not been verified",
                        Object = jObject
                    };
                }
            });
        }

        [HttpPost]
        [Route("resendotp")]
        public async Task<IServiceResponse<SignResponseDTO>> ResendOTP(UserDTO user)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var resendOTP = await _portalService.ResendOTP(user);

                return new ServiceResponse<SignResponseDTO>
                {
                    Object = resendOTP
                };
            });
        }
    }
}
