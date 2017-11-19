namespace GIGLS.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserAdditions : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "SystemUserId", c => c.String());
            AddColumn("dbo.AspNetUsers", "SystemUserRole", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "SystemUserRole");
            DropColumn("dbo.AspNetUsers", "SystemUserId");
        }
    }
}
