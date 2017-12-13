using GIGLS.Core.IServices.Fleets;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Fleets;

namespace GIGLS.Services.Implementation.Fleets
{
    public class DispatchService : IDispatchService
    {
        public Task<object> AddDispatch(DispatchDTO Dispatch)
        {
            throw new NotImplementedException();
        }

        public Task DeleteDispatch(int DispatchId)
        {
            throw new NotImplementedException();
        }

        public Task<DispatchDTO> GetDispatchById(int DispatchId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DispatchDTO>> GetDispatchs()
        {
            throw new NotImplementedException();
        }

        public Task UpdateDispatch(int DispatchId, DispatchDTO Dispatch)
        {
            throw new NotImplementedException();
        }
    }
}
