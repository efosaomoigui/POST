using GIGLS.Core;
using GIGLS.Core.Common.Helpers;
using GIGLS.Core.DTO.Routes;
using GIGLS.Core.IServices.RouteServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Services.Implementation.Routes
{
    public class RouteService : IRouteService
    {
        private readonly IUnitOfWork _uow;

        public RouteService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<PagedList<RouteDto>> GetRoutes(int pageNumber, int pageSize)
        {
            var routes = await _uow.Routes.GetPagedAsync(pageNumber, pageSize).ConfigureAwait(false);

            return routes;
        }
    }
}
