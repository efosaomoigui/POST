using GIGL.POST.Core.Repositories;
using POST.CORE.Domain;
using POST.CORE.DTO.Nav;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.CORE.IRepositories.Nav
{
    public interface ISubSubNavRepository : IRepository<SubSubNav>
    {
        Task<List<SubSubNavDTO>> GetSubSubNavsAsync();
    }
}
