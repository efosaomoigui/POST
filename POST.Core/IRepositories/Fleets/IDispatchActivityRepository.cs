using GIGL.POST.Core.Repositories;
using POST.Core.Domain;
using POST.Core.DTO.Fleets;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.Core.IRepositories.Fleets
{
    public interface IDispatchActivityRepository : IRepository<DispatchActivity>
    {
        Task<List<DispatchActivityDTO>> GetDispatchActivitiesAsync();
    }
}
