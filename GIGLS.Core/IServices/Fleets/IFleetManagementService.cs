using GIGLS.Core.IServices;
using GIGLS.Core.DTO.Fleets;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Fleets
{
    public interface IFleetManagementService : IServiceDependencyMarker
    {
        Task<IEnumerable<FleetLocationDTO>> GetFleetLocationInformation(int fleetId, DateTime date);

        Task<IEnumerable<FleetLocationDTO>> GetFleetLocations();

        Task<FleetLocationDTO> GetFleetLocationById(int fleetId);
    }
}
