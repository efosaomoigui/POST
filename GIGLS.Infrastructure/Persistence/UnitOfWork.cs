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
using GIGLS.Core.IRepositories.Devices;
using GIGLS.Core.IRepositories.BankSettlement;
using GIGLS.Core.IRepositories.InternationalRequest;
using GIGLS.Core.IRepositories.Sla;
using GIGLS.Core.IRepositories.Expenses;
using GIGLS.Core.IRepositories.Magaya;
using GIGLS.Core.IRepositories.Routes;

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
        public IIntlShipmentRequestRepository IntlShipmentRequest { get; set; }

        [Inject]
        public IShipmentRepository Shipment { get; set; }

        [Inject]
        public IIntlShipmentRequestItemRepository IntlShipmentRequestItem { get; set; }  

        [Inject]
        public IMagayaShipmentRepository MagayaShipment { get; set; } 

        [Inject]
        public IMagayaShipmentItemRepository MagayaShipmentItem { get; set; } 

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
        public ICashOnDeliveryRegisterAccountRepository CashOnDeliveryRegisterAccount { get; set; }

        [Inject]
        public IDemurrageRegisterAccountRepository DemurrageRegisterAccount { get; set; }  
        

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

        [Inject]
        public IManifestWaybillMappingRepository ManifestWaybillMapping { get; set; }

        [Inject]
        public IDeviceRepository Device { get; set; }

        [Inject]
        public IDeviceManagementRepository DeviceManagement { get; set; }

        [Inject]
        public IShipmentDeliveryOptionMappingRepository ShipmentDeliveryOptionMapping { get; set; }

        [Inject]
        public ICODSettlementSheetRepository CODSettlementSheet { get; set; }

        [Inject]
        public IPreShipmentItemRepository PreShipmentItem { get; set; }

        [Inject]
        public IPreShipmentRepository PreShipment { get; set; }

        [Inject]
        public IPreShipmentManifestMappingRepository PreShipmentManifestMapping { get; set; }

        [Inject]
        public INotificationRepository Notification { get; set; }

        [Inject]
        public ILogVisitReasonRepository LogVisitReason { get; set; }

        [Inject]
        public IWalletPaymentLogRepository WalletPaymentLog { get; set; }

        [Inject]
        public IOverdueShipmentRepository OverdueShipment { get; set; }

        [Inject]
        public IInternationalRequestReceiverRepository InternationalRequestReceiver { get; set; }

        [Inject]
        public ISLARepository SLA { get; set; }
        
        [Inject]
        public ISLASignedUserRepository SLASignedUser { get; set; }

        [Inject]
        public IExpenseTypeRepository ExpenseType { get; set; }

        [Inject]
        public IExpenditureRepository Expenditure { get; set; }

        //Bank Processing Order
        [Inject]
        public IBankProcessingOrderForShipmentAndCODRepository BankProcessingOrderForShipmentAndCOD { get; set; }

        [Inject]
        public IBankProcessingOrderCodesRepository BankProcessingOrderCodes { get; set; }

        [Inject]
        public ICodPayOutListRepository CodPayOutList { get; set; }

        //IBankProcessingOrderForShipmentAndCODRepository CodPayOutList

        [Inject]
        public IOTPRepository OTP { get; set; }

        [Inject]
        public IPreShipmentMobileRepository PreShipmentMobile { get; set; }

        [Inject]
        public IPreShipmentItemMobileRepository PreShipmentItemMobile { get; set; }


        [Inject]
        public IUserLoginEmailRepository UserLoginEmail { get; set; }

        [Inject]
        public IMobileShipmentTrackingRepository MobileShipmentTracking
        {get; set;}

        [Inject]
        public IMobilePickUpRequestsRepository MobilePickUpRequests
        { get; set; }

        [Inject]
        public IRegionRepository Region
        { get; set; }

        [Inject]
        public IRegionServiceCentreMappingRepository RegionServiceCentreMapping
        { get; set; }

        [Inject]
        public IDemurrageRepository Demurrage
        { get; set; }

        [Inject]
        public IHUBManifestWaybillMappingRepository HUBManifestWaybillMapping
        { get; set; }

        [Inject]
        public ICategoryRepository Category { get; set; }

        [Inject]
        public ISubCategoryRepository SubCategory { get; set; }


        [Inject]
        public IMobileScanStatusRepository MobileScanStatus { get; set; }

        [Inject]
        public IPartnerTransactionsRepository PartnerTransactions { get; set; }

        [Inject]
        public IMobileRatingRepository MobileRating { get; set; }

        [Inject]
        public IReferrerCodeRepository ReferrerCode { get; set; }

        [Inject]
        public IDeliveryNumberRepository DeliveryNumber { get; set; }

        [Inject]
        public IVehicleTypeRepository VehicleType { get; set; }

        [Inject]
        public IPickupManifestRepository PickupManifest { get; set; }

        [Inject]
        public IPickupManifestWaybillMappingRepository PickupManifestWaybillMapping { get; set; }

        [Inject]
        public IRiderDeliveryRepository RiderDelivery { get; set; }

        [Inject]
        public IDeliveryLocationRepository DeliveryLocation { get; set; }

        [Inject]
        public ILGARepository LGA { get; set; }

        [Inject]
        public IWaybillPaymentLogRepository WaybillPaymentLog { get; set; }

        [Inject]
        public IBankRepository Bank { get; set; }

        [Inject]
        public IActivationCampaignEmailRepository ActivationCampaignEmail { get; set; }

        [Inject]
        public IGiglgoStationRepository GiglgoStation { get; set; }

        [Inject]
        public IShipmentHashRepository ShipmentHash { get; set; }

        [Inject]
        public IFleetPartnerRepository FleetPartner { get; set; }

        [Inject]
        public IMobileGroupCodeWaybillMappingRepository MobileGroupCodeWaybillMapping { get; set; }

        [Inject]
        public IPartnerPayoutRepository PartnerPayout { get; set; }

        [Inject]
        public IEcommerceAgreementRepository EcommerceAgreement { get; set; }

        [Inject]
        public IRouteRepository Routes { get; set; }

        [Inject]
        public IShipmentPackagingTransactionsRepository ShipmentPackagingTransactions { get; set; }

        [Inject]
        public IServiceCenterPackageRepository ServiceCenterPackage { get; set; }

        [Inject]
        public IHomeDeliveryLocationRepository HomeDeliveryLocation { get; set; }

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