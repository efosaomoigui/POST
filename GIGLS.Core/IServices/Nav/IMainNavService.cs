using GIGLS.Core.IServices;
using GIGLS.CORE.DTO.Nav;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIGLS.CORE.IServices.Nav
{
    public interface IMainNavService : IServiceDependencyMarker
    {
        Task<List<MainNavDTO>> GetMainNavs();
        Task<MainNavDTO> GetMainNavById(int mainNavId);
        Task<object> AddMainNav(MainNavDTO mainNav);
        Task UpdateMainNav(int mainNavId, MainNavDTO mainNav);
        Task RemoveMainNav(int mainNavId);
    }
}
