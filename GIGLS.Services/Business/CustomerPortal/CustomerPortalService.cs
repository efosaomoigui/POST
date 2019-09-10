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


        public CustomerPortalService(IUnitOfWork uow, IShipmentService shipmentService, IInvoiceService invoiceService,
            IShipmentTrackService iShipmentTrackService, IUserService userService, IWalletTransactionService iWalletTransactionService,
            ICashOnDeliveryAccountService iCashOnDeliveryAccountService, IPricingService pricingService, ICustomerService customerService,
            IPreShipmentService preShipmentService, IWalletService walletService, IWalletPaymentLogService wallepaymenttlogService,
            ISLAService slaService, IOTPService otpService, IBankShipmentSettlementService iBankShipmentSettlementService, INumberGeneratorMonitorService numberGeneratorMonitorService,
            IPasswordGenerator codegenerator, IGlobalPropertyService globalPropertyService, IPreShipmentMobileService preShipmentMobileService)
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

            //Update to change the Corporate Paid status from 'Paid' to 'Credit'
            foreach (var item in invoicesDto)
            {
                item.PaymentStatusDisplay = item.PaymentStatus.ToString();
                if ((CompanyType.Corporate.ToString() == item.CompanyType)
                    && (PaymentStatus.Paid == item.PaymentStatus))
                {
                    item.PaymentStatusDisplay = "Credit";
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
                var result = await _iShipmentTrackService.TrackShipment(waybillNumber);
                return result;
            }
            else
            {
                throw new GenericException("Error: You cannot track this waybill number.");
            }
        }

        public async Task<IEnumerable<ShipmentTrackingDTO>> PublicTrackShipment(string waybillNumber)
        {
            var result = await _iShipmentTrackService.TrackShipment(waybillNumber);
            return result;
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
            var countryIds = new int[]{ };
            try {
               countryIds = await _userService.GetPriviledgeCountryIds(); }
            catch
            {

            }
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
                    IsFromMobile = user.IsFromMobile
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

        public List<string> GetItemTypes()
        {
            List<string> items = new List<string>();
            items.Add("NORMAL");
            items.Add("DANGEROUS GOODS");
            items.Add("FRAGILE");
            items.Add("KEEP AT ROOM TEMPERATURE");
            items.Add("KEEP UPRIGHT");
            items.Add("REFRIGERATED ON ARRIVAL");
            items.Add("SENSITIVE");
            return items;
        }

        public async Task<SignResponseDTO> SignUp(UserDTO user)
        {
            var CountryId = await _preShipmentMobileService.GetCountryId();
            var userActiveCountryId = CountryId;    //Nigeria

            var bonus = await _globalPropertyService.GetGlobalProperty(GlobalPropertyType.ReferrerCodeBonus, userActiveCountryId);
            var result = new SignResponseDTO();
                if (user.UserChannelType == UserChannelType.Partner)
                {
                  var EmailUser = await _uow.User.GetUserByEmail(user.Email);
                  if (EmailUser != null)
                  {
                    var emailpartnerdetails = await _uow.Partner.GetAsync(s => s.Email == user.Email);
                    if (emailpartnerdetails != null)
                    {
                        throw new GenericException("Email already Exists as a Partner!");
                    }
                    var phonepartnerdetails = await _uow.Partner.GetAsync(s => s.PhoneNumber == user.PhoneNumber);
                    if (phonepartnerdetails != null)
                    {
                        throw new GenericException("Phone already Exists as a Partner!");
                    }
                    else 
                    {
                        var Vehicle = "";
                        foreach(var vehicle in user.VehicleType)
                        {
                            Vehicle = vehicle;
                        }
                        EmailUser.FirstName = user.FirstName;
                        EmailUser.LastName = user.LastName;
                        EmailUser.PhoneNumber = user.PhoneNumber;
                        EmailUser.Email = user.Email;
                        var partnerDTO = new PartnerDTO
                        {
                             PartnerType = PartnerType.Individual,
                             PartnerName = user.FirstName + "" + user.LastName,
                             PartnerCode = EmailUser.UserChannelCode,
                             FirstName  = user.FirstName,
                             LastName = user.LastName,
                             Email = user.Email,
                             PhoneNumber = user.PhoneNumber,
                             UserId = EmailUser.Id,
                             IsActivated = false,
                        };
                        var FinalPartner = Mapper.Map<Partner>(partnerDTO);
                        _uow.Partner.Add(FinalPartner);
                        var vehicletypeDTO = new VehicleTypeDTO
                        {
                            Partnercode = FinalPartner.PartnerCode,
                            Vehicletype = Vehicle
                        };
                        var vehicletype = Mapper.Map<VehicleType>(vehicletypeDTO);
                        _uow.VehicleType.Add(vehicletype);
                        await _uow.CompleteAsync();
                        EmailUser.UserChannelPassword = user.Password;
                        var u = await _userService.ResetPassword(EmailUser.Id, user.Password);
                        result = await SendOTPForRegisteredUser(user);
                        if(user.Referrercode == null)
                        {
                            //Generate referrercode for user that is signing up and didnt 
                            //supply a referrecode
                            var code = await _codegenerator.Generate(5);
                            var referrerCodeDTO = new ReferrerCodeDTO
                            {
                                Referrercode = code,
                                UserId = EmailUser.Id,
                                UserCode = EmailUser.UserChannelCode
                            };
                            var referrercode = Mapper.Map<ReferrerCode>(referrerCodeDTO);
                            _uow.ReferrerCode.Add(referrercode);
                        }
                        else
                        {
                            //Generate referrercode for user that is signing up and supplies a referrerCode
                            var code = await _codegenerator.Generate(5);
                            var referrerCodeDTO = new ReferrerCodeDTO
                            {
                                Referrercode = code,
                                UserId = EmailUser.Id,
                                UserCode = EmailUser.UserChannelCode

                            };
                            var referrercode = Mapper.Map<ReferrerCode>(referrerCodeDTO);
                            _uow.ReferrerCode.Add(referrercode);
                            await _uow.CompleteAsync();

                            //based on the referrercode supplied, use it to get the wallet and update the balance 
                            var referrerCode = await _uow.ReferrerCode.GetAsync(s => s.Referrercode == user.Referrercode);
                            if(referrerCode != null)
                            {
                                var wallet = await _uow.Wallet.GetAsync(s => s.CustomerCode == referrerCode.UserCode);
                                wallet.Balance = wallet.Balance + Convert.ToDecimal(bonus.Value);
                                var transaction = new WalletTransactionDTO
                                {
                                    WalletId = wallet.WalletId,
                                    CreditDebitType = CreditDebitType.Credit,
                                    Amount = Convert.ToDecimal(bonus.Value),
                                    ServiceCentreId = 296,
                                    Waybill = "",
                                    Description = "Referral Bonus",
                                    PaymentType = PaymentType.Online,
                                    UserId = referrerCode.UserId
                                };
                                var walletTransaction = await _iWalletTransactionService.AddWalletTransaction(transaction);
                                await _uow.CompleteAsync();

                            }
                        }
                        
                    }
                  
                  }
                  if(EmailUser == null)
                  {
                    var emailpartnerdetails = await _uow.Partner.GetAsync(s => s.Email == user.Email);
                    if (emailpartnerdetails != null)
                    {
                        throw new GenericException("Email already Exists on Partners!");
                    }
                    var phonepartnerdetails = await _uow.Partner.GetAsync(s => s.PhoneNumber == user.PhoneNumber);
                    if (phonepartnerdetails != null)
                    {
                        throw new GenericException("Phone already Exists on Partners!");
                    }
                    var PartnerCode = await _numberGeneratorMonitorService.GenerateNextNumber(
                    NumberGeneratorType.Partner);
                    var Partneruser = new UserDTO()
                    {
                        ConfirmPassword = user.Password,
                        Department = PartnerType.Individual.ToString(),
                        DateCreated = DateTime.Now,
                        Designation = PartnerType.Individual.ToString(),
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Organisation = CustomerType.IndividualCustomer.ToString(),
                        Password = user.Password,
                        PhoneNumber = user.PhoneNumber,
                        UserType = UserType.Regular,
                        Username = PartnerCode,
                        UserChannelCode = PartnerCode,
                        UserChannelPassword = user.Password,
                        UserChannelType = UserChannelType.Partner,
                        PasswordExpireDate = DateTime.Now,
                        UserActiveCountryId = user.UserActiveCountryId,
                      
                       
                    };
                    var FinalUser = Mapper.Map<User>(Partneruser);
                    FinalUser.Id = Guid.NewGuid().ToString();
                    FinalUser.DateCreated = DateTime.Now.Date;
                    FinalUser.DateModified = DateTime.Now.Date;
                    FinalUser.PasswordExpireDate = DateTime.Now;
                    FinalUser.UserName = (user.UserChannelType == UserChannelType.Partner) ? user.Email : user.UserChannelCode;
                    var u = await _uow.User.RegisterUser(FinalUser, user.Password);
                    var Vehicle = "";
                    foreach (var vehicle in user.VehicleType)
                    {
                        Vehicle = vehicle;
                    }
                    var partnerDTO = new PartnerDTO
                    {
                        PartnerType = PartnerType.Individual,
                        PartnerName = user.FirstName + " " + user.LastName,
                        PartnerCode = FinalUser.UserChannelCode,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,
                        UserId = FinalUser.Id,
                        IsActivated = false,
                    };
                    var FinalPartner = Mapper.Map<Partner>(partnerDTO);
                    _uow.Partner.Add(FinalPartner);
                    var vehicletypeDTO = new VehicleTypeDTO
                    {
                        Partnercode = FinalPartner.PartnerCode,
                        Vehicletype = Vehicle
                    };
                    var vehicletype = Mapper.Map<VehicleType>(vehicletypeDTO);
                    _uow.VehicleType.Add(vehicletype);
                    _uow.Complete();
                    await _walletService.AddWallet(new WalletDTO
                    {
                        CustomerId = FinalPartner.PartnerId,
                        CustomerType = CustomerType.IndividualCustomer,
                        CustomerCode = FinalPartner.PartnerCode,
                        CompanyType = CustomerType.IndividualCustomer.ToString()
                    });
                    result = await SendOTPForRegisteredUser(user);
                    if (user.Referrercode == null)
                    {
                        //Generate referrercode for user that is signing up and didnt 
                        //supply a referrecode
                        var code = await _codegenerator.Generate(5);
                        var referrerCodeDTO = new ReferrerCodeDTO
                        {
                            Referrercode = code,
                            UserId = FinalUser.Id,
                            UserCode = FinalUser.UserChannelCode

                        };
                        var referrercode = Mapper.Map<ReferrerCode>(referrerCodeDTO);
                        _uow.ReferrerCode.Add(referrercode);
                    }
                    else
                    {
                        //Generate referrercode for user that is signing up and supplies a referrerCode
                        var code = await _codegenerator.Generate(5);
                        var referrerCodeDTO = new ReferrerCodeDTO
                        {
                            Referrercode = code,
                            UserId = FinalUser.Id,
                            UserCode = FinalUser.UserChannelCode

                        };
                        var referrercode = Mapper.Map<ReferrerCode>(referrerCodeDTO);
                        _uow.ReferrerCode.Add(referrercode);
                        await _uow.CompleteAsync();

                        //based on the referrercode supplied, use it to get the wallet and update the balance 
                        var referrerCode = await _uow.ReferrerCode.GetAsync(s => s.Referrercode == user.Referrercode);
                        if (referrerCode != null)
                        {
                            var wallet = await _uow.Wallet.GetAsync(s => s.CustomerCode == referrerCode.UserCode);
                            wallet.Balance = wallet.Balance + Convert.ToDecimal(bonus.Value);
                            var transaction = new WalletTransactionDTO
                            {
                                WalletId = wallet.WalletId,
                                CreditDebitType = CreditDebitType.Credit,
                                Amount = Convert.ToDecimal(bonus.Value),
                                ServiceCentreId = 296,
                                Waybill = "",
                                Description = "Referral Bonus",
                                PaymentType = PaymentType.Online,
                                UserId = referrerCode.UserId
                            };
                            var walletTransaction = await _iWalletTransactionService.AddWalletTransaction(transaction);
                            await _uow.CompleteAsync();

                        }
                    }
                }
                  
                }
                else if (user.UserChannelType == UserChannelType.IndividualCustomer)
                {
                var EmailUser = await _uow.User.GetUserByEmail(user.Email);
                if (EmailUser != null)
                {
                    EmailUser.FirstName = user.FirstName;
                    EmailUser.LastName = user.LastName;
                    EmailUser.PhoneNumber = user.PhoneNumber;
                    EmailUser.Email = user.Email;
                    var emailcustomerdetails = await _uow.IndividualCustomer.GetAsync(s => s.Email == user.Email);
                    if (emailcustomerdetails != null)
                    {
                        
                        if (emailcustomerdetails.IsRegisteredFromMobile == true)
                        {
                            throw new GenericException("Email already Exists on Customers!");
                        }
                        if (emailcustomerdetails.IsRegisteredFromMobile!=true)
                        {
                            emailcustomerdetails.IsRegisteredFromMobile = true;
                            emailcustomerdetails.Email = user.Email;
                            emailcustomerdetails.Password = user.Password;
                            EmailUser.UserChannelPassword = user.Password;
                            emailcustomerdetails.PhoneNumber = user.PhoneNumber;
                            await _uow.CompleteAsync();
                            var u = await _userService.ResetPassword(EmailUser.Id, user.Password);
                            result = await SendOTPForRegisteredUser(user);
                            if (user.Referrercode == null)
                            {
                                //Generate referrercode for user that is signing up and didnt 
                                //supply a referrecode
                                var code = await _codegenerator.Generate(5);
                                var referrerCodeDTO = new ReferrerCodeDTO
                                {
                                    Referrercode = code,
                                    UserId = EmailUser.Id,
                                    UserCode = EmailUser.UserChannelCode

                                };
                                var referrercode = Mapper.Map<ReferrerCode>(referrerCodeDTO);
                                _uow.ReferrerCode.Add(referrercode);
                            }
                            else
                            {
                                //Generate referrercode for user that is signing up and supplies a referrerCode
                                var code = await _codegenerator.Generate(5);
                                var referrerCodeDTO = new ReferrerCodeDTO
                                {
                                    Referrercode = code,
                                    UserId = EmailUser.Id,
                                    UserCode = EmailUser.UserChannelCode

                                };
                                var referrercode = Mapper.Map<ReferrerCode>(referrerCodeDTO);
                                _uow.ReferrerCode.Add(referrercode);

                                //based on the referrercode supplied, use it to get the wallet and update the balance 
                                var referrerCode = await _uow.ReferrerCode.GetAsync(s => s.Referrercode == user.Referrercode);
                                if (referrerCode != null)
                                {
                                    var wallet = await _uow.Wallet.GetAsync(s => s.CustomerCode == referrerCode.UserCode);
                                    wallet.Balance = wallet.Balance + Convert.ToDecimal(bonus.Value);
                                    var transaction = new WalletTransactionDTO
                                    {
                                        WalletId = wallet.WalletId,
                                        CreditDebitType = CreditDebitType.Credit,
                                        Amount = Convert.ToDecimal(bonus.Value),
                                        ServiceCentreId = 296,
                                        Waybill = "",
                                        Description = "Referral Bonus",
                                        PaymentType = PaymentType.Online,
                                        UserId = referrerCode.UserId
                                    };
                                    var walletTransaction = await _iWalletTransactionService.AddWalletTransaction(transaction);
                                    await _uow.CompleteAsync();

                                }
                            }

                        }
                       
                    }
                    var phonecustomerdetails = await _uow.IndividualCustomer.GetAsync(s => s.PhoneNumber == user.PhoneNumber);
                    if (phonecustomerdetails != null)
                    {
                        if (phonecustomerdetails.IsRegisteredFromMobile == true)
                        {
                            throw new GenericException("Phone already Exists on Customers!");
                        }
                        if (phonecustomerdetails.IsRegisteredFromMobile !=true)
                        {
                            phonecustomerdetails.IsRegisteredFromMobile = true;
                            phonecustomerdetails.Email = user.Email;
                            phonecustomerdetails.Password = user.Password;
                            phonecustomerdetails.PhoneNumber = user.PhoneNumber;
                            EmailUser.UserChannelPassword = user.Password;
                            await _uow.CompleteAsync();
                            var u = await _userService.ResetPassword(EmailUser.Id, user.Password);
                            result = await SendOTPForRegisteredUser(user);
                            if (user.Referrercode == null)
                            {
                                //Generate referrercode for user that is signing up and didnt 
                                //supply a referrecode
                                var code = await _codegenerator.Generate(5);
                                var referrerCodeDTO = new ReferrerCodeDTO
                                {
                                    Referrercode = code,
                                    UserId = EmailUser.Id,
                                    UserCode = EmailUser.UserChannelCode

                                };
                                var referrercode = Mapper.Map<ReferrerCode>(referrerCodeDTO);
                                _uow.ReferrerCode.Add(referrercode);
                            }
                            else
                            {
                                //Generate referrercode for user that is signing up and supplies a referrerCode
                                var code = await _codegenerator.Generate(5);
                                var referrerCodeDTO = new ReferrerCodeDTO
                                {
                                    Referrercode = code,
                                    UserId = EmailUser.Id,
                                    UserCode = EmailUser.UserChannelCode

                                };
                                var referrercode = Mapper.Map<ReferrerCode>(referrerCodeDTO);
                                _uow.ReferrerCode.Add(referrercode);

                                //based on the referrercode supplied, use it to get the wallet and update the balance 
                                var referrerCode = await _uow.ReferrerCode.GetAsync(s => s.Referrercode == user.Referrercode);
                                if (referrerCode != null)
                                {
                                    var wallet = await _uow.Wallet.GetAsync(s => s.CustomerCode == referrerCode.UserCode);
                                    wallet.Balance = wallet.Balance + Convert.ToDecimal(bonus.Value);
                                    var transaction = new WalletTransactionDTO
                                    {
                                        WalletId = wallet.WalletId,
                                        CreditDebitType = CreditDebitType.Credit,
                                        Amount = Convert.ToDecimal(bonus.Value),
                                        ServiceCentreId = 296,
                                        Waybill = "",
                                        Description = "Referral Bonus",
                                        PaymentType = PaymentType.Online,
                                        UserId = referrerCode.UserId
                                    };
                                    var walletTransaction = await _iWalletTransactionService.AddWalletTransaction(transaction);
                                    await _uow.CompleteAsync();

                                }
                            }

                        }
                    }
                    else
                    {
                        var customer = new IndividualCustomerDTO
                        {
                            Email = user.Email,
                            PhoneNumber = user.PhoneNumber,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Password = user.Password,
                            CustomerCode = EmailUser.UserChannelCode,
                            PictureUrl = user.PictureUrl,
                            userId = EmailUser.Id,
                            IsRegisteredFromMobile = true
                            //added this to pass channelcode };
                        };
                        var individualCustomer = Mapper.Map<IndividualCustomer>(customer);
                        EmailUser.UserChannelPassword = user.Password;
                        var u = await _userService.ResetPassword(EmailUser.Id, user.Password);
                         _uow.IndividualCustomer.Add(individualCustomer);
                        await _uow.CompleteAsync();
                        result = await SendOTPForRegisteredUser(user);
                        if (user.Referrercode == null)
                        {
                            //Generate referrercode for user that is signing up and didnt 
                            //supply a referrecode
                            var code = await _codegenerator.Generate(5);
                            var referrerCodeDTO = new ReferrerCodeDTO
                            {
                                Referrercode = code,
                                UserId = EmailUser.Id,
                                UserCode = EmailUser.UserChannelCode

                            };
                            var referrercode = Mapper.Map<ReferrerCode>(referrerCodeDTO);
                            _uow.ReferrerCode.Add(referrercode);
                            await _uow.CompleteAsync();
                        }
                        else
                        {
                            //Generate referrercode for user that is signing up and supplies a referrerCode
                            var code = await _codegenerator.Generate(5);
                            var referrerCodeDTO = new ReferrerCodeDTO
                            {
                                Referrercode = code,
                                UserId = EmailUser.Id,
                                UserCode = EmailUser.UserChannelCode

                            };
                            var referrercode = Mapper.Map<ReferrerCode>(referrerCodeDTO);
                            _uow.ReferrerCode.Add(referrercode);
                            await _uow.CompleteAsync();

                            //based on the referrercode supplied, use it to get the wallet and update the balance 
                            var referrerCode = await _uow.ReferrerCode.GetAsync(s => s.Referrercode == user.Referrercode);
                            if (referrerCode != null)
                            {
                                var wallet = await _uow.Wallet.GetAsync(s => s.CustomerCode == referrerCode.UserCode);
                                wallet.Balance = wallet.Balance + Convert.ToDecimal(bonus.Value);
                                var transaction = new WalletTransactionDTO
                                {
                                    WalletId = wallet.WalletId,
                                    CreditDebitType = CreditDebitType.Credit,
                                    Amount = Convert.ToDecimal(bonus.Value),
                                    ServiceCentreId = 296,
                                    Waybill = "",
                                    Description = "Referral Bonus",
                                    PaymentType = PaymentType.Online,
                                    UserId = referrerCode.UserId
                                };
                                var walletTransaction = await _iWalletTransactionService.AddWalletTransaction(transaction);
                                await _uow.CompleteAsync();

                            }
                        }
                    }
                }
                else
                {
                    user.IsFromMobile = true;
                    var registeredUser = await Register(user);
                    result = await SendOTPForRegisteredUser(registeredUser);
                    if (user.Referrercode == null)
                    {
                        //Generate referrercode for user that is signing up and didnt 
                        //supply a referrecode
                        var code = await _codegenerator.Generate(5);
                        var referrerCodeDTO = new ReferrerCodeDTO
                        {
                            Referrercode = code,
                            UserId = registeredUser.Id,
                            UserCode = registeredUser.UserChannelCode

                        };
                        var referrercode = Mapper.Map<ReferrerCode>(referrerCodeDTO);
                        _uow.ReferrerCode.Add(referrercode);
                        await _uow.CompleteAsync();
                    }
                    else
                    {
                        //Generate referrercode for user that is signing up and supplies a referrerCode
                        var code = await _codegenerator.Generate(5);
                        var referrerCodeDTO = new ReferrerCodeDTO
                        {
                            Referrercode = code,
                            UserId = registeredUser.Id,
                            UserCode = registeredUser.UserChannelCode

                        };
                        var referrercode = Mapper.Map<ReferrerCode>(referrerCodeDTO);
                        _uow.ReferrerCode.Add(referrercode);
                        await _uow.CompleteAsync();

                        //based on the referrercode supplied, use it to get the wallet and update the balance 
                        var referrerCode = await _uow.ReferrerCode.GetAsync(s => s.Referrercode == user.Referrercode);
                        if (referrerCode != null)
                        {
                            var wallet = await _uow.Wallet.GetAsync(s => s.CustomerCode == referrerCode.UserCode);
                            wallet.Balance = wallet.Balance + Convert.ToDecimal(bonus.Value);
                            var transaction = new WalletTransactionDTO
                            {
                                WalletId = wallet.WalletId,
                                CreditDebitType = CreditDebitType.Credit,
                                Amount = Convert.ToDecimal(bonus.Value),
                                ServiceCentreId = 296,
                                Waybill = "",
                                Description = "Referral Bonus",
                                PaymentType = PaymentType.Online,
                                UserId = referrerCode.UserId
                            };
                            var walletTransaction = await _iWalletTransactionService.AddWalletTransaction(transaction);
                            await _uow.CompleteAsync();

                        }
                    }
                }
                }
            return result;
        }

        private async Task<SignResponseDTO> SendOTPForRegisteredUser(UserDTO user)
        {
            var responseDto = new SignResponseDTO();

            var Otp = await _otpService.GenerateOTP(user);
            var message = await _otpService.SendOTP(Otp);

            string[] CombinedMessage = message.Split(',');

            var EmailResponse = CombinedMessage[0];
            var PhoneResponse = CombinedMessage[1];

            if (EmailResponse == "Accepted")
            {
                responseDto.EmailSent = true;
            }
            if (PhoneResponse == "OK")
            {
                responseDto.PhoneSent = true;
            }

            return responseDto;
        }

        public async Task<SignResponseDTO> ResendOTP(UserDTO user)
        {
            var registeredUser = await _otpService.CheckDetails(user.Email);
            if (registeredUser == null)
            {
                throw new GenericException("User has not registered!");
            }

            var result = await SendOTPForRegisteredUser(registeredUser);
            return result;
        }

        public async Task<List<StationDTO>> GetLocalStations()
        {
            var CountryId = await _preShipmentMobileService.GetCountryId();
            var countryIds = new int [1];   //NIGERIA
            countryIds[0] = CountryId;
            return await _uow.Station.GetLocalStations(countryIds);
        }

    }
}
