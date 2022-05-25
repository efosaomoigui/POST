using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGL.GIGLS.Core.Domain;
using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.DTO.Fleets;

namespace GIGLS.Core.IRepositories.Fleets
{
    public interface IFleetJobCardRepository : IRepository<FleetJobCard>
    {
        Task<List<FleetJobCardDto>> GetFleetJobCardsAsync();
        Task<List<FleetJobCardByDateDto>> GetFleetJobCardByDateRangeAsync(GetFleetJobCardByDateRangeDto dto);
        Task<List<FleetJobCardDto>> GetFleetJobCardsByFleetManagerAsync(string fleetManagerId);
        Task<FleetJobCard> GetFleetJobCardByIdAsync(int jobCardId);
        Task<List<FleetJobCardByDateDto>> GetFleetJobCardsByFleetManagerInCurrentMonthAsync(GetFleetJobCardByDateRangeDto dto);
    }
}