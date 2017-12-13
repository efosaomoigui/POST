using GIGLS.Core.IServices.Fleets;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Fleets;

namespace GIGLS.Services.Implementation.Fleets
{
    public class DispatchActivityService : IDispatchActivityService
    {
        public Task<object> AddDispatchActivity(DispatchActivityDTO DispatchActivity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteDispatchActivity(int DispatchActivityId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DispatchActivityDTO>> GetDispatchActivities()
        {
            throw new NotImplementedException();
        }

        public Task<DispatchActivityDTO> GetDispatchActivityById(int DispatchActivityId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateDispatchActivity(int DispatchActivityId, DispatchActivityDTO DispatchActivity)
        {
            throw new NotImplementedException();
        }
    }
}
