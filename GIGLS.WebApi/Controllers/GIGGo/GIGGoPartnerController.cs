using GIGLS.Core.DTO;
using GIGLS.Core.DTO.MessagingLog;
using GIGLS.Core.DTO.Partnership;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.DTO.ShipmentScan;
using GIGLS.Core.DTO.User;
using GIGLS.Core.DTO.Wallet;
using GIGLS.Core.Enums;
using GIGLS.Core.IServices;
using GIGLS.Core.IServices.CustomerPortal;
using GIGLS.CORE.DTO.Shipments;
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
using System.Web;
using System.Web.Http;

namespace GIGLS.WebApi.Controllers.GIGGo
{
    [Authorize]
    [RoutePrefix("api/giggopartner")]
    public class GIGGoPartnerController : BaseWebApiController
    {
        private readonly ICustomerPortalService _portalService;

        public GIGGoPartnerController(ICustomerPortalService portalService) : base(nameof(GIGGoPartnerController))
        {
            _portalService = portalService;
        }

        [AllowAnonymous]
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

        [AllowAnonymous]
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

        [AllowAnonymous]
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

        [AllowAnonymous]
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

        [HttpPost]
        [Route("editprofile")]
        public async Task<IServiceResponse<bool>> EditProfile(UserDTO user)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var registerUser = await _portalService.EditProfile(user);

