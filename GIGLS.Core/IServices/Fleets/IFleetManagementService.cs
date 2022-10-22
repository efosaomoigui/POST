using POST.Core.IServices;
using POST.Core.DTO.Fleets;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Core.IServices.Fleets
{
    public interface IFleetManagementService : IServiceDependencyMarker
    {
        Task<IEnumerable<FleetLocationDTO>> GetFleetLocationInformation(int fleetId, DateTime date);

        Task<IEnumerable<FleetLocationDTO>> GetFleetLocations();

        Task<FleetLocationDTO> GetFleetLocationById(int fleetId);
    }
}
