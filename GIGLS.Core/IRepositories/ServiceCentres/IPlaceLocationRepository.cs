using GIGL.POST.Core.Repositories;
using POST.Core.Domain;
using POST.Core.DTO.ServiceCentres;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POST.Core.IRepositories.ServiceCentres
{
    public interface IPlaceLocationRepository : IRepository<PlaceLocation>
    {
        Task<IEnumerable<PlaceLocationDTO>> GetLocations();
        Task<PlaceLocationDTO> GetLocationById(int locationId);
        Task<IEnumerable<PlaceLocationDTO>> GetLocationsByStateId(int stateId);
    }
}
