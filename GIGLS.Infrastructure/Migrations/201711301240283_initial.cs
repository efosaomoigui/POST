namespace GIGLS.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ZoneHaulagePrice", "ZoneId", "dbo.Zone");
            DropForeignKey("dbo.ZoneHaulagePrice", "HaulageId", "dbo.Haulage");
            DropIndex("dbo.ZoneHaulagePrice", new[] { "ZoneId" });
            DropIndex("dbo.ZoneHaulagePrice", new[] { "HaulageId" });
            DropTable("dbo.ZoneHaulagePrice");
            DropTable("dbo.Haulage");
        }
    }
}
