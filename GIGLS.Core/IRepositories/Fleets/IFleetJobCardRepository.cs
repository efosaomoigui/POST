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
    }
}