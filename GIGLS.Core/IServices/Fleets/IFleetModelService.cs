using GIGLS.Core.DTO.Fleets;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.Fleets
{ 
    public interface IFleetModelService : IServiceDependencyMarker
    {
        Task<IEnumerable<FleetModelDTO>> GetFleetModels();
        Task<FleetModelDTO> GetFleetModelById(int modelId);
        Task<object> AddFleetModel(FleetModelDTO model);
        Task UpdateFleetModel(int modelId, FleetModelDTO model);
        Task DeleteFleetModel(int modelId);
    }
}
