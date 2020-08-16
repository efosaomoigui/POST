using GIGLS.Core;
using GIGLS.Core.Common.Helpers;
using GIGLS.Core.Domain.Route;
using GIGLS.Core.DTO;
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

        public async Task<PagedList<RouteDto>> GetRoutes(BaseSearchDto model)
        {

            if (model == null)
                throw new Exception();

            var pageSize = model.PageSize == null | model.PageSize <= 0 ? 50 : model.PageSize;
            var pageIndex = model.PageIndex == null | model.PageIndex <= 0 ? 50 : model.PageIndex;

            var routes = await _uow.Routes.GetPagedAsync(pageIndex.Value, pageSize.Value, model.Keyword).ConfigureAwait(false);

            return routes;
        }

        public async Task<CreateRouteDto> AddRoute(CreateRouteDto model)
        {
            if (model == null) throw new Exception();

            var existingRoute =_uow.Routes.SingleOrDefault(p => p.DepartureCentreId == model.DepartureCenterId &&
                                  p.DestinationCentreId == model.DestinationCenterId);

            if (existingRoute != null)
            {
                throw new Exception("Route already exists");
            }

            var routeName = RouteName(model.DepartureCenterId, model.DestinationCenterId);

            int? mainRouteId = null;

            if (model.MainRouteId != null)
            {
                mainRouteId = _uow.Routes.Get(model.MainRouteId.Value)?.MainRouteId;
            }
            var route = new Route
            {
                DepartureCentreId = model.DepartureCenterId,
                DestinationCentreId = model.DestinationCenterId,
                LoaderFee = model.LoaderFee,
                DispatchFee = model.DispatchFee,
                RouteName = routeName,
                CaptainFee = model.CaptainFee,
                DateCreated = DateTime.Now,
                MainRouteId = mainRouteId
           };

            _uow.Routes.Add(route);

            return model;
        }

        private string RouteName(int DepartureId , int DestinationId)
        {
            var departure = _uow.ServiceCentre.Get(DepartureId) ?? throw new Exception("Departure service center does not exist");
            var destination = _uow.ServiceCentre.Get(DestinationId) ?? throw new Exception("Destination service center does not exist");

            return $"{departure.Name} ==> {destination.Name}";

        }
    }
}
