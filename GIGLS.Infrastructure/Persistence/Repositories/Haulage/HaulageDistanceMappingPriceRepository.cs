using AutoMapper;
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
    public class HaulageDistanceMappingPriceRepository : Repository<HaulageDistanceMappingPrice, GIGLSContext>, IHaulageDistanceMappingPriceRepository
    {
        public HaulageDistanceMappingPriceRepository(GIGLSContext context) : base(context)
        {
        }

        public Task<List<HaulageDistanceMappingPriceDTO>> GetHaulageDistanceMappingPricesAsync()
        {
            var haulageDistanceMappingPrices = Context.HaulageDistanceMappingPrice.Include("Haulage");

            var haulageDistanceMappingPriceDto = from z in haulageDistanceMappingPrices
                                                 select new HaulageDistanceMappingPriceDTO
                                                 {
                                                     HaulageDistanceMappingPriceId = z.HaulageDistanceMappingPriceId,
                                                     Price = z.Price,
                                                     HaulageId = z.HaulageId,
                                                     Haulage = new HaulageDTO
                                                     {
                                                         HaulageId = z.Haulage.HaulageId,
                                                         Tonne = z.Haulage.Tonne,
                                                         Status = z.Haulage.Status
                                                     },
                                                     StartRange = z.StartRange,
                                                     EndRange = z.EndRange,
                                                     DateCreated = z.DateCreated
                                                 };
            return Task.FromResult(haulageDistanceMappingPriceDto.ToList());
        }
    }
}
