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
                                            CategoryMinimumPrice = s.CategoryMinimumPrice,
                                            PricePerWeight = s.PricePerWeight,
                                            PriceCategoryId = s.PriceCategoryId,
                                            CountryId = s.CountryId,
                                            IsActive = s.IsActive,
                                            CategoryMinimumWeight = s.CategoryMinimumWeight,
                                            CountryName = _context.Country.Where(x => x.CountryId == countryId).FirstOrDefault().CountryName
                                };
                return Task.FromResult(PriceCategorysDTO.OrderBy(x => x.PriceCategoryName).ToList());

            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<PriceCategoryDTO> GetPriceCategoryById(int priceCategoryId)
        {
            try
            {
                var PriceCategorys = _context.PriceCategory.Where(s => s.PriceCategoryId == priceCategoryId);

                var PriceCategorysDTO = from s in PriceCategorys
                                        select new PriceCategoryDTO
                                        {
                                            PriceCategoryName = s.PriceCategoryName,
                                            CategoryMinimumPrice = s.CategoryMinimumPrice,
                                            PricePerWeight = s.PricePerWeight,
                                            PriceCategoryId = s.PriceCategoryId,
                                            CountryId = s.CountryId,
                                            CategoryMinimumWeight = s.CategoryMinimumWeight,
                                            IsActive = s.IsActive,
                                            CountryName = _context.Country.Where(x => x.CountryId == s.CountryId).FirstOrDefault().CountryName
                                        };
                return Task.FromResult(PriceCategorysDTO.FirstOrDefault());

            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<List<PriceCategoryDTO>> GetPriceCategories()
        {
            try
            {
                var categories = _context.PriceCategory;

                var categoriesDTO = from s in categories
                                join c in Context.Country on s.CountryId equals c.CountryId
                                select new PriceCategoryDTO
                                {
                                    PriceCategoryName = s.PriceCategoryName,
                                    CategoryMinimumPrice = s.CategoryMinimumPrice,
                                    PricePerWeight = s.PricePerWeight,
                                    PriceCategoryId = s.PriceCategoryId,
                                    CategoryMinimumWeight = s.CategoryMinimumWeight,
                                    CountryName = c.CountryName,
                                    IsActive = s.IsActive,
                                    CountryId = c.CountryId,
                                };
                return Task.FromResult(categoriesDTO.OrderBy(x => x.PriceCategoryName).ToList());

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
