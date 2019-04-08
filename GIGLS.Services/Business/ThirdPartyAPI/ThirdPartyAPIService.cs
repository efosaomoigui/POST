using GIGLS.Core.IServices.CustomerPortal;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Account;
using GIGLS.Core.DTO.Shipments;
using GIGLS.Core.DTO.Wallet;
using GIGLS.CORE.DTO.Report;
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
using GIGLS.Core.IServices.ThirdPartyAPI;
using System;
using GIGLS.Core.Enums;
using GIGLS.Core.IServices.Utility;

namespace GIGLS.Services.Business.CustomerPortal
{
    public class ThirdPartyAPIService : IThirdPartyAPIService
    {
        private readonly IUnitOfWork _uow;
        private readonly IPreShipmentService _preShipmentService;
        private readonly IInvoiceService _invoiceService;
        private readonly IShipmentTrackService _iShipmentTrackService;
        private readonly IUserService _userService;
        private readonly IWalletTransactionService _iWalletTransactionService;
        private readonly ICashOnDeliveryAccountService _iCashOnDeliveryAccountService;
        private readonly IPricingService _pricing;
        private readonly ICustomerService _customerService;
        private readonly IPreShipmentMobileService _preshipmentMobileService;
        private readonly IWalletService _walletService;
        private readonly INumberGeneratorMonitorService _numberGeneratorMonitorService;
        

        public ThirdPartyAPIService(IUnitOfWork uow, IPreShipmentService preShipmentService, IInvoiceService invoiceService,
            IShipmentTrackService iShipmentTrackService, IUserService userService, IWalletTransactionService iWalletTransactionService, 
            ICashOnDeliveryAccountService iCashOnDeliveryAccountService, IPricingService pricingService,
            ICustomerService customerService, IPreShipmentMobileService preshipmentMobileService, IWalletService walletService, INumberGeneratorMonitorService numberGeneratorMonitorService)
        {
            _preShipmentService = preShipmentService;
            _invoiceService = invoiceService;
            _iShipmentTrackService = iShipmentTrackService;
            _userService = userService;
            _iWalletTransactionService = iWalletTransactionService;
            _iCashOnDeliveryAccountService = iCashOnDeliveryAccountService;
            _pricing = pricingService;
            _customerService = customerService;
            _uow = uow;
            _preshipmentMobileService = preshipmentMobileService;
            _walletService = walletService;
            _numberGeneratorMonitorService = numberGeneratorMonitorService;
            MapperConfig.Initialize();
        }

        //Price API
        public async Task<decimal> GetPrice2(ThirdPartyPricingDTO thirdPartyPricingDto)
        {
            //service centres from station
            var departureServiceCentre = _uow.ServiceCentre.GetAll().Where(s => s.StationId == thirdPartyPricingDto.DepartureStationId).ToList().FirstOrDefault();
            var destinationServiceCentre = _uow.ServiceCentre.GetAll().Where(s => s.StationId == thirdPartyPricingDto.DestinationStationId).ToList().FirstOrDefault();

            //delivery options - Ecommerce
            var deliveryOption = _uow.DeliveryOption.GetAllAsQueryable().
                Where(s => s.Code == "ECC").FirstOrDefault();

            var deliveryOptionIds = new int[] { deliveryOption.DeliveryOptionId };

            var newPricingDTO = new PricingDTO()
            {
                DepartureServiceCentreId = departureServiceCentre.ServiceCentreId,
                DestinationServiceCentreId = destinationServiceCentre.ServiceCentreId,
                DeliveryOptionId = deliveryOption.DeliveryOptionId,
                DeliveryOptionIds = deliveryOptionIds.ToList(),
                Weight = thirdPartyPricingDto.Weight,
                IsVolumetric = thirdPartyPricingDto.IsVolumetric,
                Length = thirdPartyPricingDto.Length,
                Width = thirdPartyPricingDto.Width,
                Height = thirdPartyPricingDto.Height,
                ShipmentType = Core.Enums.ShipmentType.Ecommerce
            };

            return await _pricing.GetPrice(newPricingDTO);
        }


        public async Task<PreShipmentMobileDTO> GetPrice(PreShipmentMobileDTO preShipment)
        {
            var PreShipment = await _preshipmentMobileService.GetPrice(preShipment);
            return PreShipment;

        }

        public async Task<decimal> GetHaulagePrice(HaulagePricingDTO haulagePricingDto)
        {
            return await _pricing.GetHaulagePrice(haulagePricingDto);
        }

