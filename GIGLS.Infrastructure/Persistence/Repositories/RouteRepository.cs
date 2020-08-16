using GIGLS.Core.Common.Extensions;
using GIGLS.Core.Common.Helpers;
using GIGLS.Core.Domain.Route;
using GIGLS.Core.DTO.Routes;
using GIGLS.Core.IRepositories.Routes;
using GIGLS.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Infrastructure.Persistence.Repositories
{
    public class RouteRepository : Repository<Route, GIGLSContext>, IRouteRepository
    {
        private GIGLSContext _context;

        public RouteRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }

        public Task<PagedList<RouteDto>> GetPagedAsync(int page, int size)
        {
            var subroutes = _context.Routes.AsQueryable();
            var mainroutes = _context.Routes.AsQueryable();


            var routes = from subRoute in subroutes
                         join mainRoute in mainroutes

                         on subRoute.MainRouteId equals mainRoute.RouteId

                         select new RouteDto
                         {

                         };

            return routes.ToUpdatedPagedListAsync(page, size);


        }
    }
}
