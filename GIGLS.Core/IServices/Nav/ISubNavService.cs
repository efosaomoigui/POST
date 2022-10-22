using POST.Core.IServices;
using POST.CORE.DTO.Nav;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POST.CORE.IServices.Nav
{
    public interface ISubNavService : IServiceDependencyMarker
    {
        Task<List<SubNavDTO>> GetSubNavs();
        Task<SubNavDTO> GetSubNavById(int subNavId);
        Task<object> AddSubNav(SubNavDTO subNav);
        Task UpdateSubNav(int subNavId, SubNavDTO subNav);
        Task RemoveSubNav(int subNavId);
    }
}
