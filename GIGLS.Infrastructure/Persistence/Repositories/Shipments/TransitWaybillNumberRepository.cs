using GIGLS.Core.Domain;
using GIGLS.Core.IRepositories.Shipments;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.Shipments
{
    public class TransitWaybillNumberRepository : Repository<TransitWaybillNumber, GIGLSContext>, ITransitWaybillNumberRepository
    {
        public TransitWaybillNumberRepository(GIGLSContext context) : base(context)
        {
        }
    }

}
