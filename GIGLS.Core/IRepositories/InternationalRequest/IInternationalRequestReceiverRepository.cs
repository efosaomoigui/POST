using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO.InternationalShipmentDetails;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories.InternationalRequest
{
    public interface IInternationalRequestReceiverRepository : IRepository<InternationalRequestReceiver>
    {
        Task<IEnumerable<InternationalRequestReceiverDTO>> GetInternationalRequestReceivers();
    }
}
