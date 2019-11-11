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
using GIGLS.Core.IRepositories.Haulage;
using GIGLS.Core.IRepositories.MessagingLog;
using GIGLS.Core.IRepositories.ShipmentScan;
using GIGLS.Core.IRepositories.Devices;
using GIGLS.Core.IRepositories.BankSettlement;
using GIGLS.Core.IRepositories.InternationalRequest;
using GIGLS.Core.IRepositories.Sla;
using GIGLS.Core.IRepositories.Expenses;

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
        IGroupWaybillNumberRepository GroupWaybillNumber { get; set; }
        IGroupWaybillNumberMappingRepository GroupWaybillNumberMapping { get; set; }
        IWeightLimitPriceRepository WeightLimitPrice { get; set; }
        IWeightLimitRepository WeightLimit { get; set; }
        IPaymentTransactionRepository PaymentTransaction { get; set; }
        IPaymentPartialTransactionRepository PaymentPartialTransaction { get; set; }
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
        IMessageRepository Message { get; set; }
        IHaulageRepository Haulage { get; set; }
        IHaulageDistanceMappingRepository HaulageDistanceMapping { get; set; }
        IHaulageDistanceMappingPriceRepository HaulageDistanceMappingPrice { get; set; }
        IPackingListRepository PackingList { get; set; }
        ICashOnDeliveryAccountRepository CashOnDeliveryAccount { get; set; }
        ICashOnDeliveryRegisterAccountRepository CashOnDeliveryRegisterAccount { get; set; }
        IDemurrageRegisterAccountRepository DemurrageRegisterAccount { get; set; }
        ICashOnDeliveryBalanceRepository CashOnDeliveryBalance { get; set; }
        IDispatchRepository Dispatch { get; set; }
        IDispatchActivityRepository DispatchActivity { get; set; }
        IEmailSendLogRepository EmailSendLog { get; set; }
        ISmsSendLogRepository SmsSendLog { get; set; }
        IScanStatusRepository ScanStatus { get; set; }
        IGlobalPropertyRepository GlobalProperty { get; set; }
        IMissingShipmentRepository MissingShipment { get; set; }
        IShipmentCancelRepository ShipmentCancel { get; set; }
        ICountryRepository Country { get; set; }
        ICountryRouteZoneMapRepository CountryRouteZoneMap { get; set; }
        IShipmentRerouteRepository ShipmentReroute { get; set; }
        IShipmentPackagePriceRepository ShipmentPackagePrice { get; set; }
        IManifestVisitMonitoringRepository ManifestVisitMonitoring { get; set; }
        ITransitWaybillNumberRepository TransitWaybillNumber { get; set; }
        IManifestWaybillMappingRepository ManifestWaybillMapping { get; set; }
        IDeviceRepository Device { get; set; }
        IDeviceManagementRepository DeviceManagement { get; set; }
        IShipmentDeliveryOptionMappingRepository ShipmentDeliveryOptionMapping { get; set; }
        ICODSettlementSheetRepository CODSettlementSheet { get; set; }
        IPreShipmentItemRepository PreShipmentItem { get; set; }
        IPreShipmentRepository PreShipment { get; set; }
        IPreShipmentManifestMappingRepository PreShipmentManifestMapping { get; set; }
        INotificationRepository Notification { get; set; }
        ILogVisitReasonRepository LogVisitReason { get; set; }
        IWalletPaymentLogRepository WalletPaymentLog { get; set; }
        IOverdueShipmentRepository OverdueShipment { get; set; }
        IInternationalRequestReceiverRepository InternationalRequestReceiver { get; set; }
        ISLARepository SLA { get; set; }
        ISLASignedUserRepository SLASignedUser { get; set; }
        IExpenseTypeRepository ExpenseType { get; set; }
        IExpenditureRepository Expenditure { get; set; }
        IBankProcessingOrderForShipmentAndCODRepository BankProcessingOrderForShipmentAndCOD { get; set; }
        IBankProcessingOrderCodesRepository BankProcessingOrderCodes { get; set; }
        ICodPayOutListRepository CodPayOutList { get; set; }


        IOTPRepository OTP { get; set; }
        IPreShipmentMobileRepository PreShipmentMobile { get; set; }

        IPreShipmentItemMobileRepository PreShipmentItemMobile { get; set; }

        IUserLoginEmailRepository UserLoginEmail { get; set; }

        IMobileShipmentTrackingRepository MobileShipmentTracking { get; set; }
        IMobilePickUpRequestsRepository MobilePickUpRequests { get; set; }
        IMobileScanStatusRepository MobileScanStatus { get; set; }

        IRegionRepository Region { get; set; }
        IRegionServiceCentreMappingRepository RegionServiceCentreMapping { get; set; }
        IDemurrageRepository Demurrage { get; set; }

        IHUBManifestWaybillMappingRepository HUBManifestWaybillMapping { get; set; }

        ICategoryRepository Category { get; set; }

        ISubCategoryRepository SubCategory { get; set; }

        IPartnerTransactionsRepository PartnerTransactions { get; set; }

        IMobileRatingRepository MobileRating { get; set; }

        IReferrerCodeRepository ReferrerCode { get; set; }

        IDeliveryNumberRepository DeliveryNumber { get; set; }

        IVehicleTypeRepository VehicleType { get; set; }
        IPickupManifestRepository PickupManifest { get; set; }
        IPickupManifestWaybillMappingRepository PickupManifestWaybillMapping { get; set; }
        IRiderDeliveryRepository RiderDelivery { get; set; }
        IDeliveryLocationRepository DeliveryLocation { get; set; }
        ILGARepository LGA { get; set; }
        IWaybillPaymentLogRepository WaybillPaymentLog {get; set;}
        IBankRepository Bank { get; set; }
        IActivationCampaignEmailRepository ActivationCampaignEmail { get; set; }


        int Complete();
        System.Threading.Tasks.Task<int> CompleteAsync();
    }
}