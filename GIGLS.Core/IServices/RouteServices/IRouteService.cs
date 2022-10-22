using POST.Core.Common.Helpers;
using POST.Core.DTO;
using POST.Core.DTO.Routes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POST.Core.IServices.RouteServices
{
    public interface IRouteService : IServiceDependencyMarker
    {
        Task<PagedList<RouteDto>> GetRoutes(BaseSearchDto model);
        Task<CreateRouteDto> AddRoute(CreateRouteDto model);
    }
}
