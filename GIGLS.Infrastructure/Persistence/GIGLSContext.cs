using System.Data.Entity;
using GIGL.GIGLS.Core.Domain;
using GIGLS.Core.Domain;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Threading.Tasks;
using GIGLS.Core;
using System;
using GIGLS.Core.Domain.Partnership;
using GIGLS.Core.Domain.Wallet;
using GIGLS.INFRASTRUCTURE.SoftDeleteHandler;
using GIGLS.Core.Domain.Audit;
using GIGLS.CORE.Domain;
using Microsoft.AspNet.Identity.EntityFramework;
using GIGLS.Core.Domain.MessagingLog;
using GIGLS.Core.Domain.ShipmentScan;
using GIGLS.Core.Domain.Utility;
using GIGLS.Core.Domain.Devices;
using GIGLS.Core.Domain.BankSettlement;
using GIGLS.Core.Domain.SLA;
using GIGLS.Core.Domain.Expenses;

namespace GIGLS.Infrastructure.Persistence
{
    [DbConfigurationType(typeof(EntityFrameworkConfiguration))]
    public class GIGLSContext : IdentityDbContext<User, AppRole, string, IdentityUserLogin, IdentityUserRole, AppUserClaim>
    {
        //, throwIfV1Schema: false
        public GIGLSContext()
            : base("GIGLSContextDB")
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;

            //Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
        }

        public static GIGLSContext Create()
        {
            return new GIGLSContext();
        }

        #region Entities

        public DbSet<AuditTrailEvent> AuditTrailEvent { get; set; }

        public DbSet<Country> Country { get; set; }
        public DbSet<State> State { get; set; }
        public DbSet<Station> Station { get; set; }
        public DbSet<Zone> Zone { get; set; }
        //public DbSet<Role> Role { get; set; }

        //Delivery Option
        public DbSet<DeliveryOption> DeliveryOption { get; set; }
        public DbSet<DeliveryOptionPrice> DeliveryOptionPrice { get; set; }

        //Special Shipment Pricing
        public DbSet<SpecialDomesticZonePrice> SpecialDomesticZonePrice { get; set; }
        public DbSet<SpecialDomesticPackage> SpecialDomesticPackage { get; set; }

        //General Shipment Pricing
        public DbSet<DomesticRouteZoneMap> DomesticRouteZoneMap { get; set; }
        public DbSet<DomesticZonePrice> DomesticZonePrice { get; set; }
        public DbSet<CountryRouteZoneMap> CountryRouteZoneMap { get; set; }        

        //Haulauge Pricing
        public DbSet<Haulage> Haulage { get; set; }
        public DbSet<HaulageDistanceMapping> HaulageDistanceMapping { get; set; }
        public DbSet<HaulageDistanceMappingPrice> HaulageDistanceMappingPrice { get; set; }
        public DbSet<PackingList> PackingList { get; set; }

        //Waybill
        public DbSet<WaybillNumber> WaybillNumber { get; set; }

        //GroupWaybill
        public DbSet<GroupWaybillNumber> GroupWaybillNumber { get; set; }
        public DbSet<GroupWaybillNumberMapping> GroupWaybillNumberMapping { get; set; }


        //Shipment
        public DbSet<Manifest> Manifest { get; set; }
        public DbSet<ManifestGroupWaybillNumberMapping> ManifestGroupWaybillNumberMapping { get; set; }

        public DbSet<Shipment> Shipment { get; set; }
        public DbSet<ShipmentItem> ShipmentItem { get; set; }
        public DbSet<ShipmentTracking> ShipmentTracking { get; set; }
        public DbSet<ShipmentCollection> ShipmentCollection { get; set; }
        public DbSet<ShipmentReturn> ShipmentReturn { get; set; }
        public DbSet<MissingShipment> MissingShipment { get; set; }
        public DbSet<ShipmentCancel> ShipmentCancel { get; set; }
        public DbSet<ShipmentReroute> ShipmentReroute { get; set; }
        public DbSet<ShipmentPackagePrice> ShipmentPackagePrice { get; set; }

        public DbSet<ManifestVisitMonitoring> ManifestVisitMonitoring { get; set; }
        public DbSet<LogVisitReason> LogVisitReason { get; set; }
        public DbSet<ManifestWaybillMapping> ManifestWaybillMapping { get; set; }

        public DbSet<TransitWaybillNumber> TransitWaybillNumber { get; set; }

        public DbSet<ShipmentDeliveryOptionMapping> ShipmentDeliveryOptionMapping { get; set; }

        public DbSet<Demurrage> Demurrage { get; set; }
        //customer
        public DbSet<Company> Company { get; set; }
        public DbSet<CompanyContactPerson> CompanyContactPerson { get; set; }
        public DbSet<IndividualCustomer> IndividualCustomer { get; set; }

