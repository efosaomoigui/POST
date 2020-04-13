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

namespace GIGLS.Services.Business.CustomerPortal
{
    public class CustomerPortalService : ICustomerPortalService
    {
        private readonly IUnitOfWork _uow;
        private readonly IShipmentService _shipmentService;
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
        public readonly IIndividualCustomerService _individualCustomerService;
        public readonly IPartnerTransactionsService _partnertransactionservice;
        private readonly IMobileGroupCodeWaybillMappingService _groupCodeWaybillMappingService;


        public CustomerPortalService(IUnitOfWork uow, IShipmentService shipmentService, IInvoiceService invoiceService,
            IShipmentTrackService iShipmentTrackService, IUserService userService, IWalletTransactionService iWalletTransactionService,
            ICashOnDeliveryAccountService iCashOnDeliveryAccountService, IPricingService pricingService, ICustomerService customerService,
            IPreShipmentService preShipmentService, IWalletService walletService, IWalletPaymentLogService wallepaymenttlogService,
            ISLAService slaService, IOTPService otpService, IBankShipmentSettlementService iBankShipmentSettlementService, INumberGeneratorMonitorService numberGeneratorMonitorService,
            IPasswordGenerator codegenerator, IGlobalPropertyService globalPropertyService, IPreShipmentMobileService preShipmentMobileService, IMessageSenderService messageSenderService, 
            ICountryService countryService, IAdminReportService adminReportService, IIndividualCustomerService individualCustomerService, 
            IPartnerTransactionsService partnertransactionservice, IMobileGroupCodeWaybillMappingService groupCodeWaybillMappingService)
        {
            _shipmentService = shipmentService;
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
            _individualCustomerService = individualCustomerService;
            _partnertransactionservice = partnertransactionservice;
            _groupCodeWaybillMappingService = groupCodeWaybillMappingService;
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
        //my own


        public async Task UpdateWallet(int walletId, WalletTransactionDTO walletTransactionDTO)
        {
            await _walletService.UpdateWallet(walletId, walletTransactionDTO, false);
        }

        public async Task<object> AddWalletPaymentLog(WalletPaymentLogDTO walletPaymentLogDto)
        {
            var walletPaymentLog = await _wallepaymenttlogService.AddWalletPaymentLog(walletPaymentLogDto);
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

        public async Task<WalletTransactionSummaryDTO> GetWalletTransactions()
        {
            //get the current login user 
            var currentUserId = await _userService.GetCurrentUserId();
            var currentUser = await _userService.GetUserById(currentUserId);
            var wallet = await _uow.Wallet.GetAsync(s => s.CustomerCode == currentUser.UserChannelCode);

            if (wallet == null)
            {
                throw new GenericException("Wallet does not exist");
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
            //var customersCods = codsValues.Where(s=>s.CustomerCode == userchannelcode).ToList(); 

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

            if (invoicesDto.Count() > 0)
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

                if (result.Count() > 0)
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
                throw new GenericException("Error: You cannot track this waybill number.");
            }
        }

        public async Task<List<ShipmentTrackingDTO>> PublicTrackShipment(string waybillNumber)
        {
            var finalResult = new List<ShipmentTrackingDTO>();

            var result = await _iShipmentTrackService.TrackShipment(waybillNumber);

            if (result.Count() > 0)
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
            var countryIds = await _userService.GetPriviledgeCountryIds();
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
                throw new GenericException("The Mapping of Route to Zone does not exist");

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

        public async Task<List<PreShipmentDTO>> GetPreShipments(FilterOptionsDto filterOptionsDto)
        {
            try
            {
                //get the current login user 
                var currentUserId = await _userService.GetCurrentUserId();

                var preShipmentsQuery = _uow.PreShipment.PreShipmentsAsQueryable();
                preShipmentsQuery = preShipmentsQuery.Where(s => s.UserId == currentUserId);
                var preShipments = preShipmentsQuery.ToList();
                var preShipmentsDTO = Mapper.Map<List<PreShipmentDTO>>(preShipments);
                return preShipmentsDTO;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PreShipmentDTO> GetPreShipment(string waybill)
        {
            try
            {
                var preShipmentDTO = await _preShipmentService.GetPreShipment(waybill);
                return preShipmentDTO;
            }
            catch (Exception)
            {
                throw;
            }

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
                    throw new GenericException("Customer could not be created");
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
            if (userSla.Count() > 0)
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
                throw new GenericException("Wallet does not exist");
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
            var result = new SignResponseDTO();

            if (user.RequiresCod == null)
            {
                user.RequiresCod = false;
            }
            
            if (user.IsUniqueInstalled == null)
            {
                user.IsUniqueInstalled = false;
            }
            
            if (user.IsEligible == null)
            {
                user.IsEligible = false;
            }
            
            if (user.Referrercode != null)
            {
                user.RegistrationReferrercode = user.Referrercode;
            }
            
            if (user.UserChannelType != UserChannelType.Ecommerce && user.UserChannelType != UserChannelType.IndividualCustomer && user.UserChannelType != UserChannelType.Partner)
            {
                throw new GenericException($"Kindly supply valid customer channel ");
            }
            
            if (user.UserChannelType == UserChannelType.Ecommerce)
            {
                var ecommerceEmail = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.EcommerceEmail, 1);
                throw new GenericException($"{ecommerceEmail.Value}");
            }
            
            if (user.Email != null)
            {
                user.Email = user.Email.Trim().ToLower();
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
                throw new GenericException("Customer already exists!!!");
            }
            
            return result;
        }

        public async Task<UserDTO> GetCustomerCountryUsingPhoneCode(UserDTO userDTO)
        {
            if (userDTO.CountryPhoneNumberCode == null)
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
            
            foreach(var u in emailUsers)
            {
                if(u.UserChannelType == UserChannelType.Ecommerce || u.UserChannelType == UserChannelType.Partner)
                {
                    return false;
                }
                if(u.UserChannelType == UserChannelType.Employee && u.Email == user.Email)
                {
                    return false;
                }
                else
                {
                    if(u.UserChannelType == UserChannelType.IndividualCustomer && u.IsRegisteredFromMobile == true)
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
                    throw new GenericException("Employee email not allowed.");
                }
                else if (EmailUser.IsRegisteredFromMobile == true)
                {
                    throw new GenericException("Partner already exists!");
                }
                else
                {
                    var phonepartnerdetails = await _uow.Partner.GetAsync(s => s.PhoneNumber.Contains(PhoneNumber) || s.Email == user.Email);
                    if (phonepartnerdetails != null)
                    {
                        throw new GenericException("Customer already Exists as a Partner!");
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
                throw new GenericException("Customer details already Exists as a Partner!");
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
                    UserActiveCountryId = user.UserActiveCountryId
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
                    throw new GenericException("Employee email not allowed.");
                }
                else if (EmailUser.UserChannelType == UserChannelType.Ecommerce && EmailUser.IsRegisteredFromMobile != true)
                {
                    throw new GenericException("Account Already Exists. Kindly Login!!!");
                }
                else if (EmailUser.IsRegisteredFromMobile == true)
                {
                    throw new GenericException("Customer already exists!");
                }
                else
                {
                    var emailcustomerdetails = await _uow.IndividualCustomer.GetAsync(s => s.Email == user.Email || s.PhoneNumber.Contains(PhoneNumber));
                    if (emailcustomerdetails != null)
                    {
                        if (emailcustomerdetails.IsRegisteredFromMobile == true)
                        {
                            throw new GenericException("Customer aready exists!");
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

            return result;
        }

        private async Task<SignResponseDTO> EcommerceRegistration(UserDTO user)
        {
            var result = new SignResponseDTO();

            if (user.Organisation == null)
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
                    throw new GenericException("Employee email not allowed.");
                }
                else if (EmailUser.UserChannelType == UserChannelType.Ecommerce && EmailUser.IsRegisteredFromMobile != true)
                {
                    throw new GenericException("Account Already Exists. Kindly Login!!!");
                }
                else if (EmailUser.IsRegisteredFromMobile == true)
                {
                    throw new GenericException("Customer already exists!");
                }
                else
                {
                    var emailcompanydetails = await _uow.Company.GetAsync(s => s.Email == user.Email || s.PhoneNumber.Contains(PhoneNumber));                                       

                    if (emailcompanydetails != null)
                    {
                        if (emailcompanydetails.IsRegisteredFromMobile == true)
                        {
                            throw new GenericException("Email already Exists as Company Customer!");
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
                                isCodNeeded = (bool)user.RequiresCod
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
            bool isEmail = Regex.IsMatch(user.Email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
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

        public async Task<MobilePriceDTO> GetHaulagePrice(HaulagePriceDTO haulagePricingDto)
        {
            return await _preShipmentMobileService.GetHaulagePrice(haulagePricingDto);
        }

        public Task<IEnumerable<NewCountryDTO>> GetUpdatedCountries()
        {
            return _countryService.GetUpdatedCountries();
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
                    if (user.StartsWith("0"))
                    {
                        user = user.Remove(0, 1);
                    }
                    emailPhone = user;
                }
            }

            return await _otpService.CheckDetails(emailPhone, userchanneltype);
        }
        public async Task<UserDTO> CheckDetailsForCustomerPortal(string user)
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

            return await _otpService.CheckDetailsForCustomerPortal(emailPhone);
        }
        public async Task<UserDTO> CheckDetailsForMobileScanner(string user)
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

            return await _otpService.CheckDetailsForMobileScanner(emailPhone);
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
            var isDisable =  ConfigurationManager.AppSettings["DisableShipmentCreation"];
            bool disableShipmentCreation = bool.Parse(isDisable);

            if (disableShipmentCreation) {
                string message = "Pick up service is currently not available due to movement restriction. " +
                    "This service will be fully restored tomorrow. Thank you for your patience and understanding.";

                throw new GenericException(message);
            }
            return await _preShipmentMobileService.AddPreShipmentMobile(preShipment);
        }

        public async Task<MultipleShipmentOutput> AddMultiplePreShipmentMobile(NewPreShipmentMobileDTO preShipment)
        {
            return await _preShipmentMobileService.CreateMobileShipment(preShipment);
        }
        public async Task<List<PreShipmentMobileDTO>> GetPreShipmentForUser()
        {
            return await _preShipmentMobileService.GetPreShipmentForUser();
        }
        public async Task<WalletTransactionSummaryDTO> GetWalletTransactionsForMobile()
        {
            return await _iWalletTransactionService.GetWalletTransactionsForMobile();
        }
        public async Task<MobilePriceDTO> GetPrice(PreShipmentMobileDTO preShipment)
        {
            return await _preShipmentMobileService.GetPrice(preShipment);
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
       
        private async Task<UserDTO> CreateUserBasedOnCustomerType (UserDTO user)
        {
            try
            {
                var User = new UserDTO();

                if (user.Organisation == null)
                {
                    user.Organisation = user.FirstName + " " + user.LastName;
                }

                if (user.UserChannelType == UserChannelType.IndividualCustomer)
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
                      
                        //added this to pass channelcode 
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

                    //return user;
                }

                if (user.UserChannelType == UserChannelType.Ecommerce)
                {
                    var customerCode = await _numberGeneratorMonitorService.GenerateNextNumber(
                    NumberGeneratorType.CustomerCodeEcommerce);
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
                        isCodNeeded = (bool) user.RequiresCod
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
            catch(Exception)
            {
                throw;
            }
        }        

        private async Task<UserDTO> CreateNewuser (UserDTO user)
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

                return user;
            }
            catch(Exception)
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
                //var query = _uow.DeliveryNumber.GetAll();
                deliverynumberDto = await GenerateDeliveryNumber(count);
                //query = query.Where(s => s.IsUsed != true);
                //var deliverynumbers = query.ToList();
                //deliverynumberDto = Mapper.Map<List<DeliveryNumberDTO>>(deliverynumbers);
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
                int maxSize = 6;
                char[] chars = new char[62];
                string a;
                a = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
                chars = a.ToCharArray();
                int size = maxSize;
                byte[] data = new byte[1];
                RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
                crypto.GetNonZeroBytes(data);
                size = maxSize;
                data = new byte[size];
                crypto.GetNonZeroBytes(data);
                StringBuilder result = new StringBuilder(size);
                foreach (byte b in data)
                { result.Append(chars[b % (chars.Length - 1)]); }
                var strippedText = result.ToString();
                var number = new DeliveryNumber
                {
                    Number = "DN" + strippedText.ToUpper(),
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
                    throw new GenericException("This is not a GIGGo Shipment.It can not be updated");
                }
                else
                {
                    if((preShipmentMobile.shipmentstatus == MobilePickUpRequestStatus.OnwardProcessing.ToString() || preShipmentMobile.shipmentstatus == MobilePickUpRequestStatus.PickedUp.ToString())
                        && mobilePickUpRequestsDTO.Status == "Shipment created")
                    {
                        throw new GenericException($"You can not change this shipment status to {mobilePickUpRequestsDTO.Status}");
                    }
                    else if (preShipmentMobile.ZoneMapping == 1 && mobilePickUpRequestsDTO.Status == MobilePickUpRequestStatus.OnwardProcessing.ToString())
                    {
                        throw new GenericException("This is not an Inter-State Shipment");
                    }
                    else
                    {
                        preShipmentMobile.shipmentstatus = mobilePickUpRequestsDTO.Status;
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
    }
}