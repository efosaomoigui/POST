using GIGL.POST.Core.Repositories;
using POST.CORE.Domain;
using POST.CORE.DTO.Nav;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.CORE.IRepositories.Nav
{
    public interface ISubNavRepository : IRepository<SubNav>
    {
        Task<List<SubNavDTO>> GetSubNavsAsync();
    }
}
