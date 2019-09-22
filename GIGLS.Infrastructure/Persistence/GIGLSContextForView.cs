using System.Data.Entity;
using GIGLS.INFRASTRUCTURE.SoftDeleteHandler;
using GIGLS.Core.View;
using System.Data.Entity.ModelConfiguration.Conventions;
using GIGLS.Core.Domain.Wallet;
using System.Configuration;
using System;
using GIGLS.Core.View.AdminReportView;

namespace GIGLS.Infrastructure.Persistence
{
    [DbConfigurationType(typeof(EntityFrameworkConfiguration))]
    public class GIGLSContextForView : DbContext
    {
        public GIGLSContextForView()
            : base("GIGLSContextDB")
        {
            Database.SetInitializer<GIGLSContextForView>(null);
            //Database.Log = s => System.Diagnostics.Debug.WriteLine(s);

            //Get Bank Deposit Module StartDate
            var isTimeOutCheck = ConfigurationManager.AppSettings["isTimeOut"];
            var timeForTimeOut = ConfigurationManager.AppSettings["AgilityTimeOutVal"];

            if (isTimeOutCheck != null && isTimeOutCheck == "true")
            {
                if (timeForTimeOut != null)
                {
                    var outVal = Convert.ToInt32(timeForTimeOut);
                    Database.CommandTimeout = outVal;
                }
            }

        }

        public DbSet<InvoiceView> InvoiceView { get; set; }
        public DbSet<CustomerView> CustomerView { get; set; }
        public DbSet<ShipmentTrackingView> ScanTrackingView { get; set; }
        public DbSet<WalletPaymentLogView> WalletPaymentLogView { get; set; }
        public DbSet<Report_AllTimeSalesByCountry> Report_AllTimeSalesByCountry { get; set; }
        public DbSet<Report_BusiestRoute> Report_BusiestRoute { get; set; }
        public DbSet<Report_CustomerRevenue> Report_CustomerRevenue { get; set; }
        public DbSet<Report_MostShippedItemByWeight> Report_MostShippedItemByWeight { get; set; }
        public DbSet<Report_RevenuePerServiceCentre> Report_RevenuePerServiceCentre { get; set; }
        public DbSet<Report_TotalServiceCentreByState> Report_TotalServiceCentreByState { get; set; }
        public DbSet<Report_TotalOrdersDelivered> Report_TotalOrdersDelivered { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

    }
}
