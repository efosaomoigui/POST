using GIGL.GIGLS.Core.Repositories;
using GIGLS.CORE.Domain;
using GIGLS.CORE.DTO.Nav;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.CORE.IRepositories.Nav
{
    public interface ISubSubNavRepository : IRepository<SubSubNav>
    {
        Task<List<SubSubNavDTO>> GetSubSubNavsAsync();
    }
}
