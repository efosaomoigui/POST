using GIGLS.Core.Domain;
using GIGLS.Core.IRepositories.InternationalRequest;
using GIGLS.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIGLS.Core.DTO.InternationalShipmentDetails;

namespace GIGLS.Infrastructure.Persistence.Repositories.InternationalRequest
{
    public class InternationalRequestReceiverRepository : Repository<InternationalRequestReceiver, GIGLSContext>, IInternationalRequestReceiverRepository
    {
        public InternationalRequestReceiverRepository(GIGLSContext context) : base(context)
        {
        }

        public Task<IEnumerable<InternationalRequestReceiverDTO>> GetInternationalRequestReceivers()
        {
            throw new NotImplementedException();
        }
    }
}
