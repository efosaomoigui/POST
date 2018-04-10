using GIGLS.Core;
using GIGLS.Core.IRepositories;
using GIGLS.Core.IRepositories.Customers;
using GIGLS.Core.IRepositories.JobCards;
using GIGLS.Core.IRepositories.ServiceCentres;
using GIGLS.Core.IRepositories.Shipments;
using GIGLS.Core.IRepositories.Stocks;
using GIGLS.Core.IRepositories.Stores;
using GIGLS.Core.IRepositories.User;
using GIGLS.Core.IRepositories.Fleets;
using GIGLS.Core.IRepositories.Vendors;
using GIGLS.Core.IRepositories.Workshops;
using GIGLS.Core.IRepositories.Zone;
using Ninject;
using System.Threading.Tasks;
using GIGLS.Core.IRepositories.Partnership;
using GIGLS.Core.IRepositories.Wallet;
using GIGLS.Core.IRepositories.PaymentTransactions;
using GIGLS.Core.IRepositories.Account;
using GIGLS.Core.IRepositories.Client;
using GIGLS.Core.IRepositories.Utility;
using GIGLS.CORE.IRepositories.Shipments;
using GIGLS.CORE.IRepositories.Nav;
using GIGLS.Core.IRepositories.Haulage;
using GIGLS.Core.IRepositories.MessagingLog;
using GIGLS.Core.IRepositories.ShipmentScan;
using System;

namespace GIGLS.Infrastructure.Persistence
{
    public class UnitOfWork<TContext> : IUnitOfWork where TContext : GIGLSContext
    {
        private readonly TContext _context;
        public UnitOfWork(TContext context)
        {
            _context = context;
        }

        [Inject]
        public ICompanyRepository Company { get; set; }

        [Inject]
        public ICompanyContactPersonRepository CompanyContactPerson { get; set; }

        [Inject]
        public IIndividualCustomerRepository IndividualCustomer { get; set; }

        [Inject]
        public IJobCardManagementPartRepository JobCardManagementPart { get; set; }

        [Inject]
        public IJobCardManagementRepository JobCardManagement { get; set; }

        [Inject]
        public IJobCardRepository JobCard { get; set; }

        [Inject]
        public IServiceCentreRepository ServiceCentre { get; set; }

        [Inject]
        public IUserServiceCentreMappingRepository UserServiceCentreMapping { get; set; }

        [Inject]
        public IManifestRepository Manifest { get; set; }

        [Inject]
        public IManifestGroupWaybillNumberMappingRepository ManifestGroupWaybillNumberMapping { get; set; }

        [Inject]
        public IShipmentItemRepository ShipmentPackage { get; set; }

        [Inject]
        public IShipmentRepository Shipment { get; set; }

        [Inject]
        public IShipmentTrackingRepository ShipmentTracking { get; set; }

        [Inject]
        public IStockRequestPartRepository StockRequestPart { get; set; }

        [Inject]
        public IStockRequestRepository StockRequest { get; set; }

        [Inject]
        public IStockSupplyDetailsRepository StockSupplyDetails { get; set; }

        [Inject]
        public IStoreRepository Store { get; set; }

        [Inject]
        public IFleetRepository Fleet { get; set; }

        [Inject]
        public IFleetMakeRepository FleetMake { get; set; }

        [Inject]
        public IFleetModelRepository FleetModel { get; set; }

        [Inject]
        public IFleetPartInventoryHistoryRepository FleetPartInventoryHistory { get; set; }

        [Inject]
        public IFleetPartInventoryRepository FleetPartInventory { get; set; }

        [Inject]
        public IFleetPartRepository FleetPart { get; set; }

        [Inject]
        public IFleetTripRepository FleetTrip { get; set; }

        [Inject]
        public IVendorRepository Vendor { get; set; }

        [Inject]
        public IWorkshopRepository Workshop { get; set; }

        [Inject]
        public IDomesticRouteZoneMapRepository DomesticRouteZoneMap { get; set; }

        [Inject]
        public IDomesticZonePriceRepository DomesticZone { get; set; }

        [Inject]
        public ISpecialDomesticZonePriceRepository SpecialDomesticZone { get; set; }

        [Inject]
        public IDeliveryOptionRepository DeliveryOption { get; set; }

        [Inject]
        public IPartnerApplicationRepository PartnerApplication { get; set; }

        [Inject]
        public IPartnerRepository Partner { get; set; }

