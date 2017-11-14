using GIGLS.CORE.DTO.Nav;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using GIGLS.CORE.Domain;
using GIGLS.CORE.IRepositories.Nav;
using System.Linq;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.Nav
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
