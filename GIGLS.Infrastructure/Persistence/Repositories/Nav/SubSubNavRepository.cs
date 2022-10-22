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
    public class SubSubNavRepository : Repository<SubSubNav, GIGLSContext>, ISubSubNavRepository
    {
        public SubSubNavRepository(GIGLSContext context) : base(context)
        {
        }

        public Task<List<SubSubNavDTO>> GetSubSubNavsAsync()
        {
            try
            {
                var subNav = Context.SubSubNav;

                List<SubSubNavDTO> subNavDto = (from s in subNav
                                             select new SubSubNavDTO()
                                             {
                                                 SubSubNavId = s.SubSubNavId,
                                                 State = s.State,
                                                 Title = s.Title,
                                                 Param = s.Param,
                                                 SubNavId = s.SubNavId,
                                                 SubNavTitle = s.SubNav.Title
                                             }).ToList();
                return Task.FromResult(subNavDto.OrderBy(x => x.SubSubNavId).ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
