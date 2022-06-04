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
            if (context.ShipmentCategory.Any())
                return;
            if (context.InboundShipmentCategory.Any())
                return;

            context.ShipmentCategory.AddOrUpdate(
                    new ShipmentCategory() { ShipmentCategoryName = "Clothing & fabrics" },
                    new ShipmentCategory() { ShipmentCategoryName = "Cosmetics & Makeup" },
                    new ShipmentCategory() { ShipmentCategoryName = "Shoes" },
                    new ShipmentCategory() { ShipmentCategoryName = "Documents" },
                    new ShipmentCategory() { ShipmentCategoryName = "Fashion" },
                    new ShipmentCategory() { ShipmentCategoryName = "Accessories" },
                    new ShipmentCategory() { ShipmentCategoryName = "Health & Wellness" },
                    new ShipmentCategory() { ShipmentCategoryName = "Luggage & Bag" },
                    new ShipmentCategory() { ShipmentCategoryName = "Electronics" },
                    new ShipmentCategory() { ShipmentCategoryName = "Babby & Toddler" },
                    new ShipmentCategory() { ShipmentCategoryName = "Art & Craft" },
                    new ShipmentCategory() { ShipmentCategoryName = "Furniture & Decor" },
                    new ShipmentCategory() { ShipmentCategoryName = "Kitchen ware & Utensils" },
                    new ShipmentCategory() { ShipmentCategoryName = "Game & Entertainments" },
                    new ShipmentCategory() { ShipmentCategoryName = "Books & Magazine" },
                    new ShipmentCategory() { ShipmentCategoryName = "Automobile accessories" }
                );

            context.InboundShipmentCategory.AddOrUpdate(
                    new InboundShipmentCategory() { ShipmentCategoryId = 1, CountryId = 43, IsGoFaster = true, IsGoStandard = true },
                    new InboundShipmentCategory() { ShipmentCategoryId = 1, CountryId = 207, IsGoFaster = true, IsGoStandard = true },
                    new InboundShipmentCategory() { ShipmentCategoryId = 2, CountryId = 43, IsGoFaster = false, IsGoStandard = false },
                    new InboundShipmentCategory() { ShipmentCategoryId = 2, CountryId = 207, IsGoFaster = true, IsGoStandard = true },
                    new InboundShipmentCategory() { ShipmentCategoryId = 3, CountryId = 43, IsGoFaster = true, IsGoStandard = true },
                    new InboundShipmentCategory() { ShipmentCategoryId = 3, CountryId = 207, IsGoFaster = true, IsGoStandard = true },
                    new InboundShipmentCategory() { ShipmentCategoryId = 4, CountryId = 43, IsGoFaster = true, IsGoStandard = true },
                    new InboundShipmentCategory() { ShipmentCategoryId = 4, CountryId = 207, IsGoFaster = true, IsGoStandard = true },
                    new InboundShipmentCategory() { ShipmentCategoryId = 5, CountryId = 43, IsGoFaster = true, IsGoStandard = true },
                    new InboundShipmentCategory() { ShipmentCategoryId = 5, CountryId = 207, IsGoFaster = true, IsGoStandard = true },
                    new InboundShipmentCategory() { ShipmentCategoryId = 6, CountryId = 43, IsGoFaster = false, IsGoStandard = false },
                    new InboundShipmentCategory() { ShipmentCategoryId = 6, CountryId = 207, IsGoFaster = true, IsGoStandard = false },
                    new InboundShipmentCategory() { ShipmentCategoryId = 7, CountryId = 43, IsGoFaster = true, IsGoStandard = true },
                    new InboundShipmentCategory() { ShipmentCategoryId = 7, CountryId = 207, IsGoFaster = true, IsGoStandard = true },
                    new InboundShipmentCategory() { ShipmentCategoryId = 8, CountryId = 43, IsGoFaster = true, IsGoStandard = true },
                    new InboundShipmentCategory() { ShipmentCategoryId = 8, CountryId = 207, IsGoFaster = true, IsGoStandard = true },
                    new InboundShipmentCategory() { ShipmentCategoryId = 9, CountryId = 43, IsGoFaster = true, IsGoStandard = true },
                    new InboundShipmentCategory() { ShipmentCategoryId = 9, CountryId = 207, IsGoFaster = true, IsGoStandard = true },
                    new InboundShipmentCategory() { ShipmentCategoryId = 10, CountryId = 43, IsGoFaster = true, IsGoStandard = true },
                    new InboundShipmentCategory() { ShipmentCategoryId = 10, CountryId = 207, IsGoFaster = true, IsGoStandard = true },
                    new InboundShipmentCategory() { ShipmentCategoryId = 11, CountryId = 43, IsGoFaster = true, IsGoStandard = true },
                    new InboundShipmentCategory() { ShipmentCategoryId = 11, CountryId = 207, IsGoFaster = true, IsGoStandard = true },
                    new InboundShipmentCategory() { ShipmentCategoryId = 12, CountryId = 43, IsGoFaster = true, IsGoStandard = true },
                    new InboundShipmentCategory() { ShipmentCategoryId = 12, CountryId = 207, IsGoFaster = true, IsGoStandard = true },
                    new InboundShipmentCategory() { ShipmentCategoryId = 13, CountryId = 43, IsGoFaster = true, IsGoStandard = true },
                    new InboundShipmentCategory() { ShipmentCategoryId = 13, CountryId = 207, IsGoFaster = true, IsGoStandard = true },
                    new InboundShipmentCategory() { ShipmentCategoryId = 14, CountryId = 43, IsGoFaster = true, IsGoStandard = true },
                    new InboundShipmentCategory() { ShipmentCategoryId = 14, CountryId = 207, IsGoFaster = true, IsGoStandard = true },
                    new InboundShipmentCategory() { ShipmentCategoryId = 15, CountryId = 43, IsGoFaster = true, IsGoStandard = true },
                    new InboundShipmentCategory() { ShipmentCategoryId = 15, CountryId = 207, IsGoFaster = true, IsGoStandard = true },
                     new InboundShipmentCategory() { ShipmentCategoryId = 16, CountryId = 43, IsGoFaster = true, IsGoStandard = true },
                    new InboundShipmentCategory() { ShipmentCategoryId = 16, CountryId = 207, IsGoFaster = true, IsGoStandard = true }

                );
           
        }
    }
}
