using GIGLS.Core.IRepositories;
using GIGLS.Core.IRepositories.Customers;
using GIGLS.Core.IRepositories.JobCards;
using GIGLS.Core.IRepositories.Partnership;
using GIGLS.Core.IRepositories.PaymentTransactions;
using GIGLS.Core.IRepositories.ServiceCentres;
using GIGLS.Core.IRepositories.Shipments;
using GIGLS.Core.IRepositories.Stocks;
using GIGLS.Core.IRepositories.Stores;
using GIGLS.Core.IRepositories.User;
using GIGLS.Core.IRepositories.Fleets;
using GIGLS.Core.IRepositories.Vendors;
using GIGLS.Core.IRepositories.Wallet;
using GIGLS.Core.IRepositories.Workshops;
using GIGLS.Core.IRepositories.Zone;
using System;
using GIGLS.Core.IRepositories.Account;
using GIGLS.Core.IRepositories.Client;
using GIGLS.Core.IRepositories.Utility;
using GIGLS.CORE.IRepositories.Shipments;
using GIGLS.CORE.IRepositories.Nav;

namespace GIGLS.Core
{
    public interface IUnitOfWork : IDisposable
    {
        ICompanyContactPersonRepository CompanyContactPerson { get; set; }
        ICompanyRepository Company { get; set; }
        IIndividualCustomerRepository IndividualCustomer { get; set; }
        IJobCardManagementPartRepository JobCardManagementPart { get; set; }
        IJobCardManagementRepository JobCardManagement { get; set; }
        IJobCardRepository JobCard { get; set; }
        IServiceCentreRepository ServiceCentre { get; set; }
        IUserServiceCentreMappingRepository UserServiceCentreMapping { get; set; }
        IManifestRepository Manifest { get; set; }
        IManifestMonitorRepository ManifestMonitor { get; set; }
        IManifestGroupWaybillNumberMappingRepository ManifestGroupWaybillNumberMapping { get; set; }
        IShipmentItemRepository ShipmentPackage { get; set; }
        IShipmentRepository Shipment { get; set; }
        IShipmentTrackingRepository ShipmentTracking { get; set; }
        IStockRequestPartRepository StockRequestPart { get; set; }
        IStockRequestRepository StockRequest { get; set; }
        IStockSupplyDetailsRepository StockSupplyDetails { get; set; }
        IStoreRepository Store { get; set; }
        IUserRepository User { get; set; }
        IFleetRepository Fleet { get; set; }
        IFleetMakeRepository FleetMake { get; set; }
        IFleetModelRepository FleetModel { get; set; }
        IFleetPartInventoryHistoryRepository FleetPartInventoryHistory { get; set; }
        IFleetPartInventoryRepository FleetPartInventory { get; set; }
        IFleetPartRepository FleetPart { get; set; }
        IFleetTripRepository FleetTrip { get; set; }
        IVendorRepository Vendor { get; set; }
        IWorkshopRepository Workshop { get; set; }
        IDomesticRouteZoneMapRepository DomesticRouteZoneMap { get; set; }
        IDomesticZonePriceRepository DomesticZonePrice { get; set; }
        ISpecialDomesticPackageRepository SpecialDomesticPackage { get; set; }
        ISpecialDomesticZonePriceRepository SpecialDomesticZonePrice { get; set; }
        IDeliveryOptionRepository DeliveryOption { get; set; }
        IDeliveryOptionPriceRepository DeliveryOptionPrice { get; set; }
        IZoneRepository Zone { get; set; }
        IPartnerApplicationRepository PartnerApplication { get; set; }
        IPartnerRepository Partner { get; set; }
        IWalletNumberRepository WalletNumber { get; set; }
        IWalletRepository Wallet { get; set; }
        IWalletTransactionRepository WalletTransaction { get; set; }
        IStateRepository State { get; set; }
        IStationRepository Station { get; set; }
        IWaybillNumberRepository WaybillNumber { get; set; }
        IWaybillNumberMonitorRepository WaybillNumberMonitor { get; set; }
        IGroupWaybillNumberRepository GroupWaybillNumber { get; set; }
        IGroupWaybillNumberMonitorRepository GroupWaybillNumberMonitor { get; set; }
        IGroupWaybillNumberMappingRepository GroupWaybillNumberMapping { get; set; }
        IWeightLimitPriceRepository WeightLimitPrice { get; set; }
        IWeightLimitRepository WeightLimit { get; set; }
        IPaymentTransactionRepository PaymentTransaction { get; set; }
        IGeneralLedgerRepository GeneralLedger { get; set; }
        IClientNodeRepository ClientNode { get; set; }
        IInvoiceRepository Invoice { get; set; }
        IVATRepository VAT { get; set; }
        IInsuranceRepository Insurance { get; set; }
        IInvoiceShipmentRepository InvoiceShipment { get; set; }
        INumberGeneratorMonitorRepository NumberGeneratorMonitor { get; set; }
        IShipmentReturnRepository ShipmentReturn { get; set; }
        IShipmentCollectionRepository ShipmentCollection { get; set; }
        IMainNavRepository MainNav { get; set; }
        ISubNavRepository SubNav { get; set; }
        ISubSubNavRepository SubSubNav { get; set; }
        IEmailSmsRepository EmailSms { get; set; }

        int Complete();
        System.Threading.Tasks.Task<int> CompleteAsync();
    }
}