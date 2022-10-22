using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using POST.Core.DTO.Zone;
using POST.Core.IRepositories.Zone;
using POST.Infrastructure.Persistence;
using POST.Infrastructure.Persistence.Repository;
using System.Linq;
using AutoMapper;

namespace POST.INFRASTRUCTURE.Persistence.Repositories.Zone
{
    public class ZoneRepository : Repository<Core.Domain.Zone, GIGLSContext>, IZoneRepository
    {
        public ZoneRepository(GIGLSContext context) : base(context)
        {
        }

        public Task<IEnumerable<ZoneDTO>> GetActiveZonesAsync()
        {
            try
            {
                var zones = Context.Zone.Where(x => x.Status == true).ToList();

                var zoneDto = Mapper.Map<IEnumerable<ZoneDTO>>(zones);
                return Task.FromResult(zoneDto);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<IEnumerable<ZoneDTO>> GetZonesAsync()
        {
            try
            {
                var zones = Context.Zone.ToList();

                var zoneDto = Mapper.Map<IEnumerable<ZoneDTO>>(zones);
                return Task.FromResult(zoneDto);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
