using GIGLS.Core.IServices.InternationalRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIGLS.Core.DTO.InternationalShipmentDetails;

namespace GIGLS.Services.Implementation.InternationalRequest
{
    public class InternationalRequestReceiverService : IInternationalRequestReceiverService
    {
        public Task<object> AddInternationalRequestReceiver(InternationalRequestReceiverDTO internationalRequestReceiver)
        {
            throw new NotImplementedException();
        }

        public Task DeleteInternationalRequestReceiver(int receiverId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<InternationalRequestReceiverDTO>> GetInternationalRequestReceiver()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<InternationalRequestReceiverDTO>> GetInternationalRequestReceiverByCode(string code)
        {
            throw new NotImplementedException();
        }

        public Task<InternationalRequestReceiverDTO> GetInternationalRequestReceiverById(int receiverId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateInternationalRequestReceiver(int receiverId, InternationalRequestReceiverDTO internationalRequestReceiver)
        {
            throw new NotImplementedException();
        }
    }
}
