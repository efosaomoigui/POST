namespace GIGLS.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ab : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Haulage",
                c => new
                    {
                        HaulageId = c.Int(nullable: false, identity: true),
                        Tonne = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Status = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.HaulageId);
            
            CreateTable(
                "dbo.ZoneHaulagePrice",
                c => new
                    {
                        ZoneHaulagePriceId = c.Int(nullable: false, identity: true),
                        HaulageId = c.Int(nullable: false),
                        ZoneId = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ZoneHaulagePriceId)
                .ForeignKey("dbo.Haulage", t => t.HaulageId, cascadeDelete: true)
                .ForeignKey("dbo.Zone", t => t.ZoneId, cascadeDelete: true)
                .Index(t => t.HaulageId)
                .Index(t => t.ZoneId);
            
            AddColumn("dbo.GroupWaybillNumberMapping", "DepartureServiceCentreId", c => c.Int(nullable: false));
            AddColumn("dbo.GroupWaybillNumberMapping", "DestinationServiceCentreId", c => c.Int(nullable: false));
            AddColumn("dbo.GroupWaybillNumberMapping", "DepartureServiceCentre_ServiceCentreId", c => c.Int());
            AddColumn("dbo.GroupWaybillNumberMapping", "DestinationServiceCentre_ServiceCentreId", c => c.Int());
            AddColumn("dbo.ShipmentItem", "SerialNumber", c => c.Int(nullable: false));
            CreateIndex("dbo.GroupWaybillNumberMapping", "DepartureServiceCentre_ServiceCentreId");
            CreateIndex("dbo.GroupWaybillNumberMapping", "DestinationServiceCentre_ServiceCentreId");
            AddForeignKey("dbo.GroupWaybillNumberMapping", "DepartureServiceCentre_ServiceCentreId", "dbo.ServiceCentre", "ServiceCentreId");
            AddForeignKey("dbo.GroupWaybillNumberMapping", "DestinationServiceCentre_ServiceCentreId", "dbo.ServiceCentre", "ServiceCentreId");
            DropTable("dbo.GroupWaybillNumberMonitor");
            DropTable("dbo.ManifestMonitor");
            DropTable("dbo.WaybillNumberMonitor");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.WaybillNumberMonitor",
                c => new
                    {
                        WaybillNumberMonitorId = c.Int(nullable: false, identity: true),
                        ServiceCentreCode = c.String(),
                        Code = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.WaybillNumberMonitorId);
            
            CreateTable(
                "dbo.ManifestMonitor",
                c => new
                    {
                        ManifestMonitorId = c.Int(nullable: false, identity: true),
                        ServiceCentreCode = c.String(),
                        Code = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ManifestMonitorId);
            
            CreateTable(
                "dbo.GroupWaybillNumberMonitor",
                c => new
                    {
                        GroupWaybillNumberMonitorId = c.Int(nullable: false, identity: true),
                        ServiceCentreCode = c.String(),
                        Code = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.GroupWaybillNumberMonitorId);
            
            DropForeignKey("dbo.ZoneHaulagePrice", "ZoneId", "dbo.Zone");
            DropForeignKey("dbo.ZoneHaulagePrice", "HaulageId", "dbo.Haulage");
            DropForeignKey("dbo.GroupWaybillNumberMapping", "DestinationServiceCentre_ServiceCentreId", "dbo.ServiceCentre");
            DropForeignKey("dbo.GroupWaybillNumberMapping", "DepartureServiceCentre_ServiceCentreId", "dbo.ServiceCentre");
            DropIndex("dbo.ZoneHaulagePrice", new[] { "ZoneId" });
            DropIndex("dbo.ZoneHaulagePrice", new[] { "HaulageId" });
            DropIndex("dbo.GroupWaybillNumberMapping", new[] { "DestinationServiceCentre_ServiceCentreId" });
            DropIndex("dbo.GroupWaybillNumberMapping", new[] { "DepartureServiceCentre_ServiceCentreId" });
            DropColumn("dbo.ShipmentItem", "SerialNumber");
            DropColumn("dbo.GroupWaybillNumberMapping", "DestinationServiceCentre_ServiceCentreId");
            DropColumn("dbo.GroupWaybillNumberMapping", "DepartureServiceCentre_ServiceCentreId");
            DropColumn("dbo.GroupWaybillNumberMapping", "DestinationServiceCentreId");
            DropColumn("dbo.GroupWaybillNumberMapping", "DepartureServiceCentreId");
            DropTable("dbo.ZoneHaulagePrice");
            DropTable("dbo.Haulage");
        }
    }
}
