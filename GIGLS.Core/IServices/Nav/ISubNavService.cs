using GIGLS.Core.IServices;
using GIGLS.CORE.DTO.Nav;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.CORE.IServices.Nav
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
