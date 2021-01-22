using GIGLS.Core.IServices.CustomerPortal;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Account;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.DTO.Wallet;
using GIGLS.Core.IServices.Shipments;
using GIGLS.Core;
using System.Linq;
using AutoMapper;
using GIGLS.Core.IServices.Account;
using GIGLS.Core.IServices.Business;
using GIGLS.Core.IServices.User;
using GIGLS.Core.IServices.Wallet;
using GIGLS.Core.IServices.CashOnDeliveryAccount;
using GIGLS.Core.DTO.PaymentTransactions;
using GIGLS.Core.DTO.Dashboard;
using GIGLS.Infrastructure;
using GIGLS.Core.DTO.Haulage;
using GIGLS.Core.DTO.Zone;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.Core.DTO;
using Microsoft.AspNet.Identity;
using GIGLS.Core.IServices.Customers;
using GIGLS.Core.DTO.Customers;
using System;
using GIGLS.CORE.DTO.Shipments;
using GIGLS.Core.DTO.User;
using GIGLS.Core.DTO.SLA;
using GIGLS.Core.IServices.Sla;
using GIGLS.Core.Enums;
using GIGLS.Core.View;
using GIGLS.Core.IServices;
using GIGLS.Core.IServices.BankSettlement;
using GIGLS.Core.Domain.BankSettlement;
using GIGLS.Core.DTO.Partnership;
using GIGLS.Core.Domain.Partnership;
using GIGLS.Core.IServices.Utility;
using GIGL.GIGLS.Core.Domain;
using GIGLS.Core.DTO.Report;
using GIGLS.Core.IMessageService;
using System.Text.RegularExpressions;
using GIGLS.Core.DTO.Admin;
using GIGLS.Core.IServices.Report;
using GIGLS.Core.IServices.Partnership;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using GIGLS.Core.DTO.Utility;
using GIGLS.Core.IServices.Fleets;
using GIGLS.Core.DTO.Fleets;
using GIGLS.Core.DTO.MessagingLog;
using System.Net;
using GIGLS.Core.DTO.OnlinePayment;
using GIGLS.Core.IServices.Zone;
using GIGLS.Core.IServices.ShipmentScan;
using GIGLS.Core.DTO.ShipmentScan;
using GIGLS.CORE.IServices.Shipments;
using GIGLS.Core.IServices.PaymentTransactions;
using GIGLS.Core.DTO.Stores;
using System.Net.Http;
using GIGLS.CORE.DTO.Report;

namespace GIGLS.Services.Business.CustomerPortal
{
    public class CustomerPortalService : ICustomerPortalService
    {
        private readonly IUnitOfWork _uow;
        private readonly IInvoiceService _invoiceService;
        private readonly IShipmentTrackService _iShipmentTrackService;
        private readonly IUserService _userService;
        private readonly IWalletTransactionService _iWalletTransactionService;
        private readonly ICashOnDeliveryAccountService _iCashOnDeliveryAccountService;
        private readonly IPricingService _pricing;
        private readonly ICustomerService _customerService;
        private readonly IPreShipmentService _preShipmentService;
        private readonly IWalletService _walletService;
        private readonly IWalletPaymentLogService _wallepaymenttlogService;
        private readonly ISLAService _slaService;
        private readonly IOTPService _otpService;
        private readonly IBankShipmentSettlementService _iBankShipmentSettlementService;
        private readonly INumberGeneratorMonitorService _numberGeneratorMonitorService;
        private readonly IPasswordGenerator _codegenerator;
        private readonly IGlobalPropertyService _globalPropertyService;
        private readonly IPreShipmentMobileService _preShipmentMobileService;
        private readonly IMessageSenderService _messageSenderService;
        private readonly ICountryService _countryService;
        private readonly IAdminReportService _adminReportService;
        private readonly IPartnerTransactionsService _partnertransactionservice;
        private readonly IMobileGroupCodeWaybillMappingService _groupCodeWaybillMappingService;
        private readonly IDispatchService _dispatchService;
        private readonly IManifestWaybillMappingService _manifestWaybillMappingService;
        private readonly IPaystackPaymentService _paystackPaymentService;
        private readonly IUssdService _ussdService;
        private readonly IDomesticRouteZoneMapService _domesticroutezonemapservice;
        private readonly IScanStatusService _scanStatusService;
        private readonly IScanService _scanService;
        private readonly IShipmentCollectionService _collectionservice;
        private readonly ILogVisitReasonService _logService;
        private readonly IManifestVisitMonitoringService _visitService;
        private readonly IPaymentTransactionService _paymentTransactionService;
        private readonly IFlutterwavePaymentService _flutterwavePaymentService;
        private readonly IMagayaService _magayaService;
        private readonly IMobilePickUpRequestsService _mobilePickUpRequestService;
        private readonly INotificationService _notificationService;
        private readonly ICompanyService _companyService;
        private readonly IShipmentService _shipmentService; 
        private readonly IManifestGroupWaybillNumberMappingService _movementManifestService;   

        public CustomerPortalService(IUnitOfWork uow, IInvoiceService invoiceService,
            IShipmentTrackService iShipmentTrackService, IUserService userService, IWalletTransactionService iWalletTransactionService,
            ICashOnDeliveryAccountService iCashOnDeliveryAccountService, IPricingService pricingService, ICustomerService customerService,
            IPreShipmentService preShipmentService, IWalletService walletService, IWalletPaymentLogService wallepaymenttlogService,
            ISLAService slaService, IOTPService otpService, IBankShipmentSettlementService iBankShipmentSettlementService, INumberGeneratorMonitorService numberGeneratorMonitorService,
            IPasswordGenerator codegenerator, IGlobalPropertyService globalPropertyService, IPreShipmentMobileService preShipmentMobileService, IMessageSenderService messageSenderService,
            ICountryService countryService, IAdminReportService adminReportService, IPartnerTransactionsService partnertransactionservice,
            IMobileGroupCodeWaybillMappingService groupCodeWaybillMappingService, IDispatchService dispatchService, IManifestWaybillMappingService manifestWaybillMappingService,
            IPaystackPaymentService paystackPaymentService, IUssdService ussdService, IDomesticRouteZoneMapService domesticRouteZoneMapService,
            IScanStatusService scanStatusService, IScanService scanService, IShipmentCollectionService collectionService, ILogVisitReasonService logService, IManifestVisitMonitoringService visitService,
            IPaymentTransactionService paymentTransactionService, IFlutterwavePaymentService flutterwavePaymentService, IMagayaService magayaService, IMobilePickUpRequestsService mobilePickUpRequestsService,
            INotificationService notificationService,ICompanyService companyService, IShipmentService shipmentService, IManifestGroupWaybillNumberMappingService  movementManifestService) 
        {
            _invoiceService = invoiceService;
            _iShipmentTrackService = iShipmentTrackService;
            _userService = userService;
            _iWalletTransactionService = iWalletTransactionService;
            _iCashOnDeliveryAccountService = iCashOnDeliveryAccountService;
            _pricing = pricingService;
            _customerService = customerService;
            _preShipmentService = preShipmentService;
            _uow = uow;
            _walletService = walletService;
            _wallepaymenttlogService = wallepaymenttlogService;
            _slaService = slaService;
            _otpService = otpService;
            _iBankShipmentSettlementService = iBankShipmentSettlementService;
            _numberGeneratorMonitorService = numberGeneratorMonitorService;
            _codegenerator = codegenerator;
            _globalPropertyService = globalPropertyService;
            _preShipmentMobileService = preShipmentMobileService;
            _messageSenderService = messageSenderService;
            _countryService = countryService;
            _adminReportService = adminReportService;
            _partnertransactionservice = partnertransactionservice;
            _groupCodeWaybillMappingService = groupCodeWaybillMappingService;
            _dispatchService = dispatchService;
            _manifestWaybillMappingService = manifestWaybillMappingService;
            _paystackPaymentService = paystackPaymentService;
            _ussdService = ussdService;
            _domesticroutezonemapservice = domesticRouteZoneMapService;
            _scanStatusService = scanStatusService;
            _scanService = scanService;
            _collectionservice = collectionService;
            _logService = logService;
            _visitService = visitService;
            _paymentTransactionService = paymentTransactionService;
            _flutterwavePaymentService = flutterwavePaymentService;
            _magayaService = magayaService;
            _mobilePickUpRequestService = mobilePickUpRequestsService;
            _notificationService = notificationService;
            _companyService = companyService;
            _shipmentService  = shipmentService;
            _movementManifestService = movementManifestService;
            MapperConfig.Initialize();
        }


        public async Task<List<InvoiceViewDTO>> GetShipmentTransactions(ShipmentCollectionFilterCriteria f_Criteria)
        {
            //get the current login user 
            var currentUserId = await _userService.GetCurrentUserId();
            var currentUser = await _userService.GetUserById(currentUserId);

            var invoices = new List<InvoiceView>();

            var invoiceQuery = _uow.Invoice.GetCustomerTransactions();

            if (f_Criteria.IsDashboard)
            {
                invoices = invoiceQuery.Where(s => s.CustomerCode == currentUser.UserChannelCode).OrderByDescending(s => s.DateCreated).Take(5).ToList();
            }
            else
            {
                //get startDate and endDate
                var queryDate = f_Criteria.getStartDateAndEndDate();
                var startDate = queryDate.Item1;
                var endDate = queryDate.Item2;

                invoices = invoiceQuery.Where(x => x.CustomerCode == currentUser.UserChannelCode && x.DateCreated >= startDate && x.DateCreated < endDate).OrderByDescending(s => s.DateCreated).ToList();
            }

            var invoicesDto = Mapper.Map<List<InvoiceViewDTO>>(invoices);

            if (invoicesDto != null)
            {
                var countries = _uow.Country.GetAllAsQueryable().Where(x => x.IsActive == true).ToList();
                var countriesDto = Mapper.Map<List<CountryDTO>>(countries);

                foreach (var c in invoicesDto)
                {
                    c.Country = countriesDto.Where(x => x.CountryId == c.DepartureCountryId).FirstOrDefault();
                }
            }

            return await Task.FromResult(invoicesDto);
        }

        public async Task UpdateWallet(int walletId, WalletTransactionDTO walletTransactionDTO)
        {
            await _walletService.UpdateWallet(walletId, walletTransactionDTO, false);
        }

        public async Task<object> AddWalletPaymentLog(WalletPaymentLogDTO walletPaymentLogDto)
        {
            var walletPaymentLog = await _wallepaymenttlogService.AddWalletPaymentLog(walletPaymentLogDto);
            return walletPaymentLog;
        }

        public async Task<object> AddWalletPaymentLogAnonymousUser(WalletPaymentLogDTO walletPaymentLogDto)
        {
            var walletPaymentLog = await _wallepaymenttlogService.AddWalletPaymentLogAnonymousUser(walletPaymentLogDto);
            return walletPaymentLog;
        }

        public async Task<USSDResponse> InitiatePaymentUsingUSSD(WalletPaymentLogDTO walletPaymentLogDto)
        {
            var walletPaymentLog = await _wallepaymenttlogService.InitiatePaymentUsingUSSD(walletPaymentLogDto);
            return walletPaymentLog;
        }

        public async Task<object> UpdateWalletPaymentLog(WalletPaymentLogDTO walletPaymentLogDto)
        {
            //1.check to prevent multiple entries
            //var walletLogObject = _uow.WalletPaymentLog.GetAllAsQueryable().SingleOrDefault(s => 
            //    s.Reference == walletPaymentLogDto.Reference && s.IsWalletCredited == false);
            //if(walletLogObject == null)
            //{
            //    //wallet already updated
            //    return false;
            //}

            //2. update wallet for user
            if (walletPaymentLogDto.TransactionStatus.Equals("success"))
            {

                //Check if transaction exist before updating the wallet
                //to prevent duplicate entry
                var transactionExist = _uow.WalletTransaction.GetAllAsQueryable().SingleOrDefault(s => s.PaymentTypeReference == walletPaymentLogDto.Reference);

                if (transactionExist != null)
                {
                    throw new GenericException("Wallet transaction done already");
                }

                await UpdateWallet(walletPaymentLogDto.WalletId, new WalletTransactionDTO()
                {
                    WalletId = walletPaymentLogDto.WalletId,
                    Amount = walletPaymentLogDto.Amount,
                    Description = walletPaymentLogDto.Description,
                    PaymentTypeReference = walletPaymentLogDto.Reference
                }
             );
            }

            //3. update wallet log
            if (walletPaymentLogDto.TransactionStatus.Equals("success"))
            {
                walletPaymentLogDto.IsWalletCredited = true;
            }
            else
            {
                walletPaymentLogDto.IsWalletCredited = false;
            }

            await _wallepaymenttlogService.UpdateWalletPaymentLog(walletPaymentLogDto.Reference, walletPaymentLogDto);
            return walletPaymentLogDto;
        }

        public async Task<PaymentResponse> VerifyAndValidatePayment(string referenceCode)
        {
            PaymentResponse result = new PaymentResponse();

            //1. Get PaymentLog
            var paymentLog = await _uow.WalletPaymentLog.GetAsync(x => x.Reference == referenceCode);

            if (paymentLog != null)
            {
                if (paymentLog.OnlinePaymentType == OnlinePaymentType.USSD)
                {
                    result = await VerifyAndValidateUSSDPayment(referenceCode);
                }
                else if (paymentLog.OnlinePaymentType == OnlinePaymentType.Flutterwave)
                {
                    result = await VerifyAndValidateFlutterWavePayment(referenceCode);
                }
                else
                {
                    result = await _paystackPaymentService.VerifyAndProcessPayment(referenceCode);
                }
            }
            else
            {
                result.Result = false;
                result.Message = "";
                result.GatewayResponse = "Wallet Payment Log Information does not exist";
            }

            return result;
        }

