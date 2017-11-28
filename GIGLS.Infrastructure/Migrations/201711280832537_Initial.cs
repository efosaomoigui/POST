namespace GIGLS.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Company", "SettlementPeriod", c => c.Int(nullable: false));
            AddColumn("dbo.ServiceCentre", "TargetAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.ServiceCentre", "TargetOrder", c => c.Int(nullable: false));
            AddColumn("dbo.Invoice", "DueDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.PaymentTransaction", "UserId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PaymentTransaction", "UserId");
            DropColumn("dbo.Invoice", "DueDate");
            DropColumn("dbo.ServiceCentre", "TargetOrder");
            DropColumn("dbo.ServiceCentre", "TargetAmount");
            DropColumn("dbo.Company", "SettlementPeriod");
        }
    }
}
