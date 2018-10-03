using System.Data.Entity;
using GIGLS.INFRASTRUCTURE.SoftDeleteHandler;
using GIGLS.Core.View;
using System.Data.Entity.ModelConfiguration.Conventions;

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
        }

        public DbSet<InvoiceView> InvoiceView { get; set; }
        public DbSet<CustomerView> CustomerView { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

    }
}
