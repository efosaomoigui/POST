using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGL.GIGLS.Core.Domain;
using GIGLS.Core.DTO.Zone;
using GIGLS.Core.IRepositories.Zone;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;
using System.Linq;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.Zone
{
    public class DomesticZonePriceRepository : Repository<DomesticZonePrice, GIGLSContext>, IDomesticZonePriceRepository
    {
        public DomesticZonePriceRepository(GIGLSContext context) : base(context)
        {
        }

        public Task<List<DomesticZonePriceDTO>> GetDomesticZones()
        {
            try
            {
                var zones = Context.DomesticZonePrice.Include("Zone");

                var zoneDto = from z in zones
                                 select new DomesticZonePriceDTO
                                 {
                                     DomesticZonePriceId = z.DomesticZonePriceId,
                                     Price = z.Price,
                                     Weight = z.Weight,
                                     ZoneId = z.ZoneId,
                                     ZoneName = z.Zone.ZoneName,
                                     DateCreated = z.DateCreated,                                     
                                     //user logged on
                                 };
                return Task.FromResult(zoneDto.ToList());
            }
            catch (Exception )
            {
                throw;
            }
        }
    }
}