        [Inject]
        public IWalletNumberRepository WalletNumber { get; set; }

        [Inject]
        public IWalletRepository Wallet { get; set; }

        [Inject]
        public IWalletTransactionRepository WalletTransaction { get; set; }

        [Inject]
        public IStateRepository State { get; set; }

        [Inject]
        public IStationRepository Station { get; set; }

        [Inject]
        public IZoneRepository Zone { get; set; }

        [Inject]
        public IDomesticZonePriceRepository DomesticZonePrice { get; set; }

        [Inject]
        public ISpecialDomesticPackageRepository SpecialDomesticPackage { get; set; }

        [Inject]
        public ISpecialDomesticZonePriceRepository SpecialDomesticZonePrice { get; set; }

        [Inject]
        public IDeliveryOptionPriceRepository DeliveryOptionPrice { get; set; }

        [Inject]
        public IWaybillNumberRepository WaybillNumber { get; set; }

        [Inject]
        public ITransitWaybillNumberRepository TransitWaybillNumber { get; set; }

        [Inject]
        public IGroupWaybillNumberRepository GroupWaybillNumber { get; set; }

        [Inject]
        public IGroupWaybillNumberMappingRepository GroupWaybillNumberMapping { get; set; }

        [Inject]
        public IWeightLimitRepository WeightLimit { get; set; }

        [Inject]
        public IWeightLimitPriceRepository WeightLimitPrice { get; set; }

        [Inject]
        public IPaymentTransactionRepository PaymentTransaction { get; set; }

        [Inject]
        public IPaymentPartialTransactionRepository PaymentPartialTransaction { get; set; }

        [Inject]
        public IUserRepository User { get; set; }

        [Inject]
        public IGeneralLedgerRepository GeneralLedger { get; set; }

        [Inject]
        public IClientNodeRepository ClientNode { get; set; }

        [Inject]
        public IInvoiceRepository Invoice { get; set; }

        [Inject]
        public IVATRepository VAT { get; set; }

        [Inject]
        public IInsuranceRepository Insurance { get; set; }

        [Inject]
        public IInvoiceShipmentRepository InvoiceShipment { get; set; }

        [Inject]
        public INumberGeneratorMonitorRepository NumberGeneratorMonitor { get; set; }

        [Inject]
        public IShipmentReturnRepository ShipmentReturn { get; set; }

        [Inject]
        public IShipmentCollectionRepository ShipmentCollection { get; set; }

        [Inject]
        public IMainNavRepository MainNav { get; set; }

        [Inject]
        public ISubNavRepository SubNav { get; set; }

        [Inject]
        public ISubSubNavRepository SubSubNav { get; set; }

        [Inject]
        public IMessageRepository Message { get; set; }

        [Inject]
        public IHaulageRepository Haulage { get; set; }

        [Inject]
        public IHaulageDistanceMappingRepository HaulageDistanceMapping { get; set; }

        [Inject]
        public IHaulageDistanceMappingPriceRepository HaulageDistanceMappingPrice { get; set; }

        [Inject]
        public IPackingListRepository PackingList { get; set; }

        [Inject]
        public ICashOnDeliveryAccountRepository CashOnDeliveryAccount { get; set; }

        [Inject]
        public ICashOnDeliveryBalanceRepository CashOnDeliveryBalance { get; set; }

        [Inject]
        public IDispatchRepository Dispatch { get; set; }

        [Inject]
        public IDispatchActivityRepository DispatchActivity { get; set; }

        [Inject]
        public IEmailSendLogRepository EmailSendLog { get; set; }

        [Inject]
        public ISmsSendLogRepository SmsSendLog { get; set; }

        [Inject]
        public IScanStatusRepository ScanStatus { get; set; }

        [Inject]
        public IGlobalPropertyRepository GlobalProperty { get; set; }

        [Inject]
        public IMissingShipmentRepository MissingShipment { get; set; }

        [Inject]
        public IShipmentCancelRepository ShipmentCancel { get; set; }

        [Inject]
        public ICountryRepository Country { get; set; }

        [Inject]
        public ICountryRouteZoneMapRepository CountryRouteZoneMap { get; set; }

        [Inject]
        public IShipmentRerouteRepository ShipmentReroute { get; set; }

        [Inject]
        public IShipmentPackagePriceRepository ShipmentPackagePrice { get; set; }

        [Inject]
        public IManifestVisitMonitoringRepository ManifestVisitMonitoring { get; set; }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }

}