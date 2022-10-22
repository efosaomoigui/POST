using POST.Core.Domain;
using POST.Core.IRepositories.Shipments;
using POST.Infrastructure.Persistence;
using POST.Infrastructure.Persistence.Repository;

namespace POST.INFRASTRUCTURE.Persistence.Repositories.Shipments
{
    public class TransitWaybillNumberRepository : Repository<TransitWaybillNumber, GIGLSContext>, ITransitWaybillNumberRepository
    {
        public TransitWaybillNumberRepository(GIGLSContext context) : base(context)
        {
        }
    }

}