        //service centres
        public DbSet<ServiceCentre> ServiceCentre { get; set; }
        public DbSet<UserServiceCentreMapping> UserServiceCentreMapping { get; set; }

        //Vehicle
        public DbSet<Fleet> Fleet { get; set; }
        public DbSet<FleetMake> FleetMake { get; set; }
        public DbSet<FleetModel> FleetModel { get; set; }
        public DbSet<FleetPart> FleetPart { get; set; }
        public DbSet<FleetTrip> FleetTrip { get; set; }
        public DbSet<FleetPartInventory> FleetPartInventory { get; set; }
        public DbSet<FleetPartInventoryHistory> FleetPartInventoryHistory { get; set; }
        public DbSet<Dispatch> Dispatch { get; set; }
        public DbSet<DispatchActivity> DispatchActivity { get; set; }

        //Partner
        public DbSet<Partner> Partners { get; set; }

        //These entities below are not used for now
        //store
        public DbSet<Store> Store { get; set; }

        public DbSet<OTP> OTP { get; set; }

        //stock
        public DbSet<StockRequest> StockRequest { get; set; }
        public DbSet<StockRequestPart> StockRequestPart { get; set; }
        public DbSet<StockSupplyDetails> StockSupplyDetails { get; set; }

        //maintenance
        public DbSet<JobCard> JobCard { get; set; }
        public DbSet<JobCardManagement> JobCardManagement { get; set; }
        public DbSet<JobCardManagementPart> JobCardManagementPart { get; set; }

        public DbSet<Vendor> Vendor { get; set; }
        public DbSet<Workshop> Workshop { get; set; }

        //Error Logger 
        public DbSet<LogEntry> LogEntry { get; set; }

        // Partnership
        public DbSet<IdentificationType> IdentificationTypes { get; set; }
        public DbSet<PartnerApplication> PartnerApplications { get; set; }

