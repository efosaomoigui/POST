using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.Common.Helpers;
using GIGLS.Core.Domain.Route;
using GIGLS.Core.DTO.Routes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories.Routes
{
    public interface IRouteRepository : IRepository<Domain.Route.Route>
    {
        Task<PagedList<RouteDto>> GetPagedAsync(int page, int size, string keyword);
    }
}
