namespace GIGLS.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ShipmentDeclaredValue : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Shipment", "IsdeclaredVal", c => c.Boolean(nullable: false));
            AddColumn("dbo.Shipment", "DeclarationOfValueCheck", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Shipment", "DeclarationOfValueCheck");
            DropColumn("dbo.Shipment", "IsdeclaredVal");
        }
    }
}
