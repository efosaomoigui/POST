using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Fleets;

namespace GIGLS.Core.IServices.Fleets
{
    public interface IFleetDisputeMessageService : IServiceDependencyMarker
    {
        Task<bool> AddFleetDisputeMessageAsync(FleetDisputeMessageDto dto);
        Task<List<FleetDisputeMessageDto>> GetAllFleetDisputeMessagesAsync();
    }
}
