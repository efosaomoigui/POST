using GIGL.GIGLS.Core.Domain;
using GIGLS.Infrastructure.Persistence;
using GIGLS.Infrastructure.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using GIGLS.Core.DTO;
using GIGLS.Core.Domain;
using GIGLS.Core.IRepositories;
using GIGLS.Core.DTO.PaymentTransactions;

namespace GIGLS.INFRASTRUCTURE.Persistence.Repositories.PriceCategorys
{
    public class PriceCategoryRepository : Repository<PriceCategory, GIGLSContext>, IPriceCategoryRepository
    {
        private GIGLSContext _context;

        public PriceCategoryRepository(GIGLSContext context) : base(context)
        {
            _context = context;
        }

        public Task<List<PriceCategoryDTO>> GetPriceCategoriesByCountryId(int countryId)
        {
            try
            {
                var PriceCategorys = _context.PriceCategory.Where(s => s.CountryId == countryId);

                var PriceCategorysDTO = from s in PriceCategorys
                                select new PriceCategoryDTO
                                {
                                    PriceCategoryName = s.PriceCategoryName,
                                    URL = s.URL,
                                    Address = s.Address,
                                    City = s.City,
                                    PriceCategoryImage = s.PriceCategoryImage
                                };
                return Task.FromResult(PriceCategorysDTO.OrderBy(x => x.PriceCategoryName).ToList());

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
