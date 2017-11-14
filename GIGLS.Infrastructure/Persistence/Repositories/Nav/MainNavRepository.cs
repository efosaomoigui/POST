using GIGLS.CORE.Domain;
using GIGLS.CORE.DTO.Nav;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.CORE.IRepositories.Nav;
using System.Linq;
using System;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.Nav
{
    public class MainNavRepository : Repository<MainNav, GIGLSContext>, IMainNavRepository
    {
        public MainNavRepository(GIGLSContext context) : base(context)
        {
        }

        public Task<List<MainNavDTO>> GetMainNavsAsync()
        {
            try
            {
                var mainNav = Context.MainNav;

                List<MainNavDTO> mainNavDto = (from m in mainNav
                                               select new MainNavDTO()
                                               {
                                                   MainNavId = m.MainNavId,
                                                   Name = m.Name,
                                                   Param = m.Param,
                                                   Position = m.Position,
                                                   State = m.State,
                                                   SubNavs = Context.SubNav.Where(s => s.MainNavId == m.MainNavId).Select(c => new SubNavDTO
                                                   {
                                                       SubNavId = c.SubNavId,
                                                       State = c.State,
                                                       Title = c.Title,
                                                       Param = c.Param,
                                                       MainNavId = c.MainNavId,
                                                       MainNavName = c.MainNav.Name,
                                                       SubSubNavs = Context.SubSubNav.Where(ss => ss.SubSubNavId == c.SubNavId).Select(cc => new SubSubNavDTO
                                                       {
                                                           SubSubNavId = cc.SubSubNavId,
                                                           State = cc.State,
                                                           Title = cc.Title,
                                                           Param = cc.Param,
                                                           SubNavId = cc.SubNavId
                                                       }).ToList()
                                                   }).ToList(),
                                               }).ToList();
                return Task.FromResult(mainNavDto.OrderBy(x => x.MainNavId).ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
