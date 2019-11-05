﻿using GIGLS.Core.DTO;
using GIGLS.Core.DTO.Account;
using GIGLS.Core.DTO.Customers;
using GIGLS.Core.DTO.Dashboard;
using GIGLS.Core.DTO.Haulage;
using GIGLS.Core.DTO.OnlinePayment;
using GIGLS.Core.DTO.PaymentTransactions;
using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.DTO.SLA;
using GIGLS.Core.DTO.User;
using GIGLS.Core.DTO.Wallet;
using GIGLS.Core.DTO.Zone;
using GIGLS.Core.IServices;
using GIGLS.Core.IServices.Shipments;
using GIGLS.Core.IServices.CustomerPortal;
using GIGLS.Core.IServices.User;
using GIGLS.Core.IServices.Wallet;
using GIGLS.CORE.DTO.Shipments;
using GIGLS.Infrastructure;
using GIGLS.Services.Implementation;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using GIGLS.Core.IServices.ServiceCentres;
using GIGLS.Core.Domain.BankSettlement;
using GIGLS.Core.DTO.Partnership;
using GIGLS.Core.DTO.Report;
using GIGLS.Core.Enums;
using GIGLS.Core.DTO.MessagingLog;
using GIGLS.Core.IServices.Utility;
using GIGLS.Core.IMessageService;
using GIGLS.Core.DTO.Admin;

namespace GIGLS.WebApi.Controllers.CustomerPortal
{
    [Authorize]
    [RoutePrefix("api/portal")]
    public class CustomerPortalController : BaseWebApiController
    {
        //private readonly IUnitOfWork _uow;
        private readonly ICustomerPortalService _portalService;
        private readonly IPaystackPaymentService _paymentService;



        public CustomerPortalController(ICustomerPortalService portalService, IPaystackPaymentService paymentService) : base(nameof(CustomerPortalController))
        {
            // _uow = uow;
            _portalService = portalService;
            _paymentService = paymentService;


        }


        [HttpPost]
        [Route("transaction")]
        public async Task<IServiceResponse<List<InvoiceViewDTO>>> GetShipmentTransactions(ShipmentCollectionFilterCriteria f_Criteria)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var invoices = await _portalService.GetShipmentTransactions(f_Criteria);

