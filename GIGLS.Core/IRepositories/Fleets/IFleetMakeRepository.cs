using GIGL.POST.Core.Domain;
using GIGL.POST.Core.Repositories;
using POST.Core.DTO.Fleets;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Core.IRepositories.Fleets
{
    public interface IFleetMakeRepository : IRepository<FleetMake>
    {
        Task<List<FleetMakeDTO>> GetFleetMakers();
    }
}
