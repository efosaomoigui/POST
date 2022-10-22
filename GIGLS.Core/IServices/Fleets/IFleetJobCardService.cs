using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POST.Core.DTO.Fleets;

namespace POST.Core.IServices.Fleets
{
    public interface IFleetJobCardService : IServiceDependencyMarker
    {
        Task<IEnumerable<FleetJobCardDto>> GetFleetJobCardsAsync();
        Task<bool> OpenFleetJobCardsAsync(NewJobCard jobDto);
        Task<List<FleetJobCardByDateDto>> GetFleetJobCardByDateRangeAsync(GetFleetJobCardByDateRangeDto dto);
        Task<FleetJobCardDto> GetFleetJobCardByIdAsync(int jobCardId);
        Task<bool> CloseJobCardAsync(CloseJobCardDto jobCard);
        Task<List<FleetJobCardByDateDto>> GetAllFleetJobCardsByInCurrentMonthAsync();
    }
}