                return new ServiceResponse<List<InvoiceViewDTO>>
                {
                    Object = invoices
                };
            });
        }

        //[Authorize]
        [HttpPut]
        [Route("wallet/{walletId:int}")]
        public async Task<IServiceResponse<object>> UpdateWallet(int walletId, WalletTransactionDTO walletTransactionDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {
                await _portalService.UpdateWallet(walletId, walletTransactionDTO);
                return new ServiceResponse<object>
                {
                    Object = true
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
        [Route("paywithpaystack")]
        public async Task<IServiceResponse<object>> PaywithPaystack(WalletPaymentLogDTO paymentinfo)
        {
            return await HandleApiOperationAsync(async () =>
            {

                //Add wallet payment log
                var walletPaymentLog = await _portalService.AddWalletPaymentLog(paymentinfo);

                //initialize the secret key from paystack
                var testOrLiveSecret = ConfigurationManager.AppSettings["PayStackSecret"];

                //Call the paystack class implementation to do the payment
                var result = await _paymentService.MakePayment(testOrLiveSecret, paymentinfo);
                var updateresult = new object();

                if (result)
                {
                    paymentinfo.TransactionStatus = "Success";
                    updateresult = await _portalService.UpdateWalletPaymentLog(paymentinfo);
                }

                return new ServiceResponse<object>
                {
                    Object = updateresult
                };
            });
        }


        [HttpGet]
        [Route("verifypayment/{referenceCode}")]
        public async Task<IServiceResponse<PaymentResponse>> VerifyAndValidateWallet(string referenceCode)
        {
            //return await HandleApiOperationAsync(async () =>
            //{
            var result = await _paymentService.VerifyAndValidateWallet(referenceCode);

            return new ServiceResponse<PaymentResponse>
            {
                Object = result
            };
            //});
        }


        [HttpGet]
        [Route("walletpaymentlog")]
        public async Task<IServiceResponse<List<WalletPaymentLogDTO>>> GetWalletPaymentLogs([FromUri]FilterOptionsDto filterOptionsDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var walletTuple = await _portalService.GetWalletPaymentLogs(filterOptionsDto);
                return new ServiceResponse<List<WalletPaymentLogDTO>>
                {
                    Object = await walletTuple.Item1,
                    Total = walletTuple.Item2
                };
            });
        }

        [HttpPut]
        [Route("updatewalletpaymentlog")]
        public async Task<IServiceResponse<object>> UpdateWalletPaymentLog(WalletPaymentLogDTO walletPaymentLogDTO)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var walletPaymentLog = await _portalService.UpdateWalletPaymentLog(walletPaymentLogDTO);

                return new ServiceResponse<object>
                {
                    Object = walletPaymentLog
                };
            });
        }


        [HttpGet]
        [Route("wallet")]
        public async Task<IServiceResponse<WalletTransactionSummaryDTO>> GetWalletTransactions()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var walletTransactionSummary = await _portalService.GetWalletTransactions();

                return new ServiceResponse<WalletTransactionSummaryDTO>
                {
                    Object = walletTransactionSummary
                };
            });
        }

        [HttpGet]
        [Route("invoice")]
        public async Task<IServiceResponse<IEnumerable<InvoiceViewDTO>>> GetInvoices()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var invoice = await _portalService.GetInvoices();

                return new ServiceResponse<IEnumerable<InvoiceViewDTO>>
                {
                    Object = invoice
                };
            });
        }

        [HttpGet]
        [Route("bywaybill/{waybill}")]
        public async Task<IServiceResponse<InvoiceDTO>> GetInvoiceByWaybill([FromUri]  string waybill)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var invoice = await _portalService.GetInvoiceByWaybill(waybill);

                return new ServiceResponse<InvoiceDTO>
                {
                    Object = invoice
                };
            });
        }

        [HttpGet]
        [Route("GetPaidCODByCustomer")]
        public async Task<IServiceResponse<List<CodPayOutList>>> GetPaidCODByCustomer()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var invoice = await _portalService.GetPaidCODByCustomer();

                return new ServiceResponse<List<CodPayOutList>>
                {
                    Object = invoice
                };
            });
        }

        [HttpGet]
        [Route("{waybillNumber}")]
        public async Task<IServiceResponse<IEnumerable<ShipmentTrackingDTO>>> TrackShipment(string waybillNumber)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _portalService.TrackShipment(waybillNumber);

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
                var result = await _portalService.PublicTrackShipment(waybillNumber);

                return new ServiceResponse<IEnumerable<ShipmentTrackingDTO>>
                {
                    Object = result
                };
            });
        }

        [HttpGet]
        [Route("cod")]
        public async Task<IServiceResponse<CashOnDeliveryAccountSummaryDTO>> GetCashOnDeliveryAccount()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _portalService.GetCashOnDeliveryAccount();

                return new ServiceResponse<CashOnDeliveryAccountSummaryDTO>
                {
                    Object = result
                };
            });
        }

        [HttpGet]
        [Route("partialPaymentTransaction/{waybill}")]
        public async Task<IServiceResponse<IEnumerable<PaymentPartialTransactionDTO>>> GetPartialPaymentTransaction(string waybill)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var payment = await _portalService.GetPartialPaymentTransaction(waybill);

                return new ServiceResponse<IEnumerable<PaymentPartialTransactionDTO>>
                {
                    Object = payment
                };
            });
        }


        [HttpGet]
        [Route("dashboard")]
        public async Task<IServiceResponse<DashboardDTO>> GetDashboard()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var dashboard = await _portalService.GetDashboard();

                return new ServiceResponse<DashboardDTO>
                {
                    Object = dashboard
                };
            });
        }


        [HttpGet]
        [Route("state")]
        public async Task<IServiceResponse<IEnumerable<StateDTO>>> GetStates(int pageSize = 10, int page = 1)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var state = await _portalService.GetStates(pageSize, page);
                var total = _portalService.GetStatesTotal();

                return new ServiceResponse<IEnumerable<StateDTO>>
                {
                    Total = total,
                    Object = state
                };
            });
        }


        [HttpGet]
        [Route("localservicecentre")]
        public async Task<IServiceResponse<IEnumerable<ServiceCentreDTO>>> GetLocalServiceCentres()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var centres = await _portalService.GetLocalServiceCentres();
                return new ServiceResponse<IEnumerable<ServiceCentreDTO>>
                {
                    Object = centres
                };
            });
        }


        [HttpGet]
        [Route("deliveryoption")]
        public async Task<IServiceResponse<IEnumerable<DeliveryOptionDTO>>> GetDeliveryOptions()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var delivery = await _portalService.GetDeliveryOptions();

                return new ServiceResponse<IEnumerable<DeliveryOptionDTO>>
                {
                    Object = delivery
                };
            });
        }

        [HttpGet]
        [Route("specialdomesticpackage")]
        public async Task<IServiceResponse<IEnumerable<SpecialDomesticPackageDTO>>> GetSpecialDomesticPackages()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var packages = await _portalService.GetSpecialDomesticPackages();

                return new ServiceResponse<IEnumerable<SpecialDomesticPackageDTO>>
                {
                    Object = packages
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
        [Route("vat")]
        public async Task<IServiceResponse<VATDTO>> GetVATs()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var vat = await _portalService.GetVATs();
                return new ServiceResponse<VATDTO>
                {
                    Object = vat.FirstOrDefault()
                };
            });
        }

        [HttpGet]
        [Route("insurance")]
        public async Task<IServiceResponse<InsuranceDTO>> GetInsurances()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var insurance = await _portalService.GetInsurances();
                return new ServiceResponse<InsuranceDTO>
                {
                    Object = insurance.FirstOrDefault()
                };
            });
        }

        [HttpGet]
        [Route("{departure:int}/{destination:int}")]
        public async Task<IServiceResponse<DomesticRouteZoneMapDTO>> GetZone(int departure, int destination)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var zone = await _portalService.GetZone(departure, destination);

                return new ServiceResponse<DomesticRouteZoneMapDTO>
                {
                    Object = zone
                };
            });
        }

        [HttpPost]
        [Route("price")]
        public async Task<IServiceResponse<decimal>> GetPrice(PricingDTO pricingDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var price = await _portalService.GetPrice(pricingDto);

                return new ServiceResponse<decimal>
                {
                    Object = price
                };
            });
        }

        [HttpPost]
        [Route("haulageprice")]
        public async Task<IServiceResponse<decimal>> GetHaulagePrice(HaulagePricingDTO haulagePricingDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var price = await _portalService.GetHaulagePrice(haulagePricingDto);

                return new ServiceResponse<decimal>
                {
                    Object = price
                };
            });
        }

        [HttpGet]
        [Route("user/{userId}")]
        public async Task<IServiceResponse<CustomerDTO>> GetUser(string userId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var user = await _portalService.GetCustomer(userId);
                return new ServiceResponse<CustomerDTO>
                {
                    Object = user
                };
            });
        }

        [HttpPut]
        [Route("changepassword/{userid}/{currentPassword}/{newPassword}")]
        public async Task<IServiceResponse<bool>> ChangePassword(string userid, string currentPassword, string newPassword)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _portalService.ChangePassword(userid, currentPassword, newPassword);

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
        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public async Task<IServiceResponse<UserDTO>> Register(UserDTO user)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var registerUser = await _portalService.Register(user);
                return new ServiceResponse<UserDTO>
                {
                    Object = registerUser
                };
            });
        }


        [HttpGet]
        [Route("PreShipments")]
        public async Task<IServiceResponse<IEnumerable<PreShipmentDTO>>> GetPreShipments([FromUri]FilterOptionsDto filterOptionsDto)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var preShipments = await _portalService.GetPreShipments(filterOptionsDto);
                return new ServiceResponse<IEnumerable<PreShipmentDTO>>
                {
                    Object = preShipments
                };
            });
        }

        [HttpGet]
        [Route("PreShipments/{waybill}")]
        public async Task<IServiceResponse<PreShipmentDTO>> GetPreShipment(string waybill)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var preShipment = await _portalService.GetPreShipment(waybill);
                return new ServiceResponse<PreShipmentDTO>
                {
                    Object = preShipment
                };
            });
        }

        [HttpGet]
        [Route("sla")]
        public async Task<IServiceResponse<SLADTO>> GetSLA()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var sla = await _portalService.GetSLA();

                return new ServiceResponse<SLADTO>
                {
                    Object = sla
                };
            });
        }

        [HttpGet]
        [Route("sla/{slaId:int}")]
        public async Task<IServiceResponse<object>> SignSLA(int slaId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var sla = await _portalService.SignSLA(slaId);

                return new ServiceResponse<object>
                {
                    Object = sla
                };
            });
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
        [Route("validateotp/{OTP}")]
        public async Task<IServiceResponse<JObject>> IsOTPValid(int OTP)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var Otp = await _portalService.IsOTPValid(OTP);
                if (Otp != null && Otp.IsActive == true)
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
                         new KeyValuePair<string, string>("Username", Otp.Username),
                         new KeyValuePair<string, string>("Password", Otp.UserChannelPassword),
                         });

                        //setup login data
                        HttpResponseMessage responseMessage = client.PostAsync("token", formContent).Result;

                        if (!responseMessage.IsSuccessStatusCode)
                        {
                            throw new GenericException("Operation could not complete login successfully:");
                        }
                        else
                        {
                            Otp = await _portalService.GenerateReferrerCode(Otp);
                        }

                        //get access token from response body
                        var responseJson = await responseMessage.Content.ReadAsStringAsync();
                        var jObject = JObject.Parse(responseJson);

                        getTokenResponse = jObject.GetValue("access_token").ToString();

                        return new ServiceResponse<JObject>
                        {
                            Object = jObject,
                            ReferrerCode = Otp.Referrercode
                        };
                    }

                }
                else
                {
                    var data = new { IsActive = false };

                    var jObject = JObject.FromObject(data);

                    return new ServiceResponse<JObject>
                    {
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

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<IServiceResponse<JObject>> Login(MobileLoginDTO logindetail)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var user = await _portalService.CheckDetails(logindetail.UserDetail, logindetail.UserChannelType);
                var vehicle = user.VehicleType;
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
                    throw new GenericException("You are not authorized to login on this platform.");
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
                        HttpResponseMessage responseMessage = client.PostAsync("token", formContent).Result;

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
                                Object = jObject,
                                ReferrerCode = user.Referrercode,
                                AverageRatings = user.AverageRatings,
                                IsVerified = user.IsVerified,
                                PartnerType = partnerType

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
                        ShortDescription = "User has not been verified",
                        Object = jObject
                    };
                }
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

        [HttpGet]
        [Route("itemTypes")]
        public async Task<IServiceResponse<List<string>>> GetItemTypes()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var ItemTypes = _portalService.GetItemTypes();
                return new ServiceResponse<List<string>>
                {
                    Object = ItemTypes,
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

        [HttpGet]
        [Route("getStations")]
        public async Task<IServiceResponse<IEnumerable<StationDTO>>> GetStations()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var stations = await _portalService.GetLocalStations();
                return new ServiceResponse<IEnumerable<StationDTO>>
                {
                    Object = stations,
                };
            });
        }

        [HttpGet]
        [Route("getwallettransactionandpreshipmenthistory")]
        public async Task<IServiceResponse<WalletTransactionSummaryDTO>> GetWalletTransactionAndPreshipmentHistory()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var Transactionhistory = await _portalService.GetWalletTransactionsForMobile();
                var preshipments = await _portalService.GetPreShipmentForUser();
                Transactionhistory.Shipments = preshipments;
                return new ServiceResponse<WalletTransactionSummaryDTO>
                {
                    Object = Transactionhistory
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

        [HttpGet]
        [Route("getpreshipmenthistory")]
        public async Task<IServiceResponse<List<PreShipmentMobileDTO>>> GetPreshipmentHistory()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var PreshipMentMobile = await _portalService.GetPreShipmentForUser();
                return new ServiceResponse<List<PreShipmentMobileDTO>>
                {
                    Object = PreshipMentMobile,
                };
            });
        }

        //Should be discard 
        [HttpGet]
        [Route("verifypaystackpayment/{reference}/{UserId}")]
        public async Task<IServiceResponse<PaystackWebhookDTO>> VerifyMobilePayment(string reference, string UserId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var walletPaymentLog = await _paymentService.VerifyPaymentMobile(reference, UserId);
                return new ServiceResponse<PaystackWebhookDTO>
                {

                    Object = walletPaymentLog
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
        [Route("deleterecord")]
        public async Task<IServiceResponse<bool>> DeleteRecord(UserDTO user)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var response = await _portalService.deleterecord(user.Email);
                return new ServiceResponse<bool>
                {
                    Object = response
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
                    throw new GenericException("Operation could not be completed");
                }

                return new ServiceResponse<bool>
                {
                    Object = true
                };
            });
        }

        [HttpPost]
        [Route("verifypartnerdetails")]
        public async Task<IServiceResponse<bool>> VerifyPartnerDetails(PartnerDTO partner)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var response = await _portalService.VerifyPartnerDetails(partner);
                return new ServiceResponse<bool>
                {
                    Object = response
                };
            });
        }

        [HttpPost]
        [Route("saveimages")]
        public async Task<IServiceResponse<string>> LoadImage(ImageDTO images)
        {
            return await HandleApiOperationAsync(async () =>
            {

                var response = await _portalService.LoadImage(images);
                return new ServiceResponse<string>
                {
                    Object = response
                };
            });
        }

        [HttpGet]
        [Route("displayimages")]
        public async Task<IServiceResponse<List<Uri>>> DisplayImages(ImageDTO images)
        {
            return await HandleApiOperationAsync(async () =>
            {

                var response = await _portalService.DisplayImages();
                return new ServiceResponse<List<Uri>>
                {
                    Object = response
                };
            });
        }

        [HttpPost]
        [Route("getallpartnerdetails")]
        public async Task<IServiceResponse<PartnerDTO>> GetAllPartnerDetails(PartnerDTO partner)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var response = await _portalService.GetPartnerDetails(partner.Email);
                return new ServiceResponse<PartnerDTO>
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
        [HttpGet]
        [Route("getpreshipmentmobiledetailsfromdeliverynumber/{deliverynumber}")]
        public async Task<IServiceResponse<PreShipmentSummaryDTO>> GetPreshipmentmobiledetailsfromdeliverynumber(string deliverynumber)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var preshipment = await _portalService.GetShipmentDetailsFromDeliveryNumber(deliverynumber);
                return new ServiceResponse<PreShipmentSummaryDTO>
                {
                    Object = preshipment
                };
            });
        }
        [HttpPost]
        [Route("approveshipment/{waybill}")]
        public async Task<IServiceResponse<bool>> Approveshipment(string waybill)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _portalService.ApproveShipment(waybill);
                return new ServiceResponse<bool>
                {
                    Object = result
                };
            });
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("getcountries")]
        public async Task<IServiceResponse<List<NewCountryDTO>>> getcountries()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var countries = await _portalService.GetUpdatedCountries();
                return new ServiceResponse<List<NewCountryDTO>>
                {
                    Object = countries.ToList()
                };
            });
        }

        [HttpGet]
        [Route("getallstations")]
        public async Task<IServiceResponse<Dictionary<string, List<StationDTO>>>> getstations()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var stations = await _portalService.GetAllStations();
                return new ServiceResponse<Dictionary<string, List<StationDTO>>>
                {
                    Object = stations
                };
            });
        }

        [HttpPost]
        [Route("gethaulagepriceformobile")]
        public async Task<IServiceResponse<MobilePriceDTO>> gethaulageprice(HaulagePriceDTO haulage)
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
        [HttpPost]
        [Route("updatevehicleprofile")]
        public async Task<IServiceResponse<bool>> updatevehicleprofile(UserDTO user)
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

        [AllowAnonymous]
        [HttpGet]
        [Route("websiteData")]
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
                //headers.Add("Content-Type", "application/json");
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



    }
}