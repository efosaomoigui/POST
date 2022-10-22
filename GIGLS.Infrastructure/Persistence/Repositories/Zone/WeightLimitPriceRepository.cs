using POST.Core.Domain;
using POST.Core.IRepositories.Zone;
using POST.Infrastructure.Persistence;
using POST.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using POST.Core.DTO.Zone;

namespace POST.INFRASTRUCTURE.Persistence.Repositories.Zone
{
    public class WeightLimitPriceRepository : Repository<WeightLimitPrice, GIGLSContext>, IWeightLimitPriceRepository
    {
        public WeightLimitPriceRepository(GIGLSContext context) : base(context)
        {
        }

        public Task<List<WeightLimitPriceDTO>> GetWeightLimitPrices()
        {
            try
            {
                var options = Context.WeightLimitPrices.Include("Zone");
                var optionDto = from r in options
                                select new WeightLimitPriceDTO
                                {
                                    WeightLimitPriceId = r.WeightLimitPriceId,
                                    Weight = r.Weight,
                                    Price = r.Price,
                                    ZoneId = r.ZoneId,
                                    ZoneName = r.Zone.ZoneName,
                                    RegularEcommerceType = r.RegularEcommerceType,
                                    CountryId = r.CountryId
                                };
                return Task.FromResult(optionDto.ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
