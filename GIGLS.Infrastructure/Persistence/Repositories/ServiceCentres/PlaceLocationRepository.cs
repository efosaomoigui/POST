using GIGLS.Core.Domain;
using GIGLS.Core.DTO.ServiceCentres;
using GIGLS.Core.IRepositories.ServiceCentres;
using GIGLS.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Infrastructure.Persistence.Repositories.ServiceCentres
{
    public class PlaceLocationRepository : Repository<PlaceLocation, GIGLSContext>, IPlaceLocationRepository
    {
        private GIGLSContext _context;
        public PlaceLocationRepository(GIGLSContext context)
            : base(context)
        {
            _context = context;
        }

        public Task<PlaceLocationDTO> GetLocationById(int locationId)
        {
            var location = Context.PlaceLocation.Where(x => x.PlaceLocationId == locationId);
            var locationDto = GetLocation(location);
            return locationDto;
        }

        public Task<IEnumerable<PlaceLocationDTO>> GetLocations()
        {
            var location = Context.PlaceLocation;
            var locationDto = GetListOfLocations(location);
            return locationDto;
        }

        private Task<IEnumerable<PlaceLocationDTO>> GetListOfLocations(IQueryable<PlaceLocation> locations)
        {
            var locationdto = from p in locations
                              join s in Context.ServiceCentre on p.BaseStationId equals s.ServiceCentreId
                              join st in Context.State on p.StateId equals st.StateId
                              orderby p.DateCreated ascending
                              select new PlaceLocationDTO
                              {
                                  PlaceLocationId = p.PlaceLocationId,
                                  PlaceLocationName =p.PlaceLocationName,
                                  BaseStation = s.Name,
                                  BaseStationId = s.ServiceCentreId,
                                  StateId = st.StateId,
                                  StateName = st.StateName,
                                  IsExpressHomeDelivery = p.IsExpressHomeDelivery,
                                  IsExtraMileDelivery = p.IsExtraMileDelivery,
                                  IsGIGGO = p.IsGIGGO,
                                  IsHomeDelivery = p.IsHomeDelivery,
                                  IsNormalHomeDelivery = p.IsNormalHomeDelivery,
                              };
            return Task.FromResult(locationdto.AsEnumerable());
        }

        private Task<PlaceLocationDTO> GetLocation(IQueryable<PlaceLocation> locations)
        {
            var locationdto = from p in locations
                              join s in Context.ServiceCentre on p.BaseStationId equals s.ServiceCentreId
                              join st in Context.State on p.StateId equals st.StateId
                              orderby p.DateCreated ascending
                              select new PlaceLocationDTO
                              {
                                  PlaceLocationId = p.PlaceLocationId,
                                  PlaceLocationName = p.PlaceLocationName,
                                  BaseStation = s.Name,
                                  BaseStationId = s.ServiceCentreId,
                                  StateId = st.StateId,
                                  StateName = st.StateName,
                                  IsExpressHomeDelivery = p.IsExpressHomeDelivery,
                                  IsExtraMileDelivery = p.IsExtraMileDelivery,
                                  IsGIGGO = p.IsGIGGO,
                                  IsHomeDelivery = p.IsHomeDelivery,
                                  IsNormalHomeDelivery = p.IsNormalHomeDelivery,
                              };
            return Task.FromResult(locationdto.FirstOrDefault());
        }
    }
}
