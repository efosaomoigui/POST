using GIGLS.Core.IRepositories.ServiceCentres;
using GIGLS.Infrastructure.Persistence.Repository;
using GIGLS.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GIGLS.Core.DTO.ServiceCentres;
using AutoMapper;

namespace GIGLS.Infrastructure.Persistence.Repositories.ServiceCentres
{
    public class HomeDeliveryLocationRepository : Repository<HomeDeliveryLocation, GIGLSContext>, IHomeDeliveryLocationRepository
    {
        private GIGLSContext _context;
        public HomeDeliveryLocationRepository(GIGLSContext context)
            : base(context)
        {
            _context = context;
        }

        public Task<IEnumerable<HomeDeliveryLocationDTO>> GetActiveHomeDeliveryLocations()
        {
            try
            {
                var locations = _context.HomeDeliveryLocation.Where(x => x.Status == true).ToList();
                var locationsDTO = Mapper.Map<IEnumerable<HomeDeliveryLocationDTO>>(locations);
                return Task.FromResult(locationsDTO);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