        private async Task<PaymentResponse> VerifyAndValidateUSSDPayment(string referenceCode)
        {
            PaymentResponse response = new PaymentResponse();

            var result = await _ussdService.VerifyAndValidatePayment(referenceCode);

            response.Result = result.Status;
            response.Status = result.data.Status;
            response.Message = result.Message;
            response.GatewayResponse = result.data.Gateway_Response;
            return response;
        }

        private async Task<PaymentResponse> VerifyAndValidateFlutterWavePayment(string referenceCode)
        {
            PaymentResponse response = new PaymentResponse();
            var result = await _flutterwavePaymentService.VerifyAndValidateMobilePayment(referenceCode);

            response.Result = result.Status;
            response.Status = result.data.Status;
            response.Message = result.Message;
            response.GatewayResponse = result.data.Gateway_Response;
            return response;
        }

        public async Task<GatewayCodeResponse> GetGatewayCode()
        {
            return await _ussdService.GetGatewayCode();
        }

        public async Task<WalletTransactionSummaryDTO> GetWalletTransactions()
        {
            //get the current login user 
            var currentUserId = await _userService.GetCurrentUserId();
            var currentUser = await _userService.GetUserById(currentUserId);
            var wallet = await _uow.Wallet.GetAsync(s => s.CustomerCode == currentUser.UserChannelCode);

            if (wallet == null)
            {
                throw new GenericException("Wallet does not exist", $"{(int)HttpStatusCode.NotFound}");
            }

            var walletTransactionSummary = await _iWalletTransactionService.GetWalletTransactionByWalletId(wallet.WalletId);
            return walletTransactionSummary;
        }

        public async Task<InvoiceDTO> GetInvoiceByWaybill(string waybill)
        {
            var invoice = await _invoiceService.GetInvoiceByWaybill(waybill);
            if (invoice != null)
            {
                var shipmentPreparedBy = await _userService.GetUserById(invoice.Shipment.UserId);
                invoice.Shipment.UserId = shipmentPreparedBy.LastName + " " + shipmentPreparedBy.FirstName;
            }
            return invoice;
        }

        public async Task<List<CodPayOutList>> GetPaidCODByCustomer()
        {
            var userchannelcode = await _userService.GetUserChannelCode();
            var codsValues = await _iBankShipmentSettlementService.GetPaidOutCODListsByCustomer(userchannelcode);
            return codsValues;
        }

        public async Task<IEnumerable<InvoiceViewDTO>> GetInvoices()
        {
            //get the current login user 
            var currentUserId = await _userService.GetCurrentUserId();
            var currentUser = await _userService.GetUserById(currentUserId);

            var invoices = _uow.Invoice.GetCustomerInvoices().Where(s => s.CustomerCode == currentUser.UserChannelCode).OrderByDescending(s => s.DateCreated).ToList();
            invoices = invoices.OrderByDescending(s => s.DateCreated).ToList();

            var invoicesDto = Mapper.Map<List<InvoiceViewDTO>>(invoices);

            if (invoicesDto.Any())
            {
                var countries = _uow.Country.GetAllAsQueryable().Where(x => x.IsActive == true).ToList();
                var countriesDto = Mapper.Map<List<CountryDTO>>(countries);

                //Update to change the Corporate Paid status from 'Paid' to 'Credit'
                foreach (var item in invoicesDto)
                {
                    item.PaymentStatusDisplay = item.PaymentStatus.ToString();
                    if ((CompanyType.Corporate.ToString() == item.CompanyType)
                        && (PaymentStatus.Paid == item.PaymentStatus))
                    {
                        item.PaymentStatusDisplay = "Credit";
                    }

                    item.Country = countriesDto.Where(x => x.CountryId == item.DepartureCountryId).FirstOrDefault();
                }

            }

            return invoicesDto;
        }

        public async Task<IEnumerable<ShipmentTrackingDTO>> TrackShipment(string waybillNumber)
        {
            //1. Verify the waybill is attached to the login user
            var currentUserId = await _userService.GetCurrentUserId();
            var currentUser = await _userService.GetUserById(currentUserId);

            var invoices = _uow.Shipment.GetAllAsQueryable().Where(s => s.CustomerCode == currentUser.UserChannelCode && s.Waybill == waybillNumber).FirstOrDefault();

            if (invoices != null)
            {
                var finalResult = new List<ShipmentTrackingDTO>();
                var result = await _iShipmentTrackService.TrackShipment(waybillNumber);

                if (result.Any())
                {
                    string smim = ShipmentScanStatus.SMIM.ToString();
                    string fms = ShipmentScanStatus.FMS.ToString();

                    foreach (var tracking in result)
                    {
                        if (!(tracking.Status.Equals(smim) || tracking.Status.Equals(fms)))
                        {
                            finalResult.Add(tracking);
                        }
                    }
                }

                return finalResult;
            }
            else
            {
                throw new GenericException("Error: You cannot track this waybill number.", $"{(int)HttpStatusCode.NotFound}");
            }
        }

        public async Task<List<ShipmentTrackingDTO>> PublicTrackShipment(string waybillNumber)
        {
            var finalResult = new List<ShipmentTrackingDTO>();

            var result = await _iShipmentTrackService.TrackShipment(waybillNumber);

            if (result.Any())
            {
                string smim = ShipmentScanStatus.SMIM.ToString();
                string fms = ShipmentScanStatus.FMS.ToString();
                string thirdparty = ShipmentScanStatus.THIRDPARTY.ToString();


                foreach (var tracking in result)
                {
                    if (!(tracking.Status.Equals(smim) || tracking.Status.Equals(fms) || tracking.Status.Equals(thirdparty)))
                    {
                        finalResult.Add(tracking);
                    }
                }
            }
            return finalResult;
        }

        public async Task<CashOnDeliveryAccountSummaryDTO> GetCashOnDeliveryAccount()
        {
            //get the current login user 
            var currentUserId = await _userService.GetCurrentUserId();
            var currentUser = await _userService.GetUserById(currentUserId);
            var wallet = await _uow.Wallet.GetAsync(s => s.CustomerCode == currentUser.UserChannelCode);
            var result = await _iCashOnDeliveryAccountService.GetCashOnDeliveryAccountByWallet(wallet.WalletNumber);
            return result;
        }

        public async Task<IEnumerable<PaymentPartialTransactionDTO>> GetPartialPaymentTransaction(string waybill)
        {
            var transaction = await _uow.PaymentPartialTransaction.FindAsync(x => x.Waybill.Equals(waybill));
            return Mapper.Map<IEnumerable<PaymentPartialTransactionDTO>>(transaction);
        }

        public async Task<DashboardDTO> GetDashboard()
        {
            var dashboardDTO = new DashboardDTO();

            //get the current login user 
            var currentUserId = await _userService.GetCurrentUserId();
            var currentUser = await _userService.GetUserById(currentUserId);
            var wallet = await _uow.Wallet.GetAsync(s => s.CustomerCode == currentUser.UserChannelCode);

            if (wallet != null)
            {
                int invoices = _uow.Shipment.GetAllAsQueryable().Where(s => s.CustomerCode == currentUser.UserChannelCode && s.IsCancelled == false).Count();
                dashboardDTO.TotalShipmentOrdered = invoices;
                dashboardDTO.WalletBalance = wallet.Balance;
            }

            if (currentUser != null)
            {
                dashboardDTO.UserActiveCountry = await GetUserActiveCountry(currentUser);
            }

            return await Task.FromResult(dashboardDTO);
        }

        public async Task<IEnumerable<StateDTO>> GetStates(int pageSize, int page)
        {
            var states = await _uow.State.GetStatesAsync(pageSize, page);
            return states.OrderBy(x => x.StateName).ToList();
        }

        public int GetStatesTotal()
        {
            var states = _uow.State.GetStatesTotal();
            return states;
        }

        public async Task<List<ServiceCentreDTO>> GetLocalServiceCentres()
        {
            var userId = await _userService.GetCurrentUserId();
            var user = await _userService.GetUserById(userId);

            if (user.UserActiveCountryId == 0)
            {
                user.UserActiveCountryId = 1;
            }

            int[] countryIds = new int[] { user.UserActiveCountryId };
            return await _uow.ServiceCentre.GetLocalServiceCentres(countryIds);
        }

        public async Task<IEnumerable<DeliveryOptionDTO>> GetDeliveryOptions()
        {
            return await _uow.DeliveryOption.GetDeliveryOptions();
        }

        public Task<IEnumerable<SpecialDomesticPackageDTO>> GetSpecialDomesticPackages()
        {
            return Task.FromResult(Mapper.Map<IEnumerable<SpecialDomesticPackage>, IEnumerable<SpecialDomesticPackageDTO>>(_uow.SpecialDomesticPackage.GetAll()));
        }

        public async Task<IEnumerable<HaulageDTO>> GetHaulages()
        {
            var haulages = await _uow.Haulage.GetHaulagesAsync();
            return haulages;
        }


        public async Task<IEnumerable<InsuranceDTO>> GetInsurances()
        {
            var insurances = await _uow.Insurance.GetInsurancesAsync();
            return insurances;
        }

        public async Task<IEnumerable<VATDTO>> GetVATs()
        {
            var vats = await _uow.VAT.GetVATsAsync();
            return vats;
        }

        public async Task<DomesticRouteZoneMapDTO> GetZone(int departure, int destination)
        {
            // get serviceCenters
            var departureServiceCenter = _uow.ServiceCentre.Get(departure);
            var destinationServiceCenter = _uow.ServiceCentre.Get(destination);

            // use Stations
            var routeZoneMap = await _uow.DomesticRouteZoneMap.GetAsync(r =>
                r.DepartureId == departureServiceCenter.StationId &&
                r.DestinationId == destinationServiceCenter.StationId, "Zone,Destination,Departure");

            if (routeZoneMap == null)
                throw new GenericException("The Mapping of Route to Zone does not exist", $"{(int)HttpStatusCode.NotFound}");

            return Mapper.Map<DomesticRouteZoneMapDTO>(routeZoneMap);
        }

        public async Task<decimal> GetPrice(PricingDTO pricingDto)
        {
            return await _pricing.GetPrice(pricingDto);
        }

        public async Task<decimal> GetHaulagePrice(HaulagePricingDTO haulagePricingDto)
        {
            return await _pricing.GetHaulagePrice(haulagePricingDto);
        }

        public async Task<CustomerDTO> GetCustomer(string userId)
        {
            var user = await _userService.GetUserById(userId);
            var customer = await _customerService.GetCustomer(user.UserChannelCode, user.UserChannelType);
            return customer;
        }

        public async Task<IdentityResult> ChangePassword(string userid, string currentPassword, string newPassword)
        {
            return await _userService.ChangePassword(userid, currentPassword, newPassword);
        }

        public async Task<IdentityResult> ChangePassword(ChangePasswordDTO passwordDTO)
        {
            var user = await _userService.GetCurrentUserId();
            return await _userService.ChangePassword(user, passwordDTO.CurrentPassword, passwordDTO.NewPassword);
        }

        public async Task<UserDTO> Register(UserDTO user)
        {
            try
            {
                CustomerType customerType = CustomerType.IndividualCustomer;
                CompanyType companyType = CompanyType.Client;

                if (user.UserChannelType == UserChannelType.Ecommerce)
                {
                    customerType = CustomerType.Company;
                    companyType = CompanyType.Ecommerce;
                }
                else
                {
                    customerType = CustomerType.IndividualCustomer;
                }
                //1. convert user data to customer data
                var customer = new CustomerDTO
                {
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Name = user.Organisation,
                    CustomerType = customerType,
                    CompanyType = companyType,
                    Password = user.Password,
                    CustomerCode = user.UserChannelCode,
                    PictureUrl = user.PictureUrl,
                    IsFromMobile = user.IsFromMobile,
                    UserActiveCountryId = user.UserActiveCountryId
                    //added this to pass channelcode 


                };
                //2. Create customer data
                var result = await _customerService.CreateCustomer(customer);

                if (result != null)
                {
                    user.UserChannelCode = result.CustomerCode;
                }
                else
                {
                    throw new GenericException("Customer could not be created", $"{(int)HttpStatusCode.Forbidden}");
                }
            }
            catch (Exception)
            {
                throw;
            }

            return user;
        }

        public async Task<SLADTO> GetSLA()
        {
            //1. get the user channel type
            var userId = await _userService.GetCurrentUserId();
            var user = await _userService.GetUserById(userId);

            UserChannelType channelType = user.UserChannelType;
            SLAType SLAType;

            //2. Use the channel type to display the SLA type for the user
            switch (channelType)
            {
                case UserChannelType.Corporate:
                    SLAType = SLAType.Corporate;
                    break;
                case UserChannelType.Ecommerce:
                    SLAType = SLAType.Ecommerce;
                    break;
                default:
                    SLAType = SLAType.Reseller;
                    break;
            }

            var sla = await _slaService.GetSLAByType(SLAType);

            //check if the SLA has been signed by the user
            var userSla = await _uow.SLASignedUser.FindAsync(x => x.UserId == userId);
            if (userSla.Any())
            {
                sla.IsSigned = true;
            }

            return sla;
        }

        public async Task<object> SignSLA(int slaId)
        {
            var signed = await _slaService.UserSignedSLA(slaId);
            return signed;
        }


        public async Task<Tuple<Task<List<WalletPaymentLogDTO>>, int>> GetWalletPaymentLogs(FilterOptionsDto filterOptionsDto)
        {
            int WalletId = await GetWalletNummber();

            var walletPaymentLogView = _uow.WalletPaymentLog.GetWalletPaymentLogs(filterOptionsDto, WalletId);
            return walletPaymentLogView;
        }

        private async Task<int> GetWalletNummber()
        {
            //Get the current login user 
            var currentUserId = await _userService.GetCurrentUserId();
            var currentUser = await _userService.GetUserById(currentUserId);

            var wallet = await _uow.Wallet.GetAsync(s => s.CustomerCode.ToLower() == currentUser.UserChannelCode.ToLower());

            if (wallet == null)
            {
                throw new GenericException("Wallet does not exist", $"{(int)HttpStatusCode.NotFound}");
            }

            return wallet.WalletId;
        }

