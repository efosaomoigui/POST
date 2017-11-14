using GIGLS.Core.DTO.Fleets;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Fleets
{
    public interface IFleetTripService : IServiceDependencyMarker
    {
        Task<List<FleetTripDTO>> GetFleetTrips();
        Task<FleetTripDTO> GetFleetTripById(int tripId);
        Task<object> AddFleetTrip(FleetTripDTO trip);
        Task UpdateFleetTrip(int tripId, FleetTripDTO trip);
        Task DeleteFleetTrip(int tripId);
    }
}
