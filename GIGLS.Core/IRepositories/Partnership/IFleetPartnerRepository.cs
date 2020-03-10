using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.Domain.Partnership;
using GIGLS.Core.DTO.Partnership;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories.Partnership
{
    public interface IFleetPartnerRepository : IRepository<FleetPartner>
    {
        Task<List<FleetPartnerDTO>> GetFleetPartnersAsync();
        Task<int> FleetCount(string fleetCode);
        Task<List<VehicleTypeDTO>> GetVehiclesAttachedToFleetPartner(string fleetPartnerCode);
    }
}
