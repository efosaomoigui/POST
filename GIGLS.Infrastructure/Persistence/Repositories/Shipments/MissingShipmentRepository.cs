using GIGLS.Core.Domain;
using GIGLS.Core.IRepositories.Shipments;
using GIGLS.Infrastructure.Persistence.Repository;

namespace GIGLS.Infrastructure.Persistence.Repositories.Shipments
{
    public class MissingShipmentRepository : Repository<MissingShipment, GIGLSContext>, IMissingShipmentRepository
    {
        public MissingShipmentRepository(GIGLSContext context) : base(context)
        {
        }
    }
}
