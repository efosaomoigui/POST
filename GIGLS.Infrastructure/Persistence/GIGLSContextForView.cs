using System.Data.Entity;
using GIGLS.INFRASTRUCTURE.SoftDeleteHandler;
using GIGLS.Core.View;

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

    }
}