                return new ServiceResponse<bool>
                {
                    Object = registerUser
                };
            });
        }

        [HttpPost]
        [Route("updatevehicleprofile")]
        public async Task<IServiceResponse<bool>> UpdateVehicleProfile(UserDTO user)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _portalService.UpdateVehicleProfile(user);
                return new ServiceResponse<bool>
                {
                    Object = result
                };
            });
        }

        [HttpPost]
        [Route("addmobilepickuprequest")]
        public async Task<IServiceResponse<PreShipmentMobileDTO>> AddPickupRequest(MobilePickUpRequestsDTO PickupRequest)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var shipmentItem = await _portalService.AddMobilePickupRequest(PickupRequest);

                return new ServiceResponse<PreShipmentMobileDTO>
                {
                    Object = shipmentItem
                };
            });
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("itemTypes")]
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

        [HttpPost]
        [Route("updatemobilepickuprequests")]
        public async Task<IServiceResponse<object>> UpdatePickupRequests(MobilePickUpRequestsDTO PickupRequest)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var flag = await _portalService.UpdateMobilePickupRequest(PickupRequest);

                return new ServiceResponse<object>
                {
                    Object = flag
                };
            });
        }

        [HttpGet]
        [Route("getmobilepickuprequests")]
        public async Task<IServiceResponse<List<MobilePickUpRequestsDTO>>> GetPickupRequests()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var PickUpRequests = await _portalService.GetMobilePickupRequest();

                return new ServiceResponse<List<MobilePickUpRequestsDTO>>
                {
                    Object = PickUpRequests
                };
            });
        }

        [HttpPost]
        [Route("updatepreshipmentmobile")]
        public async Task<IServiceResponse<bool>> UpdatePreshipmentMobile(List<PreShipmentItemMobileDTO> preshipment)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var flag = await _portalService.UpdatePreShipmentMobileDetails(preshipment);

                return new ServiceResponse<bool>
                {
                    Object = flag
                };
            });
        }

        [HttpGet]
        [Route("getpartnerwallettransactions")]
        public async Task<IServiceResponse<SummaryTransactionsDTO>> GetPartnerwalletTransactions()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var totalTransactions = await _portalService.GetPartnerWalletTransactions();

                return new ServiceResponse<SummaryTransactionsDTO>
                {
                    Object = totalTransactions
                };
            });
        }

        [HttpPost]
        [Route("getprice")]
        public async Task<IServiceResponse<MobilePriceDTO>> GetPrice(PreShipmentMobileDTO PreshipmentMobile)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var Price = await _portalService.GetPrice(PreshipmentMobile);

                return new ServiceResponse<MobilePriceDTO>
                {
                    Object = Price,
                };
            });
        }

        [HttpPost]
        [Route("addratings")]
        public async Task<object> Addratings(MobileRatingDTO rating)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var flag = await _portalService.AddRatings(rating);

                return new ServiceResponse<object>
                {
                    Object = flag
                };
            });
        }

        [HttpGet]
        [Route("partnermonthlytransactions")]
        public async Task<IServiceResponse<Partnerdto>> PartnerMonthlyTransactions()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var transactions = await _portalService.GetMonthlyPartnerTransactions();

                return new ServiceResponse<Partnerdto>
                {
                    Object = transactions
                };
            });
        }

        [HttpGet]
        [Route("getpreshipmentmobiledetails/{waybillNumber}")]
        public async Task<IServiceResponse<PreShipmentMobileDTO>> GetPreshipmentMobileDetails(string waybillNumber)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var details = await _portalService.GetPreShipmentDetail(waybillNumber);

                return new ServiceResponse<PreShipmentMobileDTO>
                {
                    Object = details
                };
            });
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("forgotpassword")]
        public async Task<IServiceResponse<bool>> ForgotPassword(UserDTO user)
        {
            return await HandleApiOperationAsync(async () =>
            {
                if (string.IsNullOrWhiteSpace(user.Email))
                {
                    throw new GenericException("NULL INPUT", $"{(int)HttpStatusCode.BadRequest}");
                }

                string password = await _portalService.Generate(6);
                var User = await _portalService.ForgotPassword(user.Email, password);

                if (User.Succeeded)
                {
                    var passwordMessage = new PasswordMessageDTO()
                    {
                        Password = password,
                        UserEmail = user.Email
                    };

                    await _portalService.SendGenericEmailMessage(MessageType.PEmail, passwordMessage);
                }
                else
                {
                    throw new GenericException("Information does not exist, kindly provide correct email", $"{(int)HttpStatusCode.NotFound}");
                }

                return new ServiceResponse<bool>
                {
                    Code = $"{(int)HttpStatusCode.OK}",
                    Object = true
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

        [HttpPost]
        [Route("adddeliverynumber")]
        public async Task<IServiceResponse<bool>> UpdateDeliveryNumber(MobileShipmentNumberDTO detail)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var response = await _portalService.UpdateDeliveryNumber(detail);

                return new ServiceResponse<bool>
                {
                    Object = response
                };
            });
        }

        [HttpPost]
        [Route("updatereceiverdetails")]
        public async Task<IServiceResponse<bool>> UpdateReceiverDetails(PreShipmentMobileDTO receiver)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var response = await _portalService.UpdateReceiverDetails(receiver);
                return new ServiceResponse<bool>
                {
                    Object = response
                };
            });
        }

        [HttpPost]
        [Route("cancelshipmentwithnocharge")]
        public async Task<object> CancelShipmentWithNoCharge(CancelShipmentDTO shipment)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var flag = await _portalService.CancelShipmentWithNoCharge(shipment);

                return new ServiceResponse<object>
                {
                    Object = flag
                };
            });
        }

        [HttpGet]
        [Route("scanstatus")]
        public async Task<IServiceResponse<IEnumerable<ScanStatusDTO>>> GetScanStatus()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var scanStatus = await _portalService.GetScanStatus();

                return new ServiceResponse<IEnumerable<ScanStatusDTO>>
                {
                    Object = scanStatus
                };
            });
        }

        [HttpPost]
        [Route("scanmultiple")]
        public async Task<IServiceResponse<bool>> ScanMultipleShipment(List<ScanDTO> scanList)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _portalService.ScanMultipleShipment(scanList);

                return new ServiceResponse<bool>
                {
                    Object = result
                };
            });
        }

        [HttpGet]
        [Route("waybillsinmanifestfordispatch")]
        public async Task<IServiceResponse<List<ManifestWaybillMappingDTO>>> GetWaybillsInManifestForDispatch()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var groupWaybillNumbersInManifest = await _portalService.GetWaybillsInManifestForDispatch();

                return new ServiceResponse<List<ManifestWaybillMappingDTO>>
                {
                    Object = groupWaybillNumbersInManifest
                };
            });
        }

        [HttpPut]
        [Route("collected")]
        public async Task<IServiceResponse<bool>> ReleaseShipmentForCollection(ShipmentCollectionDTO shipmentCollection)
        {
            return await HandleApiOperationAsync(async () => {
                await _portalService.ReleaseShipmentForCollectionOnScanner(shipmentCollection);
                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [HttpGet]
        [Route("getlogvisit")]
        public async Task<IServiceResponse<List<LogVisitReasonDTO>>> GetLogVisitReasons()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var logVisitReasons = await _portalService.GetLogVisitReasons();

                return new ServiceResponse<List<LogVisitReasonDTO>>
                {
                    Object = logVisitReasons
                };
            });
        }

        [HttpPost]
        [Route("logwaybillvisit")]
        public async Task<IServiceResponse<object>> AddManifest(ManifestVisitMonitoringDTO manifestVisitMonitoringDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var manifest = await _portalService.AddManifestVisitMonitoring(manifestVisitMonitoringDTO);
                return new ServiceResponse<object>
                {
                    Object = manifest
                };
            });
        }


    }

}