        //Capture PreShipment API
        public async Task<PreShipmentDTO> AddPreShipment(ThirdPartyPreShipmentDTO thirdPartyPreShipmentDTO)
        {
            try
            {


                //1. convert thirdPartyPreShipmentDTO to PreShipmentDTO
                var preShipmentDTO = Mapper.Map<PreShipmentDTO>(thirdPartyPreShipmentDTO);

                decimal totalPrice = 0.0M;
                //2. convert the shipment items
                var preShipmentItemDTOList = new List<PreShipmentItemDTO>();
                foreach (var thirdPartyPreShipmentItem in thirdPartyPreShipmentDTO.PreShipmentItems)
                {
                    var preShipmentItemDTO = Mapper.Map<PreShipmentItemDTO>(thirdPartyPreShipmentItem);
                    preShipmentItemDTOList.Add(preShipmentItemDTO);
                    totalPrice += preShipmentItemDTO.Price;
                }
                preShipmentDTO.PreShipmentItems = preShipmentItemDTOList;


                //3. set other default values
                //3.1 price
                preShipmentDTO.Total = totalPrice;
                preShipmentDTO.GrandTotal = totalPrice;

                //3.2 customer info
                var currentUserId = await _userService.GetCurrentUserId();
                var currentUser = await _userService.GetUserById(currentUserId);
                preShipmentDTO.CustomerCode = currentUser.UserChannelCode;

                var newPreShipment = await _preShipmentService.AddPreShipment(preShipmentDTO);
                return newPreShipment;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Route API


        //Track API
        public async Task<IEnumerable<ShipmentTrackingDTO>> TrackShipment(string waybillNumber)
        {
            //1. Verify the waybill is attached to the login user
            var currentUserId = await _userService.GetCurrentUserId();
            var currentUser = await _userService.GetUserById(currentUserId);

            var invoices =
                _uow.Invoice.GetAllFromInvoiceView().Where(s =>
                s.CustomerCode == currentUser.UserChannelCode && s.Waybill == waybillNumber).ToList();

            if (invoices.Count > 0)
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

        //Invoice API
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

        public async Task<IEnumerable<InvoiceViewDTO>> GetInvoices()
        {
            //get the current login user 
            var currentUserId = await _userService.GetCurrentUserId();
            var currentUser = await _userService.GetUserById(currentUserId);

            var invoices = _uow.Invoice.GetAllFromInvoiceView().Where(s => s.CustomerCode == currentUser.UserChannelCode).ToList();
            invoices = invoices.OrderByDescending(s => s.DateCreated).ToList();

            var invoicesDto = Mapper.Map<List<InvoiceViewDTO>>(invoices);
            return invoicesDto;
        }

        //Transaction History API
        public async Task<List<InvoiceViewDTO>> GetShipmentTransactions(ShipmentFilterCriteria f_Criteria)
        {
            //get the current login user 
            var currentUserId = await _userService.GetCurrentUserId();
            var currentUser = await _userService.GetUserById(currentUserId);

            var invoices = _uow.Invoice.GetAllFromInvoiceView().Where(s => s.CustomerCode == currentUser.UserChannelCode).ToList();
            invoices = invoices.OrderByDescending(s => s.DateCreated).ToList();

            var invoicesDto = Mapper.Map<List<InvoiceViewDTO>>(invoices);
            return await Task.FromResult(invoicesDto);
        }

        public async Task<WalletTransactionSummaryDTO> GetWalletTransactions()
        {
            //get the current login user 
            var currentUserId = await _userService.GetCurrentUserId();
            var currentUser = await _userService.GetUserById(currentUserId);
            var wallet = await _uow.Wallet.GetAsync(s => s.CustomerCode == currentUser.UserChannelCode);

            var walletTransactionSummary = await _iWalletTransactionService.GetWalletTransactionByWalletId(wallet.WalletId);
            return walletTransactionSummary;
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


        //General API
        public async Task<DashboardDTO> GetDashboard()
        {
            var dashboardDTO = new DashboardDTO();

            //get the current login user 
            var currentUserId = await _userService.GetCurrentUserId();
            var currentUser = await _userService.GetUserById(currentUserId);
            var wallet = await _uow.Wallet.GetAsync(s => s.CustomerCode == currentUser.UserChannelCode);

            if (wallet != null)
            {
                var invoices = _uow.Invoice.GetAllFromInvoiceView().Where(s => s.CustomerCode == currentUser.UserChannelCode).ToList();
                var invoicesDto = Mapper.Map<List<InvoiceViewDTO>>(invoices);

                // 
                dashboardDTO.TotalShipmentOrdered = invoices.Count();
                dashboardDTO.WalletBalance = wallet.Balance;
            }

            return await Task.FromResult(dashboardDTO);
        }


        //For Quick Quotes
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
            return await _uow.ServiceCentre.GetLocalServiceCentres();
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

        public async Task<IEnumerable<StationDTO>> GetLocalStations()
        {
            var localStations = await _uow.Station.GetLocalStations();
            return localStations.OrderBy(x => x.StationName).ToList();
        }

        public async Task<IEnumerable<StationDTO>> GetInternationalStations()
        {
            var internationalStations = await _uow.Station.GetInternationalStations();
            return internationalStations.OrderBy(x => x.StationName).ToList();
        }

        public async Task<PreShipmentMobileDTO> CreatePreShipment(PreShipmentMobileDTO preShipmentDTO)
        {
            // get the current user info
            var PreshipmentPriceDTO = await GetPrice(preShipmentDTO);
            var preshipmentDto = await _preshipmentMobileService.AddPreShipmentMobile(PreshipmentPriceDTO);
            return preshipmentDto;
        }
        public async Task<List<PreShipmentMobileDTO>> GetPreShipmentById(string userid)
        {
            // get the current user info
            var preshipmentDto = await _preshipmentMobileService.GetPreShipmentForUser(userid);
            return preshipmentDto;
        }


    }
}
