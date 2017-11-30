using GIGLS.Core.Domain;
using GIGLS.Core.DTO.Haulage;
using GIGLS.Core.IRepositories;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories
{
    public class ZoneHaulagePriceRepository : Repository<ZoneHaulagePrice, GIGLSContext>, IZoneHaulagePriceRepository
    {
        public ZoneHaulagePriceRepository(GIGLSContext context) : base(context)
        {
        }

        public Task<List<ZoneHaulagePriceDTO>> GetZoneHaulagePricesAsync()
        {
            var zoneHaulagePrices = Context.ZoneHaulagePrice.Include("Haulage,Zone");

            var zoneHaulagePriceDto = from z in zoneHaulagePrices
                          select new ZoneHaulagePriceDTO
                          {
                              ZoneHaulagePriceId = z.ZoneHaulagePriceId,
                              Price = z.Price,
                              HaulageId = z.HaulageId,
                              Tonne = z.Haulage.Tonne,
                              ZoneId = z.ZoneId,
                              ZoneName = z.Zone.ZoneName,
                              DateCreated = z.DateCreated                              
                          };
            return Task.FromResult(zoneHaulagePriceDto.ToList());
        }
    }
}
