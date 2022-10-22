using POST.Core.Domain;
using POST.Core.IRepositories;
using POST.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POST.Infrastructure.Persistence.Repositories
{
    public class DeliveryNumberRepository : Repository<DeliveryNumber, GIGLSContext>, IDeliveryNumberRepository
    {
        public DeliveryNumberRepository(GIGLSContext context) : base(context)
        {
        }
    }
}