        public async Task<List<string>> GetItemTypes()
        {
            List<string> items = new List<string>
            {
                "NORMAL",
                "DANGEROUS GOODS",
                "FRAGILE",
                "KEEP AT ROOM TEMPERATURE",
                "KEEP UPRIGHT",
                "REFRIGERATED ON ARRIVAL",
                "SENSITIVE"
            };

            return await Task.FromResult(items);
        }

        public async Task<SignResponseDTO> SignUp(UserDTO user)
        {
            if (user == null)
                throw new GenericException("NULL INPUT");

            var result = new SignResponseDTO();

            if (user.RequiresCod == null)
            {
                user.RequiresCod = false;
            }

            if (user.IsUniqueInstalled == null)
            {
                user.IsUniqueInstalled = false;
            }

            if (!string.IsNullOrWhiteSpace(user.Email))
            {
                user.Email = user.Email.Trim().ToLower();
            }
            if (!String.IsNullOrEmpty(user.PhoneNumber))
            {
                user.PhoneNumber = user.PhoneNumber.Trim();
            }

            //validate email
            bool isEmail = Regex.IsMatch(user.Email, @"\A(?:[a-z0-9_]+(?:\.[a-z0-9_]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
            if (!isEmail)
            {
                throw new GenericException("Invalid Email Address", $"{(int)HttpStatusCode.Forbidden}");
            }

            //automatic enable Ecommerce eligibility
            user.IsEligible = true;

            if (!string.IsNullOrWhiteSpace(user.Referrercode))
            {
                user.RegistrationReferrercode = user.Referrercode;
            }

            if (user.UserChannelType != UserChannelType.Ecommerce && user.UserChannelType != UserChannelType.IndividualCustomer && user.UserChannelType != UserChannelType.Partner)
            {
                throw new GenericException($"Kindly supply valid customer channel ");
            }

            if (user.UserChannelType == UserChannelType.Ecommerce)
            {
                if (string.IsNullOrEmpty(user.Organisation))
                {
                    throw new GenericException($"Kindly supply your company name ");
                }
            }

            if (user.UserChannelType == UserChannelType.Ecommerce)
            {
                string message = await SendRegistrationMessage(user);
                throw new GenericException($"{message}", $"{(int)HttpStatusCode.ServiceUnavailable}");
            }

            //use to handle multiple this kind of value +234+2349022736119
            user.PhoneNumber = "+" + user.PhoneNumber.Split('+').Last();

            user = await GetCustomerCountryUsingPhoneCode(user);

            bool checkRegistrationAccess = await CheckRegistrationAccess(user);

            if (checkRegistrationAccess)
            {
                if (user.UserChannelType == UserChannelType.Partner)
                {
                    return await PartnerRegistration(user);
                }
                else if (user.UserChannelType == UserChannelType.IndividualCustomer)
                {
                    return await IndividualRegistration(user);
                }
                else if (user.UserChannelType == UserChannelType.Ecommerce)
                {
                    return await EcommerceRegistration(user);
                }
            }
            else
            {
                throw new GenericException("Customer already exists!!!", $"{(int)HttpStatusCode.Forbidden}");
            }

            return result;
        }

        //Notify Ecommerce Team for Ecommerce Regstration Attempt 
        private async Task<string> SendRegistrationMessage(UserDTO user)
        {
            var ecommerceEmail = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.EcommerceEmail, 1);

            //seperate email by comma and send message to those email
            string[] ecommerceEmails = ecommerceEmail.Value.Split(',').ToArray();

            //customer email, customer phone, receiver email
            EcommerceMessageDTO email = new EcommerceMessageDTO
            {
                CustomerEmail = user.Email,
                CustomerPhoneNumber = user.PhoneNumber,
                CustomerCompanyName = user.Organisation,
                BusinessNature = user.BusinessNature
            };

            foreach (string data in ecommerceEmails)
            {
                email.EcommerceEmail = data;
                await _messageSenderService.SendGenericEmailMessage(MessageType.ENM, email);
            }

            //4. return registration message to the customer
            var message = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.RegistrationMessage, 1);
            return message.Value;
        }

        public async Task<UserDTO> GetCustomerCountryUsingPhoneCode(UserDTO userDTO)
        {
            if (string.IsNullOrWhiteSpace(userDTO.CountryPhoneNumberCode))
            {
                //Get all countries
                var country = await _uow.Country.FindAsync(x => x.PhoneNumberCode != null);

                foreach (var c in country)
                {
                    if (userDTO.PhoneNumber.Contains(c.PhoneNumberCode))
                    {
                        userDTO.UserActiveCountryId = c.CountryId;
                        return userDTO;
                    }
                }
            }
            else
            {
                var countryid = await _uow.Country.GetAsync(s => s.PhoneNumberCode.Equals(userDTO.CountryPhoneNumberCode));
                userDTO.UserActiveCountryId = countryid.CountryId;
            }

            return userDTO;
        }

