using POST.Core.Domain;
using POST.Core.IRepositories.InternationalRequest;
using POST.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POST.Core.DTO.InternationalShipmentDetails;

namespace POST.Infrastructure.Persistence.Repositories.InternationalRequest
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
