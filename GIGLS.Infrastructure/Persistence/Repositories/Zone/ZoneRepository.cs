using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGLS.Core.DTO.Zone;
using GIGLS.Core.IRepositories.Zone;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;
using System.Linq;
using AutoMapper;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.Zone
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
