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
using Audit.EntityFramework;
using GIGLS.Core.Domain.Audit;
using GIGLS.CORE.Domain;
using Microsoft.AspNet.Identity.EntityFramework;
using GIGLS.Infrastructure.Migrations;
using GIGLS.Core.Domain.MessagingLog;

namespace GIGLS.Infrastructure.Persistence
{
    [DbConfigurationType(typeof(EntityFrameworkConfiguration))]
    public class GIGLSContext : AuditIdentityDbContext<User, AppRole, string, IdentityUserLogin, IdentityUserRole, AppUserClaim>
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
        //public DbSet<User> User { get; set; }
        //public DbSet<AppRole> AppRole { get; set; } 

        public DbSet<AuditTrailEvent> AuditTrailEvent { get; set; }

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

        //Weight Limit
        public DbSet<WeightLimit> WeightLimits { get; set; }
        public DbSet<WeightLimitPrice> WeightLimitPrices { get; set; }

        //Payment
        public DbSet<PaymentTransaction> PaymentTransactions { get; set; }

        //GeneralLedger
        public DbSet<Core.Domain.GeneralLedger> GeneralLedger { get; set; }
        public DbSet<Invoice> Invoice { get; set; }
        public DbSet<VAT> VAT { get; set; }
        public DbSet<Insurance> Insurance { get; set; }
        public DbSet<InvoiceShipment> InvoiceShipment { get; set; }

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


        #endregion


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer(
                new MigrateDatabaseToLatestVersion<GIGLSContext, Migrations.Configuration>());

            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<DomesticRouteZoneMap>().Property(p => p.DestinationId).IsOptional();
            modelBuilder.Entity<DomesticRouteZoneMap>().Property(p => p.DepartureId).IsOptional();
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
