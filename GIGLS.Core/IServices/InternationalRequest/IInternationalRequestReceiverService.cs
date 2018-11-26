using GIGLS.Core.DTO.InternationalShipmentDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.InternationalRequest
{
    public interface IInternationalRequestReceiverService : IServiceDependencyMarker
    {
        Task<IEnumerable<InternationalRequestReceiverDTO>> GetInternationalRequestReceiver();
        Task<InternationalRequestReceiverDTO> GetInternationalRequestReceiverById(int receiverId);
        Task<IEnumerable<InternationalRequestReceiverDTO>> GetInternationalRequestReceiverByCode(string code);
        Task<object> AddInternationalRequestReceiver(InternationalRequestReceiverDTO internationalRequestReceiver);
        Task UpdateInternationalRequestReceiver(int receiverId, InternationalRequestReceiverDTO internationalRequestReceiver);
        Task DeleteInternationalRequestReceiver(int receiverId);
    }
}
