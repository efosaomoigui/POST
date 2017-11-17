namespace GIGLS.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class a : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Insurance",
                c => new
                    {
                        InsuranceId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.InsuranceId);
            
            CreateTable(
                "dbo.ShipmentCollection",
                c => new
                    {
                        Waybill = c.String(nullable: false, maxLength: 100),
                        Name = c.String(),
                        PhoneNumber = c.String(),
                        Email = c.String(),
                        Address = c.String(),
                        City = c.String(),
                        State = c.String(),
                        IndentificationUrl = c.String(),
                        ShipmentScanStatus = c.Int(nullable: false),
                        UserId = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Waybill)
                .Index(t => t.Waybill, unique: true);
            
            CreateTable(
                "dbo.ShipmentReturn",
                c => new
                    {
                        WaybillNew = c.String(nullable: false, maxLength: 100),
                        WaybillOld = c.String(maxLength: 100),
                        Discount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        OriginalPayment = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.WaybillNew)
                .Index(t => t.WaybillNew, unique: true)
                .Index(t => t.WaybillOld, unique: true);
            
            CreateTable(
                "dbo.VAT",
                c => new
                    {
                        VATId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Type = c.Int(nullable: false),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.VATId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.ShipmentReturn", new[] { "WaybillOld" });
            DropIndex("dbo.ShipmentReturn", new[] { "WaybillNew" });
            DropIndex("dbo.ShipmentCollection", new[] { "Waybill" });
            DropTable("dbo.VAT");
            DropTable("dbo.ShipmentReturn");
            DropTable("dbo.ShipmentCollection");
            DropTable("dbo.Insurance");
        }
    }
}
