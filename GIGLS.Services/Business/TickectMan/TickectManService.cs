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
using Newtonsoft.Json.Linq;

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

        public async Task<ShipmentDTO> AddShipment(NewShipmentDTO newShipmentDTO)
        {
            //map to real shipmentdto
            var ShipmentDTO = JObject.FromObject(newShipmentDTO).ToObject<ShipmentDTO>();
            //Update SenderAddress for corporate customers
            ShipmentDTO.SenderAddress = null;
            ShipmentDTO.SenderState = null;
            if (ShipmentDTO.Customer[0].CompanyType == CompanyType.Corporate)
            {
                ShipmentDTO.SenderAddress = ShipmentDTO.Customer[0].Address;
                ShipmentDTO.SenderState = ShipmentDTO.Customer[0].State;
            }

            //set some data to null
            ShipmentDTO.ShipmentCollection = null;
            ShipmentDTO.Demurrage = null;
            ShipmentDTO.Invoice = null;
            ShipmentDTO.ShipmentCancel = null;
            ShipmentDTO.ShipmentReroute = null;
            ShipmentDTO.DeliveryOption = null;
            ShipmentDTO.IsFromMobile = false;
            var shipment = await _shipmentService.AddShipment(ShipmentDTO);
            if (!String.IsNullOrEmpty(shipment.Waybill))
            {
                var invoiceObj = await _invoiceService.GetInvoiceByWaybill(shipment.Waybill);
                if (invoiceObj != null)
                {
                    invoiceObj.Shipment = null;
                    shipment.Invoice = invoiceObj;
                }
            }
            return shipment;
        }

       
        public async Task<IEnumerable<CountryDTO>> GetActiveCountries()
        {
            return await _countryService.GetActiveCountries();
        }

        public async Task<IEnumerable<SpecialDomesticPackageDTO>> GetActiveSpecialDomesticPackages()
        {
            return await _specialPackageService.GetActiveSpecialDomesticPackages();
        }

        public async Task<object> GetCustomerBySearchParam(string customerType, SearchOption option)
        {
            return await _customerService.GetCustomerBySearchParam(customerType, option);
        }

        public async Task<IEnumerable<DeliveryOptionPriceDTO>> GetDeliveryOptionPrices()
        {
            return await _deliveryOptionPriceService.GetDeliveryOptionPrices();
        }

        public async Task<ShipmentDTO> GetDropOffShipmentForProcessing(string code)
        {
            return await _shipmentService.GetDropOffShipmentForProcessing(code);
        }

        public async Task<NewPricingDTO> GetGrandPriceForShipment(NewShipmentDTO newShipmentDTO)
        {
           return await _pricing.GetGrandPriceForShipment(newShipmentDTO);
        }


        public async Task<IEnumerable<LGADTO>> GetLGAs()
        {
            return await _lgaService.GetLGAs();
        }

        public async Task<decimal> GetPrice(PricingDTO pricingDto)
        {
            return await _pricing.GetPrice(pricingDto);
        }

        public async Task<MobilePriceDTO> GetPriceForDropOff(PreShipmentMobileDTO preshipmentMobile)
        {
            return await _portalService.GetPriceForDropOff(preshipmentMobile);
        }

        public async Task<DailySalesDTO> GetSalesForServiceCentre(DateFilterForDropOff dateFilterCriteria)
        {
            var accountFilterCriteria = JObject.FromObject(dateFilterCriteria).ToObject<AccountFilterCriteria>();
            var dailySales = await _shipmentService.GetSalesForServiceCentre(accountFilterCriteria);
            if (dailySales.TotalSales > 0)
            {
                var factor = Convert.ToDecimal(Math.Pow(10, -2));
                dailySales.TotalSales = Math.Round(dailySales.TotalSales * factor) / factor;
            }
            return dailySales;
        }

        public async Task<List<ServiceCentreDTO>> GetServiceCentresBySingleCountry(int countryId)
        {
            //2. priviledged users service centres
            var usersServiceCentresId = await _userService.GetPriviledgeServiceCenters();
            return await _portalService.GetServiceCentresBySingleCountry(countryId);
        }

        public async Task<ShipmentDTO> GetShipment(string waybill)
        {
            var shipment = await _shipmentService.GetShipment(waybill);
            if (shipment.GrandTotal > 0)
            {
                var factor = Convert.ToDecimal(Math.Pow(10, -2));
                shipment.GrandTotal = Math.Round(shipment.GrandTotal * factor) / factor;
                shipment.Vat = Math.Round((decimal)shipment.Vat * factor) / factor;
                shipment.vatvalue_display = Math.Round((decimal)shipment.vatvalue_display * factor) / factor;
                shipment.Total = Math.Round((decimal)shipment.Total * factor) / factor;
                shipment.DiscountValue = Math.Round((decimal)shipment.DiscountValue * factor) / factor;

                foreach (var item in shipment.ShipmentItems)
                {
                    item.Price = Math.Round(item.Price * factor) / factor;
                }
            }
            return shipment;
        }

        public async Task<ShipmentCollectionDTO> GetShipmentCollectionById(string waybill)
        {
            return await _shipmentCollectionService.GetShipmentCollectionById(waybill);
        }

        public async Task<List<ShipmentPackagePriceDTO>> GetShipmentPackagePrices()
        {
            return await _packagePriceService.GetShipmentPackagePrices();
        }

        public async Task<DailySalesDTO> GetWaybillForServiceCentre(string waybill)
        {
            return await _shipmentService.GetWaybillForServiceCentre(waybill);
        }

        public async Task<DomesticRouteZoneMapDTO> GetZone(int departure, int destination)
        {
            return await _domesticRouteZoneMapService.GetZone(departure, destination);
        }

        public async Task<bool> ProcessPayment(PaymentTransactionDTO paymentDto)
        {
          return  await _paymentService.ProcessPayment(paymentDto);
        }

        public async Task<bool> ProcessPaymentPartial(PaymentPartialTransactionProcessDTO paymentPartialTransactionProcessDTO)
        {
            return await _paymentService.ProcessPaymentPartial(paymentPartialTransactionProcessDTO);
        }

        public async Task ReleaseShipmentForCollection(ShipmentCollectionDTOForFastTrack shipmentCollectionforDto)
        {
            var shipmentCollection = JObject.FromObject(shipmentCollectionforDto).ToObject<ShipmentCollectionDTO>();
            shipmentCollection.ShipmentScanStatus = Core.Enums.ShipmentScanStatus.OKT;
            if (shipmentCollection.IsComingFromDispatch)
            {
                shipmentCollection.ShipmentScanStatus = Core.Enums.ShipmentScanStatus.OKC;
            }
            await _shipmentCollectionService.ReleaseShipmentForCollection(shipmentCollection);
        }
    }
}