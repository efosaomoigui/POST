using GIGLS.Core.DTO.ServiceCentres;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.IServices.ServiceCentres
{
    public interface IPlaceLocationService : IServiceDependencyMarker
    {
        Task<IEnumerable<PlaceLocationDTO>> GetLocations();
        Task<PlaceLocationDTO> GetLocationById(int locationId);
        Task<object> AddLocation(PlaceLocationDTO locationDto);
        Task UpdateLocation(int locationId, PlaceLocationDTO locationDto);
        Task DeleteLocation(int locationId);
        Task UpdateHomeDeliveryLocation(int locationId, bool status);
        Task UpdateNormalHomeDeliveryLocation(int locationId, bool status);
        Task UpdateExpressHomeDeliveryLocation(int locationId, bool status);
        Task UpdateExtraMileDeliveryLocation(int locationId, bool status);
        Task UpdateGIGGOLocation(int locationId, bool status);
        Task<IEnumerable<PlaceLocationDTO>> GetLocationsByStateId(int stateId);
        Task CreateOrUpdateLocationList(List<PlaceLocationDTO> locationDtos);
        Task UpdateLocationList(UpdatePlaceLocationsDTO locationDtos);
    }
}
