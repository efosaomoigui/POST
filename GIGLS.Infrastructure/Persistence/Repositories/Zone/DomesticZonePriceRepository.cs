using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GIGL.POST.Core.Domain;
using POST.Core.DTO.Zone;
using POST.Core.IRepositories.Zone;
using POST.Infrastructure.Persistence;
using POST.Infrastructure.Persistence.Repository;
using System.Linq;
using POST.Core.Enums;

namespace POST.INFRASTRUCTURE.Persistence.Repositories.Zone
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
                                     RegularEcommerceType = z.RegularEcommerceType,
                                     CountryId = z.CountryId
                                     //user logged on
                                 };
                return Task.FromResult(zoneDto.ToList());
            }
            catch (Exception )
            {
                throw;
            }
        }

        public Task<DomesticZonePrice> GetDomesticZonePrice(int zoneId, decimal weight, RegularEcommerceType regularEcommerceType, int countryId)
        {
            try
            {
                var zones = Context.DomesticZonePrice.FirstOrDefault(d => d.ZoneId == zoneId 
                    && d.Weight >= weight 
                    && d.RegularEcommerceType == regularEcommerceType
                    && d.CountryId == countryId);
                return Task.FromResult(zones);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
