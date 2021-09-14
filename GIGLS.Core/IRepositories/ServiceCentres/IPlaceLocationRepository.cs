using GIGL.GIGLS.Core.Repositories;
using GIGLS.Core.Domain;
using GIGLS.Core.DTO.ServiceCentres;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Core.IRepositories.ServiceCentres
{
    public interface IPlaceLocationRepository : IRepository<PlaceLocation>
    {
        Task<IEnumerable<PlaceLocationDTO>> GetLocations();
        Task<PlaceLocationDTO> GetLocationById(int locationId);
    }
}
