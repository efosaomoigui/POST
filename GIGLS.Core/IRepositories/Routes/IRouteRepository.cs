using GIGL.POST.Core.Repositories;
using POST.Core.Common.Helpers;
using POST.Core.Domain.Route;
using POST.Core.DTO.Routes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POST.Core.IRepositories.Routes
{
    public interface IRouteRepository : IRepository<Domain.Route.Route>
    {
        Task<PagedList<RouteDto>> GetPagedAsync(int page, int size, string keyword);
    }
}
