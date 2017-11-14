using GIGLS.Core.IServices;
using GIGLS.CORE.DTO.Nav;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.CORE.IServices.Nav
{
    public interface ISubSubNavService : IServiceDependencyMarker
    {
        Task<List<SubSubNavDTO>> GetSubSubNavs();
        Task<SubSubNavDTO> GetSubSubNavById(int subSubNavId);
        Task<object> AddSubSubNav(SubSubNavDTO subSubNav);
        Task UpdateSubSubNav(int subSubNavId, SubSubNavDTO subSubNav);
        Task RemoveSubSubNav(int subSubNavId);
    }
}
