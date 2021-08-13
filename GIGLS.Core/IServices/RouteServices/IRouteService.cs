using GIGLS.Core.Common.Helpers;
using GIGLS.Core.DTO;
using GIGLS.Core.DTO.Routes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.RouteServices
{
    public interface IRouteService : IServiceDependencyMarker
    {
        Task<PagedList<RouteDto>> GetRoutes(BaseSearchDto model);
        Task<CreateRouteDto> AddRoute(CreateRouteDto model);
    }
}
