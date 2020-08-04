

using GIGLS.Core.DTO;
using GIGLS.Core.DTO.Haulage;
using GIGLS.Core.DTO.MessagingLog;
using GIGLS.Core.DTO.PaymentTransactions;
using GIGLS.Core.DTO.Report;
using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.DTO.User;
using GIGLS.Core.DTO.Wallet;
using GIGLS.Core.DTO.Zone;
using GIGLS.Core.Enums;
using GIGLS.Core.IServices;
using GIGLS.Core.IServices.CustomerPortal;
using GIGLS.Infrastructure;
using GIGLS.Services.Implementation;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;

namespace GIGLS.WebApi.Controllers.GIGGo
{
    [Authorize]
    [RoutePrefix("api/giggocustomer")]
    public class GIGGoCustomerController : BaseWebApiController
    {
        private readonly ICustomerPortalService _portalService;

        public GIGGoCustomerController(ICustomerPortalService portalService) : base(nameof(GIGGoCustomerController))
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
        [Route("haulage")]
        public async Task<IServiceResponse<IEnumerable<HaulageDTO>>> GetHaulages()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var haulage = await _portalService.GetHaulages();

                return new ServiceResponse<IEnumerable<HaulageDTO>>
                {
                    Object = haulage
                };
            });
        }

        [HttpGet]
        [Route("getwalletbalance")]
        public async Task<IServiceResponse<decimal>> GetWalletBalance()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var wallet = await _portalService.GetWalletBalance();
                return new ServiceResponse<decimal>
                {
                    Object = wallet.Balance
                };
            });
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("getStations")]
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

        [AllowAnonymous]
        [HttpGet]
        [Route("getactivelgas")]
        public async Task<IServiceResponse<IEnumerable<LGADTO>>> GetActiveLGAs()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var lga = await _portalService.GetActiveLGAs();
                return new ServiceResponse<IEnumerable<LGADTO>>
                {
                    Object = lga

                };
            });
        }

        [HttpPost]
        [Route("gethaulagepriceformobile")]
        public async Task<IServiceResponse<MobilePriceDTO>> GetHaulagePrice(HaulagePriceDTO haulage)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var haulagePrice = await _portalService.GetHaulagePrice(haulage);
                return new ServiceResponse<MobilePriceDTO>
                {
                    Object = haulagePrice
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

        [HttpPost]
        [Route("getwallettransactionandpreshipmenthistory")]
        public async Task<IServiceResponse<ModifiedWalletTransactionSummaryDTO>> GetWalletTransactionAndPreshipmentHistory(ShipmentCollectionFilterCriteria filterCriteria)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var Transactionhistory = await _portalService.GetWalletTransactionsForMobile(filterCriteria);
                return new ServiceResponse<ModifiedWalletTransactionSummaryDTO>
                {
                    Object = Transactionhistory
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

        [HttpGet]
        [Route("getpreshipmentindispute")]
        public async Task<IServiceResponse<List<PreShipmentMobileDTO>>> GetPreshipmentInDispute()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var shipments = await _portalService.GetDisputePreShipment();

                return new ServiceResponse<List<PreShipmentMobileDTO>>
                {
                    Object = shipments
                };
            });
        }

        [HttpPost]
        [Route("addwalletpaymentlog")]
        public async Task<IServiceResponse<object>> AddWalletPaymentLog(WalletPaymentLogDTO walletPaymentLogDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var walletPaymentLog = await _portalService.AddWalletPaymentLog(walletPaymentLogDTO);

                return new ServiceResponse<object>
                {
                    Object = walletPaymentLog
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
        [Route("dropoffprice")]
        public async Task<IServiceResponse<MobilePriceDTO>> GetPriceForDropOff(PreShipmentMobileDTO PreshipmentMobile)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var Price = await _portalService.GetPriceForDropOff(PreshipmentMobile);

                return new ServiceResponse<MobilePriceDTO>
                {
                    Object = Price,
                };
            });
        }

        [HttpPost]
        [Route("createdropoff")]
        public async Task<IServiceResponse<bool>> CreateOrUpdateDropOff(PreShipmentDTO preShipmentDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var preshipMentMobile = await _portalService.CreateOrUpdateDropOff(preShipmentDTO);

                return new ServiceResponse<bool>
                {
                    Object = preshipMentMobile
                };
            });
        }

        [HttpPost]
        [Route("createshipment")]
        public async Task<IServiceResponse<object>> CreateShipment(PreShipmentMobileDTO PreshipmentMobile)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var PreshipMentMobile = await _portalService.AddPreShipmentMobile(PreshipmentMobile);

                return new ServiceResponse<object>
                {
                    Object = PreshipMentMobile
                };
            });
        }

        [HttpPost]
        [Route("resolvedispute")]
        public async Task<object> ResolveDispute(PreShipmentMobileDTO shipment)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var flag = await _portalService.ResolveDisputeForMobile(shipment);

                return new ServiceResponse<object>
                {
                    Object = flag
                };
            });
        }

        [HttpPost]
        [Route("cancelshipment/{waybillNumber}")]
        public async Task<object> CancelShipment(string waybillNumber)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var flag = await _portalService.CancelShipment(waybillNumber);

                return new ServiceResponse<object>
                {
                    Object = flag
                };
            });
        }


    }

}