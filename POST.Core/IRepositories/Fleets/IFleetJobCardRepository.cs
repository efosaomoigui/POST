using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGL.POST.Core.Domain;
using GIGL.POST.Core.Repositories;
using POST.Core.DTO.Fleets;

namespace POST.Core.IRepositories.Fleets
{
    public interface IFleetJobCardRepository : IRepository<FleetJobCard>
    {
        Task<List<FleetJobCardDto>> GetFleetJobCardsAsync();
        Task<List<FleetJobCardByDateDto>> GetFleetJobCardByDateRangeAsync(GetFleetJobCardByDateRangeDto dto);
        Task<FleetJobCard> GetFleetJobCardByIdAsync(int jobCardId);
        Task<List<FleetJobCardByDateDto>> GetFleetJobCardsInCurrentMonthAsync(GetFleetJobCardByDateRangeDto dto);
    }
}