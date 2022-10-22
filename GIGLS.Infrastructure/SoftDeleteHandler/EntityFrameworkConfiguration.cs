using System.Data.Entity;

namespace POST.INFRASTRUCTURE.SoftDeleteHandler
{
    public class EntityFrameworkConfiguration : DbConfiguration
    {
        public EntityFrameworkConfiguration()
        {
            AddInterceptor(new SoftDeleteInterceptor());
        }
    }
}
