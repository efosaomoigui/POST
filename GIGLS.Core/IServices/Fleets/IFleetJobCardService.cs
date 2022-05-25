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
        Task<IEnumerable<FleetJobCardDto>> GetFleetJobCardsAsync();
        Task<bool> OpenFleetJobCardsAsync(OpenFleetJobCardDto fleetJob);
        Task<List<FleetJobCardByDateDto>> GetFleetJobCardByDateRangeAsync(GetFleetJobCardByDateRangeDto dto);
        Task<IEnumerable<FleetJobCardDto>> GetFleetJobCardsByFleetManagerAsync();
        Task<FleetJobCardDto> GetFleetJobCardByIdAsync(int jobCardId);
        Task<bool> CloseJobCardByIdAsync(int jobCardId);
        Task<List<FleetJobCardByDateDto>> GetFleetJobCardsByFleetManagerInCurrentMonthAsync();
    }
}