        // Wallet
        public DbSet<AccountSummary> AccountSummaries { get; set; }
        public DbSet<AccountTransaction> AccountTransactions { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<WalletTransaction> WalletTransactions { get; set; }
        public DbSet<WalletNumber> WalletNumbers { get; set; }
        public DbSet<CashOnDeliveryAccount> CashOnDeliveryAccount { get; set; }
        public DbSet<CashOnDeliveryBalance> CashOnDeliveryBalance { get; set; }
        public DbSet<CashOnDeliveryRegisterAccount> CashOnDeliveryRegisterAccount { get; set; }
        public DbSet<DemurrageRegisterAccount> DemurrageRegisterAccount { get; set; }
        public DbSet<WaybillPaymentLog> WaybillPaymentLog { get; set; }
        public DbSet<CountryRateConversion> CountryRateConversion { get; set; }

        //Weight Limit
        public DbSet<WeightLimit> WeightLimits { get; set; }
        public DbSet<WeightLimitPrice> WeightLimitPrices { get; set; }

        //Payment
        public DbSet<PaymentTransaction> PaymentTransactions { get; set; }
        public DbSet<PaymentPartialTransaction> PaymentPartialTransactions { get; set; }

        //GeneralLedger
        public DbSet<Core.Domain.GeneralLedger> GeneralLedger { get; set; }
        public DbSet<Invoice> Invoice { get; set; }
        public DbSet<VAT> VAT { get; set; }
        public DbSet<Insurance> Insurance { get; set; }
        public DbSet<InvoiceShipment> InvoiceShipment { get; set; }
        public DbSet<ExpenseType> ExpenseType { get; set; }
        public DbSet<Expenditure> Expenditure { get; set; }

        //ClientNode
        public DbSet<ClientNode> ClientNode { get; set; }

        //NumberGeneratorMonitor
        public DbSet<NumberGeneratorMonitor> NumberGeneratorMonitor { get; set; }

        //Navigation
        public DbSet<MainNav> MainNav { get; set; }
        public DbSet<SubNav> SubNav { get; set; }
        public DbSet<SubSubNav> SubSubNav { get; set; }

        //Message
        public DbSet<Message> Message { get; set; }
        public DbSet<EmailSendLog> EmailSendLog { get; set; }
        public DbSet<SmsSendLog> SmsSendLog { get; set; }

        //Shipment Scan Status
        public DbSet<ScanStatus> ScanStatus { get; set; }

        //Global Property
        public DbSet<GlobalProperty> GlobalProperty { get; set; }

        //Device Management
        public DbSet<Device> Device { get; set; }
        public DbSet<DeviceManagement> DeviceManagement { get; set; }

        //BankSettlement
        public DbSet<CODSettlementSheet> CODSettlementSheet { get; set; }

        //PreShipment
        public DbSet<PreShipment> PreShipment { get; set; }
        public DbSet<PreShipmentItem> PreShipmentItem { get; set; }

        //PreShipmentManifestMapping
        public DbSet<PreShipmentManifestMapping> PreShipmentManifestMapping { get; set; }

        public DbSet<Notification> Notification { get; set; }
        public DbSet<WalletPaymentLog> WalletPaymentLog { get; set; }

        public DbSet<OverdueShipment> OverdueShipment { get; set; }

        //international request

        public DbSet<InternationalRequestReceiver> InternationalRequestReceivers { get; set; }
        public DbSet<InternationalRequestReceiverItem> InternationalRequestReceiverItems { get; set; }

        //SLA
        public DbSet<SLA> SLAs { get; set; }
        public DbSet<SLASignedUser> SLASignedUsers { get; set; }

        //Bank Settlement Order
        public DbSet<BankProcessingOrderForShipmentAndCOD> BankProcessingOrderForShipmentAndCOD { get; set; }
        public DbSet<BankProcessingOrderCodes> BankProcessingOrderCodes { get; set; }
        public DbSet<CodPayOutList> CodPayOutList { get; set; }

        public DbSet<PreShipmentItemMobile> PresShipmentItemMobile { get; set; }

        public DbSet<PreShipmentMobile> PresShipmentMobile { get; set; }
        public DbSet<MobileShipmentTracking> MobileShipmentTracking { get; set; }

        public DbSet<Location> Location { get; set; }

        public DbSet<UserLoginEmail> UserLoginEmail { get; set; }
        public DbSet<MobilePickUpRequests> MobilePickUpRequests { get; set; }

        //Region
        public DbSet<Region> Regions { get; set; }
        public DbSet<RegionServiceCentreMapping> RegionServiceCentreMappings { get; set; }
        public DbSet<MobileScanStatus> MobileScanStatus { get; set; }

        public DbSet<HUBManifestWaybillMapping> HUBManifestWaybillMapping { get; set; }

        public DbSet<Category> Category { get; set; }

        public DbSet<SubCategory> SubCategory { get; set; }

        public DbSet<PartnerTransactions> PartnerTransactions { get; set; }

        public DbSet<MobileRating> MobileRating { get; set; }

        public DbSet<ReferrerCode> ReferrerCode { get; set; }

        public DbSet<DeliveryNumber> DeliveryNumber { get; set; }

        public DbSet<VehicleType> VehicleType { get; set; }

        public DbSet<PickupManifest> PickupManifest { get; set; }
        
        public DbSet<PickupManifestWaybillMapping> PickupManifestWaybillMapping { get; set; }
        public DbSet<RiderDelivery> RiderDelivery { get; set; }
        public DbSet<DeliveryLocation> DeliveryLocation { get; set; }
        public DbSet<LGA> LGA { get; set; }
        public DbSet<Bank> Bank { get; set; }
        public DbSet<ActivationCampaignEmail> ActivationCampaignEmail { get; set; }

        #endregion


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer(
                new MigrateDatabaseToLatestVersion<GIGLSContext, Migrations.Configuration>());

            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<DomesticRouteZoneMap>().Property(p => p.DestinationId).IsOptional();
            modelBuilder.Entity<DomesticRouteZoneMap>().Property(p => p.DepartureId).IsOptional();

            modelBuilder.Entity<CountryRouteZoneMap>().Property(p => p.DestinationId).IsOptional();
            modelBuilder.Entity<CountryRouteZoneMap>().Property(p => p.DepartureId).IsOptional();

            modelBuilder.Entity<CountryRateConversion>().Property(p => p.DepartureCountryId).IsOptional();
            modelBuilder.Entity<CountryRateConversion>().Property(p => p.DestinationCountryId).IsOptional();
        }

        #region Customize to handle Date and Delete status of Entities
        public override int SaveChanges()
        {
            PerformEntityAudit();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync()
        {
            PerformEntityAudit();
            return base.SaveChangesAsync();
        }

        private void PerformEntityAudit()
        {
            foreach (var entry in ChangeTracker.Entries<IAuditable>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        var currentDateTime = DateTime.Now;
                        entry.Entity.DateCreated = currentDateTime;
                        entry.Entity.DateModified = currentDateTime;
                        entry.Entity.IsDeleted = false;
                        break;

                    case EntityState.Modified:
                        entry.Entity.DateModified = DateTime.Now;
                        //entry.Property(e => e.RowVersion).OriginalValue =
                        //    entry.Property(e => e.RowVersion).CurrentValue;
                        break;

                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entry.Entity.DateModified = DateTime.Now;
                        entry.Entity.IsDeleted = true;
                        break;
                }
            }
        }

        #endregion
    }
}
