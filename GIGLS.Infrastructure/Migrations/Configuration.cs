namespace GIGLS.Infrastructure.Migrations
{
    using GIGLS.Core.Domain;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<GIGLS.Infrastructure.Persistence.GIGLSContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;

        }

        protected override void Seed(GIGLS.Infrastructure.Persistence.GIGLSContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            if (context.InboundCategory.Any())
                return; 

            context.InboundCategory.AddOrUpdate(
                    new InboundCategory() { CategoryName = "Clothing & fabrics", IsGoFaster = true, IsGoStandard = true }, 
                    new InboundCategory() { CategoryName = "Cosmetics & Makeup", IsGoFaster = true, IsGoStandard = true }, 
                    new InboundCategory() { CategoryName = "Shoes", IsGoFaster = true, IsGoStandard = true }, 
                    new InboundCategory() { CategoryName = "Documents", IsGoFaster = true, IsGoStandard = true }, 
                    new InboundCategory() { CategoryName = "Fashion", IsGoFaster = true, IsGoStandard = true },
                    new InboundCategory() { CategoryName = "Accessories", IsGoFaster = true, IsGoStandard = false }, 
                    new InboundCategory() { CategoryName = "Health & Wellness", IsGoFaster = true, IsGoStandard = true }, 
                    new InboundCategory() { CategoryName = "Luggage & Bag", IsGoFaster = true, IsGoStandard = true }, 
                    new InboundCategory() { CategoryName = "Electronics", IsGoFaster = true, IsGoStandard = true }, 
                    new InboundCategory() { CategoryName = "Babby & Toddler", IsGoFaster = true, IsGoStandard = true },
                    new InboundCategory() { CategoryName = "Art & Craft", IsGoFaster = true, IsGoStandard = true }, 
                    new InboundCategory() { CategoryName = "Furniture & Decor", IsGoFaster = true, IsGoStandard = true }, 
                    new InboundCategory() { CategoryName = "Kitchen ware & Utensils", IsGoFaster = true, IsGoStandard = true }, 
                    new InboundCategory() { CategoryName = "Game & Entertainments", IsGoFaster = true, IsGoStandard = true }, 
                    new InboundCategory() { CategoryName = "Books & Magazine", IsGoFaster = true, IsGoStandard = true },
                    new InboundCategory() { CategoryName = "Automobile accessories", IsGoFaster = true, IsGoStandard = true }
                );
        }
    }
}
