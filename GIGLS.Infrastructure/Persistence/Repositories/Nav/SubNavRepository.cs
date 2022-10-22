using POST.CORE.DTO.Nav;
using POST.Infrastructure.Persistence;
using POST.Infrastructure.Persistence.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using POST.CORE.Domain;
using POST.CORE.IRepositories.Nav;
using System.Linq;

namespace POST.INFRASTRUCTURE.Persistence.Repositories.Nav
{
    public class SubNavRepository : Repository<SubNav, GIGLSContext>, ISubNavRepository
    {
        public SubNavRepository(GIGLSContext context) : base(context)
        {
        }

        public Task<List<SubNavDTO>> GetSubNavsAsync()
        {
            try
            {
                var subNav = Context.SubNav;

                List<SubNavDTO> subNavDto = (from s in subNav
                                               select new SubNavDTO()
                                               {
                                                   SubNavId = s.SubNavId,
                                                   State = s.State,
                                                   Title = s.Title,
                                                   Param = s.Param,
                                                   MainNavId = s.MainNavId,
                                                   MainNavName = s.MainNav.Name,
                                                   SubSubNavs = Context.SubSubNav.Where(ss => ss.SubSubNavId == s.SubNavId).Select(cc => new SubSubNavDTO
                                                   {
                                                       SubSubNavId = cc.SubSubNavId,
                                                       State = cc.State,
                                                       Title = cc.Title,
                                                       Param = cc.Param,
                                                       SubNavId = cc.SubNavId
                                                   }).ToList()
                                               }).ToList();
                return Task.FromResult(subNavDto.OrderBy(x => x.SubNavId).ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
