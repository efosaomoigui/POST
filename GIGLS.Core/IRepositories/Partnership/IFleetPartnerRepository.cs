using GIGL.POST.Core.Repositories;
using POST.Core.Domain.Partnership;
using POST.Core.DTO.Fleets;
using POST.Core.DTO.Partnership;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Core.IRepositories.Partnership
{
    public interface IFleetPartnerRepository : IRepository<FleetPartner>
    {
        Task<List<FleetPartnerDTO>> GetFleetPartnersAsync();
        Task<int> FleetCount(string fleetCode);
        Task<List<VehicleTypeDTO>> GetVehiclesAttachedToFleetPartner(string fleetPartnerCode);
        Task<List<PartnerDTO>> GetExternalPartnersNotAttachedToAnyFleetPartner();
        Task<List<AssetDTO>> GetFleetAttachedToEnterprisePartner(string fleetPartnerCode);
        Task<AssetDetailsDTO> GetFleetAttachedToEnterprisePartnerById(int fleetId);
        Task<List<AssetTripDTO>> GetFleetTrips(int fleetId);
        Task<List<AssetTripDTO>> GetFleetTripsByPartner(string partnercode);
    }
}
