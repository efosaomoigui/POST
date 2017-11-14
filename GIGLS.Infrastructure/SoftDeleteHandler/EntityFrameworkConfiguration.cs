using System.Data.Entity;

namespace GIGLS.INFRASTRUCTURE.SoftDeleteHandler
{
    public class EntityFrameworkConfiguration : DbConfiguration
    {
        public EntityFrameworkConfiguration()
        {
            AddInterceptor(new SoftDeleteInterceptor());
        }
    }
}
