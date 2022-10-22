using POST.Core.IRepositories.ServiceCentres;
using POST.Infrastructure.Persistence;
using POST.Infrastructure.Persistence.Repository;
using POST.Core.Domain;
using POST.Core.DTO.ServiceCentres;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Linq;
using AutoMapper;

namespace POST.INFRASTRUCTURE.Persistence.Repositories.ServiceCentres
{
    public class LGARepository : Repository<LGA, GIGLSContext>, ILGARepository
    {
        private GIGLSContext _context;
        public LGARepository(GIGLSContext context)
            : base(context)
        {
            _context = context;
        }
        
        public Task<IEnumerable<LGADTO>> GetActiveLGAs()
        {
            try
            {
                var lgas = Context.LGA.Where(x => x.Status == true).ToList();
                var lgasDto = Mapper.Map<IEnumerable<LGADTO>>(lgas);
                return Task.FromResult(lgasDto);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<IEnumerable<LGADTO>> GetActiveHomeDeliveryLocations()
        {
            try
            {
                var lgas = Context.LGA.Where(x => x.HomeDeliveryStatus == true).ToList();
                var lgasDto = Mapper.Map<IEnumerable<LGADTO>>(lgas);
                return Task.FromResult(lgasDto);
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}