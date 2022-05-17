using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Fleets;

namespace GIGLS.Core.IServices.Fleets
{
    public interface IFleetJobCardService : IServiceDependencyMarker
    {
        Task<IEnumerable<FleetJobCardDTO>> GetFleetJobCardsAsync();
    }
}
