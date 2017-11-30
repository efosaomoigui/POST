namespace GIGLS.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
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
            
        }
    }
}
