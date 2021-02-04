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
using GIGLS.Core.IServices.TickectMan;
using GIGLS.Core.IServices.ServiceCentres;

namespace GIGLS.Services.Business.CustomerPortal
{
    public class TickectManService : ITickectManService
    {
        private readonly IUnitOfWork _uow;
        private readonly IInvoiceService _invoiceService;
        private readonly IShipmentTrackService _iShipmentTrackService;
        private readonly IUserService _userService;
        private readonly IDeliveryOptionPriceService _deliveryOptionPriceService;
        private readonly IDomesticRouteZoneMapService _domesticRouteZoneMapService;
        private readonly IShipmentService _shipmentService;
        private readonly IShipmentPackagePriceService _packagePriceService;
        private readonly ICustomerService _customerService;
        private readonly IPricingService _pricing;
        private readonly IPaymentService _paymentService;
        private readonly ICustomerPortalService _portalService;
        private readonly IShipmentCollectionService _shipmentCollectionService;
        private readonly IServiceCentreService _serviceCentreService;
        private readonly ICountryService _countryService;
        private readonly ILGAService _lgaService;
        private readonly ISpecialDomesticPackageService _specialPackageService;

        public TickectManService(IUnitOfWork uow,IDeliveryOptionPriceService deliveryOptionPriceService, IDomesticRouteZoneMapService domesticRouteZoneMapService, IShipmentService shipmentService,
           IShipmentPackagePriceService packagePriceService, ICustomerService customerService, IPricingService pricing,
           IPaymentService paymentService, ICustomerPortalService portalService, IShipmentCollectionService shipmentCollectionService, IServiceCentreService serviceCentreService, IUserService userService, ICountryService countryService, ILGAService lgaService,
           ISpecialDomesticPackageService specialPackageService, IInvoiceService invoiceService) 
        {
            _uow = uow;
            _deliveryOptionPriceService = deliveryOptionPriceService;
            _domesticRouteZoneMapService = domesticRouteZoneMapService;
            _shipmentService = shipmentService;
            _packagePriceService = packagePriceService;
            _customerService = customerService;
            _pricing = pricing;
            _paymentService = paymentService;
            _portalService = portalService;
            _shipmentCollectionService = shipmentCollectionService;
            _serviceCentreService = serviceCentreService;
            _userService = userService;
            _countryService = countryService;
            _lgaService = lgaService;
            _specialPackageService = specialPackageService;
            _invoiceService = invoiceService;

        }

        public Task<ShipmentDTO> AddShipment(ShipmentDTO shipment)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CountryDTO>> GetActiveCountries()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<SpecialDomesticPackageDTO>> GetActiveSpecialDomesticPackages()
        {
            throw new NotImplementedException();
        }

        public Task<object> GetCustomerBySearchParam(string customerType, SearchOption option)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<DeliveryOptionPriceDTO>> GetDeliveryOptionPrices()
        {
            return await _deliveryOptionPriceService.GetDeliveryOptionPrices();
        }

        public Task<ShipmentDTO> GetDropOffShipmentForProcessing(string code)
        {
            throw new NotImplementedException();
        }

        public Task<NewPricingDTO> GetGrandPriceForShipment(NewShipmentDTO newShipmentDTO)
        {
            throw new NotImplementedException();
        }

        public Task<InvoiceDTO> GetInvoiceByWaybill(string waybill)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<LGADTO>> GetLGAs()
        {
            throw new NotImplementedException();
        }

        public Task<decimal> GetPrice(PricingDTO pricingDto)
        {
            throw new NotImplementedException();
        }

        public Task<MobilePriceDTO> GetPriceForDropOff(PreShipmentMobileDTO preShipment)
        {
            throw new NotImplementedException();
        }

        public Task<DailySalesDTO> GetSalesForServiceCentre(AccountFilterCriteria accountFilterCriteria)
        {
            throw new NotImplementedException();
        }

        public Task<List<ServiceCentreDTO>> GetServiceCentresBySingleCountry(int countryId)
        {
            throw new NotImplementedException();
        }

        public Task<ShipmentDTO> GetShipment(string waybill)
        {
            throw new NotImplementedException();
        }

        public Task<ShipmentCollectionDTO> GetShipmentCollectionById(string waybill)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ShipmentPackagePriceDTO>> GetShipmentPackagePrices()
        {
            return await _packagePriceService.GetShipmentPackagePrices();
        }

        public Task<DailySalesDTO> GetWaybillForServiceCentre(string waybill)
        {
            throw new NotImplementedException();
        }

        public async Task<DomesticRouteZoneMapDTO> GetZone(int departure, int destination)
        {
            return await _domesticRouteZoneMapService.GetZone(departure, destination);
        }

        public Task<bool> ProcessPayment(PaymentTransactionDTO paymentDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ProcessPaymentPartial(PaymentPartialTransactionProcessDTO paymentPartialTransactionProcessDTO)
        {
            throw new NotImplementedException();
        }

        public Task ReleaseShipmentForCollection(ShipmentCollectionDTO shipmentCollection)
        {
            throw new NotImplementedException();
        }
    }
}