        private async Task<bool> CheckRegistrationAccess(UserDTO user)
        {
            if (user.PhoneNumber.StartsWith("0"))
            {
                user.PhoneNumber = user.PhoneNumber.Substring(1, user.PhoneNumber.Length - 1);
            }

            var emailUsers = await _uow.User.GetUserListByEmailorPhoneNumber(user.Email, user.PhoneNumber);

            foreach (var u in emailUsers)
            {
                if (u.UserChannelType == UserChannelType.Ecommerce || u.UserChannelType == UserChannelType.Partner)
                {
                    return false;
                }
                if (u.UserChannelType == UserChannelType.Employee && u.Email == user.Email)
                {
                    return false;
                }
                else
                {
                    if (u.UserChannelType == UserChannelType.IndividualCustomer && u.IsRegisteredFromMobile == true)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private async Task<SignResponseDTO> PartnerRegistration(UserDTO user)
        {
            var result = new SignResponseDTO();

            var PhoneNumber = user.PhoneNumber;
            var EmailUser = await _uow.User.GetUserByEmailorPhoneNumber(user.Email, PhoneNumber);

            if (EmailUser != null)
            {
                if (EmailUser.Email != null)
                {
                    EmailUser.Email = EmailUser.Email.Trim();
                    EmailUser.Email = EmailUser.Email.ToLower();
                }

                if (EmailUser.UserChannelType == UserChannelType.Employee && EmailUser.Email == user.Email)
                {
                    throw new GenericException("Employee email not allowed.", $"{(int)HttpStatusCode.Forbidden}");
                }
                else if (EmailUser.IsRegisteredFromMobile == true)
                {
                    throw new GenericException("Partner already exists!", $"{(int)HttpStatusCode.Forbidden}");
                }
                else
                {
                    var phonepartnerdetails = await _uow.Partner.GetAsync(s => s.PhoneNumber.Contains(PhoneNumber) || s.Email == user.Email);
                    if (phonepartnerdetails != null)
                    {
                        throw new GenericException("Customer already Exists as a Partner!", $"{(int)HttpStatusCode.Forbidden}");
                    }
                    else
                    {
                        //1. add new
                        if (EmailUser.UserChannelType == UserChannelType.Employee && EmailUser.Email != user.Email)
                        {
                            return await AddNewPartner(user, PhoneNumber);
                        }
                        else
                        {
                            //1. update partner record
                            var Vehicle = "";
                            foreach (var vehicle in user.VehicleType)
                            {
                                Vehicle = vehicle;
                            }

                            EmailUser.FirstName = user.FirstName;
                            EmailUser.LastName = user.LastName;
                            EmailUser.PhoneNumber = user.PhoneNumber;
                            EmailUser.Email = user.Email;
                            EmailUser.IsRegisteredFromMobile = true;
                            EmailUser.UserActiveCountryId = user.UserActiveCountryId;
                            EmailUser.AppType = user.AppType;
                            EmailUser.UserName = user.Email;
                            EmailUser.UserChannelPassword = user.Password;
                            var UpdatedUser = Mapper.Map<UserDTO>(EmailUser);
                            var u = await _userService.UpdateUser(UpdatedUser.Id, UpdatedUser);

                            var partnerDTO = new Partner
                            {
                                PartnerType = PartnerType.DeliveryPartner,
                                PartnerName = user.FirstName + " " + user.LastName,
                                PartnerCode = EmailUser.UserChannelCode,
                                FirstName = user.FirstName,
                                LastName = user.LastName,
                                Email = user.Email,
                                PhoneNumber = user.PhoneNumber,
                                UserId = EmailUser.Id,
                                IsActivated = false,
                                UserActiveCountryId = user.UserActiveCountryId
                            };
                            _uow.Partner.Add(partnerDTO);

                            var vehicletypeDTO = new VehicleType
                            {
                                Partnercode = partnerDTO.PartnerCode,
                                Vehicletype = Vehicle
                            };
                            _uow.VehicleType.Add(vehicletypeDTO);

                            user.UserChannelCode = EmailUser.UserChannelCode;
                            user.Id = EmailUser.Id;
                            await _uow.CompleteAsync();

                            var response = await _userService.ResetPassword(EmailUser.Id, user.Password);
                            if (response.Succeeded)
                            {
                                result = await SendOTPForRegisteredUser(user);
                            }
                            var User = Mapper.Map<UserDTO>(EmailUser);
                            await GenerateReferrerCode(User);
                        }
                    }
                }
            }
            else
            {
                //2. New user
                return await AddNewPartner(user, PhoneNumber);
            }

            return result;
        }

        private async Task<SignResponseDTO> AddNewPartner(UserDTO user, string PhoneNumber)
        {
            var result = new SignResponseDTO();

            var phonepartnerdetails = await _uow.Partner.GetAsync(s => s.PhoneNumber.Contains(PhoneNumber) || s.Email == user.Email);
            if (phonepartnerdetails != null)
            {
                throw new GenericException("Customer details already Exists as a Partner!", $"{(int)HttpStatusCode.Forbidden}");
            }
            else
            {
                var PartnerCode = await _numberGeneratorMonitorService.GenerateNextNumber(NumberGeneratorType.Partner);
                user.UserChannelCode = PartnerCode;

                //create login access
                var FinalUser = await CreateNewuser(user);

                var Vehicle = "";
                foreach (var vehicle in user.VehicleType)
                {
                    Vehicle = vehicle;
                }

                var partnerDTO = new Partner
                {
                    PartnerType = PartnerType.DeliveryPartner,
                    PartnerName = user.FirstName + " " + user.LastName,
                    PartnerCode = FinalUser.UserChannelCode,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    UserId = FinalUser.Id,
                    IsActivated = false,
                    UserActiveCountryId = user.UserActiveCountryId,
                    ActivityStatus = ActivityStatus.Idle,
                    ActivityDate = DateTime.Now
                };
                _uow.Partner.Add(partnerDTO);

                var vehicletypeDTO = new VehicleType
                {
                    Partnercode = partnerDTO.PartnerCode,
                    Vehicletype = Vehicle
                };
                _uow.VehicleType.Add(vehicletypeDTO);

                await _uow.CompleteAsync();

                await _walletService.AddWallet(new WalletDTO
                {
                    CustomerId = partnerDTO.PartnerId,
                    CustomerType = CustomerType.Partner,
                    CustomerCode = partnerDTO.PartnerCode,
                    CompanyType = CustomerType.Partner.ToString()
                });

                result = await SendOTPForRegisteredUser(user);
                var User = Mapper.Map<UserDTO>(FinalUser);
                user.Id = FinalUser.Id;
                await GenerateReferrerCode(user);
            }

            return result;
        }

        private async Task<SignResponseDTO> IndividualRegistration(UserDTO user)
        {
            var result = new SignResponseDTO();

            var PhoneNumber = user.PhoneNumber;
            var EmailUser = await _uow.User.GetUserByEmailorPhoneNumber(user.Email, PhoneNumber);

            if (EmailUser != null)
            {
                if (EmailUser.Email != null)
                {
                    EmailUser.Email = EmailUser.Email.Trim().ToLower();
                }

                if (EmailUser.UserChannelType == UserChannelType.Employee && EmailUser.Email == user.Email)
                {
                    throw new GenericException("Employee email not allowed.", $"{(int)HttpStatusCode.Forbidden}");
                }
                else if (EmailUser.UserChannelType == UserChannelType.Ecommerce && EmailUser.IsRegisteredFromMobile != true)
                {
                    throw new GenericException("Account Already Exists. Kindly Login!!!", $"{(int)HttpStatusCode.Forbidden}");
                }
                else if (EmailUser.IsRegisteredFromMobile == true)
                {
                    throw new GenericException("Customer already exists!", $"{(int)HttpStatusCode.Forbidden}");
                }
                else
                {
                    var emailcustomerdetails = await _uow.IndividualCustomer.GetAsync(s => s.Email == user.Email || s.PhoneNumber.Contains(PhoneNumber));
                    if (emailcustomerdetails != null)
                    {
                        if (emailcustomerdetails.IsRegisteredFromMobile == true)
                        {
                            throw new GenericException("Customer already exists!", $"{(int)HttpStatusCode.Forbidden}");
                        }
                        else
                        {
                            emailcustomerdetails.IsRegisteredFromMobile = true;
                            emailcustomerdetails.Email = user.Email;
                            emailcustomerdetails.Password = user.Password;
                            emailcustomerdetails.PhoneNumber = user.PhoneNumber;
                            emailcustomerdetails.FirstName = user.FirstName;
                            emailcustomerdetails.LastName = user.LastName;
                            emailcustomerdetails.UserActiveCountryId = user.UserActiveCountryId;
                        }
                    }
                    else
                    {
                        if (EmailUser.UserChannelType == UserChannelType.Employee & EmailUser.Email != user.Email)
                        {
                            user.UserChannelType = UserChannelType.IndividualCustomer;
                            user.IsFromMobile = true;
                            var registeredUser = await CreateUserBasedOnCustomerType(user);
                            user.UserChannelCode = registeredUser.UserChannelCode;
                            user.Id = registeredUser.Id;
                            result = await SendOTPForRegisteredUser(registeredUser);
                            await GenerateReferrerCode(user);
                            return result;
                        }
                        else
                        {
                            var customer = new IndividualCustomer
                            {
                                Email = user.Email,
                                PhoneNumber = user.PhoneNumber,
                                FirstName = user.FirstName,
                                LastName = user.LastName,
                                Password = user.Password,
                                CustomerCode = EmailUser.UserChannelCode,
                                PictureUrl = user.PictureUrl,
                                IsRegisteredFromMobile = true,
                                UserActiveCountryId = user.UserActiveCountryId
                            };

                            _uow.IndividualCustomer.Add(customer);
                            EmailUser.UserChannelPassword = user.Password;
                        }
                    }

                    //update user table
                    EmailUser.FirstName = user.FirstName;
                    EmailUser.LastName = user.LastName;
                    EmailUser.PhoneNumber = user.PhoneNumber;
                    EmailUser.Email = user.Email;
                    EmailUser.IsRegisteredFromMobile = true;
                    EmailUser.UserActiveCountryId = user.UserActiveCountryId;
                    EmailUser.DateModified = DateTime.Now;
                    EmailUser.AppType = user.AppType;
                    EmailUser.UserName = user.Email;
                    EmailUser.UserChannelPassword = user.Password;
                    var UpdatedUser = Mapper.Map<UserDTO>(EmailUser);
                    var update = await _userService.UpdateUser(UpdatedUser.Id, UpdatedUser);
                    var resetPassword = await _userService.ResetPassword(EmailUser.Id, user.Password);
                    await _uow.CompleteAsync();

                    result = await SendOTPForRegisteredUser(user);
                    var User = Mapper.Map<UserDTO>(EmailUser);
                    user.UserChannelCode = EmailUser.UserChannelCode;
                    user.Id = EmailUser.Id;
                    await GenerateReferrerCode(user);
                    return result;
                }
            }
            else if (EmailUser == null)
            {
                user.UserChannelType = UserChannelType.IndividualCustomer;
                user.IsFromMobile = true;
                var registeredUser = await CreateUserBasedOnCustomerType(user);
                result = await SendOTPForRegisteredUser(registeredUser);
                user.UserChannelCode = registeredUser.UserChannelCode;
                user.Id = registeredUser.Id;
                await GenerateReferrerCode(user);
            }

            ////SEND EMAIL TO NEW SIGNEE
            var companyMessagingDTO = new CompanyMessagingDTO();
            companyMessagingDTO.Name = user.FirstName + "" + user.LastName;
            companyMessagingDTO.Email = user.Email;
            companyMessagingDTO.PhoneNumber = user.PhoneNumber;
            companyMessagingDTO.Rank = Rank.Basic;
            companyMessagingDTO.IsFromMobile = user.IsRegisteredFromMobile;
            companyMessagingDTO.UserChannelType = user.UserChannelType;
            await _companyService.SendMessageToNewSignUps(companyMessagingDTO);

            return result;
        }

        private async Task<SignResponseDTO> EcommerceRegistration(UserDTO user)
        {
            var result = new SignResponseDTO();

            if (string.IsNullOrEmpty(user.Organisation))
            {
                user.Organisation = user.FirstName + " " + user.LastName;
            }

            var PhoneNumber = user.PhoneNumber;
            var EmailUser = await _uow.User.GetUserByEmailorPhoneNumber(user.Email, PhoneNumber);
            if (EmailUser != null)
            {
                if (EmailUser.Email != null)
                {
                    EmailUser.Email = EmailUser.Email.Trim();
                    EmailUser.Email = EmailUser.Email.ToLower();
                }

                if (EmailUser.UserChannelType == UserChannelType.Employee && EmailUser.Email == user.Email)
                {
                    throw new GenericException("Employee email not allowed.", $"{(int)HttpStatusCode.Forbidden}");
                }
                else if (EmailUser.UserChannelType == UserChannelType.Ecommerce && EmailUser.IsRegisteredFromMobile != true)
                {
                    throw new GenericException("Account Already Exists. Kindly Login!!!", $"{(int)HttpStatusCode.Forbidden}");
                }
                else if (EmailUser.IsRegisteredFromMobile == true)
                {
                    throw new GenericException("Customer already exists!", $"{(int)HttpStatusCode.Forbidden}");
                }
                else
                {
                    var emailcompanydetails = await _uow.Company.GetAsync(s => s.Email == user.Email || s.PhoneNumber.Contains(PhoneNumber));

                    if (emailcompanydetails != null)
                    {
                        if (emailcompanydetails.IsRegisteredFromMobile == true)
                        {
                            throw new GenericException("Email already Exists as Company Customer!", $"{(int)HttpStatusCode.Forbidden}");
                        }
                        else if (emailcompanydetails.Name.Equals(user.Organisation, StringComparison.OrdinalIgnoreCase))
                        {
                            throw new GenericException($"Company Name Already Exists. Kindly provide another one!!!", $"{(int)HttpStatusCode.Forbidden}");
                        }
                        else
                        {
                            emailcompanydetails.IsRegisteredFromMobile = true;
                            emailcompanydetails.Email = user.Email;
                            emailcompanydetails.Password = user.Password;
                            emailcompanydetails.PhoneNumber = user.PhoneNumber;
                            emailcompanydetails.FirstName = user.FirstName;
                            emailcompanydetails.LastName = user.LastName;
                            emailcompanydetails.Name = user.Organisation;
                            emailcompanydetails.UserActiveCountryId = user.UserActiveCountryId;
                            emailcompanydetails.IsEligible = user.IsEligible;
                        }
                    }
                    else
                    {
                        if (EmailUser.UserChannelType == UserChannelType.Employee && EmailUser.Email != user.Email)
                        {
                            user.UserChannelType = UserChannelType.Ecommerce;
                            user.IsFromMobile = true;
                            var registeredUser = await CreateUserBasedOnCustomerType(user);
                            user.UserChannelCode = registeredUser.UserChannelCode;
                            user.Id = registeredUser.Id;
                            result = await SendOTPForRegisteredUser(registeredUser);
                            await GenerateReferrerCode(user);
                            return result;
                        }
                        else
                        {
                            var checkCompanyName = await _uow.Company.FindAsync(x => x.Name == user.Organisation);

                            if (checkCompanyName.Any())
                            {
                                throw new GenericException($"Company Name Already Exists. Kindly provide another one!!!", $"{(int)HttpStatusCode.Forbidden}");
                            }

                            var customer = new Company
                            {
                                Email = user.Email,
                                PhoneNumber = user.PhoneNumber,
                                FirstName = user.FirstName,
                                LastName = user.LastName,
                                Password = user.Password,
                                CustomerCode = EmailUser.UserChannelCode,
                                CompanyType = CompanyType.Ecommerce,
                                Discount = 0.00M,
                                IsRegisteredFromMobile = true,
                                CompanyStatus = CompanyStatus.Active,
                                SettlementPeriod = 1,
                                ReturnServiceCentre = 0,
                                UserActiveCountryId = user.UserActiveCountryId,
                                Name = user.Organisation,
                                isCodNeeded = (bool)user.RequiresCod,
                                IsEligible = user.IsEligible
                            };
                            _uow.Company.Add(customer);

                            EmailUser.UserChannelPassword = user.Password;
                        }
                    }

                    EmailUser.FirstName = user.FirstName;
                    EmailUser.LastName = user.LastName;
                    EmailUser.PhoneNumber = user.PhoneNumber;
                    EmailUser.Email = user.Email;
                    EmailUser.IsRegisteredFromMobile = true;
                    EmailUser.UserActiveCountryId = user.UserActiveCountryId;
                    EmailUser.AppType = user.AppType;
                    EmailUser.UserName = user.Email;
                    EmailUser.UserChannelPassword = user.Password;
                    var UpdatedUser = Mapper.Map<UserDTO>(EmailUser);
                    var update = await _userService.UpdateUser(UpdatedUser.Id, UpdatedUser);
                    var u = await _userService.ResetPassword(EmailUser.Id, user.Password);
                    await _uow.CompleteAsync();

                    result = await SendOTPForRegisteredUser(user);
                    user.UserChannelCode = EmailUser.UserChannelCode;
                    user.Id = EmailUser.Id;
                    await GenerateReferrerCode(user);
                    return result;
                }
            }
            else if (EmailUser == null)
            {
                user.UserChannelType = UserChannelType.Ecommerce;
                user.IsFromMobile = true;
                var registeredUser = await CreateUserBasedOnCustomerType(user);
                result = await SendOTPForRegisteredUser(registeredUser);
                user.UserChannelCode = registeredUser.UserChannelCode;
                user.Id = registeredUser.Id;
                await GenerateReferrerCode(user);
            }

            return result;
        }

        private async Task<SignResponseDTO> SendOTPForRegisteredUser(UserDTO user)
        {
            var Otp = await _otpService.GenerateOTP(user);
            await _otpService.SendOTP(Otp);

            return new SignResponseDTO
            {
                EmailSent = true,
                PhoneSent = true
            };
        }

        public async Task<SignResponseDTO> ResendOTP(UserDTO user)
        {
            var registerUser = await CheckUser(user);
            var result = await SendOTPForRegisteredUser(registerUser);
            return result;
        }

        public async Task<List<StationDTO>> GetLocalStations()
        {
            var CountryId = await _preShipmentMobileService.GetCountryId();
            var countryIds = new int[1];   //NIGERIA
            countryIds[0] = CountryId;
            return await _uow.Station.GetLocalStationsWithoutSuperServiceCentre(countryIds);
        }

        public async Task<UserDTO> CheckUser(UserDTO user)
        {
            // bool isEmail = Regex.IsMatch(user.Email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
            bool isEmail = Regex.IsMatch(user.Email, @"\A(?:[a-z0-9_]+(?:\.[a-z0-9_]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
            if (isEmail)
            {
                user.Email.Trim();
                var registerUser = await _userService.GetUserByEmail(user.Email);
                return registerUser;
            }
            else
            {
                bool IsPhone = Regex.IsMatch(user.PhoneNumber, @"\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})");
                if (IsPhone)
                {
                    user.PhoneNumber = user.PhoneNumber.Remove(0, 1);
                    var registerUser = await _userService.GetUserByPhone(user.PhoneNumber);
                    return registerUser;
                }
                else
                {
                    throw new GenericException("Invalid Details");
                }
            }
        }

        public async Task<Dictionary<string, List<StationDTO>>> GetAllStations()
        {
            Dictionary<string, List<StationDTO>> StationDictionary = new Dictionary<string, List<StationDTO>>();
            //1. getcountries
            var countries = _uow.Country.GetAll().ToList().Where(s => s.IsActive == true);
            //2. get the station attached to a country based on the contryId
            foreach (var country in countries)
            {
                var countryIds = new int[1];
                countryIds[0] = country.CountryId;
                var stationdtos = await _uow.Station.GetLocalStations(countryIds);
                //3. add the country and its' stations 
                StationDictionary.Add(country.CountryName, stationdtos);
            }
            return StationDictionary;
        }

        private async Task<CountryDTO> GetUserActiveCountry(UserDTO user)
        {
            CountryDTO userActiveCountry = new CountryDTO { };

            if (user != null)
            {
                int userActiveCountryId = user.UserActiveCountryId;

                if (userActiveCountryId == 0)
                {
                    if (user.UserChannelType == UserChannelType.IndividualCustomer)
                    {
                        userActiveCountryId = _uow.IndividualCustomer.GetAllAsQueryable().Where(x => x.CustomerCode == user.UserChannelCode).Select(x => x.UserActiveCountryId).FirstOrDefault();
                    }
                    else if (user.UserChannelType == UserChannelType.Ecommerce || user.UserChannelType == UserChannelType.Corporate)
                    {
                        userActiveCountryId = _uow.Company.GetAllAsQueryable().Where(x => x.CustomerCode == user.UserChannelCode).Select(x => x.UserActiveCountryId).FirstOrDefault();
                    }
                }

                var country = await _uow.Country.GetAsync(userActiveCountryId);
                userActiveCountry = Mapper.Map<CountryDTO>(country);
            }

            return userActiveCountry;
        }

        public async Task<object> GetUserCountryCode(UserDTO user)
        {
            var userCountry = await GetUserActiveCountry(user);

            string countryCode = userCountry.CurrencyCode.Length <= 2 ? userCountry.CurrencyCode : userCountry.CurrencyCode.Substring(0, 2);

            return new { CountryCode = countryCode, CurrencyCode = userCountry.CurrencyCode, CountryId = userCountry.CountryId, CurrencySymbol = userCountry.CurrencySymbol };
        }

        public async Task<MobilePriceDTO> GetHaulagePrice(HaulagePriceDTO haulagePricingDto)
        {
            return await _preShipmentMobileService.GetHaulagePrice(haulagePricingDto);
        }

        public Task<IEnumerable<NewCountryDTO>> GetUpdatedCountries()
        {
            return _countryService.GetUpdatedCountries();
        }

        public async Task<IEnumerable<NewCountryDTO>> GetActiveCountries()
        {
            var activeCountries = await _countryService.GetActiveCountries();
            var result = Mapper.Map<IEnumerable<NewCountryDTO>>(activeCountries);
            return result;
        }

        public async Task<bool> ApproveShipment(ApproveShipmentDTO detail)
        {
            return await _preShipmentMobileService.ApproveShipment(detail);
        }

        public Task<PreShipmentSummaryDTO> GetShipmentDetailsFromDeliveryNumber(string DeliveryNumber)
        {
            return _preShipmentMobileService.GetShipmentDetailsFromDeliveryNumber(DeliveryNumber);
        }

        public async Task<bool> UpdateReceiverDetails(PreShipmentMobileDTO receiver)
        {
            return await _preShipmentMobileService.UpdateReceiverDetails(receiver);
        }

        public async Task<PartnerDTO> GetPartnerDetails(string Email)
        {
            return await _preShipmentMobileService.GetPartnerDetails(Email);
        }
        public async Task<List<Uri>> DisplayImages()
        {
            return await _preShipmentMobileService.DisplayImages();
        }
        public async Task<string> LoadImage(ImageDTO images)
        {
            return await _preShipmentMobileService.LoadImage(images);
        }
        public async Task<bool> VerifyPartnerDetails(PartnerDTO partnerDto)
        {
            return await _preShipmentMobileService.VerifyPartnerDetails(partnerDto);
        }
        public Task<string> Generate(int length)
        {
            return _codegenerator.Generate(6);
        }
        public async Task<IdentityResult> ForgotPassword(string email, string password)
        {
            if (string.IsNullOrEmpty(email.Trim()))
            {
                throw new GenericException("Operation could not complete, kindly supply valid credential", $"{(int)HttpStatusCode.Forbidden}");
            }

            return await _userService.ForgotPassword(email, password);
        }

        public async Task SendGenericEmailMessage(MessageType messageType, object obj)
        {
            await _messageSenderService.SendGenericEmailMessage(messageType, obj);
        }

        public async Task<bool> deleterecord(string detail)
        {
            return await _preShipmentMobileService.deleterecord(detail);
        }
        public async Task<bool> UpdateDeliveryNumber(MobileShipmentNumberDTO detail)
        {
            return await _preShipmentMobileService.UpdateDeliveryNumber(detail);
        }
        public async Task<bool> UpdateDeliveryNumberV2(MobileShipmentNumberDTO detail)
        {
            return await _preShipmentMobileService.UpdateDeliveryNumberV2(detail);
        }
        public async Task<bool> VerifyDeliveryCode(MobileShipmentNumberDTO detail)
        {
            return await _preShipmentMobileService.VerifyDeliveryCode(detail);
        }
        public async Task<Partnerdto> GetMonthlyPartnerTransactions()
        {
            return await _preShipmentMobileService.GetMonthlyPartnerTransactions();
        }
        public async Task<object> AddRatings(MobileRatingDTO rating)
        {
            return await _preShipmentMobileService.AddRatings(rating);
        }
        public async Task<object> CancelShipment(string Waybill)
        {
            return await _preShipmentMobileService.CancelShipment(Waybill);
        }

        public async Task<UserDTO> IsOTPValid(int OTP)
        {
            return await _otpService.IsOTPValid(OTP);
        }

        public async Task<UserDTO> ValidateOTP(OTPDTO otp)
        {
            return await _otpService.ValidateOTP(otp);
        }

        public async Task<UserDTO> CheckDetails(string user, string userchanneltype)
        {
            string emailPhone = "";

            bool isEmail = Regex.IsMatch(user, @"\A(?:[a-z0-9_]+(?:\.[a-z0-9_]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
            if (isEmail)
            {
                emailPhone = user;
            }
            else
            {
                bool IsPhone = Regex.IsMatch(user, @"\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})");
                if (IsPhone)
                {
                    if (user.StartsWith("0"))
                    {
                        user = user.Remove(0, 1);
                    }
                    emailPhone = user;
                }
                else
                {
                    emailPhone = user;
                }
            }

            return await _otpService.CheckDetails(emailPhone, userchanneltype);
        }
        public async Task<UserDTO> CheckDetailsForCustomerPortal(string user)
        {
            string emailPhone = "";

            //bool isEmail = Regex.IsMatch(user, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
            bool isEmail = Regex.IsMatch(user, @"\A(?:[a-z0-9_]+(?:\.[a-z0-9_]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
            if (isEmail)
            {
                emailPhone = user;
            }
            else
            {
                bool IsPhone = Regex.IsMatch(user, @"\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})");
                if (IsPhone)
                {
                    user = user.Remove(0, 1);
                    emailPhone = user;
                }
                else
                {
                    emailPhone = user;
                }
            }

            return await _otpService.CheckDetailsForCustomerPortal(emailPhone);
        }
        public async Task<UserDTO> CheckDetailsForMobileScanner(string user)
        {
            string emailPhone = "";

            //bool isEmail = Regex.IsMatch(user, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
            bool isEmail = Regex.IsMatch(user, @"\A(?:[a-z0-9_]+(?:\.[a-z0-9_]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
            if (isEmail)
            {
                emailPhone = user;
            }
            else
            {
                bool IsPhone = Regex.IsMatch(user, @"\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})");
                if (IsPhone)
                {
                    user = user.Remove(0, 1);
                    emailPhone = user;
                }
                else
                {
                    emailPhone = user;
                }
            }

            return await _otpService.CheckDetailsForMobileScanner(emailPhone);
        }

        public async Task<UserDTO> CheckDetailsForAgentApp(string user)
        {
            string emailPhone = "";

            bool isEmail = Regex.IsMatch(user, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
            if (isEmail)
            {
                emailPhone = user;
            }
            else
            {
                bool IsPhone = Regex.IsMatch(user, @"\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})");
                if (IsPhone)
                {
                    user = user.Remove(0, 1);
                    emailPhone = user;
                }
                else
                {
                    emailPhone = user;
                }
            }

            return await _otpService.CheckDetailsForAgentApp(emailPhone);
        }

        public async Task<bool> CreateCustomer(string CustomerCode)
        {
            return await _preShipmentMobileService.CreateCustomer(CustomerCode);
        }
        public async Task<PartnerDTO> CreatePartner(string CustomerCode)
        {
            return await _preShipmentMobileService.CreatePartner(CustomerCode);
        }
        public async Task<bool> CreateCompany(string CustomerCode)
        {
            return await _preShipmentMobileService.CreateCompany(CustomerCode);
        }

        public async Task<bool> EditProfile(UserDTO user)
        {
            return await _preShipmentMobileService.EditProfile(user);
        }

        public async Task<object> AddPreShipmentMobile(PreShipmentMobileDTO preShipment)
        {
            var isDisable = ConfigurationManager.AppSettings["DisableShipmentCreation"];
            bool disableShipmentCreation = bool.Parse(isDisable);

            bool allowTestUser = await AllowTestingUserToCreateShipment();

            if (allowTestUser)
            {
                disableShipmentCreation = false;
            }

            if (disableShipmentCreation)
            {
                string message = ConfigurationManager.AppSettings["DisableShipmentCreationMessage"];
                throw new GenericException(message, $"{(int)HttpStatusCode.ServiceUnavailable}");
            }
            return await _preShipmentMobileService.AddPreShipmentMobile(preShipment);
        }

        public async Task<PreShipmentMobileThirdPartyDTO> AddPreShipmentMobileForThirdParty(CreatePreShipmentMobileDTO preShipment)
        {
            var isDisable = ConfigurationManager.AppSettings["DisableShipmentCreation"];
            bool disableShipmentCreation = bool.Parse(isDisable);

            bool allowTestUser = await AllowTestingUserToCreateShipment();

            if (allowTestUser)
            {
                disableShipmentCreation = false;
            }

            if (disableShipmentCreation)
            {
                string message = ConfigurationManager.AppSettings["DisableShipmentCreationMessage"];
                throw new GenericException(message, $"{(int)HttpStatusCode.ServiceUnavailable}");
            }
            return await _preShipmentMobileService.AddPreShipmentMobileThirdParty(preShipment);
        }


        //Remove later, quick fix to test live shipment
        private async Task<bool> AllowTestingUserToCreateShipment()
        {
            bool result = false;

            //Excluding It Test
            string excludeUserList = ConfigurationManager.AppSettings["excludeUserList"];
            string[] testUserId = excludeUserList.Split(',').ToArray();

            var testUser = await _userService.GetCurrentUserId();

            if (testUserId.Contains(testUser))
            {
                result = true;
            }

            return result;
        }

        public async Task<MultipleShipmentOutput> AddMultiplePreShipmentMobile(NewPreShipmentMobileDTO preShipment)
        {
            return await _preShipmentMobileService.CreateMobileShipment(preShipment);
        }
        public async Task<List<PreShipmentMobileDTO>> GetPreShipmentForUser()
        {
            return await _preShipmentMobileService.GetPreShipmentForUser();
        }
        public async Task<List<TransactionPreShipmentDTO>> GetPreShipmentForUser(UserDTO user, ShipmentCollectionFilterCriteria filterCriteria)
        {
            return await _preShipmentMobileService.GetPreShipmentForUser(user, filterCriteria);
        }
        public async Task<WalletTransactionSummaryDTO> GetWalletTransactionsForMobile()
        {
            return await _iWalletTransactionService.GetWalletTransactionsForMobile();
        }

        public async Task<ModifiedWalletTransactionSummaryDTO> GetWalletTransactionsForMobile(ShipmentCollectionFilterCriteria filterCriteria)
        {
            var currentUser = await _userService.GetCurrentUserId();
            var user = await _uow.User.GetUserById(currentUser);
            var userDTO = Mapper.Map<UserDTO>(user);

            var transactionSummary = await _iWalletTransactionService.GetWalletTransactionsForMobile(userDTO, filterCriteria);
            var preshipments = await GetPreShipmentForUser(userDTO, filterCriteria);
            var status = await GetShipmentStatus();

            transactionSummary.Shipments = preshipments;
            transactionSummary.Status = status;
            return transactionSummary;
        }

        public async Task<MobilePriceDTO> GetPrice(PreShipmentMobileDTO preShipment)
        {
            if (!preShipment.PreShipmentItems.Any())
            {
                throw new GenericException($"Shipment Items cannot be empty", $"{(int)HttpStatusCode.Forbidden}");
            }

            var zoneid = await _domesticroutezonemapservice.GetZoneMobile(preShipment.SenderStationId, preShipment.ReceiverStationId);
            preShipment.ZoneMapping = zoneid.ZoneId;

            if (string.IsNullOrEmpty(preShipment.VehicleType))
            {
                return await _preShipmentMobileService.GetPrice(preShipment);
            }

            if (preShipment.VehicleType.ToLower() == Vehicletype.Bike.ToString().ToLower() && preShipment.ZoneMapping == 1
                && preShipment.SenderLocation.Latitude != null && preShipment.SenderLocation.Longitude != null
                && preShipment.ReceiverLocation.Latitude != null && preShipment.ReceiverLocation.Longitude != null)
            {
                return await _preShipmentMobileService.GetPriceForBike(preShipment);
            }
            else
            {
                return await _preShipmentMobileService.GetPrice(preShipment);
            }
        }

        public async Task<MobilePriceDTO> GetPriceForDropOff(PreShipmentMobileDTO preShipment)
        {
            return await _preShipmentMobileService.GetPriceForDropOff(preShipment);
        }

        public async Task<MultipleMobilePriceDTO> GetPriceForMultipleShipments(NewPreShipmentMobileDTO preShipment)
        {
            return await _preShipmentMobileService.GetPriceForMultipleShipments(preShipment);
        }
        public async Task<MobileGroupCodeWaybillMappingDTO> GetWaybillDetailsInGroup(string groupCode)
        {
            return await _groupCodeWaybillMappingService.GetWaybillDetailsInGroup(groupCode);
        }
        public async Task<MobileGroupCodeWaybillMappingDTO> GetWaybillNumbersInGroup(string groupCode)
        {
            return await _groupCodeWaybillMappingService.GetWaybillNumbersInGroup(groupCode);
        }
        public async Task<WalletDTO> GetWalletBalance()
        {
            return await _walletService.GetWalletBalance();
        }
        public async Task<WalletDTO> GetWalletBalanceWithName()
        {
            return await _walletService.GetWalletBalanceWithName();
        }
        public async Task<SpecialResultDTO> GetSpecialPackages()
        {
            return await _preShipmentMobileService.GetSpecialPackages();
        }
        public async Task<MobileShipmentTrackingHistoryDTO> trackShipment(string waybillNumber)
        {
            return await _preShipmentMobileService.TrackShipment(waybillNumber);
        }
        public async Task<PreShipmentMobileDTO> AddMobilePickupRequest(MobilePickUpRequestsDTO pickuprequest)
        {
            return await _preShipmentMobileService.AddMobilePickupRequest(pickuprequest);
        }
        public async Task<bool> ChangeShipmentOwnershipForPartner(PartnerReAssignmentDTO request)
        {
            return await _preShipmentMobileService.ChangeShipmentOwnershipForPartner(request);
        }

        public async Task<List<PreShipmentMobileDTO>> AddMobilePickupRequestMultipleShipment(MobilePickUpRequestsDTO pickuprequest)
        {
            return await _preShipmentMobileService.AddMobilePickupRequestMultipleShipment(pickuprequest);
        }

        public async Task<List<MobilePickUpRequestsDTO>> GetMobilePickupRequest()
        {
            return await _preShipmentMobileService.GetMobilePickupRequest();
        }
        public async Task<bool> UpdateMobilePickupRequest(MobilePickUpRequestsDTO pickuprequest)
        {
            return await _preShipmentMobileService.UpdateMobilePickupRequest(pickuprequest);
        }
        public async Task<bool> UpdateMobilePickupRequestUsingGroupCode(MobilePickUpRequestsDTO pickuprequest)
        {
            return await _preShipmentMobileService.UpdateMobilePickupRequestUsingGroupCode(pickuprequest);
        }
        public async Task<bool> UpdateMobilePickupRequestUsingWaybill(MobilePickUpRequestsDTO pickuprequest)
        {
            return await _preShipmentMobileService.UpdateMobilePickupRequestUsingWaybill(pickuprequest);
        }

        public async Task<object> ResolveDisputeForMobile(PreShipmentMobileDTO preShipment)
        {
            return await _preShipmentMobileService.ResolveDisputeForMobile(preShipment);
        }
        public async Task<object> ResolveDisputeForMultipleShipment(PreShipmentMobileDTO preShipment)
        {
            return await _preShipmentMobileService.ResolveDisputeForMultipleShipments(preShipment);
        }
        public async Task<PreShipmentMobileDTO> GetPreShipmentDetail(string waybill)
        {
            return await _preShipmentMobileService.GetPreShipmentDetail(waybill);
        }
        public async Task<bool> UpdatePreShipmentMobileDetails(List<PreShipmentItemMobileDTO> preshipmentmobile)
        {
            return await _preShipmentMobileService.UpdatePreShipmentMobileDetails(preshipmentmobile);
        }
        public async Task<List<PreShipmentMobileDTO>> GetDisputePreShipment()
        {
            return await _preShipmentMobileService.GetDisputePreShipment();
        }
        public async Task<SummaryTransactionsDTO> GetPartnerWalletTransactions()
        {
            return await _preShipmentMobileService.GetPartnerWalletTransactions();
        }
        public async Task<bool> UpdateVehicleProfile(UserDTO user)
        {
            return await _preShipmentMobileService.UpdateVehicleProfile(user);
        }
        public async Task<IEnumerable<LGADTO>> GetActiveLGAs()
        {
            try
            {
                return await _uow.LGA.GetActiveLGAs();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IEnumerable<LGADTO>> GetActiveHomeDeliveryLocations()
        {
            try
            {
                return await _uow.LGA.GetActiveHomeDeliveryLocations();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<AdminReportDTO> WebsiteData()
        {
            return await _adminReportService.DisplayWebsiteData();
        }

        public async Task AddWallet(WalletDTO wallet)
        {
            var exists = await _uow.Wallet.ExistAsync(s => s.CustomerCode == wallet.CustomerCode);
            if (!exists)
            {
                await _walletService.AddWallet(wallet);
            }
        }

        private async Task<UserDTO> CreateUserBasedOnCustomerType(UserDTO user)
        {
            try
            {
                var User = new UserDTO();

                if (string.IsNullOrEmpty(user.Organisation))
                {
                    user.Organisation = user.FirstName + " " + user.LastName;
                }

                if (user.UserChannelType == UserChannelType.IndividualCustomer)
                {
                    var emailcustomerdetails = await _uow.IndividualCustomer.GetAsync(s => s.Email == user.Email || s.PhoneNumber.Contains(user.PhoneNumber));
                    if (emailcustomerdetails != null)
                    {
                        emailcustomerdetails.IsRegisteredFromMobile = true;
                        emailcustomerdetails.Email = user.Email;
                        emailcustomerdetails.Password = user.Password;
                        emailcustomerdetails.PhoneNumber = user.PhoneNumber;
                        emailcustomerdetails.FirstName = user.FirstName;
                        emailcustomerdetails.LastName = user.LastName;
                        emailcustomerdetails.UserActiveCountryId = user.UserActiveCountryId;

                        // add customer to user's table.
                        user.UserChannelCode = emailcustomerdetails.CustomerCode;
                        User = await CreateNewuser(user);
                        await _uow.CompleteAsync();
                    }
                    else
                    {
                        var customerCode = await _numberGeneratorMonitorService.GenerateNextNumber(NumberGeneratorType.CustomerCodeIndividual);
                        user.UserChannelCode = customerCode;
                        var customer = new IndividualCustomer
                        {
                            Email = user.Email,
                            PhoneNumber = user.PhoneNumber,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            CustomerCode = customerCode,
                            PictureUrl = user.PictureUrl,
                            IsRegisteredFromMobile = true,
                            UserActiveCountryId = user.UserActiveCountryId
                        };

                        //update : we need to check if the customer exists before adding 
                        _uow.IndividualCustomer.Add(customer);

                        // add customer to user's table.
                        User = await CreateNewuser(user);

                        await _uow.CompleteAsync();

                        // add customer to a wallet
                        await _walletService.AddWallet(new WalletDTO
                        {
                            CustomerId = customer.IndividualCustomerId,
                            CustomerType = CustomerType.IndividualCustomer,
                            CustomerCode = customer.CustomerCode,
                            CompanyType = CustomerType.IndividualCustomer.ToString()
                        });
                    }
                }

                if (user.UserChannelType == UserChannelType.Ecommerce)
                {
                    var checkCompanyName = await _uow.Company.FindAsync(x => x.Name == user.Organisation);

                    if (checkCompanyName.Any())
                    {
                        throw new GenericException($"Company Name Already Exists. Kindly provide another one!!!", $"{(int)HttpStatusCode.Forbidden}");
                    }

                    var customerCode = await _numberGeneratorMonitorService.GenerateNextNumber(NumberGeneratorType.CustomerCodeEcommerce);
                    user.UserChannelCode = customerCode;
                    var companydto = new Company
                    {
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        CustomerCode = customerCode,
                        IsRegisteredFromMobile = true,
                        UserActiveCountryId = user.UserActiveCountryId,
                        CompanyType = CompanyType.Ecommerce,
                        Name = user.Organisation,
                        isCodNeeded = (bool)user.RequiresCod,
                        IsEligible = user.IsEligible
                    };

                    _uow.Company.Add(companydto);

                    // add customer to user's table.
                    User = await CreateNewuser(user);
                    await _uow.CompleteAsync();

                    // add customer to a wallet
                    await _walletService.AddWallet(new WalletDTO
                    {
                        CustomerId = companydto.CompanyId,
                        CustomerType = CustomerType.Company,
                        CustomerCode = customerCode,
                        CompanyType = CompanyType.Ecommerce.ToString()
                    });

                    //return user;
                }

                return User;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<UserDTO> CreateNewuser(UserDTO user)
        {
            try
            {
                var result = new UserDTO
                {
                    ConfirmPassword = user.Password,
                    DateCreated = DateTime.Now,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Password = user.Password,
                    PhoneNumber = user.PhoneNumber,
                    UserType = UserType.Regular,
                    Username = user.UserChannelCode,
                    UserChannelCode = user.UserChannelCode,
                    UserChannelPassword = user.Password,
                    PasswordExpireDate = DateTime.Now,
                    UserActiveCountryId = user.UserActiveCountryId,
                    IsFromMobile = true,
                    IsRegisteredFromMobile = true,
                    AppType = user.AppType,
                    IsUniqueInstalled = user.IsUniqueInstalled,
                    RegistrationReferrercode = user.RegistrationReferrercode
                };

                string username = null;
                string customerType = null;

                if (user.UserChannelType == UserChannelType.IndividualCustomer)
                {
                    customerType = CustomerType.IndividualCustomer.ToString();
                    result.Department = customerType;
                    result.Designation = customerType;
                    result.UserChannelType = UserChannelType.IndividualCustomer;
                    username = (user.UserChannelType == UserChannelType.IndividualCustomer) ? user.Email : user.UserChannelCode;
                }
                else if (user.UserChannelType == UserChannelType.Ecommerce)
                {
                    customerType = CustomerType.Company.ToString();
                    result.Department = customerType;
                    result.Designation = customerType;
                    result.Organisation = user.Organisation;
                    result.UserChannelType = UserChannelType.Ecommerce;
                    username = (user.UserChannelType == UserChannelType.Ecommerce) ? user.Email : user.UserChannelCode;
                }
                else
                {
                    customerType = CustomerType.Partner.ToString();
                    result.Department = customerType;
                    result.Designation = customerType;
                    result.UserChannelType = UserChannelType.Partner;
                    username = (user.UserChannelType == UserChannelType.Partner) ? user.Email : user.UserChannelCode;
                }

                var FinalUser = Mapper.Map<User>(result);
                FinalUser.Id = Guid.NewGuid().ToString();
                FinalUser.DateCreated = DateTime.Now.Date;
                FinalUser.DateModified = DateTime.Now.Date;
                FinalUser.PasswordExpireDate = DateTime.Now;
                FinalUser.UserName = username;
                var u = await _uow.User.RegisterUser(FinalUser, user.Password);
                user.Id = FinalUser.Id;

                if (!u.Succeeded)
                {
                    throw new GenericException($"{string.Join(", ", u.Errors.ToList())}", $"{(int)HttpStatusCode.BadRequest}");
                }

                return user;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserDTO> GenerateReferrerCode(UserDTO user)
        {
            return await _otpService.GenerateReferrerCode(user);
        }
        public async Task<string> Decrypt()
        {
            var GoogleApiKey = ConfigurationManager.AppSettings["DistanceApiKey"];
            return await _partnertransactionservice.Decrypt(GoogleApiKey);
        }

        public async Task<string> EncryptWebsiteKey()
        {
            var apiKey = ConfigurationManager.AppSettings["WebsiteKey"];
            return await _partnertransactionservice.Encrypt(apiKey);
        }
        public async Task<object> CancelShipmentWithNoCharge(CancelShipmentDTO shipment)
        {
            return await _preShipmentMobileService.CancelShipmentWithNoCharge(shipment.Waybill, shipment.Userchanneltype);
        }

        public async Task SendPickUpRequestMessage(string userId)
        {
            await _messageSenderService.SendVoiceMessageAsync(userId);
        }
        public async Task<List<GiglgoStationDTO>> GetGoStations()
        {
            return await _preShipmentMobileService.GetGoStations();
        }
        public async Task<List<DeliveryNumberDTO>> GetDeliveryNumbers(int count)
        {
            try
            {
                var deliverynumberDto = new List<DeliveryNumberDTO>();
                deliverynumberDto = await GenerateDeliveryNumber(count);
                return await Task.FromResult(deliverynumberDto);
            }
            catch (Exception)
            {
                throw;
            }
        }
        private async Task<List<DeliveryNumberDTO>> GenerateDeliveryNumber(int value)
        {
            var deliveryNumberlist = new List<DeliveryNumberDTO>();
            for (int i = 0; i < value; i++)
            {
                var tagNumber = await _preShipmentMobileService.GenerateDeliveryCode();
                var number = new DeliveryNumber
                {
                    Number = tagNumber,
                    IsUsed = false,
                };
                var deliverynumberDTO = Mapper.Map<DeliveryNumberDTO>(number);
                deliveryNumberlist.Add(deliverynumberDTO);
                _uow.DeliveryNumber.Add(number);
                await _uow.CompleteAsync();
            }
            return await Task.FromResult(deliveryNumberlist);
        }
        public async Task<bool> UpdateGIGGoShipmentStaus(MobilePickUpRequestsDTO mobilePickUpRequestsDTO)
        {
            try
            {
                var preShipmentMobile = await _uow.PreShipmentMobile.GetAsync(x => x.Waybill == mobilePickUpRequestsDTO.Waybill);
                if (preShipmentMobile == null)
                {
                    throw new GenericException("This is not a GIGGo Shipment. It can not be updated", $"{(int)HttpStatusCode.Forbidden}");
                }
                else
                {
                    if ((preShipmentMobile.shipmentstatus == MobilePickUpRequestStatus.OnwardProcessing.ToString() || preShipmentMobile.shipmentstatus == MobilePickUpRequestStatus.PickedUp.ToString())
                        && mobilePickUpRequestsDTO.Status == "Shipment created")
                    {
                        throw new GenericException($"You can not change this shipment status to {mobilePickUpRequestsDTO.Status}", $"{(int)HttpStatusCode.Forbidden}");
                    }
                    else if (preShipmentMobile.ZoneMapping == 1 && mobilePickUpRequestsDTO.Status == MobilePickUpRequestStatus.OnwardProcessing.ToString())
                    {
                        throw new GenericException("This is not an Inter-State Shipment", $"{(int)HttpStatusCode.Forbidden}");
                    }
                    else if (preShipmentMobile.shipmentstatus == MobilePickUpRequestStatus.Delivered.ToString())
                    {
                        throw new GenericException("This shipment has already been delivered. No further action can be taken", $"{(int)HttpStatusCode.Forbidden}");
                    }
                    else if (preShipmentMobile.shipmentstatus == MobilePickUpRequestStatus.Cancelled.ToString())
                    {
                        throw new GenericException("The GIGGo Shipment has been cancelled. It can not be updated", $"{(int)HttpStatusCode.Forbidden}");
                    }
                    else
                    {
                        preShipmentMobile.shipmentstatus = mobilePickUpRequestsDTO.Status;

                        string pickedUp = MobilePickUpRequestStatus.PickedUp.ToString();
                        string onwardProcessing = MobilePickUpRequestStatus.OnwardProcessing.ToString();
                        string delivered = MobilePickUpRequestStatus.Delivered.ToString();

                        if (mobilePickUpRequestsDTO.Status == pickedUp || mobilePickUpRequestsDTO.Status == onwardProcessing || mobilePickUpRequestsDTO.Status == delivered)
                        {
                            ShipmentScanStatus status = ShipmentScanStatus.MCRT;

                            if (mobilePickUpRequestsDTO.Status == pickedUp)
                            {
                                status = ShipmentScanStatus.MSHC;
                            }

                            if (mobilePickUpRequestsDTO.Status == onwardProcessing)
                            {
                                status = ShipmentScanStatus.MSVC;
                            }

                            if (mobilePickUpRequestsDTO.Status == delivered)
                            {
                                status = ShipmentScanStatus.MAHD;
                            }

                            await _preShipmentMobileService.ScanMobileShipment(new ScanDTO
                            {
                                WaybillNumber = mobilePickUpRequestsDTO.Waybill,
                                ShipmentScanStatus = status
                            });
                        }

                        await _uow.CompleteAsync();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<bool> CreateOrUpdateDropOff(PreShipmentDTO preShipmentDTO)
        {
            bool tempCode;

            if (preShipmentDTO == null)
            {
                throw new GenericException("NULL INPUT");
            }

            if (string.IsNullOrWhiteSpace(preShipmentDTO.TempCode))
            {
                tempCode = await CreateTemporaryShipment(preShipmentDTO);
            }
            else
            {
                var existingPreShipment = await _uow.PreShipment.GetAsync(x => x.TempCode == preShipmentDTO.TempCode);
                if (existingPreShipment != null)
                {
                    tempCode = await UpdateTemporaryShipment(preShipmentDTO);
                }
                else
                {
                    tempCode = await CreateTemporaryShipment(preShipmentDTO);
                }
            }

            return tempCode;
        }

        //Drop Off for only Fast Track Agent
        public async Task<bool> CreateOrUpdateDropOffForAgent(PreShipmentDTO preShipmentDTO)
        {
            bool tempCode;

            if (preShipmentDTO == null)
            {
                throw new GenericException("NULL INPUT");
            }

            if (string.IsNullOrWhiteSpace(preShipmentDTO.TempCode))
            {
                tempCode = await CreateTemporaryShipmentForAgent(preShipmentDTO);
            }
            else
            {
                var existingPreShipment = await _uow.PreShipment.GetAsync(x => x.TempCode == preShipmentDTO.TempCode);
                if (existingPreShipment != null)
                {
                    tempCode = await UpdateTemporaryShipment(preShipmentDTO);
                }
                else
                {
                    tempCode = await CreateTemporaryShipmentForAgent(preShipmentDTO);
                }
            }

            return tempCode;
        }

        private async Task<bool> CreateTemporaryShipment(PreShipmentDTO preShipmentDTO)
        {
            try
            {
                if (preShipmentDTO == null)
                {
                    throw new GenericException("NULL INPUT");
                }

                //validate the input
                if (!preShipmentDTO.PreShipmentItems.Any())
                {
                    throw new GenericException("Shipment Items cannot be empty");
                }

                // get the sender info
                var currentUserId = await _userService.GetCurrentUserId();
                preShipmentDTO.SenderUserId = currentUserId;
                preShipmentDTO.IsAgent = false;

                //Get the role and name of the customer
                var user = await _userService.GetUserById(currentUserId);
                preShipmentDTO.CustomerCode = user.UserChannelCode;
                preShipmentDTO.CompanyType = user.UserChannelType.ToString();

                //For Agent
                if (user.SystemUserRole == "FastTrack Agent")
                {
                    preShipmentDTO.CompanyType = UserChannelType.IndividualCustomer.ToString();
                    preShipmentDTO.IsAgent = true;

                    //Validate sender name & phone number
                    if (string.IsNullOrWhiteSpace(preShipmentDTO.SenderName))
                    {
                        throw new GenericException("Sender Name can not be empty");
                    }

                    if (string.IsNullOrWhiteSpace(preShipmentDTO.SenderPhoneNumber))
                    {
                        throw new GenericException("Sender Phone Number can not be empty");
                    }
                }
                else
                {
                    //Get the customer name
                    if (user.UserChannelType != UserChannelType.Ecommerce && user.UserChannelType != UserChannelType.Corporate)
                    {
                        var customer = await _uow.IndividualCustomer.GetAsync(x => x.CustomerCode == user.UserChannelCode);

                        if (customer != null)
                        {
                            preShipmentDTO.SenderName = customer.FirstName + " " + customer.LastName;
                            preShipmentDTO.SenderPhoneNumber = customer.PhoneNumber;
                        }
                    }
                    else
                    {
                        var customer = await _uow.Company.GetAsync(x => x.CustomerCode == user.UserChannelCode);

                        if (customer != null)
                        {
                            //If the account is not active, block the customer from creating shipment
                            if (customer.CompanyStatus != CompanyStatus.Active)
                            {
                                throw new GenericException($"Your account has been {customer.CompanyStatus}, contact support for assistance", $"{(int)HttpStatusCode.Forbidden}");
                            }

                            preShipmentDTO.SenderName = customer.Name;
                            preShipmentDTO.SenderPhoneNumber = customer.PhoneNumber;
                        }
                    }
                }

                var country = await _uow.Country.GetCountryByStationId(preShipmentDTO.DepartureStationId);
                preShipmentDTO.CountryId = country.CountryId;

                var newPreShipment = Mapper.Map<PreShipment>(preShipmentDTO);
                newPreShipment.TempCode = await _numberGeneratorMonitorService.GenerateNextNumber(NumberGeneratorType.PreShipmentCode); ;
                newPreShipment.ApproximateItemsWeight = 0;
                newPreShipment.IsProcessed = false;
                newPreShipment.IsActive = true;

                // add serial numbers to the ShipmentItems
                var serialNumber = 1;
                foreach (var shipmentItem in newPreShipment.PreShipmentItems)
                {
                    shipmentItem.SerialNumber = serialNumber;
                    shipmentItem.Nature = shipmentItem.Nature.ToUpper();

                    if (shipmentItem.SpecialPackageId == null)
                    {
                        shipmentItem.SpecialPackageId = 0;
                    }

                    //sum item weight
                    //check for volumetric weight
                    if (shipmentItem.IsVolumetric)
                    {
                        double volume = (shipmentItem.Length * shipmentItem.Height * shipmentItem.Width) / 5000;
                        double Weight = shipmentItem.Weight > volume ? shipmentItem.Weight : volume;
                        newPreShipment.ApproximateItemsWeight += Weight;
                    }
                    else
                    {
                        newPreShipment.ApproximateItemsWeight += shipmentItem.Weight;
                    }
                    newPreShipment.Value += shipmentItem.ItemValue;
                    serialNumber++;
                }
                _uow.PreShipment.Add(newPreShipment);
                await _uow.CompleteAsync();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Create Drop Off for Fast Track Agent
        private async Task<bool> CreateTemporaryShipmentForAgent(PreShipmentDTO preShipmentDTO)
        {
            try
            {
                if (preShipmentDTO == null)
                {
                    throw new GenericException("NULL INPUT");
                }

                //validate the input
                if (!preShipmentDTO.PreShipmentItems.Any())
                {
                    throw new GenericException("Shipment Items cannot be empty");
                }

                if (string.IsNullOrWhiteSpace(preShipmentDTO.SenderName))
                {
                    throw new GenericException("Sender Name can not be empty");
                }

                if (string.IsNullOrWhiteSpace(preShipmentDTO.SenderPhoneNumber))
                {
                    throw new GenericException("Sender Phone Number can not be empty");
                }

                // get the sender info
                var currentUserId = await _userService.GetCurrentUserId();
                preShipmentDTO.SenderUserId = currentUserId;

                //Get the role and name of the customer
                var user = await _userService.GetUserById(currentUserId);
                preShipmentDTO.CustomerCode = user.UserChannelCode;
                preShipmentDTO.CompanyType = UserChannelType.IndividualCustomer.ToString();
                preShipmentDTO.IsAgent = true;

                var country = await _uow.Country.GetCountryByStationId(preShipmentDTO.DepartureStationId);
                preShipmentDTO.CountryId = country.CountryId;

                var newPreShipment = Mapper.Map<PreShipment>(preShipmentDTO);
                newPreShipment.TempCode = await _numberGeneratorMonitorService.GenerateNextNumber(NumberGeneratorType.PreShipmentCode); ;
                newPreShipment.ApproximateItemsWeight = 0;
                newPreShipment.IsProcessed = false;

                // add serial numbers to the ShipmentItems
                var serialNumber = 1;
                foreach (var shipmentItem in newPreShipment.PreShipmentItems)
                {
                    shipmentItem.SerialNumber = serialNumber;
                    shipmentItem.Nature = shipmentItem.Nature.ToUpper();

                    if (shipmentItem.SpecialPackageId == null)
                    {
                        shipmentItem.SpecialPackageId = 0;
                    }

                    //sum item weight
                    //check for volumetric weight
                    if (shipmentItem.IsVolumetric)
                    {
                        double volume = (shipmentItem.Length * shipmentItem.Height * shipmentItem.Width) / 5000;
                        double Weight = shipmentItem.Weight > volume ? shipmentItem.Weight : volume;
                        newPreShipment.ApproximateItemsWeight += Weight;
                    }
                    else
                    {
                        newPreShipment.ApproximateItemsWeight += shipmentItem.Weight;
                    }

                    newPreShipment.Value += shipmentItem.ItemValue;
                    serialNumber++;
                }
                _uow.PreShipment.Add(newPreShipment);
                await _uow.CompleteAsync();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateServiceCentre(int serviceCentreId, ServiceCentreDTO service)
        {
            try
            {
                var centre = await _uow.ServiceCentre.GetAsync(serviceCentreId);
                if (centre == null || serviceCentreId != service.ServiceCentreId)
                {
                    throw new GenericException("Service Centre does not exist");
                }

                //1. update the old service centre code to the new one in Number Generator Monitor if they are different
                if (centre.Code.ToLower() != service.Code.ToLower())
                {
                    var numberGenerator = await _uow.NumberGeneratorMonitor.FindAsync(x => x.ServiceCentreCode == centre.Code);

                    foreach (var number in numberGenerator)
                    {
                        number.ServiceCentreCode = service.Code;
                    }
                }

                var station = await _uow.Station.GetAsync(service.StationId);

                //2. Update the service centre details
                centre.Name = service.Name;
                centre.PhoneNumber = service.PhoneNumber;
                centre.Address = service.Address;
                centre.City = service.City;
                centre.Email = service.Email;
                centre.StationId = station.StationId;
                centre.IsActive = true;
                centre.Code = service.Code;
                centre.TargetAmount = service.TargetAmount;
                centre.TargetOrder = service.TargetOrder;
                centre.IsHUB = service.IsHUB;
                _uow.Complete();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task<bool> UpdateTemporaryShipment(PreShipmentDTO preShipmentDTO)
        {
            try
            {
                if (preShipmentDTO == null)
                {
                    throw new GenericException("NULL INPUT");
                }

                //validate the input
                if (!preShipmentDTO.PreShipmentItems.Any())
                {
                    throw new GenericException("Items cannot be empty");
                }

                var existingPreShipment = await _uow.PreShipment.GetAsync(x => x.TempCode == preShipmentDTO.TempCode);
                if (existingPreShipment == null)
                {
                    throw new GenericException("Drop off Shipment does not exist", $"{(int)HttpStatusCode.NotFound}");
                }
                else
                {
                    if (existingPreShipment.IsProcessed)
                    {
                        throw new GenericException("Drop off Shipment already processed", $"{(int)HttpStatusCode.Forbidden}");
                    }
                }

                if (string.IsNullOrWhiteSpace(preShipmentDTO.SenderName))
                {
                    throw new GenericException("Sender Name can not be empty");
                }

                if (string.IsNullOrWhiteSpace(preShipmentDTO.SenderPhoneNumber))
                {
                    throw new GenericException("Sender Phone Number can not be empty");
                }

                // update receiver
                existingPreShipment.ReceiverAddress = preShipmentDTO.ReceiverAddress;
                existingPreShipment.ReceiverCity = preShipmentDTO.ReceiverCity;
                existingPreShipment.ReceiverName = preShipmentDTO.ReceiverName;
                existingPreShipment.ReceiverPhoneNumber = preShipmentDTO.ReceiverPhoneNumber;
                existingPreShipment.PickupOptions = preShipmentDTO.PickupOptions;
                existingPreShipment.SenderCity = preShipmentDTO.SenderCity;
                existingPreShipment.Value = preShipmentDTO.Value;
                existingPreShipment.DepartureStationId = preShipmentDTO.DepartureStationId;
                existingPreShipment.DestinationStationId = preShipmentDTO.DestinationStationId;
                existingPreShipment.SenderPhoneNumber = preShipmentDTO.SenderPhoneNumber;
                existingPreShipment.DestinationServiceCenterId = preShipmentDTO.DestinationServiceCenterId;

                if (existingPreShipment.IsAgent)
                {
                    existingPreShipment.SenderName = preShipmentDTO.SenderName;
                }

                //update items
                foreach (var preShipmentItemDTO in preShipmentDTO.PreShipmentItems)
                {
                    var preshipment = await _uow.PreShipmentItem.GetAsync(s => s.PreShipmentId == preShipmentItemDTO.PreShipmentId && s.PreShipmentItemId == preShipmentItemDTO.PreShipmentItemId);
                    if (preshipment != null)
                    {
                        if (preShipmentItemDTO.SpecialPackageId == null)
                        {
                            preShipmentItemDTO.SpecialPackageId = 0;
                        }

                        preshipment.Description = preShipmentItemDTO.Description;
                        preshipment.Nature = preShipmentItemDTO.Nature;
                        preshipment.Quantity = preShipmentItemDTO.Quantity;
                        preshipment.Weight = (double)preShipmentItemDTO.Weight;
                        preshipment.ShipmentType = preShipmentItemDTO.ShipmentType;
                        preshipment.SpecialPackageId = preShipmentItemDTO.SpecialPackageId;
                        preshipment.ItemValue = preShipmentItemDTO.ItemValue;
                        existingPreShipment.ApproximateItemsWeight += preshipment.Weight;
                        existingPreShipment.Value += preShipmentItemDTO.ItemValue;
                    }
                }
                await _uow.CompleteAsync();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdatePickupManifestStatus(ManifestStatusDTO manifestStatusDTO)
        {
            if (manifestStatusDTO != null)
            {
                await _dispatchService.UpdatePickupManifestStatus(manifestStatusDTO);
            }
        }

        public async Task<List<PickupManifestWaybillMappingDTO>> GetWaybillsInPickupManifest(string manifestCode)
        {
            var pickupDetails = await _manifestWaybillMappingService.GetWaybillsInPickupManifest(manifestCode);

            return pickupDetails;
        }

        public async Task<List<PreShipmentDTO>> GetDropOffsForUser(ShipmentCollectionFilterCriteria filterCriteria)
        {
            //get the current login user 
            var currentUserId = await _userService.GetCurrentUserId();

            var dropOffs = await _uow.PreShipment.GetDropOffsForUser(filterCriteria, currentUserId);

            return dropOffs;
        }

        public async Task<PreShipmentDTO> GetDropOffDetail(string tempCode)
        {
            try
            {
                var preShipment = await _uow.PreShipment.GetAsync(x => x.TempCode == tempCode && x.IsActive == true, "PreShipmentItems");
                if (preShipment != null)
                {
                    PreShipmentDTO dropOffDTO = Mapper.Map<PreShipmentDTO>(preShipment);
                    return dropOffDTO;
                }
                else
                {
                    throw new GenericException($"DropOff with code: {tempCode} does not exist", $"{(int)HttpStatusCode.NotFound}");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<LocationDTO>> GetPresentDayShipmentLocations()
        {
            return await _preShipmentMobileService.GetPresentDayShipmentLocations();
        }

        //Get Shipment Information for Danfo App
        public async Task<ShipmentDetailDanfoDTO> GetShipmentDetailForDanfo(string waybill)
        {
            if (string.IsNullOrEmpty(waybill))
            {
                throw new GenericException("Waybill can not be null");
            }

            var shipment = await _uow.Shipment.GetAsync(x => x.Waybill == waybill && x.ShipmentScanStatus != ShipmentScanStatus.SSC);

            if (shipment == null)
            {
                throw new GenericException($"Waybill {waybill} does not exist", $"{(int)HttpStatusCode.NotFound}");
            }

            //get CustomerDetails
            if (shipment.CustomerType.Contains("Individual"))
            {
                shipment.CustomerType = CustomerType.IndividualCustomer.ToString();
            }

            CustomerType customerType = (CustomerType)Enum.Parse(typeof(CustomerType), shipment.CustomerType);
            var customerDetails = await _customerService.GetCustomer(shipment.CustomerId, customerType);

            var shipmentDetail = new ShipmentDetailDanfoDTO
            {
                Waybill = shipment.Waybill,
                CustomerEmail = customerDetails.Email,
                CustomerNumber = customerDetails.PhoneNumber,
                DateCreated = shipment.DateCreated
            };

            return shipmentDetail;
        }

        private async Task<List<string>> GetShipmentStatus()
        {
            List<string> status = new List<string>
            {
                "Shipment created",
                "Assigned for Pickup",
                "Processing",
                "Cancelled",
                "Dispute",
                "Delivered",
                "Visited",
                "Resolved",
                "OnwardProcessing"
            };

            return await Task.FromResult(status);
        }

        public async Task<IEnumerable<ScanStatusDTO>> GetScanStatus()
        {
            return await _scanStatusService.GetNonHiddenScanStatus();
        }

        public async Task<bool> ScanMultipleShipment(List<ScanDTO> scanList)
        {
            return await _scanService.ScanMultipleShipment(scanList);
        }
        
        public async Task<List<MovementDispatchDTO>> GetManifestsInMovementManifestForMovementDispatch() 
        {
            return await _manifestWaybillMappingService.GetManifestsInMovementManifestForDispatch();
        }

        public async Task<List<ManifestWaybillMappingDTO>> GetWaybillsInManifestForDispatch()
        {
            return await _manifestWaybillMappingService.GetWaybillsInManifestForDispatch();
        }

        public async Task ReleaseShipmentForCollectionOnScanner(ShipmentCollectionDTO shipmentCollection)
        {
            await _collectionservice.ReleaseShipmentForCollectionOnScanner(shipmentCollection);
        }

        public async Task<List<LogVisitReasonDTO>> GetLogVisitReasons()
        {
            return await _logService.GetLogVisitReasons();
        }

        public async Task<object> AddManifestVisitMonitoring(ManifestVisitMonitoringDTO manifestVisitMonitoringDTO)
        {
            return await _visitService.AddManifestVisitMonitoring(manifestVisitMonitoringDTO);
        }

        public async Task<List<OutstandingPaymentsDTO>> GetOutstandingPayments()
        {

            var currentUserId = await _userService.GetCurrentUserId();
            var currentUser = await _userService.GetUserById(currentUserId);

            var outstandingShipments = await _uow.PreShipmentMobile.GetAllOutstandingShipmentsForUser(currentUser.UserChannelCode);

            return outstandingShipments;

        }

        public async Task<bool> PayForShipment(string waybill)
        {
            //if (waybill.Contains("AWR"))
            //{
            //    throw new GenericException($"Payment not allowed for the Shipment {waybill}", $"{(int)HttpStatusCode.Forbidden}");
            //}

            var currentUserId = await _userService.GetCurrentUserId();
            var currentUser = await _userService.GetUserById(currentUserId);

            //block third party payment process
            var shipment = await _uow.Shipment.GetAsync(x => x.Waybill == waybill);

            if (shipment != null)
            {
                if (shipment.CustomerCode != currentUser.UserChannelCode)
                {
                    throw new GenericException($"Third Party Payment not allowed for the Shipment {waybill}", $"{(int)HttpStatusCode.Forbidden}");
                }
            }

            var invoice = await _uow.Invoice.GetAsync(x => x.Waybill == waybill);

            if (invoice == null)
            {
                throw new GenericException("Waybill  Not Found", $"{(int)HttpStatusCode.NotFound}");
            }
            if (invoice.PaymentStatus == PaymentStatus.Paid)
            {
                throw new GenericException($"Payment already made for the Shipment {waybill}", $"{(int)HttpStatusCode.Forbidden}");
            }

            var wallet = await _walletService.GetWalletBalance();

            if (wallet.Balance < invoice.Amount)
            {
                throw new GenericException("Insufficient Balance In Wallet", $"{(int)HttpStatusCode.Forbidden}");
            }

            var transactionDTO = new PaymentTransactionDTO
            {
                Waybill = invoice.Waybill,
                TransactionCode = wallet.WalletNumber,
                PaymentType = PaymentType.Wallet,
                FromApp = true
            };
            var result = await _paymentTransactionService.ProcessNewPaymentTransaction(transactionDTO);
            return result;
        }

        public async Task<bool> DeleteDropOff(string tempCode)
        {
            try
            {
                var currentUserId = await _userService.GetCurrentUserId();

                var dropoff = await _uow.PreShipment.GetAsync(x => x.TempCode == tempCode && x.SenderUserId == currentUserId);

                if (dropoff == null)
                {
                    throw new GenericException("Drop off Shipment does not exist", $"{(int)HttpStatusCode.NotFound}");
                }
                dropoff.IsActive = false;

                await _uow.CompleteAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<WalletTransactionDTO>> GetWalletTransactionsForMobilePaginated(ShipmentAndPreShipmentParamDTO shipmentAndPreShipmentParamDTO)
        {
            return await _iWalletTransactionService.GetWalletTransactionsForMobilePaginated(shipmentAndPreShipmentParamDTO);
        }

        public async Task<List<PreShipmentMobileDTO>> GetPreShipmentsAndShipmentsPaginated(ShipmentAndPreShipmentParamDTO shipmentAndPreShipmentParamDTO)
        {
            return await _preShipmentMobileService.GetPreShipmentsAndShipmentsPaginated(shipmentAndPreShipmentParamDTO);
        }

        public async Task<IEnumerable<StationDTO>> GetStationsByCountry(int countryId)
        {
            return await _uow.Station.GetStationsByCountry(countryId);
        }

        public async Task<bool> ProfileInternationalUser(IntertnationalUserProfilerDTO intlUserProfiler)
        {
            var currentUserId = await _userService.GetCurrentUserId();
            var user = await _uow.User.GetUserById(currentUserId);

            if (user == null)
            {
                throw new GenericException("User does not exist!", $"{(int)HttpStatusCode.NotFound}");
            }
            user.IdentificationImage = intlUserProfiler.IdentificationImage;
            user.IdentificationNumber = intlUserProfiler.IdentificationNumber;
            user.IsInternational = true;
            user.IdentificationType = intlUserProfiler.IdentificationType;
           // await _uow.User.UpdateUser(currentUserId, user);
            //also update company table
            var company = await _uow.Company.GetAsync(x => x.CustomerCode == user.UserChannelCode);
            if (company != null)
            {
                company.IsInternational = true;
                company.IdentificationImageUrl = intlUserProfiler.IdentificationImage;
                company.IdentificationNumber = intlUserProfiler.IdentificationNumber;
                company.IdentificationType = intlUserProfiler.IdentificationType;
            }
            _uow.Complete();
            return true;
        }

        public async Task<List<ServiceCentreDTO>> GetServiceCentresByStation(int stationId)
        {
            return await _uow.ServiceCentre.GetServiceCentresByStationId(stationId);
        }

        public async Task<List<ServiceCentreDTO>> GetServiceCentresBySingleCountry(int countryId)
        {
            return await _uow.ServiceCentre.GetServiceCentresBySingleCountry(countryId);
        }

        public async Task<List<MobilePickUpRequestsDTO>> GetAllMobilePickUpRequestsPaginated(ShipmentAndPreShipmentParamDTO shipmentAndPreShipmentParamDTO)
        {
            return await _mobilePickUpRequestService.GetAllMobilePickUpRequestsPaginated(shipmentAndPreShipmentParamDTO);
        }

        public async Task<List<PreshipmentManifestDTO>> GetAllManifestForPreShipmentMobile()
        {
            return await _manifestWaybillMappingService.GetAllManifestForPreShipmentMobile();
        }

        public async Task<bool> UpdatePreshipmentMobileStatusToPickedup(string manifestNumber, List<string> waybills)
        {
          return  await _dispatchService.UpdatePreshipmentMobileStatusToPickedup(manifestNumber, waybills);
        }
        public async Task<bool> UpdatePreShipmentMobile(PreShipmentMobile preshipmentmobile)
        {
            try
            {
                await _uow.CompleteAsync();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<object> CreateNotification(NotificationDTO notificationDTO)
        {
            return await _notificationService.CreateNotification(notificationDTO);
        }

        public async Task<IEnumerable<NotificationDTO>> GetNotifications(bool? IsRead)
        {
            return await _notificationService.GetNotifications(IsRead);
        }

        public async Task UpdateNotificationAsRead(int notificationId)
        {
            await _notificationService.UpdateNotificationAsRead(notificationId);
        }

        //Get International Shipments Terms and Conditions
        public async Task<MessageDTO> GetIntlMessageForApp()
        {
            return await _messageSenderService.GetMessageByType(MessageType.ISTC);
        }

        public async Task<List<StoreDTO>> GetStoresByCountry(int countryId)
        {
            return await _uow.Store.GetStoresByCountryId(countryId);
        }

        public async Task<List<IntlShipmentRequestDTO>> GetIntlShipmentRequestsForUser(ShipmentCollectionFilterCriteria filterCriteria)
        {
            //get the current login user 
            var currentUserId = await _userService.GetCurrentUserId();
            int count = 0;
            var requests = await _uow.IntlShipmentRequest.GetIntlShipmentRequestsForUser(filterCriteria, currentUserId);
            if (requests.Any())
            {
                var consolidated = requests.Where(x => x.Consolidated).OrderBy(x => x.DateCreated).ToList();
                if (consolidated.Any())
                {
                    foreach (var item in consolidated)
                    {
                       count++;
                        item.ItemCount = $"{count} of {count}";
                    } 
                }
            }

            return requests;
        }



        public async Task<ResponseDTO> UnboardUser(NewCompanyDTO company)
        {
            return await _companyService.UnboardUser(company);
        }

        public async Task<ResponseDTO> ValidateUser(UserValidationNewDTO userDetail)
        {
            var result = new ResponseDTO();

            if (userDetail == null)
            {
                result.Succeeded = false;
                result.Message = $"Invalid payload";
                return result;
            }
            if (!String.IsNullOrEmpty(userDetail.BusinessName))
            {
                var company = await _uow.Company.GetAsync(x => x.Name.ToLower() == userDetail.BusinessName.ToLower());
                if (company != null)
                {
                    result.Exist = true;
                    result.Message = "User detail already exist";
                    result.Succeeded = false;
                    return result;
                }

                //also check aspnet table for business name
                var user = await _uow.User.GetUserByCompanyName(userDetail.BusinessName);
                if (user != null)
                {
                    result.Exist = true;
                    result.Message = "User detail already exist";
                    result.Succeeded = false;
                    return result;
                }
            }
            if (!String.IsNullOrEmpty(userDetail.Email))
            {
                var user = await _uow.User.GetUserByEmail(userDetail.Email);
                if (user != null)
                {
                    result.Exist = true;
                    result.Message = "User detail already exist";
                    result.Succeeded = false;
                    return result;
                }
            }

             if (!String.IsNullOrEmpty(userDetail.PhoneNumber))
            {
                var user = await _uow.User.GetUserByPhoneNumber(userDetail.PhoneNumber);
                if (user != null)
                {
                    result.Exist = true;
                    result.Message = "User detail already exist";
                    result.Succeeded = false;
                    return result;
                }
            }

            result.Exist = false;
            result.Message = "User detail does not exist";
            result.Succeeded = true;
            return result;
        }

        public async Task<ResponseDTO> UpdateUserRank(UserValidationDTO userValidationDTO)
        {
            return await _companyService.UpdateUserRank(userValidationDTO);
        }

        public async Task<bool> SendMessage(NewMessageDTO newMessageDTO)
        {
           // var msgType = (MessageType)Enum.Parse(typeof(MessageType), "ESEAS");
            return await _messageSenderService.SendMessage(MessageType.ESEAS, newMessageDTO.EmailSmsType,newMessageDTO);
        }

        public async Task<UserDTO> GetUserByEmail(string email)
        {
            var user = await _uow.User.GetUserByEmail(email);
            var userDTO = Mapper.Map<UserDTO>(user);
            return userDTO; 
        }

        public async Task<ResponseDTO> ChargeWallet(ChargeWalletDTO chargeWalletDTO)
        {
            return await _walletService.ChargeWallet(chargeWalletDTO);
        }

        public async Task<bool> ReleaseMovementManifest(ReleaseMovementManifestDto valMovementManifest)
        {
           return await  _shipmentService.ReleaseMovementManifest(valMovementManifest);
        }

        public async Task<IEnumerable<MovementManifestNumberDTO>> GetAllManifestMovementManifestNumberMappings(DateFilterCriteria dateFilterCriteria)
        {
            return await _movementManifestService.GetAllManifestMovementManifestNumberMappings(dateFilterCriteria);
        }

        public async Task<ResponseDTO> VerifyBVNNo(string bvnNo)
        {
            return await _paystackPaymentService.VerifyBVN(bvnNo);
        }
    }
}