using GIGL.POST.Core.Repositories;
using POST.Core.Domain;
using POST.Core.DTO.InternationalShipmentDetails;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Core.IRepositories.InternationalRequest
{
    public interface IInternationalRequestReceiverRepository : IRepository<InternationalRequestReceiver>
    {
        Task<IEnumerable<InternationalRequestReceiverDTO>> GetInternationalRequestReceivers();
    }
}
